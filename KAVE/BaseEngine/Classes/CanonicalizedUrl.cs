using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KAVE
{
    public class CanonicalizedUrl
    {

        /// <summary>
        /// Canconicalizes a url into a standard format
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Parse(string url)
        {
            string scheme = null;
            string hostName;
            string path;
            string querystring;

            //split into hostname, path and querystring
            Regex schemePattern = new Regex("^http.?://");
            Match schemeMatch = schemePattern.Match(url);
            if (schemeMatch.Success)
                scheme = schemeMatch.Value;

            //unescape the hex values in url 
            url = UnescapeHex(url);


            hostName = HostName(url);
            hostName = CleanHostName(hostName);

            path = CleanEscapePathCharacters(Path(url));
            path = CleanPathTraversal(path);

            querystring = CleanQueryString(Querystring(url));

            //reassemble the url in the canonicalized form
            StringBuilder sb = new StringBuilder();
            if (scheme != null)
                sb.Append(scheme);

            sb.Append(hostName);
            sb.Append("/");

            if (path != null)
                sb.Append(path);
            if (querystring != null)
            {
                sb.Append('?');
                sb.Append(querystring);
            }

            return sb.ToString();

        }

        /// <summary>
        /// Determines if the url is a hostname or
        /// an IP address.
        /// </summary>
        /// <param name="url">url to test. This should already be canonicalized</param>
        /// <returns>bool indicating if the url contains a hostname
        /// or if false an ip address</returns>
        public static bool IsHostName(string url)
        {
            string hostpart = HostName(url);

            if (hostpart.IndexOf('.') == -1)
                return false;

            string lastDotPart = hostpart.Substring(hostpart.LastIndexOf('.') + 1);

            foreach (char c in lastDotPart)
            {
                if (!Char.IsLetter(c))
                    return false;
            }

            return true;
        }

        #region querystring functions

        /// <summary>
        /// extract the querystring part of the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string Querystring(string url)
        {
            string querystringPath = null;
            int indexOfQuestionMark = url.IndexOf('?');
            int indexOfHash = url.IndexOf('#');

            if (indexOfQuestionMark > -1)
            {
                if (indexOfHash > -1)
                    querystringPath = url.Substring(indexOfQuestionMark + 1, url.Length - indexOfHash - 2);
                else
                    querystringPath = url.Substring(indexOfQuestionMark + 1);
            }

            return querystringPath;
        }

        private static string CleanQueryString(string querystring)
        {
            string unescaped = UnescapeHex(querystring);
            return EscapeHex(unescaped);
        }

        #endregion

        #region path functions

        /// <summary>
        /// extracts the path part from the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string Path(string url)
        {
            string pathPart = null;

            if (url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                int pathStart = url.IndexOf('/', 7);
                if (pathStart > -1 & pathStart != url.Length -1)
                {
                    pathPart = url.Substring(pathStart + 1);
                }
            }
            else if (url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                int pathStart = url.IndexOf('/', 8);
                if (pathStart > -1 & pathStart != url.Length - 1)
                {
                    pathPart = url.Substring(pathStart + 1);
                }
            }
            else
            {
                int pathStart = url.IndexOf('/');
                if (pathStart > -1 & pathStart != url.Length - 1)
                {
                    pathPart = url.Substring(pathStart + 1);
                }
            }

            //check if we have a querystring to remove
            if (pathPart != null)
            {
                int queryStringStart = pathPart.IndexOf('?');
                if(queryStringStart != -1)
                    pathPart = pathPart.Substring(0, queryStringStart);
            }

            //check if we have a region to remove
            if (pathPart != null)
            {
                int regionStart = pathPart.IndexOf('#');
                if (regionStart != -1)
                    pathPart = pathPart.Substring(0, regionStart);
            }

            return pathPart;
        }



        /// <summary>
        /// Cleans a path by removing any illegal char
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string CleanEscapePathCharacters(string path)
        {
            path = UnescapeHex(path);


            return EscapeHex(path);
        }


        /// <summary>
        /// Cleans a path of any traversal elements
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string CleanPathTraversal(string path)
        {

            if (String.IsNullOrEmpty(path))
                return null;

            string[] pathParts = path.Split('/');


            //remove the path traversal elements in the path
            if (pathParts.Length > 0)
            {
                for(int i =0; i <pathParts.Length; i++)
                {
                    if (pathParts[i] == ".")
                        pathParts[i] = null;
                    else if (pathParts[i] == "..")
                    {
                        pathParts[i] = null;
                        if (i > 0)
                            pathParts[i - 1] = null;
                    }
                }


                //reassemble the path parts back together
                StringBuilder pathBuilder = new StringBuilder();

                foreach (string pathPart in pathParts)
                {
                    if (!String.IsNullOrEmpty(pathPart))
                    {
                        pathBuilder.Append(pathPart);
                        pathBuilder.Append("/");
                    }
                }

                //remove the last trailing slash if the input path
                //didnt have one
                if(!path.EndsWith("/"))
                    pathBuilder.Remove(pathBuilder.Length - 1, 1);

                if (pathBuilder.Length > 0)
                    path = pathBuilder.ToString();
                else
                    path = null;

            }

            
            return path;
        }

        #endregion

        #region host name functions

        /// <summary>
        /// Extracts the hostname from a url.
        /// </summary>
        /// <param name="url">string containing the url to extract</param>
        /// <param name="includeTrailingSlash">bool indicating if the hostname
        /// name will have a trailing slash</param>
        /// <returns></returns>
        public static string HostName(string url, bool includeTrailingSlash)
        {
            string domainPart = null;
            url = url.ToLower();

            //get the domain part of the url
            if (url.StartsWith("http://"))
            {
                int domainEndChar = url.IndexOf('/', 7);
                if (domainEndChar == -1)
                    domainPart = url.Substring(7);
                else
                    domainPart = url.Substring(7, domainEndChar - 7);
            }
            else if (url.StartsWith("https://"))
            {
                int domainEndChar = url.IndexOf('/', 8);
                if (domainEndChar == -1)
                    domainPart = url.Substring(8);
                else
                    domainPart = url.Substring(8, domainEndChar - 8);
            }
            else
            {
                int domainEndChar = url.IndexOf('/');
                if (domainEndChar == -1)
                    domainPart = url;
                else
                    domainPart = url.Substring(0, domainEndChar);
            }

            //remove double dots and trailing dots
            domainPart = RemoveDoubleDotHost(domainPart);

            if (domainPart.IndexOf('.') == -1)
                domainPart = ExtractHostFromIp(domainPart);
            else if (!Char.IsLetter(domainPart[domainPart.LastIndexOf('.') + 1]))
                domainPart = ExtractHostFromIp(domainPart);

            if (includeTrailingSlash)
                domainPart += "/";

            return domainPart;
        }

        /// <summary>
        /// Takes a string which isnt a domain name and attempts to
        /// generate and validate the input into a x.x.x.x ip address.
        /// </summary>
        /// <remarks>This method will attempt to decode oct, hex and dec values
        /// into the correct the ip</remarks>
        /// <param name="domainPart"></param>
        /// <returns></returns>
        private static string ExtractHostFromIp(string domainPart)
        {
            string[] octets = domainPart.Split('.');
            int[] decimalOctaves = new int[octets.Length];

            for(int i = 0; i < octets.Length; i++)
            {
                if (octets[i].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                {
                    //hex convert to a decimal
                    long temp = Convert.ToInt64(octets[i], 16);
                    temp = temp & 0xffffffff;
                    decimalOctaves[i] = (Int32)temp;
                }
                else if (octets[i].StartsWith("0"))
                {
                    //oct convert to a decimal
                    long temp = Convert.ToInt64(octets[i], 8);
                    temp = temp & 0xffffffff;
                    decimalOctaves[i] = (Int32)temp;
                    //decimalOctaves[i] = Convert.ToInt32(octets[i], 8);
                }
                else
                    decimalOctaves[i] = Int32.Parse(octets[i]);
            }

            int combinedDecimal = 0;

            for (int i = 0; i < decimalOctaves.Length; i++)
            {
                //only the last decimal can be expanded over 8bits
                //to make up the ip, the other components should
                //be restricted to 8bit in size

                if (i == decimalOctaves.Length - 1)
                {
                    //expand base on the number of other components
                    //decimalOctaves[i] = decimalOctaves[i] & (0x0fffffff);// / decimalOctaves.Length * 1);
                }
                else
                {
                    //restrict to an 8bit size
                    decimalOctaves[i] = decimalOctaves[i] & 0xff;
                }

                if (i != decimalOctaves.Length - 1)
                    combinedDecimal = (decimalOctaves[i] << (8 * (3 - i))) | combinedDecimal;
                else
                    combinedDecimal = decimalOctaves[i] | combinedDecimal;
            }

            byte[] ip = BitConverter.GetBytes(combinedDecimal);

            StringBuilder bufffer = new StringBuilder();
            //for (int i = 0; i < ip.Length; i++)
            //{
            //    int ipPart = ip[i];
            //    bufffer.Append(ipPart);
            //    if (i < 3)
            //        bufffer.Append('.');
            //}

            for (int i = ip.Length - 1; i >= 0; i--)
            {
                int ipPart = ip[i];
                bufffer.Append(ipPart);
                if (i > 0)
                    bufffer.Append('.');
            }

            return bufffer.ToString();
        }

        /// <summary>
        /// Extracts the hostname from a url. No trailing 
        /// slash will be present on the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HostName(string url)
        {
            return HostName(url, false);
        }


        /// <summary>
        /// Removes double dots from a host name, this method
        /// will also trim any trailing dots from the host name
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        private static string RemoveDoubleDotHost(string hostName)
        {
            //remove any double ".." in the domainpart
            while (hostName.Contains(".."))
            {
                hostName = hostName.Replace("..", ".");
            }
            //the start and end char shouldnt contain a dot
            if (hostName[0] == '.')
                hostName = hostName.Substring(1);
            if (hostName[hostName.Length - 1] == '.')
                hostName = hostName.Substring(0, hostName.Length - 1);

            return hostName;
        }

        /// <summary>
        /// clean the host name to remove any illegal chars, only
        /// alphanumeric, '.' and '-' should be allowed in the url.
        /// replace any illegal chars with hex escaped value
        /// </summary>
        /// <remarks>This will first fully unescape the hostname then
        /// re escape again</remarks>
        /// <param name="hostName"></param>
        /// <returns></returns>
        private static string CleanHostName(string hostName)
        {
            hostName = UnescapeHex(hostName);
            return EscapeHex(hostName, '.', '-');
        }      


        #endregion


        /// <summary>
        /// Unescape a url to remove all the hex escaped values.
        /// This method will repeatly unescape a string until it
        /// contains no more hex encoded values
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string UnescapeHex(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return null;

            //unescape then run again
            Regex unescape = new Regex(@"%(.{2})");

            while (unescape.IsMatch(inputString))
            {
                MatchCollection hexMatches = unescape.Matches(inputString);

                StringBuilder unescapedHostName = new StringBuilder();
                int hexIndex = 0;

                foreach (Match hexMatch in hexMatches)
                {
                    unescapedHostName.Append(inputString.Substring(hexIndex, hexMatch.Index - hexIndex));

                    string hexValue = hexMatch.Groups[1].Value;
                    int asciiInt = Int32.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                    unescapedHostName.Append((char)asciiInt);

                    hexIndex = hexMatch.Index + hexMatch.Length;
                }

                unescapedHostName.Append(inputString.Substring(hexIndex, inputString.Length - hexIndex));
                inputString = unescapedHostName.ToString();
            }

            return inputString;
        }

        /// <summary>
        /// Escape a string by only allowing alpha numeric and any
        /// characters suppplied in the allowedChars parameter
        /// </summary>
        /// <remarks>This required since we need to be really strict on
        /// the hostname to pass one of the google example test. 
        /// <para>http://google^.com/ = http://google%5E.com/</para>
        /// <para>If we simple escape char &lt;= ASCII 32, &gt;= 127
        /// then we fail the testcase. this escape should only be used for the host name part
        /// of the url</para></remarks>
        /// <param name="inputString"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        private static string EscapeHex(string inputString, params char[] allowedChars)
        {

            if (String.IsNullOrEmpty(inputString))
                return null;

            StringBuilder cleaned = new StringBuilder();

            foreach (char c in inputString)
            {
                if (Char.IsLetterOrDigit(c) | ArrayContainsChar(allowedChars, c))
                    cleaned.Append(c);
                else
                {
                    cleaned.Append('%');
                    cleaned.AppendFormat("{0:X2}", (int)c);
                }
            }

            return cleaned.ToString();
        }

        /// <summary>
        /// Escape any char that &lt;= ASCII 32, &gt;= 127
        /// into hex encoded values
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string EscapeHex(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return inputString;

            StringBuilder cleaned = new StringBuilder();

            foreach (char c in inputString)
            {
                int asciiValue = (int)c;
                if (asciiValue <= 32 | asciiValue >= 127)
                {
                    cleaned.Append('%');
                    cleaned.AppendFormat("{0:X2}", asciiValue);
                }
                else
                    cleaned.Append(c);
                                
            }

            return cleaned.ToString();
        }

        /// <summary>
        /// Help method to determine if char existing in an array
        /// of characters
        /// </summary>
        /// <param name="array"></param>
        /// <param name="toMatch"></param>
        /// <returns></returns>
        private static bool ArrayContainsChar(char[] array, char toMatch)
        {
            foreach (char c in array)
            {
                if (c == toMatch)
                    return true;
            }

            return false;
        }
    }
}
