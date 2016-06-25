using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using KAVE.BaseEngine;

namespace KAVE.Heuristic
{

    public class Disassembler
    {
        Collection<TypeDefinition> _TD;
        Collection<MethodDefinition> _MD;
        AssemblyDefinition assembly;
        public Collection<TypeDefinition> Types
        {
            get { return _TD; }
        }
        public Collection<MethodDefinition> Methods
        {
            get { return _MD; }
          
        }
        public AssemblyDefinition Assembly
        {
            get { return assembly; }
        }
        SQLiteConnection SDB;
        public Disassembler(string AssemblyFile, SQLiteConnection SDBO)
        {
            try
            {
                _TD = new Collection<TypeDefinition>();
                _MD = new Collection<MethodDefinition>();
                if (AssemblyFile.Contains(Application.StartupPath))
                {
                    assembly = null;
                }
                else
                {
                    assembly = AssemblyFactory.GetAssembly(AssemblyFile);
                }
            
                SDB = SDBO;
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex,3);
            }
            finally
            {

            }
        }

        public void LoadAssembly()
        {
            _TD.Clear();
          
            foreach (TypeDefinition type in this.assembly.MainModule.Types)
            {
                _TD.Add(type);
                foreach (MethodDefinition md in type.Methods)
                {
                    if (md.Body != null)
                    {
                        if (md.Body.Instructions.Count > 0)
                            _MD.Add(md);
             
                    }
            
                }
            }
            if (!_MD.Contains(assembly.EntryPoint))
               _MD.Add(assembly.EntryPoint);
          
        }
        void Rate(List<string> instructions, out bool rate)
        {
            int found = 0;
            rate = false;
            foreach (string ins in instructions)
            {
                int it = 0;
                string sresult = CheckCode(ins, out it);
                if (sresult != null)
                {
                    if (it > 0)
                    {
                        found += it;
                    }
              
                }
              
            }
            if (instructions.Count < 10)
            {
                if (found > 2)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
            else if (instructions.Count < 100)
            {
                if (found > 9)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
            else if (instructions.Count < 200)
            {
                if (found > 14)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
            else if (instructions.Count < 300)
            {
                if (found > 24)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
            else if (instructions.Count < 500)
            {
                if (found > 29)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
            else
            {
                if (found > 39)
                {
                    rate = true;
                }
                else
                {
                    rate = false;
                }
            }
        }
         public void DisassembleAndRate(string outputFile, out bool result)
        {
          
            bool r = false;
            try
            {
                if (assembly != null)
                {
                    List<string> lst = new List<string>();
                    using (StreamWriter str = new StreamWriter(outputFile, false))
                    {
                        foreach (MethodDefinition method in _MD)
                        {
                            foreach (Instruction ins in method.Body.Instructions)
                            {
                                string ilcode = InstructionText(ins);
                                str.WriteLine(ilcode);
                                lst.Add(ilcode);
                            }
                        }
                    }
                    bool rate = false;
                    Rate(lst, out rate);
                    r = rate;
                    result = rate;
                    File.Delete(outputFile);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex,3);
            }
            finally
            {
                result = r;

            }
        }
        public string InstructionText(Instruction inst)
        {
            if (inst.Operand is Mono.Cecil.Cil.Instruction)
            {
                Mono.Cecil.Cil.Instruction instruccion = (Instruction)inst.Operand;
                return string.Format("{0} {1}", inst.OpCode.ToString(), instruccion.Offset.ToString());
            }
            else if (inst.Operand is string)
            {
                return string.Format("{0} \"{1}\"", inst.OpCode.ToString(), inst.Operand.ToString());
            }
            else if (inst.Operand is MethodReference)
            {
                MethodReference metodo = (MethodReference)inst.Operand;
                return inst.OpCode.ToString() + " " + metodo.ToString();
            }
            else if (inst.Operand != null)
            {
                return inst.OpCode.ToString() + " " + inst.Operand.ToString();
            }
            else
            {
                return inst.OpCode.ToString();
            }
        }

        public string CheckCode(string Code, out int rate)
        {
            string sresult = null;
            rate = 0;
            using (SQLiteCommand cmd = new SQLiteCommand(SDB))
            {
                cmd.CommandText = "SELECT * FROM HEURISTIC";
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (Code.Contains((string)dr["instruction"]))
                    {
                       rate = Convert.ToInt32(dr["rate"]);

                    }
                    else
                    {

                    }
                }
            }

            return sresult;
        }
    }
}
