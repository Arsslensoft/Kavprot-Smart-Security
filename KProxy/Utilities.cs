namespace KProxy
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class Utilities
    {
        internal const int MOD_ALT = 1;
        internal const int MOD_CONTROL = 2;
        internal const int MOD_SHIFT = 4;
        internal const int MOD_WIN = 8;
        private static Encoding[] sniffableEncodings = new Encoding[] { Encoding.UTF32, Encoding.BigEndianUnicode, Encoding.Unicode, Encoding.UTF8 };
        internal const int WM_COPYDATA = 0x4a;
        internal const int WM_HOTKEY = 0x312;
        internal const int WM_SIZE = 5;

        private Utilities()
        {
        }

        private static void _WriteChunkSizeToStream(MemoryStream oMS, int iLen)
        {
            string s = iLen.ToString("x");
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            oMS.Write(bytes, 0, bytes.Length);
        }

        private static void _WriteCRLFToStream(MemoryStream oMS)
        {
            oMS.WriteByte(13);
            oMS.WriteByte(10);
        }

        public static bool areOriginsEquivalent(string sOrigin1, string sOrigin2, int iDefaultPort)
        {
            string str;
            string str3;
            if (string.Equals(sOrigin1, sOrigin2, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            int iPort = iDefaultPort;
            CrackHostAndPort(sOrigin1, out str, ref iPort);
            string a = str + ":" + iPort.ToString();
            iPort = iDefaultPort;
            CrackHostAndPort(sOrigin2, out str3, ref iPort);
            string b = str3 + ":" + iPort.ToString();
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        [CodeDescription("Returns a string representing a Hex view of a byte array. Slow.")]
        public static string ByteArrayToHexView(byte[] inArr, int iBytesPerLine)
        {
            return ByteArrayToHexView(inArr, iBytesPerLine, inArr.Length);
        }

        [CodeDescription("Returns a string representing a Hex view of a byte array. PERF: Slow.")]
        public static string ByteArrayToHexView(byte[] inArr, int iBytesPerLine, int iMaxByteCount)
        {
            if ((inArr == null) || (inArr.Length == 0))
            {
                return string.Empty;
            }
            if ((iBytesPerLine < 1) || (iMaxByteCount < 1))
            {
                throw new ArgumentOutOfRangeException("iBytesPerLine", "iBytesPerLine and iMaxByteCount must be >0");
            }
            iMaxByteCount = Math.Min(iMaxByteCount, inArr.Length);
            StringBuilder builder = new StringBuilder(iMaxByteCount * 5);
            int num = 0;
            bool flag = false;
            while (num < iMaxByteCount)
            {
                int num2 = Math.Min(iBytesPerLine, iMaxByteCount - num);
                flag = num2 < iBytesPerLine;
                for (int i = 0; i < num2; i++)
                {
                    builder.Append(inArr[num + i].ToString("X2"));
                    builder.Append(" ");
                }
                if (flag)
                {
                    builder.Append(new string(' ', 3 * (iBytesPerLine - num2)));
                }
                builder.Append(" ");
                for (int j = 0; j < num2; j++)
                {
                    if (inArr[num + j] < 0x20)
                    {
                        builder.Append(".");
                    }
                    else
                    {
                        builder.Append((char) inArr[num + j]);
                    }
                }
                if (flag)
                {
                    builder.Append(new string(' ', iBytesPerLine - num2));
                }
                builder.Append("\r\n");
                num += iBytesPerLine;
            }
            return builder.ToString();
        }

        [CodeDescription("Returns a string representing a Hex stream of a byte array. Slow.")]
        public static string ByteArrayToString(byte[] inArr)
        {
            if (inArr == null)
            {
                return "null";
            }
            if (inArr.Length == 0)
            {
                return "empty";
            }
            StringBuilder builder = new StringBuilder(inArr.Length * 3);
            for (int i = 0; i < inArr.Length; i++)
            {
                builder.Append(inArr[i].ToString("X2") + ' ');
            }
            return builder.ToString();
        }

        [CodeDescription("Returns a byte[] representing the bzip2'd representation of writeData[]")]
        public static byte[] bzip2Compress(byte[] writeData)
        {
            if ((writeData != null) && (writeData.Length != 0))
            {
                throw new NotSupportedException("This application was compiled without BZIP2 support.");
            }
            return new byte[0];
        }

        public static byte[] bzip2Expand(byte[] compressedData)
        {
            if ((compressedData != null) && (compressedData.Length != 0))
            {
                throw new NotSupportedException("This application was compiled without BZIP2 support.");
            }
            return new byte[0];
        }

        [CodeDescription("Convert a full path into one that uses environment variables, e.g. %SYSTEM%")]
        public static string CollapsePath(string sPath)
        {
            StringBuilder pszBuf = new StringBuilder(0x103);
            if (PathUnExpandEnvStrings(sPath, pszBuf, pszBuf.Capacity))
            {
                return pszBuf.ToString();
            }
            return sPath;
        }

        public static int CompareVersions(string sRequiredVersion, Version verTest)
        {
            string[] strArray = sRequiredVersion.Split(new char[] { '.' });
            if (strArray.Length != 4)
            {
                return 5;
            }
            VersionStruct struct2 = new VersionStruct();
            if ((!int.TryParse(strArray[0], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out struct2.Major) || !int.TryParse(strArray[1], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out struct2.Minor)) || (!int.TryParse(strArray[2], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out struct2.Build) || !int.TryParse(strArray[3], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out struct2.Private)))
            {
                return 6;
            }
            if (struct2.Major > verTest.Major)
            {
                return 4;
            }
            if (verTest.Major > struct2.Major)
            {
                return -4;
            }
            if (struct2.Minor > verTest.Minor)
            {
                return 3;
            }
            if (verTest.Minor > struct2.Minor)
            {
                return -3;
            }
            if (struct2.Build > verTest.Build)
            {
                return 2;
            }
            if (verTest.Build > struct2.Build)
            {
                return -2;
            }
            if (struct2.Private > verTest.Revision)
            {
                return 1;
            }
            if (verTest.Revision > struct2.Private)
            {
                return -1;
            }
            return 0;
        }

        internal static string ContentTypeForFileExtension(string sExtension)
        {
            if ((sExtension == null) || (sExtension.Length < 1))
            {
                return null;
            }
            if (sExtension == ".js")
            {
                return "text/javascript";
            }
            if (sExtension == ".css")
            {
                return "text/css";
            }
            string str = null;
            try
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(sExtension, false);
                if (key != null)
                {
                    str = (string) key.GetValue("Content Type");
                    key.Close();
                }
            }
            catch (SecurityException)
            {
            }
            return str;
        }

        [CodeDescription("Copy a string to the clipboard, with exception handling.")]
        public static bool CopyToClipboard(string sText)
        {
            DataObject oData = new DataObject();
            oData.SetData(DataFormats.Text, sText);
            return CopyToClipboard(oData);
        }

        public static bool CopyToClipboard(DataObject oData)
        {
            try
            {
                Clipboard.SetDataObject(oData, true);
                return true;
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("Please disable any clipboard monitoring tools and try again.\n\n" + exception.Message, ".NET Framework Bug");
                return true;
            }
        }

        [CodeDescription("This function cracks the Host/Port combo, removing IPV6 brackets if needed.")]
        public static void CrackHostAndPort(string sHostPort, out string sHostname, ref int iPort)
        {
            int length = sHostPort.LastIndexOf(':');
            if ((length > -1) && (length > sHostPort.LastIndexOf(']')))
            {
                if (!int.TryParse(sHostPort.Substring(length + 1), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out iPort))
                {
                    iPort = -1;
                }
                sHostname = sHostPort.Substring(0, length);
            }
            else
            {
                sHostname = sHostPort;
            }
            if (sHostname.StartsWith("[", StringComparison.Ordinal) && sHostname.EndsWith("]", StringComparison.Ordinal))
            {
                sHostname = sHostname.Substring(1, sHostname.Length - 2);
            }
        }

        [CodeDescription("Returns a byte[] containing a DEFLATE'd copy of writeData[]")]
        public static byte[] DeflaterCompress(byte[] writeData)
        {
            if ((writeData == null) || (writeData.Length == 0))
            {
                return new byte[0];
            }
            try
            {
                MemoryStream stream = new MemoryStream();
                using (DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Compress))
                {
                    stream2.Write(writeData, 0, writeData.Length);
                }
                return stream.ToArray();
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("The content could not be compressed.\n\n" + exception.Message, "KProxy: Deflation failed");
                return writeData;
            }
        }

        [CodeDescription("Returns a byte[] representing the INFLATE'd representation of compressedData[]")]
        public static byte[] DeflaterExpand(byte[] compressedData)
        {
            try
            {
                return DeflaterExpandInternal(KPCONFIG.bUseXceedDecompressForDeflate, compressedData);
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("The content could not be decompressed.\n\n" + exception.Message, "KProxy: Inflation failed");
                return new byte[0];
            }
        }

        public static byte[] DeflaterExpandInternal(bool bUseXceed, byte[] compressedData)
        {
            if ((compressedData == null) || (compressedData.Length == 0))
            {
                return new byte[0];
            }
            int index = 0;
            if (((compressedData.Length > 2) && (compressedData[0] == 120)) && (compressedData[1] == 0x9c))
            {
                index = 2;
            }
            if (bUseXceed)
            {
                throw new NotSupportedException("This application was compiled without Xceed support.");
            }
            MemoryStream stream = new MemoryStream(compressedData, index, compressedData.Length - index);
            MemoryStream stream2 = new MemoryStream(compressedData.Length);
            using (DeflateStream stream3 = new DeflateStream(stream, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[0x8000];
                int count = 0;
                while ((count = stream3.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream2.Write(buffer, 0, count);
                }
            }
            return stream2.ToArray();
        }

        internal static string DescribeException(Exception eX)
        {
            StringBuilder builder = new StringBuilder(0x200);
            builder.Append(eX.Message);
            if (eX.InnerException != null)
            {
                builder.Append(" < ");
                builder.Append(eX.InnerException.Message);
            }
            return builder.ToString();
        }

        public static byte[] doChunk(byte[] writeData, int iSuggestedChunkCount)
        {
            if ((writeData == null) || (writeData.Length < 1))
            {
                return Encoding.ASCII.GetBytes("0\r\n\r\n");
            }
            if (iSuggestedChunkCount < 1)
            {
                iSuggestedChunkCount = 1;
            }
            if (iSuggestedChunkCount > writeData.Length)
            {
                iSuggestedChunkCount = writeData.Length;
            }
            MemoryStream oMS = new MemoryStream(writeData.Length + (10 * iSuggestedChunkCount));
            int offset = 0;
            do
            {
                int num2 = writeData.Length - offset;
                int num3 = num2 / iSuggestedChunkCount;
                num3 = Math.Max(1, num3);
                num3 = Math.Min(num2, num3);
                _WriteChunkSizeToStream(oMS, num3);
                _WriteCRLFToStream(oMS);
                oMS.Write(writeData, offset, num3);
                _WriteCRLFToStream(oMS);
                offset += num3;
                iSuggestedChunkCount--;
                if (iSuggestedChunkCount < 1)
                {
                    iSuggestedChunkCount = 1;
                }
            }
            while (offset < writeData.Length);
            _WriteChunkSizeToStream(oMS, 0);
            _WriteCRLFToStream(oMS);
            _WriteCRLFToStream(oMS);
            return oMS.ToArray();
        }

        public static byte[] doUnchunk(byte[] writeData)
        {
            if ((writeData == null) || (writeData.Length == 0))
            {
                return new byte[0];
            }
            MemoryStream stream = new MemoryStream(writeData.Length);
            int index = 0;
            bool flag = false;
            while (!flag && (index <= (writeData.Length - 3)))
            {
                int num3;
                string sInput = Encoding.ASCII.GetString(writeData, index, Math.Min(0x40, writeData.Length - index));
                int length = sInput.IndexOf("\r\n", StringComparison.Ordinal);
                if (length <= 0)
                {
                    throw new InvalidDataException("HTTP Error: The chunked content is corrupt. Cannot find Chunk-Length in expected location. Offset: " + index.ToString());
                }
                index += length + 2;
                sInput = sInput.Substring(0, length);
                length = sInput.IndexOf(';');
                if (length > 0)
                {
                    sInput = sInput.Substring(0, length);
                }
                if (!TryHexParse(sInput, out num3))
                {
                    throw new InvalidDataException("HTTP Error: The chunked content is corrupt. Chunk-Length was malformed." + index.ToString());
                }
                if (num3 == 0)
                {
                    flag = true;
                }
                else
                {
                    if (writeData.Length < (num3 + index))
                    {
                        throw new InvalidDataException("HTTP Error: The chunked entity body is corrupt. The final chunk length is greater than the number of bytes remaining.");
                    }
                    stream.Write(writeData, index, num3);
                    index += num3 + 2;
                }
            }
            if ((!flag && KPCONFIG.bReportHTTPErrors) && !KPCONFIG.QuietMode)
            {
                KProxyApplication.DoNotifyUser("Chunked body did not terminate properly with 0-sized chunk.", "HTTP Protocol Violation");
            }
            byte[] dst = new byte[stream.Length];
            Buffer.BlockCopy(stream.GetBuffer(), 0, dst, 0, dst.Length);
            return dst;
        }

        public static void EnsureOverwritable(string sFilename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFilename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sFilename));
            }
            if (System.IO.File.Exists(sFilename))
            {
                FileAttributes attributes = System.IO.File.GetAttributes(sFilename);
                System.IO.File.SetAttributes(sFilename, attributes & ~(FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly));
            }
        }

        internal static string EnsurePathIsAbsolute(string sRootPath, string sFilename)
        {
            try
            {
                if (!Path.IsPathRooted(sFilename))
                {
                    sFilename = sRootPath + sFilename;
                }
            }
            catch
            {
            }
            return sFilename;
        }

        internal static bool KProxyMeetsVersionRequirement(Assembly assemblyInput, string sWhatType)
        {
            if (!assemblyInput.IsDefined(typeof(RequiredVersionAttribute), false))
            {
                return false;
            }
            RequiredVersionAttribute customAttribute = (RequiredVersionAttribute) Attribute.GetCustomAttribute(assemblyInput, typeof(RequiredVersionAttribute));
            int num = CompareVersions(customAttribute.RequiredVersion, KPCONFIG.KProxyVersionInfo);
            if (num > 0)
            {
                KProxyApplication.DoNotifyUser(string.Format("The {0} in {1} require KProxy v{2} or later. (You have v{3})\n\nPlease install the latest version of KProxy from http://www.KProxy2.com.\n\nCode: {4}", new object[] { sWhatType, assemblyInput.CodeBase, customAttribute.RequiredVersion, KPCONFIG.KProxyVersionInfo, num }), "Extension Not Loaded");
                return false;
            }
            return true;
        }

        internal static string FileExtensionForMIMEType(string sMIME)
        {
            sMIME = sMIME.ToLower();
            switch (sMIME)
            {
                case "text/css":
                    return ".css";

                case "text/html":
                    return ".htm";

                case "text/javascript":
                case "application/javascript":
                case "application/x-javascript":
                    return ".js";

                case "image/jpg":
                case "image/jpeg":
                    return ".jpg";

                case "image/gif":
                    return ".gif";

                case "image/png":
                    return ".png";

                case "image/x-icon":
                    return ".ico";

                case "text/xml":
                    return ".xml";

                case "video/x-flv":
                    return ".flv";

                case "video/mp4":
                    return ".mp4";
            }
            return ".txt";
        }

        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);
        public static string GetCommaTokenValue(string sString, string sTokenName)
        {
            string str = null;
            if ((sString != null) && (sString.Length > 0))
            {
                System.Text.RegularExpressions.Match match = new Regex(sTokenName + "\\s?=?\\s?[\"]?(?<TokenValue>[^\";,]*)", RegexOptions.IgnoreCase).Match(sString);
                if (match.Success && (match.Groups["TokenValue"] != null))
                {
                    str = match.Groups["TokenValue"].Value;
                }
            }
            return str;
        }

        [CodeDescription("Gets (via Headers or Sniff) the provided body's text Encoding. Returns KPCONFIG.oHeaderEncoding (usually UTF-8) if unknown. Potentially slow.")]
        public static Encoding getEntityBodyEncoding(HTTPHeaders oHeaders, byte[] oBody)
        {
            if (oHeaders == null)
            {
                return KPCONFIG.oHeaderEncoding;
            }
            string tokenValue = oHeaders.GetTokenValue("Content-Type", "charset");
            if (tokenValue != null)
            {
                try
                {
                    return Encoding.GetEncoding(tokenValue);
                }
                catch
                {
                }
            }
            Encoding oHeaderEncoding = KPCONFIG.oHeaderEncoding;
            if ((oBody != null) && (oBody.Length >= 2))
            {
                foreach (Encoding encoding2 in sniffableEncodings)
                {
                    byte[] preamble = encoding2.GetPreamble();
                    if (oBody.Length >= preamble.Length)
                    {
                        bool flag = preamble.Length > 0;
                        for (int i = 0; i < preamble.Length; i++)
                        {
                            if (preamble[i] != oBody[i])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            oHeaderEncoding = encoding2;
                            break;
                        }
                    }
                }
                if (oHeaders.ExistsAndContains("Content-Type", "html"))
                {
                    string input = oHeaderEncoding.GetString(oBody, 0, Math.Min(0x1000, oBody.Length));
                    MatchCollection matchs = new Regex("<meta.*charset\\s*=\\s*(?<thecharset>[^\\\"]*)", RegexOptions.IgnoreCase).Matches(input);
                    if ((matchs.Count <= 0) || (matchs[0].Groups.Count <= 0))
                    {
                        return oHeaderEncoding;
                    }
                    try
                    {
                        oHeaderEncoding = Encoding.GetEncoding(matchs[0].Groups[1].Value);
                    }
                    catch
                    {
                    }
                }
            }
            return oHeaderEncoding;
        }

        [CodeDescription("Run an executable, wait for it to exit, and return its output as a string.")]
        public static string GetExecutableOutput(string sExecute, string sParams, out int iExitCode)
        {
            iExitCode = -999;
            StringBuilder builder = new StringBuilder();
            builder.Append("Results from " + sExecute + " " + sParams + "\r\n\r\n");
            try
            {
                string str;
                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = sExecute;
                process.StartInfo.Arguments = sParams;
                process.Start();
                while ((str = process.StandardOutput.ReadLine()) != null)
                {
                    str = str.TrimEnd(new char[0]);
                    if (str != string.Empty)
                    {
                        builder.Append(str + "\r\n");
                    }
                }
                iExitCode = process.ExitCode;
                process.Dispose();
            }
            catch (Exception exception)
            {
                builder.Append("Exception thrown: " + exception.ToString() + "\r\n" + exception.StackTrace.ToString());
            }
            builder.Append("-------------------------------------------\r\n");
            return builder.ToString();
        }

        internal static string GetFirstLocalResponse(string sFilename)
        {
            try
            {
                if (!Path.IsPathRooted(sFilename))
                {
                    string str = sFilename;
                    sFilename = KPCONFIG.GetPath("TemplateResponses") + str;
                    if (!System.IO.File.Exists(sFilename))
                    {
                        sFilename = KPCONFIG.GetPath("Responses") + str;
                    }
                }
            }
            catch
            {
            }
            return sFilename;
        }

        [CodeDescription("Returns an bool from the registry, or bDefault if the registry key is missing or cannot be used as an bool.")]
        public static bool GetRegistryBool(RegistryKey oReg, string sName, bool bDefault)
        {
            bool flag = bDefault;
            object obj2 = oReg.GetValue(sName);
            if (obj2 is int)
            {
                return (1 == ((int) obj2));
            }
            if (obj2 is string)
            {
                flag = string.Equals(obj2 as string, "true", StringComparison.OrdinalIgnoreCase);
            }
            return flag;
        }

        [CodeDescription("Returns an float from the registry, or flDefault if the registry key is missing or cannot be used as an float.")]
        public static float GetRegistryFloat(RegistryKey oReg, string sName, float flDefault)
        {
            float result = flDefault;
            object obj2 = oReg.GetValue(sName);
            if (obj2 is int)
            {
                return (float) obj2;
            }
            if ((obj2 is string) && !float.TryParse((string) obj2, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                result = flDefault;
            }
            return result;
        }

        [CodeDescription("Returns an integer from the registry, or iDefault if the registry key is missing or cannot be used as an integer.")]
        public static int GetRegistryInt(RegistryKey oReg, string sName, int iDefault)
        {
            int result = iDefault;
            object obj2 = oReg.GetValue(sName);
            if (obj2 is int)
            {
                return (int) obj2;
            }
            if ((obj2 is string) && !int.TryParse((string) obj2, out result))
            {
                return iDefault;
            }
            return result;
        }

        [CodeDescription("Gets (via Headers or Sniff) the Response Text Encoding. Returns KPCONFIG.oHeaderEncoding (usually UTF-8) if unknown. Potentially slow.")]
        public static Encoding getResponseBodyEncoding(Session oSession)
        {
            if ((oSession != null) && (oSession.bHasResponse && (oSession.oResponse.headers != null)))
            {
                return getEntityBodyEncoding(oSession.oResponse.headers, oSession.responseBodyBytes);
            }
            return KPCONFIG.oHeaderEncoding;
        }

        [CodeDescription("Gets a string from a byte-array, stripping a BOM if present.")]
        public static string GetStringFromArrayRemovingBOM(byte[] arrInput, Encoding oDefaultEncoding)
        {
            if ((arrInput != null) && (arrInput.Length >= 2))
            {
                foreach (Encoding encoding in sniffableEncodings)
                {
                    byte[] preamble = encoding.GetPreamble();
                    if (arrInput.Length >= preamble.Length)
                    {
                        bool flag = preamble.Length > 0;
                        for (int i = 0; i < preamble.Length; i++)
                        {
                            if (preamble[i] != arrInput[i])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            int length = encoding.GetPreamble().Length;
                            return encoding.GetString(arrInput, length, arrInput.Length - length);
                        }
                    }
                }
            }
            return oDefaultEncoding.GetString(arrInput);
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        internal static extern IntPtr GlobalFree(IntPtr hMem);
        internal static void GlobalFreeIfNonZero(IntPtr hMem)
        {
            if (IntPtr.Zero != hMem)
            {
                GlobalFree(hMem);
            }
        }

        [CodeDescription("Returns a byte[] containing a gzip-compressed copy of writeData[]")]
        public static byte[] GzipCompress(byte[] writeData)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress))
                {
                    stream2.Write(writeData, 0, writeData.Length);
                }
                return stream.ToArray();
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("The content could not be compressed.\n\n" + exception.Message, "KProxy: GZip failed");
                return writeData;
            }
        }

        [CodeDescription("Returns a byte[] containing an un-gzipped copy of compressedData[]")]
        public static byte[] GzipExpand(byte[] compressedData)
        {
            try
            {
                return GzipExpandInternal(KPCONFIG.bUseXceedDecompressForGZIP, compressedData);
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("The content could not be decompressed.\n\n" + exception.Message, "KProxy: UnGZip failed");
                return new byte[0];
            }
        }

        public static byte[] GzipExpandInternal(bool bUseXceed, byte[] compressedData)
        {
            if ((compressedData == null) || (compressedData.Length == 0))
            {
                return new byte[0];
            }
            MemoryStream stream = new MemoryStream(compressedData);
            MemoryStream stream2 = new MemoryStream(compressedData.Length);
            if (bUseXceed)
            {
                throw new NotSupportedException("This application was compiled without Xceed support.");
            }
            using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[0x8000];
                int count = 0;
                while ((count = stream3.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream2.Write(buffer, 0, count);
                }
            }
            return stream2.ToArray();
        }

        public static string HtmlEncode(string sInput)
        {
            if (sInput == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(sInput.Length);
            int length = sInput.Length;
            for (int i = 0; i < length; i++)
            {
                switch (sInput[i])
                {
                    case '<':
                    {
                        builder.Append("&lt;");
                        continue;
                    }
                    case '>':
                    {
                        builder.Append("&gt;");
                        continue;
                    }
                    case '&':
                    {
                        builder.Append("&amp;");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("&quot;");
                        continue;
                    }
                }
                if (sInput[i] > '\x009f')
                {
                    builder.Append("&#");
                    builder.Append(((int) sInput[i]).ToString(NumberFormatInfo.InvariantInfo));
                    builder.Append(";");
                }
                else
                {
                    builder.Append(sInput[i]);
                }
            }
            return builder.ToString();
        }

        [CodeDescription("Returns TRUE if the HTTP Method MAY have a body.")]
        public static bool HTTPMethodAllowsBody(string sMethod)
        {
            if (((!("POST" == sMethod) && !("PUT" == sMethod)) && (!("PROPPATCH" == sMethod) && !("LOCK" == sMethod))) && !("PROPFIND" == sMethod))
            {
                return ("SEARCH" == sMethod);
            }
            return true;
        }

        [CodeDescription("Returns TRUE if the HTTP Method MUST have a body.")]
        public static bool HTTPMethodRequiresBody(string sMethod)
        {
            return ("PROPPATCH" == sMethod);
        }

        public static IPEndPoint IPEndPointFromHostPortString(string sHostAndPort)
        {
            if ((sHostAndPort == null) || sHostAndPort.Trim().Equals(string.Empty))
            {
                return null;
            }
            sHostAndPort = TrimAfter(sHostAndPort, ';');
            try
            {
                string str;
                int iPort = 80;
                CrackHostAndPort(sHostAndPort, out str, ref iPort);
                return new IPEndPoint(DNSResolver.GetIPAddress(str, true), iPort);
            }
            catch
            {
                return null;
            }
        }

        [CodeDescription("This function attempts to be a ~fast~ way to return an IP from a hoststring that contains an IP-Literal. ")]
        public static IPAddress IPFromString(string sHost)
        {
            for (int i = 0; i < sHost.Length; i++)
            {
                if (((((sHost[i] != '.') && (sHost[i] != ':')) && ((sHost[i] < '0') || (sHost[i] > '9'))) && ((sHost[i] < 'A') || (sHost[i] > 'F'))) && ((sHost[i] < 'a') || (sHost[i] > 'f')))
                {
                    return null;
                }
            }
            if (sHost.EndsWith("."))
            {
                sHost = TrimBeforeLast(sHost, '.');
            }
            try
            {
                return IPAddress.Parse(sHost);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsBinaryMIME(string sContentType)
        {
            if (string.IsNullOrEmpty(sContentType))
            {
                return false;
            }
            if (sContentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (sContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return !sContentType.StartsWith("image/svg+xml", StringComparison.OrdinalIgnoreCase);
            }
            return (sContentType.StartsWith("application/octet", StringComparison.OrdinalIgnoreCase) || (sContentType.StartsWith("application/x-shockwave-flash", StringComparison.OrdinalIgnoreCase) || (sContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase) || sContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))));
        }

        public static bool IsBrowserProcessName(string sProcessName)
        {
            if (string.IsNullOrEmpty(sProcessName))
            {
                return false;
            }
            if (((!sProcessName.StartsWith("ie", StringComparison.OrdinalIgnoreCase) && !sProcessName.StartsWith("firefox", StringComparison.OrdinalIgnoreCase)) && (!sProcessName.StartsWith("chrome", StringComparison.OrdinalIgnoreCase) && !sProcessName.StartsWith("opera", StringComparison.OrdinalIgnoreCase))) && !sProcessName.StartsWith("webkit", StringComparison.OrdinalIgnoreCase))
            {
                return sProcessName.StartsWith("safari", StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        internal static bool IsChunkedBodyComplete(Session m_session, MemoryStream oData, long iStartAtOffset, out long outStartOfLatestChunk, out long outEndOfEntity)
        {
            int index = (int) iStartAtOffset;
            outStartOfLatestChunk = index;
            outEndOfEntity = -1L;
            byte[] bytes = oData.GetBuffer();
            long length = oData.Length;
            while (index < length)
            {
                outStartOfLatestChunk = index;
                string sInput = Encoding.ASCII.GetString(bytes, index, Math.Min(0x40, ((int) length) - index));
                int num3 = sInput.IndexOf("\r\n", StringComparison.Ordinal);
                if (num3 > -1)
                {
                    index += num3 + 2;
                    sInput = sInput.Substring(0, num3);
                }
                else
                {
                    return false;
                }
                num3 = sInput.IndexOf(';');
                if (num3 > -1)
                {
                    sInput = sInput.Substring(0, num3);
                }
                int iOutput = 0;
                if (!TryHexParse(sInput, out iOutput))
                {
                    SessionFlags flagViolation = (m_session.state <= SessionStates.ReadingRequest) ? SessionFlags.ProtocolViolationInRequest : SessionFlags.ProtocolViolationInResponse;
                    KProxyApplication.HandleHTTPError(m_session, flagViolation, true, true, "Illegal chunked encoding. '" + sInput + "' is not a hexadecimal number.");
                    return true;
                }
                if (iOutput == 0)
                {
                    bool flag = true;
                    bool flag2 = false;
                    if ((index + 2) <= length)
                    {
                        for (int i = bytes[index++]; index <= length; i = bytes[index++])
                        {
                            int num6 = i;
                            if (num6 != 10)
                            {
                                if (num6 != 13)
                                {
                                    goto Label_010F;
                                }
                                flag2 = true;
                            }
                            else if (flag2)
                            {
                                if (flag)
                                {
                                    outEndOfEntity = index;
                                    return true;
                                }
                                flag = true;
                                flag2 = false;
                            }
                            else
                            {
                                flag2 = false;
                                flag = false;
                            }
                            continue;
                        Label_010F:
                            flag2 = false;
                            flag = false;
                        }
                    }
                    return false;
                }
                index += iOutput + 2;
            }
            return false;
        }

        internal static bool isHTTP200Array(byte[] arrData)
        {
            return ((((((arrData.Length > 12) && (arrData[0] == 0x48)) && ((arrData[1] == 0x54) && (arrData[2] == 0x54))) && (((arrData[3] == 80) && (arrData[4] == 0x2f)) && ((arrData[5] == 0x31) && (arrData[6] == 0x2e)))) && ((arrData[9] == 50) && (arrData[10] == 0x30))) && (arrData[11] == 0x30));
        }

        [CodeDescription("Returns true if True if the sHostAndPort's host is 127.0.0.1, 'localhost', or ::1. Note that list is not complete.")]
        public static bool isLocalhost(string sHostAndPort)
        {
            string str;
            int iPort = 0;
            CrackHostAndPort(sHostAndPort, out str, ref iPort);
            return isLocalhostname(str);
        }

        [CodeDescription("Returns true if True if the sHostname is 127.0.0.1, 'localhost', or ::1. Note that list is not complete.")]
        public static bool isLocalhostname(string sHostname)
        {
            if ((!string.Equals(sHostname, "localhost", StringComparison.OrdinalIgnoreCase) && !string.Equals(sHostname, "localhost.", StringComparison.OrdinalIgnoreCase)) && !string.Equals(sHostname, "127.0.0.1", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(sHostname, "::1", StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        [CodeDescription("Returns false if Hostname contains any dots or colons.")]
        public static bool isPlainHostName(string sHostAndPort)
        {
            string str;
            int iPort = 0;
            CrackHostAndPort(sHostAndPort, out str, ref iPort);
            char[] anyOf = new char[] { '.', ':' };
            return (str.IndexOfAny(anyOf) < 0);
        }

        [CodeDescription("ShellExecutes the sURL.")]
        public static bool LaunchHyperlink(string sURL)
        {
            try
            {
                using (Process.Start(sURL))
                {
                }
                return true;
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("Your web browser is not correctly configured to launch hyperlinks.\n\nTo see this content, visit:\n\t" + sURL + "\n...in your web browser.\n\nError: " + exception.Message, "Error");
            }
            return false;
        }

        public static string ObtainOpenFilename(string sDialogTitle, string sFilter)
        {
            return ObtainOpenFilename(sDialogTitle, sFilter, null);
        }

        public static string ObtainOpenFilename(string sDialogTitle, string sFilter, string sInitialDirectory)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Title = sDialogTitle;
            dialog.Filter = sFilter;
            if (!string.IsNullOrEmpty(sInitialDirectory))
            {
                dialog.InitialDirectory = sInitialDirectory;
                dialog.RestoreDirectory = true;
            }
            dialog.CustomPlaces.Add(KPCONFIG.GetPath("Captures"));
            string fileName = null;
            if (DialogResult.OK == dialog.ShowDialog())
            {
                fileName = dialog.FileName;
            }
            dialog.Dispose();
            return fileName;
        }

        public static string ObtainSaveFilename(string sDialogTitle, string sFilter)
        {
            return ObtainSaveFilename(sDialogTitle, sFilter, null);
        }

        public static string ObtainSaveFilename(string sDialogTitle, string sFilter, string sInitialDirectory)
        {
            FileDialog dialog = new SaveFileDialog();
            dialog.Title = sDialogTitle;
            dialog.Filter = sFilter;
            if (!string.IsNullOrEmpty(sInitialDirectory))
            {
                dialog.InitialDirectory = sInitialDirectory;
                dialog.RestoreDirectory = true;
            }
            dialog.CustomPlaces.Add(KPCONFIG.GetPath("Captures"));
            string fileName = null;
            if (DialogResult.OK == dialog.ShowDialog())
            {
                fileName = dialog.FileName;
            }
            dialog.Dispose();
            return fileName;
        }

        [CodeDescription("Tokenize a string into tokens. Delimits on whitespace; \" marks are dropped unless preceded by \\ characters.")]
        public static string[] Parameterize(string sInput)
        {
            List<string> list = new List<string>();
            bool flag = false;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sInput.Length; i++)
            {
                switch (sInput[i])
                {
                    case ' ':
                    case '\t':
                    {
                        if (flag)
                        {
                            goto Label_00A1;
                        }
                        if ((builder.Length > 0) || ((i > 0) && (sInput[i - 1] == '"')))
                        {
                            list.Add(builder.ToString());
                            builder.Length = 0;
                        }
                        continue;
                    }
                    case '"':
                    {
                        if ((i <= 0) || (sInput[i - 1] != '\\'))
                        {
                            break;
                        }
                        builder.Remove(builder.Length - 1, 1);
                        builder.Append('"');
                        continue;
                    }
                    default:
                        goto Label_00B1;
                }
                flag = !flag;
                continue;
            Label_00A1:
                builder.Append(sInput[i]);
                continue;
            Label_00B1:
                builder.Append(sInput[i]);
            }
            if (builder.Length > 0)
            {
                list.Add(builder.ToString());
            }
            return list.ToArray();
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("shlwapi.dll", CharSet=CharSet.Auto)]
        internal static extern bool PathUnExpandEnvStrings(string pszPath, [Out] StringBuilder pszBuf, int cchBuf);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("winmm.dll", SetLastError=true)]
        internal static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf);
        [CodeDescription("Reads oStream until arrBytes is filled.")]
        public static int ReadEntireStream(Stream oStream, byte[] arrBytes)
        {
            int offset = 0;
            while (offset < arrBytes.LongLength)
            {
                offset += oStream.Read(arrBytes, offset, arrBytes.Length - offset);
            }
            return offset;
        }

        internal static string RegExEscape(string sString, bool bAddPrefixCaret, bool bAddSuffixDollarSign)
        {
            StringBuilder builder = new StringBuilder();
            if (bAddPrefixCaret)
            {
                builder.Append("^");
            }
            foreach (char ch in sString)
            {
                switch (ch)
                {
                    case '#':
                    case '$':
                    case '(':
                    case ')':
                    case '+':
                    case '.':
                    case '[':
                    case '\\':
                    case '^':
                    case '?':
                    case '{':
                    case '|':
                        builder.Append('\\');
                        break;

                    default:
                        if (ch == '*')
                        {
                            builder.Append('.');
                        }
                        break;
                }
                builder.Append(ch);
            }
            if (bAddSuffixDollarSign)
            {
                builder.Append('$');
            }
            return builder.ToString();
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        public static bool RunExecutable(string sExecute, string sParams)
        {
            try
            {
                using (Process.Start(sExecute, sParams))
                {
                }
                return true;
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser(string.Format("Failed to execute: {0}\nwith parameters: {1}\r\n\r\n{2}\r\n{3}", new object[] { sExecute, sParams, exception.Message, exception.StackTrace.ToString() }), "ShellExecute Failed");
            }
            return false;
        }

        [CodeDescription("Run an executable and wait for it to exit.")]
        public static bool RunExecutableAndWait(string sExecute, string sParams)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = sExecute;
                process.StartInfo.Arguments = sParams;
                process.Start();
                process.WaitForExit();
                process.Dispose();
                return true;
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser("KProxy Exception thrown: " + exception.ToString() + "\r\n" + exception.StackTrace.ToString(), "ShellExecute Failed");
                return false;
            }
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [CodeDescription("Save a string to the registry. Correctly handles null Value, saving as String.Empty.")]
        public static void SetRegistryString(RegistryKey oReg, string sName, string sValue)
        {
            if (sName != null)
            {
                if (sValue == null)
                {
                    sValue = string.Empty;
                }
                oReg.SetValue(sName, sValue);
            }
        }

        internal static string StringToCF_HTML(string inStr)
        {
            string str = "<HTML><HEAD><STYLE>.REQUEST { font: 8pt Courier New; color: blue;} .RESPONSE { font: 8pt Courier New; color: green;}</STYLE></HEAD><BODY>" + inStr + "</BODY></HTML>";
            string format = "Version:1.0\r\nStartHTML:{0:00000000}\r\nEndHTML:{1:00000000}\r\nStartFragment:{0:00000000}\r\nEndFragment:{1:00000000}\r\n";
            return (string.Format(format, format.Length - 0x10, (str.Length + format.Length) - 0x10) + str);
        }

        [CodeDescription("Returns the part of a string up to (but NOT including) the first instance of specified delimiter. If delim not found, returns entire string.")]
        public static string TrimAfter(string sString, char chDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            int index = sString.IndexOf(chDelim);
            if (index < 0)
            {
                return sString;
            }
            return sString.Substring(0, index);
        }

        public static string TrimAfter(string sString, int iMaxLength)
        {
            return TrimTo(sString, iMaxLength);
        }

        [CodeDescription("Returns the part of a string up to (but NOT including) the first instance of specified substring. If delim not found, returns entire string.")]
        public static string TrimAfter(string sString, string sDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            if (sDelim == null)
            {
                return sString;
            }
            int index = sString.IndexOf(sDelim);
            if (index < 0)
            {
                return sString;
            }
            return sString.Substring(0, index);
        }

        [CodeDescription("Returns the part of a string after (but NOT including) the first instance of specified delimiter. If delim not found, returns entire string.")]
        public static string TrimBefore(string sString, char chDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            int index = sString.IndexOf(chDelim);
            if (index < 0)
            {
                return sString;
            }
            return sString.Substring(index + 1);
        }

        [CodeDescription("Returns the part of a string after (but NOT including) the first instance of specified substring. If delim not found, returns entire string.")]
        public static string TrimBefore(string sString, string sDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            if (sDelim == null)
            {
                return sString;
            }
            int index = sString.IndexOf(sDelim);
            if (index < 0)
            {
                return sString;
            }
            return sString.Substring(index + sDelim.Length);
        }

        [CodeDescription("Returns the part of a string after (but not including) the last instance of specified delimiter. If delim not found, returns entire string.")]
        public static string TrimBeforeLast(string sString, char chDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            int num = sString.LastIndexOf(chDelim);
            if (num < 0)
            {
                return sString;
            }
            return sString.Substring(num + 1);
        }

        [CodeDescription("Returns the part of a string after (but not including) the last instance of specified substring. If delim not found, returns entire string.")]
        public static string TrimBeforeLast(string sString, string sDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            if (sDelim == null)
            {
                return sString;
            }
            int num = sString.LastIndexOf(sDelim);
            if (num < 0)
            {
                return sString;
            }
            return sString.Substring(num + sDelim.Length);
        }

        [CodeDescription("Returns the first iMaxLength or fewer characters from the target string.")]
        public static string TrimTo(string sString, int iMaxLength)
        {
            if (string.IsNullOrEmpty(sString))
            {
                return string.Empty;
            }
            if (iMaxLength >= sString.Length)
            {
                return sString;
            }
            return sString.Substring(0, iMaxLength);
        }

        [CodeDescription("Returns the part of a string after (and including) the first instance of specified substring. If delim not found, returns entire string.")]
        public static string TrimUpTo(string sString, string sDelim)
        {
            if (sString == null)
            {
                return string.Empty;
            }
            if (sDelim == null)
            {
                return sString;
            }
            int index = sString.IndexOf(sDelim);
            if (index < 0)
            {
                return sString;
            }
            return sString.Substring(index);
        }

        [CodeDescription("Try parsing the string for a Hex-formatted int. If it fails, return false and 0 in iOutput.")]
        public static bool TryHexParse(string sInput, out int iOutput)
        {
            return int.TryParse(sInput, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out iOutput);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public static string UrlEncode(string sInput)
        {
            return UrlEncodeChars(sInput, Encoding.UTF8);
        }

        public static string UrlEncode(string sInput, Encoding oEnc)
        {
            return UrlEncodeChars(sInput, oEnc);
        }

        private static string UrlEncodeChars(string str, Encoding oEnc)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            foreach (char ch in str)
            {
                if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || (((ch >= '0') && (ch <= '9')) || ((((ch == '-') || (ch == '.')) || ((ch == '(') || (ch == ')'))) || (((ch == '*') || (ch == '\'')) || ((ch == '_') || (ch == '!'))))))
                {
                    builder.Append(ch);
                }
                else if (ch == ' ')
                {
                    builder.Append("+");
                }
                else
                {
                    foreach (byte num in oEnc.GetBytes(new char[] { ch }))
                    {
                        builder.Append("%");
                        builder.Append(num.ToString("X2"));
                    }
                }
            }
            return builder.ToString();
        }

        public static string UrlPathEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            int index = str.IndexOf('?');
            if (index >= 0)
            {
                return (UrlPathEncode(str.Substring(0, index)) + str.Substring(index));
            }
            return UrlPathEncodeChars(str);
        }

        private static string UrlPathEncodeChars(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            foreach (char ch in str)
            {
                if ((ch > ' ') && (ch < '\x007f'))
                {
                    builder.Append(ch);
                }
                else if (ch < '!')
                {
                    builder.Append("%");
                    builder.Append(((byte) ch).ToString("X2"));
                }
                else
                {
                    foreach (byte num in Encoding.UTF8.GetBytes(new char[] { ch }))
                    {
                        builder.Append("%");
                        builder.Append(num.ToString("X2"));
                    }
                }
            }
            return builder.ToString();
        }

        public static void utilDecodeHTTPBody(HTTPHeaders oHeaders, ref byte[] arrBody)
        {
            if ((arrBody != null) && (arrBody.LongLength > 0L))
            {
                if (oHeaders.ExistsAndContains("Transfer-Encoding", "chunked"))
                {
                    arrBody = doUnchunk(arrBody);
                }
                if (oHeaders.ExistsAndContains("Transfer-Encoding", "gzip") || oHeaders.ExistsAndContains("Content-Encoding", "gzip"))
                {
                    arrBody = GzipExpand(arrBody);
                }
                if (oHeaders.ExistsAndContains("Transfer-Encoding", "deflate") || oHeaders.ExistsAndContains("Content-Encoding", "deflate"))
                {
                    arrBody = DeflaterExpand(arrBody);
                }
                if (oHeaders.ExistsAndContains("Transfer-Encoding", "bzip2") || oHeaders.ExistsAndContains("Content-Encoding", "bzip2"))
                {
                    arrBody = bzip2Expand(arrBody);
                }
            }
        }

        [CodeDescription("Writes arrBytes to a file, creating the target directory and overwriting if the file exists.")]
        public static void WriteArrayToFile(string sFilename, byte[] arrBytes)
        {
            EnsureOverwritable(sFilename);
            FileStream stream = new FileStream(sFilename, FileMode.Create, FileAccess.Write);
            if (arrBytes != null)
            {
                stream.Write(arrBytes, 0, arrBytes.Length);
            }
            stream.Close();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        internal struct SendDataStruct
        {
            public IntPtr dwData;
            public int cbData;
            public string strData;
        }

        [Flags]
        internal enum SoundFlags
        {
            SND_ALIAS = 0x10000,
            SND_ALIAS_ID = 0x110000,
            SND_ASYNC = 1,
            SND_FILENAME = 0x20000,
            SND_LOOP = 8,
            SND_MEMORY = 4,
            SND_NODEFAULT = 2,
            SND_NOSTOP = 0x10,
            SND_NOWAIT = 0x2000,
            SND_RESOURCE = 0x40004,
            SND_SYNC = 0
        }
    }
}

