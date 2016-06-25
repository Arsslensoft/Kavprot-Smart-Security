using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AForge.Vision.Motion;
using AForge.Video;
using System.Threading;
using AForge.Controls;
using AForge.Video.DirectShow;
using System.Media;
using DevComponents.DotNetBar;

namespace KHS
{
    public partial class Form1 : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<VideoSourcePlayer, string> players;
        // opened video source
        private IVideoSource videoSource = null;
        // motion detector
        MotionDetector detector = new MotionDetector(
            new SimpleBackgroundModelingDetector(true, true),
            new MotionAreaHighlighting());
        // statistics length
        private const int statLength = 15;
        // current statistics index
        private int statIndex = 0;
        // ready statistics values
        private int statReady = 0;
        // statistics array
        private int[] statCount = new int[statLength];
        // load cameras, viddeo players, tabs
        void LoadCameras()
        {
            try
            {
                // enumerate video devices
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                players = new Dictionary<VideoSourcePlayer, string>();
                // add all devices to combo
                foreach (FilterInfo device in videoDevices)
                {
            
                  // create tabs and source players
                     DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
                     DevComponents.DotNetBar.SuperTabItem superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
              
                     superTabItem1.AttachedControl = superTabControlPanel1;
                     superTabItem1.GlobalItem = false;
                     superTabItem1.Name = "CAMTAB" + (superTabControl1.Tabs.Count).ToString();
                     superTabItem1.Text = "CAM "+(superTabControl1.Tabs.Count).ToString();
  
                     superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
                     superTabControlPanel1.Location = new System.Drawing.Point(0, 23);
                     superTabControlPanel1.Name = "CAM"+(superTabControl1.Tabs.Count).ToString();
                     superTabControlPanel1.Size = new System.Drawing.Size(898, 348);
                     superTabControlPanel1.TabIndex = 0;
                     superTabControlPanel1.TabItem = superTabItem1;
                  
                    this.superTabControl1.Controls.Add(superTabControlPanel1);
                     this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            superTabItem1});

                     VideoSourcePlayer p = new VideoSourcePlayer();
                    
                    p.Name = "VP"+superTabControl1.Tabs.Count.ToString();
                     p.Dock = DockStyle.Fill;
                     p.Size = new Size(800, 300);
                     p.Location = new Point(0, 23);
                    p.NewFrame += new VideoSourcePlayer.NewFrameHandler(p_NewFrame);

                     superTabControlPanel1.Controls.Add(p);

                     players.Add(p, device.Name);
                }
            }
            catch (ApplicationException)
            {
                MessageBox.Show("No capture device");
            }

        }
        void Start()
        {
            try
            {
                foreach (KeyValuePair<VideoSourcePlayer, string> vp in players)
                {
                    VideoCaptureDevice videoSource = new VideoCaptureDevice(vp.Value);
                   OpenVideoSource(videoSource, vp.Key);
                }
                // open it
                buttonItem1.Enabled = true;
           buttonItem2.Enabled = true;
            }
            catch
            {

            }
            finally
            {

            }
        }
        void Stop()
        {
            try
            {
                CloseVideoSource();
                buttonItem1.Enabled = false;
                buttonItem2.Enabled = false;
            }
            catch
            {

            }
        }
        // Open video source
        private void OpenVideoSource(IVideoSource source, VideoSourcePlayer videoSourcePlayer)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;
           
            // close previous video source
            CloseVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset statistics
            statIndex = statReady = 0;

            videoSource = source;

            this.Cursor = Cursors.Default;
            currentplayer = videoSourcePlayer;

        }
        VideoSourcePlayer currentplayer = null;
        // Close current video source
        private void CloseVideoSource()
        {
            foreach (KeyValuePair<VideoSourcePlayer, string> vp in players)
            {
                // set busy cursor
                this.Cursor = Cursors.WaitCursor;

                // stop current video source
                vp.Key.SignalToStop();

                // wait 2 seconds until camera stops
                for (int i = 0; (i < 50) && (vp.Key.IsRunning); i++)
                {
                    Thread.Sleep(100);
                }
                if (vp.Key.IsRunning)
                    vp.Key.Stop();

                // reset motion detector
                if (detector != null)
                    detector.Reset();

                vp.Key.BorderColor = Color.Black;
                this.Cursor = Cursors.Default;
            }
        }
        bool playing = false;
        // New frame received by the player
        private void p_NewFrame(object sender, ref Bitmap image)
        {
            lock (this)
            {
                if (detector != null)
                {
                    float motionLevel = detector.ProcessFrame(image);

                    if (motionLevel > 0.015f)
                    {
                        if (!playing)
                        {

                            sp.PlaySync();
                            playing = true;
                            btnstrt.Enabled = true;

                        }
                       // alert
                    }

                   }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonX1.Enabled = false;
            buttonX2.Enabled = false;
            LoadCameras();
        }

        private void btnstrt_Click(object sender, EventArgs e)
        {
            try
            {
                Start();
                buttonX1.Enabled = false;
                btnstrt.Enabled = false;
          }
            catch
            {

            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try{
            Stop();
            buttonX1.Enabled = false;
            btnstrt.Enabled = true;
                 }
            catch
            {

            }
        }
        SoundPlayer sp = new SoundPlayer(Application.StartupPath + @"\Sounds\SIREN.wav");
        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Stop();
                buttonX2.Enabled = false;
            }
            catch
            {

            }
        }
         
        private void superTabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {
            currentplayer = (VideoSourcePlayer)superTabControl1.SelectedPanel.Controls[0];

        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            try{
            currentplayer.Start();
               }
            catch
            {

            }
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentplayer.Stop();

            }
            catch
            {

            }
        }
    }
}
