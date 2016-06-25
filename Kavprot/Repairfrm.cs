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
    public partial class Repairfrm : UserControl
    {
        public Repairfrm()
        {
            InitializeComponent();
        }

        private void Repairfrm_Load(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (filetxt.Text != string.Empty)
            {
                Virus vi = new Virus(virntxt.Text, filetxt.Text, FileFormat.GetFileFormat(filetxt.Text));
                FileFormat.GetFileFormat(filetxt.Text).Repair(vi);
            }
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Virus vi = new Virus(virntxt.Text, openFileDialog1.FileName, FileFormat.GetFileFormat(openFileDialog1.FileName));
                    FileFormat.GetFileFormat(openFileDialog1.FileName).Repair(vi);
                }
                else
                {

                }
            }
        }

    }
}
