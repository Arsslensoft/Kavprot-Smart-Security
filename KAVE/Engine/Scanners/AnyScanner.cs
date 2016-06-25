using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using System.IO;

namespace KAVE.Engine
{
   public class AnyScanner : IScanner
    {
        public string Name
        {
            get { return "ANY-SCANNER"; }
        }
        public int MaximumSize
        {
            get { return 100000000; }
        }
        public object ScanHS(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Length < MaximumSize)
            {
              return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
            }
            else
                return null;
        }
        public object Scan(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Length < MaximumSize)
                return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
            else
                return null;
        }
        public object ScanM(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Length < MaximumSize)
                       return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
              else
                return null;
        }

        public object ScanHS(string filename, System.Windows.Forms.Label lb)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Length < MaximumSize)
            {
                                return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
            }
            else
                return null;
        }
        public object Scan(string filename, System.Windows.Forms.Label lb)
        {
            return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
        }
        public object ScanM(string filename, System.Windows.Forms.Label lb)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Length < MaximumSize)
                return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
            else
                return null;
        }

        public bool Repair(Virus virus)
        {
            return false;
        }

    }
}
