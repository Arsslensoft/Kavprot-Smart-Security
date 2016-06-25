using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using JVP.Types;
using System.Text.RegularExpressions;
using KAVE.BaseEngine;

namespace JVP
{
   public static class CoreManager
    {
       public static List<Core> Cores;
       public static void Initialize()
       {
           try
           {
               Cores = new List<Core>();
               // initialize as computer core number
               for (int i = 1; i == Environment.ProcessorCount; )
               {
                   Cores.Add(new JVPCore());
                   i++;
               }
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       public static int i = 0;
       public static Core SelectCore()
       {
           if (i < Cores.Count)
           {
               i++;
               return Cores[i-1];

           }
           else if (i == Cores.Count)
           {
               i = 1;
               return Cores[0];
           }
           else
           {
               i = 1;
               return Cores[0];
           }

       }
       public static void SendSignal(string Output, object data)
       {
           SelectCore().SendSignal(Output, data);
       }
       public static object ExecuteInstruction(object data, string Operand)
       {
                   return SelectCore().ExecuteInstruction(data, Operand);
       }
       public static object ExecuteInstruction(object data)
       {
           string[] d = data.ToString().Split('=');
           return SelectCore().ExecuteInstruction(d[1], d[0]);
       }
       public static object ExecuteAssembly(string asm, object data)
       {
           return SelectCore().ExecuteAssembly(asm, data);
       }
    }
}
