using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            labelX6.Text = "Username : " + Activation.User + " Expires : " + Activation.Expiration;
            
        }
    }
}
