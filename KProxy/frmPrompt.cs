namespace KProxy
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class frmPrompt : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private Container components = null;
        private PictureBox pbImage;
        public TextBox txtInput;
        public RichTextBox txtMessage;

        public frmPrompt()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static string GetUserString(string sTitle, string sPrompt, string sDefault)
        {
            return GetUserString(sTitle, sPrompt, sDefault, false);
        }

        public static string GetUserString(string sTitle, string sPrompt, string sDefault, bool bReturnNullIfCancelled)
        {
            frmPrompt prompt = new frmPrompt();
            prompt.StartPosition = FormStartPosition.CenterScreen;
            prompt.Text = sTitle;
            prompt.txtMessage.Text = sPrompt;
            prompt.txtInput.Text = sDefault;
            DialogResult result = prompt.ShowDialog();
            if (DialogResult.OK == result)
            {
                sDefault = prompt.txtInput.Text;
            }
            prompt.Dispose();
            if (bReturnNullIfCancelled && (DialogResult.OK != result))
            {
                return null;
            }
            return sDefault;
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(frmPrompt));
            this.btnCancel = new Button();
            this.txtMessage = new RichTextBox();
            this.btnOk = new Button();
            this.pbImage = new PictureBox();
            this.txtInput = new TextBox();
            base.SuspendLayout();
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.No;
            this.btnCancel.Location = new Point(0xe0, 0x58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.txtMessage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtMessage.BackColor = SystemColors.Control;
            this.txtMessage.BorderStyle = BorderStyle.None;
            this.txtMessage.Location = new Point(0x30, 11);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new Size(0x150, 0x25);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Text = "Text ";
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Location = new Point(0x130, 0x58);
            this.btnOk.Name = "btnOk";
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.pbImage.Image = (Image) manager.GetObject("pbImage.Image");
            this.pbImage.Location = new Point(8, 8);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new Size(0x20, 0x20);
            this.pbImage.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pbImage.TabIndex = 7;
            this.pbImage.TabStop = false;
            this.txtInput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.txtInput.Location = new Point(0x30, 0x38);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new Size(0x148, 0x15);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "";
            base.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new Size(5, 14);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x188, 120);
            base.Controls.Add(this.txtInput);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.txtMessage);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.pbImage);
            this.Font = new Font("Tahoma", 8.25f);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimizeBox = false;
            this.MinimumSize = new Size(200, 100);
            base.Name = "frmPrompt";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = " Kavprot Proxy Prompt";
            base.ResumeLayout(false);
        }
    }
}

