using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KAVE.BaseEngine
{
    internal class FileSecurity
    {
        public FileSecurity()
        {
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }
        }
        internal string[] ACTReadAllLines(string filename)
        {
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Activation.Decrypt(File.ReadAllText(filename)));
            string[] lines = File.ReadAllLines(tempfolder + "axfdghhuii.avdb");
            File.Delete(tempfolder + "axfdghhuii.avdb");
            return lines;
        }
        internal void ACTWriteAllLines(string[] lines, string filename)
        {

            using (StreamWriter str = new StreamWriter(tempfolder + "axfdghhuii.avdb", true))
            {
                foreach (string line in lines)
                {
                    str.WriteLine(line);
                }
                str.Close();
            }
            string scontent = File.ReadAllText(tempfolder + "axfdghhuii.avdb");
            string encrypted = Activation.Encrypt(scontent);
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", encrypted);
            File.Delete(filename);
            File.Move(tempfolder + "axfdghhuii.avdb", filename);
        }
        internal string ReadAllText(string filename)
        {
            return Security.Decrypt(File.ReadAllText(filename));
        }
        internal string tempfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + Security.GetMd5Hashofstring("AVDefender") + @"\";
        internal string[] ReadAllLines(string filename)
        {
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Security.Decrypt(File.ReadAllText(filename)));
            string[] lines = File.ReadAllLines(tempfolder + "axfdghhuii.avdb");
            File.Delete(tempfolder + "axfdghhuii.avdb");
            return lines;
        }
        internal void WriteAllLines(string[] lines, string filename)
        {
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Security.Decrypt(File.ReadAllText(filename)));

            using (StreamWriter str = new StreamWriter(tempfolder + "axfdghhuii.avdb", true))
            {
                foreach (string line in lines)
                {
                    str.WriteLine(line);
                }
                str.Close();
            }
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Security.Encrypt(File.ReadAllText(tempfolder + "axfdghhuii.avdb")));
            File.Delete(filename);
            File.Move(tempfolder + "axfdghhuii.avdb", filename);
        }
        internal void WriteList(List<string> items, string filename)
        {
     
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Security.Decrypt(File.ReadAllText(filename)));

            using (StreamWriter str = new StreamWriter(tempfolder + "axfdghhuii.avdb", true))
            {
                foreach (string line in items)
                {
                    str.WriteLine(line);
                }
                str.Close();
            }
            File.WriteAllText(tempfolder + "axfdghhuii.avdb", Security.Encrypt(File.ReadAllText(tempfolder + "axfdghhuii.avdb")));
            File.Delete(filename);
            File.Move(tempfolder + "axfdghhuii.avdb", filename);
        }


    }
}
