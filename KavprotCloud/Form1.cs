using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using KAVE;
using KAVE.BaseEngine;

namespace KavprotCloud
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            KavprotManager.Initialize(KavprotInitialization.Engine);

        }
       
        void StartCloud()
        {
            try
            {
                CloudProt.Protect();
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KavprotManager.ShutDown();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
            this.Hide();
            StartCloud();
        }
    }
}
