using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using KAVE;

namespace Kavprot
{
    public partial class ScanTypeFrm : UserControl
    {
        public ScanTypeFrm()
        {
            InitializeComponent();
        }

        private void fullscanbtn_Click(object sender, EventArgs e)
        {
            ScanForm frm = new ScanForm(ScanType.Full, @"C:\");
            frm.Show();
        }

        private void quickscanbtn_Click(object sender, EventArgs e)
        {
            ScanForm frm = new ScanForm(ScanType.Quick, @"C:\");
            frm.Show();
        }

        private void zonescan_Click(object sender, EventArgs e)
        {
            MainC c = (MainC)Forms.GetControl("main");
            if (c.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ScanForm frm = new ScanForm(ScanType.Zone, c.folderBrowserDialog1.SelectedPath);
                frm.Show();
            }
            else
            {

            }
        }

        private void scanfilebtn_Click(object sender, EventArgs e)
        {
            MainC c = (MainC)Forms.GetControl("main");
            if (c.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ScanForm frm = new ScanForm(ScanType.File, c.openFileDialog1.FileName);
                frm.Show();
            }
            else
            {

            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            ScanForm frm = new ScanForm(ScanType.IDP, null);
            frm.Show();
        }
    }
}
