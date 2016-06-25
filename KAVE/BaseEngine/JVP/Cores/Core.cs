using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JVP
{
   public class JVPCore : Core
    {
       public string ProductID
       {
           get { return "Arsslensoft KavprotAI Virtual Processor Core @ 23000 IPS"; }
       }
       public int Speed
       {
           get { return 83; }
       }
       public int Transfert
       {
           get { return 1024; }
       }
       JVPAssembly _asm;
      public JVPAssembly JVPASM
       {
           get { return _asm; }
       }
       public void Initialize()
       {
           _asm = new JVPAssembly(this);
       }
       public void Dispose()
       {

       }
       public event JVPSignal OnNewSignal;
     /// <summary>
     /// Send a signal to the corresponding output
     /// </summary>
     /// <param name="Output">Output control</param>
     /// <param name="data">data to be sent</param>
      public void SendSignal(string Output, object data)
       {
           if (OnNewSignal != null)
               OnNewSignal(Output, data);
       }

      public object ExecuteInstruction(object data, string Operand)
      {
          switch (Operand)
          {
              case "EVAL":
                  JVPASM.Expr.Expression = data.ToString();
                 return JVPASM.Expr.Evaluate();

              case "SAYT":
                 SendSignal("SPEECH", data);
                 return true;
              case "MSGB":
                 MessageBox.Show(data.ToString());
                 return true;
              case "SDNG":
                 SendSignal(data.ToString().Split(',')[0], data.ToString().Split(',')[1]);
                 return true;
                 
          }
          return false;
      }

     public object ExecuteAssembly(string assembly, object data)
      {
          return JVPASM.ExecuteASM(assembly, data);
      }

       // random
     public int MakeRandomDecision(int start, int end)
     {
         Random rand = new Random();
        return rand.Next(start, end);
     }

     public double RandomPropability()
     {
         Random rand = new Random();

         return rand.NextDouble();
     }

    }
}
