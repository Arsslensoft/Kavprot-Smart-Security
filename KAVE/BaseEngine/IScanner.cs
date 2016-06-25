using System;
using System.Collections.Generic;
using System.Text;

namespace KAVE.BaseEngine
{
   public interface IScanner
    {
       int MaximumSize { get; }
       string Name { get; }
      
       object Scan(string filename);
       object ScanM(string filename);
       object ScanHS(string filename);

       object Scan(string filename, System.Windows.Forms.Label lb);
       object ScanM(string filename, System.Windows.Forms.Label lb);
       object ScanHS(string filename, System.Windows.Forms.Label lb);



       bool Repair(Virus virus);
       
    }
}
