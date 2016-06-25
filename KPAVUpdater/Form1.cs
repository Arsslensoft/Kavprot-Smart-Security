using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using KCompress;

namespace KPAVUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ptimer.Enabled = true;
        }

        private void ptimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ptimer.Enabled = false;
                label1.Text = "Stoping Kavprot Protection";
                Process[] pr = Process.GetProcessesByName("Kavprot");
                Process[] prs = Process.GetProcessesByName("KavprotCloud");
                foreach (Process p in pr)
                {
                    p.Kill();
                }
                foreach (Process p in prs)
                {
                    p.Kill();
                }

            }
            catch (Exception ex)
            {
              
            }
            finally
            {
                progressBar1.Value = 50;
                itimer.Enabled = true;
            }

        }
        void Extract()
        {

            //KavProtCPLExtractor.SetLibraryPath(Application.StartupPath + @"\7z.dll");
            using (KCompressExtractor extr = new KCompressExtractor(Environment.GetCommandLineArgs()[0]))
            {
                extr.ExtractArchive(Application.StartupPath);
            }
        }
        private void itimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Extract();
             }
            catch (Exception ex)
            {

            }
            finally
            {
                progressBar1.Value = 100;
                MessageBox.Show("Kavprot Antivirus Updated", "Update Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(Application.StartupPath + @"\Kavprot.exe");
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you Accept To Update Kavprot From this file " + Environment.GetCommandLineArgs()[0], "Update Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {

            }
        }
    }
}
