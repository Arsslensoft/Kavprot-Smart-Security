using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using KAVE.Engine;
using KAVE.BaseEngine;

namespace KAVE
{
    public enum ScanType
    {
        Quick,
        Full,
        Zone,
        File,
        IDP
    }
    public partial class ScanForm : DevComponents.DotNetBar.Metro.MetroForm, IDisposable
    {
        ScanType scantyp;
        string SFolder;
        public ScanForm(ScanType scantp, string folder)
        {
            InitializeComponent();
            SFolder = folder;
            scantyp = scantp;
        }

        private void ScanForm_Load(object sender, EventArgs e)
        {

        }
        void FullScan()
        {
            try
            {
                Scanner.FullScan(curfilequickscan, progressQuickscan,fullscanlist);
            }
            finally
            {

            }
        }
        void ZoneScan()
        {
            try
            {

                Scanner.ZoneScan(SFolder, curfilequickscan, progressQuickscan, fullscanlist);
            }
            finally
            {

            }
        }
        void FileScan()
        {
            try
            {
                if (SFolder.EndsWith(".exe"))
                {
                    Scanner.HeuristicScan(SFolder);
                }
                else if (SFolder.EndsWith(".dll"))
                {
                    Scanner.HeuristicScan(SFolder);
                 
                }
                else
                {

                }
             
               
            }
            finally
            {
                Scanner.ScanFile(SFolder);
            }
        }
        void QuickScan()
        {
            try
            {

                Scanner.QuickScan(curfilequickscan, progressQuickscan, fullscanlist, false);
            }
            finally
            {

            }
        }
        void IDPScan()
        {
            try
            {

                Scanner.IDPScan(curfilequickscan, progressQuickscan, fullscanlist);
            }
            finally
            {

            }
        }
        private void quickscanbtn_Click(object sender, EventArgs e)
        {
            try
            {
                quickscanbtn.Enabled = false;
                cancelquickscan.Enabled = true;
               scanworker.RunWorkerAsync();
                fullscanlist.Items.Clear();
            }
            finally
            {

            }
        }
        Thread ScanThread;
        private void scanworker_DoWork(object sender, DoWorkEventArgs e)
        {
            //curfilequickscan.Text = "Scanning...";
            if (scantyp == ScanType.Quick)
            {
                if (ScanThread != null)
                {
                    if (ScanThread.IsAlive == true)
                    {

                     
                    }
                    else
                    {
                        
                        ScanThread = new Thread(new ThreadStart(QuickScan));
                        ScanThread.Name = "ScanThread";
                        ScanThread.Start();
                        ScanThread.Join();
                    }
                }
                else
                {
                    
                    ScanThread = new Thread(new ThreadStart(QuickScan));
                    ScanThread.Name = "ScanThread";
                    ScanThread.Start();
                    ScanThread.Join();
                }
            }
            else if (scantyp == ScanType.IDP)
            {
                if (ScanThread != null)
                {
                    if (ScanThread.IsAlive == true)
                    {


                    }
                    else
                    {
                      
                        ScanThread = new Thread(new ThreadStart(IDPScan));
                        ScanThread.Name = "ScanThread";
                        ScanThread.Start();
                        ScanThread.Join();
                    }
                }
                else
                {
                    //urfilequickscan.Text = "Scanning...";
                    ScanThread = new Thread(new ThreadStart(IDPScan));
                    ScanThread.Name = "ScanThread";
                    ScanThread.Start();
                    ScanThread.Join();
                }
            }
            else if (scantyp == ScanType.Full)
            {
                if (ScanThread != null)
                {
                    if (ScanThread.IsAlive == true)
                    {
                       
                    }
                    else
                    {
                        curfilequickscan.Text = "Scanning...";
                        ScanThread = new Thread(new ThreadStart(FullScan));
                        ScanThread.Name = "ScanThread";
                        ScanThread.Start();
                        ScanThread.Join();
                    }
                }
                else
                {
                    curfilequickscan.Text = "Scanning...";
                    ScanThread = new Thread(new ThreadStart(FullScan));
                    ScanThread.Name = "ScanThread";
                    ScanThread.Start();
                    ScanThread.Join();
                }
            }
            else if (scantyp == ScanType.File)
            {
                if (ScanThread != null)
                {
                    if (ScanThread.IsAlive == true)
                    {

                    }
                    else
                    {
                        ScanThread = new Thread(new ThreadStart(FileScan));
                        ScanThread.Name = "ScanThread";
                        ScanThread.Start();
                        ScanThread.Join();
                    }
                }
                else
                {
               
                    ScanThread = new Thread(new ThreadStart(FileScan));
                    ScanThread.Name = "ScanThread";
                    ScanThread.Start();
                    ScanThread.Join();
                }
            }
            else
            {
                if (ScanThread != null)
                {
                    if (ScanThread.IsAlive == true)
                    {

                       
                    }
                    else
                    {
                      
                        ScanThread = new Thread(new ThreadStart(ZoneScan));
                        ScanThread.Name = "ScanThread";
                        ScanThread.Start();
                        ScanThread.Join();
                    }
                }
                else
                {
                    
                    ScanThread = new Thread(new ThreadStart(ZoneScan));
                    ScanThread.Name = "ScanThread";
                    ScanThread.Start();
                    ScanThread.Join();
                }
            }
        }

        private void scanworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            curfilequickscan.Text = "Scan Completed";
            quickscanbtn.Enabled = true;
            this.cancelquickscan.Enabled = false;
        
        }

        private void cancelquickscan_Click(object sender, EventArgs e)
        {
            ScanThread.Abort();
            scanworker.CancelAsync();
            this.progressQuickscan.Value = 0;
            this.fullscanlist.Items.Clear();
        
        }

        private void fullscanlist_ItemClick(object sender, EventArgs e)
        {
            foreach (VirusItem item in fullscanlist.SelectedItems)
            {
                Alert.Infected(item.Virus);
                fullscanlist.Items.Remove(item);
            }
        }

        private void fullscanlist_ItemAdded(object sender, EventArgs e)
        {
            fullscanlist.Refresh();
         AVEngine.AlertVirus();
        }

        private void ScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
       
 
     

 

    }
}
