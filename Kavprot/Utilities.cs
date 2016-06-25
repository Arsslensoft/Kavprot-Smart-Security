using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Kavprot
{
    public partial class Utilities : UserControl
    {
        public Utilities()
        {
            InitializeComponent();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            CryptoCenter m = (CryptoCenter)Forms.GetControl("crypto");
            Forms.frm.ShowModalPanel(m, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }
        private void buttonX5_Click(object sender, EventArgs e)
        {
            WebSmartD m = (WebSmartD)Forms.GetControl("wsd");
            Forms.frm.ShowModalPanel(m, DevComponents.DotNetBar.Controls.eSlideSide.Left);

        }


    }
}
