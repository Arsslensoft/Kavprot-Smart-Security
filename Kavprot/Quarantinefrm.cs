using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KAVE;

namespace Kavprot
{
    public partial class Quarantinefrm : UserControl
    {
        public Quarantinefrm()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

                Quarantine.Activate(listBox1.SelectedItem.ToString());
          
        }

        private void Quarantinefrm_Load(object sender, EventArgs e)
        {
            List<string> t = Quarantine.Filelist();
            foreach (string it in t)
            {
                listBox1.Items.Add(it);
            }
        }

    }
}
