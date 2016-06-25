using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using KAVE;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class Updatefrm : UserControl
    {
        public Updatefrm()
        {
            InitializeComponent();
        }
        #region vdb
        void UpdateVDB()
        {
            try
            {
               UpdateManager.UpdateVDB(progressBarX1, labelX1);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        Thread thr;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
              
                thr = new Thread(new ThreadStart(UpdateVDB));
                thr.Priority = ThreadPriority.Normal;
                thr.Name = "UPDATETHREAD";
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
            expandablePanel1.Expanded = false;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
              try
            {
                expandablePanel1.Expanded = true;
            backgroundWorker1.RunWorkerAsync();
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
        #region PROG
        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                progworker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        Thread progthr;
        void UpdateProg()
        {
            try{
                UpdateManager.UpdateProgram(progressBarX2, label1);
             }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        private void progworker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                expandablePanel2.Expanded = true;
                progthr = new Thread(new ThreadStart(UpdateProg));
                progthr.Priority = ThreadPriority.Normal;
                progthr.Name = "UPDATEPROGTHREAD";
                progthr.Start();
                progthr.Join();
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }

        private void progworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            expandablePanel2.Expanded = false;
        }
#endregion
 

    }
}
