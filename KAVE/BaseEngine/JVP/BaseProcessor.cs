using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
using JVP.ALN;

namespace JVP
{
    public struct Function
    {
        public string type;
        public string ASM;
        public string function;
        public object[] Params;
        public object Invoke()
        {
            if (ASM == "LOCAL")
            {
                Type tp = Type.GetType(type);
                MethodInfo mtd = tp.GetMethod(function, BindingFlags.Public | BindingFlags.Static);
                return mtd.Invoke(null, Params);
            }
            else
            {
                Assembly asm = Assembly.LoadFrom(ASM);
                Type tp = asm.GetType(type);
                MethodInfo mtd = tp.GetMethod(function, BindingFlags.Public | BindingFlags.Static);
                return mtd.Invoke(null, Params);

            }
        }
        public Function(string tp, string asm, string func, object[] parameter)
        {
            Params = parameter;
            type = tp;
            ASM = asm;
            function = func;
        }
    }

    /// <summary>
    /// JVP ASM Parser, Base JVP fuctionalitites 
    /// </summary>
   public class JVPAssembly
    {
       Core JVPCore;
       public JVPAssembly(Core core)
       {
           JVPCore = core;
           Variables = new Dictionary<string, object>();
           Splitter = new Regex(@"\r\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
           stack = new List<object>();
           Functions = new List<Function>();
           Expr = new JVP.ALN.ExpressionEval();
       }

       public Random randgen = new Random();
       public JVP.ALN.ExpressionEval Expr;
       public List<object> stack;
       public List<Function> Functions;
       public Dictionary<string,object> Variables;
       public Regex Splitter;
       /// <summary>
       /// Execute JVP Assembly Code
       /// </summary>
       /// <param name="casmcode">Assembly Code</param>
       /// <param name="data">Data to be processed</param>
       /// <returns>possible processed data</returns>
       public object ExecuteASM(string asmcode, object data)
       {
               object RETURN = null;
           try
           {
               string[] Instructions = Splitter.Split(asmcode.Replace("\r\n", "")); // all instructions
               Variables.Add("data", data);
               foreach (string ins in Instructions)
               {
                   if (ins != string.Empty && ins != null)
                   {
                       string opcode = ins.Substring(0, 4);
                       string operand = ins.Remove(0, 5);
                       switch (opcode)
                       {
                           case "DEVI":
                               Expr.SetVariable(operand.Split(',')[0], ConvertVal(operand.Split(',')[1], operand.Split(',')[2]));
                               break;
                           case "DGVI":

                               Variables.Add(operand.Split(',')[0], ConvertVal(operand.Split(',')[1], operand.Split(',')[2]));
                               break;
                           case "FILL":
                               if (Variables.ContainsKey(operand.Split(',')[0]))
                               {
                                   Variables[operand.Split(',')[0]] = ConvertVal(operand.Split(',')[1], operand.Split(',')[2]);
                               }
                               break;
                           case "PUSH":
                               if (Variables.ContainsKey(operand.Split(',')[0]))
                               {

                                   stack.Add(Variables[operand.Split(',')[0]]);
                               }
                               else
                               {

                                   stack.Add(ConvertVal(operand.Split(',')[0], operand.Split(',')[1]));
                               }
                               break;
                           case "CALL":
                               Functions.Add(CreateFunction(operand));
                               break;
                           case "POPV":
                               if (Variables.ContainsKey(operand))
                               {
                                   if (stack.Contains(Variables[operand]))
                                       stack.Remove(Variables[operand]);
                               }
                               else
                               {
                                   stack.Remove(operand);
                               }
                               break;

                           case "PRET":
                               string[] pret = operand.Split(',');
                               if (Variables.ContainsKey(pret[0]))
                               {
                                   RETURN = Variables[pret[0]];
                               }
                               else
                               {
                                   RETURN = ConvertVal(pret[0], pret[1]);
                               }
                               break;

                           case "SAYT":
                               string[] pvs = operand.Split(',');
                               string pa = "";
                               foreach (string pv in pvs)
                               {
                                   if (Variables.ContainsKey(pv))
                                       pa = pa + " " + Variables[pv].ToString();
                                   else
                                       pa = pa + " " + pv;
                               }
                               JVPCore.SendSignal("SPEECH", pa);
                               break;
                           case "MSGB":
                               string[] avs = operand.Split(',');
                               string sa = "";
                               foreach (string xv in avs)
                               {
                                   if (Variables.ContainsKey(xv))
                                       sa = sa + " " + Variables[xv].ToString();
                                   else
                                       sa = sa + " " + xv;
                               }
                               System.Windows.Forms.MessageBox.Show(sa);
                               break;
                           case "SDNG":
                               string[] savs = operand.Split(',');

                               if (Variables.ContainsKey(savs[1]))
                                   JVPCore.SendSignal(savs[0], Variables[savs[1]].ToString());
                               else
                                   JVPCore.SendSignal(savs[0], savs[1]);
                               
                               
                               break;
                           case "EVAL":
                               // evaluate expression and assign variable if exist
                               if (Variables.ContainsKey(operand.Split(',')[0]))
                               {
                                   Expr.Expression = operand.Split(',')[1];
                                   if(operand.Split(',')[2] == "Int32")
                                       Variables[operand.Split(',')[0]] = Expr.EvaluateInt();
                                   else if (operand.Split(',')[2] == "Bool")
                                       Variables[operand.Split(',')[0]] = Expr.EvaluateBool();
                                   else if (operand.Split(',')[2] == "Double")
                                       Variables[operand.Split(',')[0]] = Expr.EvaluateDouble();
                                   else if (operand.Split(',')[2] == "Int64")
                                       Variables[operand.Split(',')[0]] = Expr.EvaluateLong();
                                   else
                                       Variables[operand.Split(',')[0]] = Expr.Evaluate();
                               }
                               else
                               {
                                   Expr.Expression = operand.Split(',')[1];
                                   RETURN = Expr.Evaluate();
                               }
                               break;
                       }
                   }
                 
               }
               if (Functions.Count != 0)
               {
                   foreach (Function fn in Functions)
                   {
                       if (RETURN == null)
                           RETURN = fn.Invoke();
                       else
                           fn.Invoke();
                   }
               }
               
           }
           catch
           {
             // Log.LogEvent("Execution Failed ASM " + asmcode, Application.StartupPath + @"\Logs\JVPEVENTS.txt");
               return null;
           }
           finally
           {
               Variables.Clear();
               stack.Clear();
               Functions.Clear();
              
           }
           return RETURN;
       }
       public object ConvertVal(string val, string type)
       {
           switch (type)
           {
               case "Int16":
                   return Convert.ToInt16(val);
                 
               case "Int32":
                   return Convert.ToInt32(val);
                  
               case "Int64":
                   return Convert.ToInt64(val);
                
               case "Double":
                   return Convert.ToDouble(val);
                  
               case "Decimal":
                   return Convert.ToDecimal(val);
                  
               case "Date":
                   return Convert.ToDateTime(val);
                  
               case "Char":
                   return Convert.ToChar(val);
                   
               case "Byte":
                   return Convert.ToByte(val);
                  
               case "Bool":
                   return Convert.ToBoolean(val);
                  
               case "SByte":
                   return Convert.ToSByte(val);
                 
               case "UInt16":
                   return Convert.ToUInt16(val);
                  
               case "UInt32":
                   return Convert.ToUInt32(val);
                 
               case "UInt64":
                   return Convert.ToUInt64(val);
                  
               case "Single":
                   return Convert.ToSingle(val);
                  
               case "String":
                   return Convert.ToString(val);
                  
           }
           return val;
       }
       // asm,type,method,v1,v2,v3....vn
       public Function CreateFunction(string operand)
       {
           int i = 0;
           List<object> objs = new List<object>();
         
           string[] a =operand.Split(',');

           for (i = 0; i < a.Length - 3; i++)
           {
              
               objs.Add(stack[i]);
           }

           return new Function(a[1], a[0], a[2], objs.ToArray());

       }


    
    }
}
