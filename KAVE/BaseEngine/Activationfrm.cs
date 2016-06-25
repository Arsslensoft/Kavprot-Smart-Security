using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace KAVE.BaseEngine
{
    public partial class ActivationForm : DevComponents.DotNetBar.Office2007Form
    {
        public ActivationForm()
        {
            InitializeComponent();
        }
        bool reg = false;
        void Work()
        {
            try
            {

                reg = Activation.GenerateActivation(utxt.Text, passtxt.Text, sktxt.Text, textBoxX1.Text);
            }
            catch
            {

            }
            finally
            {
            }
        }
        Thread thr = null;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try {
                if (backgroundWorker1.IsBusy)
                {

                }
                else
                {
                    circularProgress1.Visible = true;
                    circularProgress1.Value = 50;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch
            {

            }
            finally
            {
            }        
        
        }

        private void ActivationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Process.GetCurrentProcess().Kill();
            }
            catch
            {

            }
            finally
            {
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try{
            thr = new Thread(new ThreadStart(Work));
            thr.Name = "ActivationThread";
            thr.Start();
            thr.Join();
            }
            catch
            {

            }
            finally
            {
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                circularProgress1.Value = 100;
            if (reg)
            {
                MessageBox.Show("Kavprot Has been registred successfully.\r\n kavprot will restart now", "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Kavprot registration failed.", "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
            catch
            {

            }
            finally
            {
            }
        }
    }
}
