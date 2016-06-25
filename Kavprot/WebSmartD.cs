using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class WebSmartD : UserControl
    {
        public WebSmartD()
        {
            InitializeComponent();
            textBox1.Lines = File.ReadAllLines(Application.StartupPath + @"\Conf\WEBSD.dic");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllLines(Application.StartupPath + @"\Conf\WEBSD.dic", textBox1.Lines);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            MainC m = (MainC)Forms.GetControl("main");
            Forms.frm.ShowModalPanel(m, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }
    }
}
