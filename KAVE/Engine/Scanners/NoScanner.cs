using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;

namespace KAVE.Engine
{
    public class NoScanner : IScanner
    {
        public string Name
        {
            get { return "NO-SCANNER"; }
        }
        public int MaximumSize
        {
            get { return 0; }
        }

        public object ScanHS(string filename)
        {
            return null;
        }
        public object Scan(string filename)
        {
            return null;
        }
        public object ScanM(string filename)
        {
            return null;
        }

        public object ScanHS(string filename, System.Windows.Forms.Label lb)
        {
            return null;
        }
        public object Scan(string filename, System.Windows.Forms.Label lb)
        {
            return null;
        }
        public object ScanM(string filename, System.Windows.Forms.Label lb)
        {
            return null;
        }
        public bool Repair(Virus virus)
        {
            return false;
        }

    }
}
