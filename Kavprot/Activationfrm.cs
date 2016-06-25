using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using KAVE.BaseEngine;


namespace Kavprot
{
    public partial class Activationfrm : UserControl
    {
        public Activationfrm()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
           bool reg = Activation.GenerateActivation(utxt.Text, passtxt.Text, sktxt.Text, textBoxX1.Text);
           if (reg)
           {
               labelX7.Text = "Registration Success";
               labelX7.ForeColor = Color.Green;
           }
           else
           {
               labelX7.Text = "Registration Failed";
               labelX7.ForeColor = Color.Red;
           }
        }
    }
}
