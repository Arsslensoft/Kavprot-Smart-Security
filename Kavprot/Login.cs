using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KAVE;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class Login : DevComponents.DotNetBar.Office2007Form
    {
       internal DialogResult result;
        public Login()
        {
            InitializeComponent();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            KavprotManager.ShutDown();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (Activation.Login(textBoxX2.Text, textBoxX1.Text))
            {
                result = DialogResult.OK;
                this.Close();
            }
            else
            {

            }
        }
    }
}
