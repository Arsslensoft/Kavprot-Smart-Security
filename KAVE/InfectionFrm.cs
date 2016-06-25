using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KAVE.BaseEngine;

namespace KAVE
{
    public partial class InfectionFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        Virus vi;
        public InfectionFrm(Virus vu)
        {
            InitializeComponent();
            vi = vu;
        }
        bool Learn = false;
        public InfectionFrm(Virus vu, bool learn)
        {
            InitializeComponent();
            vi = vu;
            Learn = learn;
        }
        private void InfectionFrm_Load(object sender, EventArgs e)
        {
            virlb.Text = vi.Name;
            flb.Text = vi.Location;
            if(Learn)
                buttonX1.Visible = true;
        }

        private void quarantinebtn_Click(object sender, EventArgs e)
        {
            try
            {
                ScanSolutions.PutQuarantine(vi.Location, vi.Name);
                MessageBox.Show("Successfully quarantined", "Quarantine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void repairbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if(vi.Scanner.Repair(vi))
                   MessageBox.Show("Successfully repaired", "Repair", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        private void removebtn_Click(object sender, EventArgs e)
        {
            try
            {
                ScanSolutions.Remove(vi.Location);
                MessageBox.Show("Successfully removed", "Removal", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                Alert.LearnDefinitin(vi.Location, vi.Name);
                MessageBox.Show("Successfully Learned", "Learning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

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
