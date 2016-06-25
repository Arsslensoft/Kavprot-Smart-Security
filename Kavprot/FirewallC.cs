using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using KAVE.Monitors;
using DevComponents.DotNetBar;
using KAVE;
using System.Data.SQLite;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class FirewallC : UserControl
    {
        public FirewallC()
        {
            InitializeComponent();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            try{
            if (switchButton1.Value)
            {
                Firewall.Add("AllowAll", filetxt.Text);
                   LabelItem lb = new LabelItem();
                lb.Text = filetxt.Text + "|AllowAll";
                lb.Name = "lb" + itemPanel1.Items.Count.ToString();
                itemPanel1.Items.Add(lb);
            }
            else
            {
                Firewall.Add("DenyAll", filetxt.Text);
                   LabelItem lb = new LabelItem();
                   lb.Text = filetxt.Text + "|DenyAll";
                   lb.Name = "lb" + itemPanel1.Items.Count.ToString();
                   itemPanel1.Items.Add(lb);
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

        private void remvbtn_Click(object sender, EventArgs e)
        {
            try
            {
                LabelItem lb = (LabelItem)itemPanel1.SelectedItem;
                Firewall.Remove(lb.Text.Split('|')[0], lb.Text.Split('|')[1]);
                itemPanel1.Items.Remove(lb);
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public void SLoad()
        {
            try{
                itemPanel1.Items.Clear();
                using (SQLiteCommand cmd = new SQLiteCommand(VDB.SDB))
                {
                    cmd.CommandText = "SELECT * FROM TDI";
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        LabelItem lb = new LabelItem();
                        lb.Text = (string)dr["app"] + "|" + (string)dr["access"];
                        lb.Name = "lb" + itemPanel1.Items.Count.ToString();
                        itemPanel1.Items.Add(lb);
                    }
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
    }
}
