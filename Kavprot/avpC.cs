using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using KAVE;
using DevComponents.DotNetBar;
using KAVE.BaseEngine;
using System.Text.RegularExpressions;

namespace Kavprot
{
    public partial class AVPC : UserControl
    {
        public AVPC()
        {
            InitializeComponent();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            try{
            if (Directory.Exists(filetxt.Text))
            {
                uint x = 0;
                if(readcheck.Checked)
                    x |= (uint)(0x00000020 | 0x00000200);

               if(delcheck.Checked)
                    x |= (uint)(0x00000100 | 0x00004000);
               if(wrcheck.Checked)
                    x |= (uint)(0x00000400 | 0x00000040 | 0x00000080);

                  if(rencheck.Checked)
                    x |= (uint)(0x00002000);
                 
                if(qscheck.Checked)
                    x |= (uint)(0x00020000 | 0x00010000 | 0x00001000 |  0x00008000 | 0x00000800 | 0x00000010);
                if(accheck.Checked)
                    x |= (uint)( 0x00040000);



                using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Conf\FSR.klist", true))
                       str.WriteLine(x.ToString() + "=" + filetxt.Text);
                    
               

                        LabelItem lb = new LabelItem();
                        lb.Text = filetxt.Text + "|" + x.ToString();
                        lb.Name = filetxt.Text;

                        itemPanel1.Items.Add(lb);
                    
                
            }
                }
            catch(Exception ex)
            {
                AntiCrash.LogException(ex);
            }
        }

        private void remvbtn_Click(object sender, EventArgs e)
        {
            try
            {
                LabelItem item = (LabelItem)itemPanel1.SelectedItem;
      
                itemPanel1.Items.Remove(item);
                using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Conf\FSR.klist", false))
                {
                    foreach (BaseItem it in itemPanel1.Items)
                    {
                        LabelItem l = (LabelItem)it;
                        str.WriteLine(l.Text.Split('|')[1]+"="+l.Text.Split('|')[0]);
                    }
                }
            }
            catch(Exception ex)
            {
                AntiCrash.LogException(ex);
            }
        }
        Regex r = new Regex("=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public void SLoad()
        {
            try
            {
                itemPanel1.Items.Clear();
                     using (StreamReader sr = new StreamReader(Application.StartupPath + @"\Conf\FSR.klist"))
                    {
                        if (sr.Peek() >= 0)
                        {
                            string[] l = r.Split(sr.ReadLine(), 2);
                            LabelItem lb = new LabelItem();
                            lb.Text = l[1] + "|" + l[0];
                            lb.Name = l[1];
                            itemPanel1.Items.Add(lb);
            
                        }
                    }
                 
                       
                    
                
            }
            catch(Exception ex)
            {
                AntiCrash.LogException(ex);
            }
        }

    }
}
