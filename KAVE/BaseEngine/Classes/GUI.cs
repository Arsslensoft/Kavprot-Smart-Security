using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;

namespace KAVE.BaseEngine
{
    internal delegate void InvokeProgress(ProgressBarX progress, int value, int maximum);
    internal delegate void InvokeLabel(string text, Label CurFile);
    internal delegate void InvokeItemPanel(Virus virus, ItemPanel lst);  
    /// <summary>
    /// Graphic User Interface Safe call engine
    /// </summary>
    internal static class GUI
    {
        public static void UpdateProgress(ProgressBarX progress, int val, int max)
        {
            IAsyncResult res;
            res = progress.BeginInvoke(new InvokeProgress(UpdateProgressX), new object[] { progress, val, max });
            progress.EndInvoke(res);
        }
        public static void UpdateLabel(Label label, string val)
        {
            IAsyncResult res;
            res = label.BeginInvoke(new InvokeLabel(UpdateCurFile), new object[] { val, label });
            label.EndInvoke(res);
        }
        static void UpdateCurFile(string text, Label CurFile)
        {
            CurFile.Text = text;

        }

        static void UpdateProgressX(ProgressBarX progress, int value, int maximum)
        {
            progress.Minimum = 0;
            progress.Maximum = maximum;

            progress.Value = value;
            progress.Text = value.ToString();
        }

        public static void UpdatePanel(Virus virus, ItemPanel panel)
        {
            IAsyncResult res;
            res = panel.BeginInvoke(new InvokeItemPanel(UpdatePanelX), new object[] { virus, panel });
            panel.EndInvoke(res);
        }
        static void UpdatePanelX(Virus virus, ItemPanel panel)
        {

            VirusItem vi = new VirusItem(virus);
            if (!panel.Items.Contains(vi))
                panel.Items.Add(vi);
           
        }
    }
    public class VirusItem : DevComponents.DotNetBar.CheckBoxItem
    {
        public VirusItem(Virus vi)
        {
            _vir = vi;
            this.Text = vi.Location + " " + vi.Name;
            this.Name = Security.GetMd5Hashofstring(vi.Location);
        }
        Virus _vir;
        public Virus Virus
        {
            get { return _vir; }
        }
    }
}
