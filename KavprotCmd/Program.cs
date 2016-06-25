using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using KAVE;
using KAVE.BaseEngine;
using KAVE.Engine;


namespace KavprotCmd
{
    class Program
    {
        public class Arguments
        {
            // Variables

            private StringDictionary Parameters;

            // Constructor

            public Arguments(string[] Args)
            {
                Parameters = new StringDictionary();
                Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

                Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

                string Parameter = null;
                string[] Parts;

                // Valid parameters forms:

                // {-,/,--}param{ ,=,:}((",')value(",'))

                // Examples: 

                // -param1 value1 --param2 /param3:"Test-:-work" 

                //   /param4=happy -param5 '--=nice=--'

                foreach (string Txt in Args)
                {
                    // Look for new parameters (-,/ or --) and a

                    // possible enclosed value (=,:)

                    Parts = Spliter.Split(Txt, 3);

                    switch (Parts.Length)
                    {
                        // Found a value (for the last parameter 

                        // found (space separator))

                        case 1:
                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                {
                                    Parts[0] =
                                        Remover.Replace(Parts[0], "$1");

                                    Parameters.Add(Parameter, Parts[0]);
                                }
                                Parameter = null;
                            }
                            // else Error: no parameter waiting for a value (skipped)

                            break;

                        // Found just a parameter

                        case 2:
                            // The last parameter is still waiting. 

                            // With no value, set it to true.

                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                    Parameters.Add(Parameter, "true");
                            }
                            Parameter = Parts[1];
                            break;

                        // Parameter with enclosed value

                        case 3:
                            // The last parameter is still waiting. 

                            // With no value, set it to true.

                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                    Parameters.Add(Parameter, "true");
                            }

                            Parameter = Parts[1];

                            // Remove possible enclosing characters (",')

                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[2] = Remover.Replace(Parts[2], "$1");
                                Parameters.Add(Parameter, Parts[2]);
                            }

                            Parameter = null;
                            break;
                    }
                }
                // In case a parameter is still waiting

                if (Parameter != null)
                {
                    if (!Parameters.ContainsKey(Parameter))
                        Parameters.Add(Parameter, "true");
                }
            }

            // Retrieve a parameter value if it exists 

            // (overriding C# indexer property)

            public string this[string Param]
            {
                get
                {
                    return (Parameters[Param]);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Copyright (c) 2010-2012 Arsslensoft. All rights reserved");
            Console.WriteLine("Copyright (c) 2010-2012 Arsslensoft Labs. All rights reserved");
            Console.WriteLine("______________________________________________________________");

            Arguments CommandLine = new Arguments(args);
            if (CommandLine["scan"] != null)
            {
                string filename = CommandLine["scan"];
                if (File.Exists(filename))
                {

                    KavprotManager.Initialize(KavprotInitialization.Engine);
                    Console.WriteLine("Kavprot Antivirus Engine Initialized.");
                    Console.WriteLine(filename + " Ready.");
                    object s = FileFormat.GetFileFormat(filename).ScanHS(filename);
                   if (s != null)
                   {
                       Console.WriteLine(filename + " infected with " + s);
                   }
                   else
                   {
                       Console.WriteLine(filename + " is Safe ");
                   }
                   
                }
                else
                {

                }
            }
            else if (CommandLine["scanpath"] != null)
            {
                string dirname = CommandLine["scanpath"];
                if (Directory.Exists(dirname))
                {

                    KavprotManager.Initialize(KavprotInitialization.Engine);
                    Console.WriteLine("Kavprot Antivirus Engine Initialized.");
                    Console.WriteLine(dirname + " Ready.");
                    List<string> sd = FileHelper.GetFilesRecursive(dirname);
                    foreach (string filename in sd)
                    {
                         object s = FileFormat.GetFileFormat(filename).ScanHS(filename);
                        if (s != null)
                        {
                            Console.WriteLine(filename + " infected with " + s);
                        }
                        else
                        {
                            Console.WriteLine(filename + " is Safe ");
                        }

                    }
                    Console.WriteLine(sd.Count + " file scanned ");
                }
                else
                {

                }
            }
            else
            {

            }
            Console.Read();
        }
    }
}
