using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using KAVE.BaseEngine;
using KAVE;

namespace Kavprot
{
    public partial class Backupfrm : UserControl
    {
        public Backupfrm()
        {
            InitializeComponent();
        }
        #region Backup
        private void selectfilebtn_Click(object sender, EventArgs e)
        {
            if (openfiledlg.ShowDialog() == DialogResult.OK)
            {
                textBoxX2.Text = openfiledlg.FileName;
            }
            else
            {

            }
        }

        private void selectbkdest_Click(object sender, EventArgs e)
        {
            if (savefiledlg.ShowDialog() == DialogResult.OK)
            {
                textBoxX1.Text = savefiledlg.FileName;
            }
            else
            {

            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (backupdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxX3.Text = backupdlg.FileName;
            }
            else
            {

            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (filedlg.ShowDialog() == DialogResult.OK)
            {
                textBoxX4.Text = filedlg.FileName;
            }
            else
            {

            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            try
            {
            // create backup
            Backup.Make(textBoxX2.Text, textBoxX1.Text, textBoxX5.Text);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxX1.Checked)
                {
                    // restore backup
                    Backup.Restore(textBoxX4.Text, textBoxX3.Text, "KPAVRSBK");
                    MessageBox.Show(File.ReadAllText(textBoxX3.Text + "info"));
                }
                else
                {
                    // restore backup
                    Backup.Restore(textBoxX4.Text, textBoxX3.Text, textBoxX6.Text);
                }
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        #endregion

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                textBoxX6.Enabled = false;
            }
            else
            {
                textBoxX6.Enabled = true;
            } 
        }
        void MakeRescue()
        {
            try{
            SystemRescue.MakeRescue(folder, file, progressBarX1);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
                MessageBox.Show("BACKUP ERROR","SYS RESCUE");
            }
            finally
            {

            }
        }
        string folder;
        string file;
        private void buttonX5_Click(object sender, EventArgs e)
        {
         
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folder = folderBrowserDialog1.SelectedPath;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file = saveFileDialog1.FileName;
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {

                }
            }
            else
            {

            }
        }
        Thread thr;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                thr = new Thread(new ThreadStart(MakeRescue));
                thr.Name = "RSCAGENT";
                thr.Priority = ThreadPriority.Normal;
                thr.Start();
                thr.Join();
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Rescue Completed");
            progressBarX1.Value = 0;
        }


    }
}
