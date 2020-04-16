using System.Drawing;
using System.Windows.Forms;

namespace PingCryptoTest_Zeev
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mBtnBrowse = new System.Windows.Forms.Button();
            this.mBtnEncrypt = new System.Windows.Forms.Button();
            this.mBtnDecrypt = new System.Windows.Forms.Button();
            this.mBtnCreateKey = new System.Windows.Forms.Button();
            //this.lblMessages = new System.Windows.Forms.ListView();

            this.SuspendLayout();
            // 
            // mBtnBrowse
            // 
            this.mBtnBrowse.Location = new System.Drawing.Point(68, 50);
            this.mBtnBrowse.Name = "mBtnBrowse";
            this.mBtnBrowse.Size = new System.Drawing.Size(120, 32);
            this.mBtnBrowse.TabIndex = 0;
            this.mBtnBrowse.Text = "Browse";
            this.mBtnBrowse.Click += new System.EventHandler(this.btnBrowse_OnClick);
            this.mBtnBrowse.Font = new Font(mBtnBrowse.Font.FontFamily, 10);
            // 
            // mBtnEncrypt
            // 
            this.mBtnEncrypt.Location = new System.Drawing.Point(68, 150);
            this.mBtnEncrypt.Name = "mBtnEncrypt";
            this.mBtnEncrypt.Size = new System.Drawing.Size(120, 32);
            this.mBtnEncrypt.TabIndex = 0;
            this.mBtnEncrypt.Text = "Encrypt";
            this.mBtnEncrypt.Click += new System.EventHandler(this.btnEncrypt_OnClick);
            this.mBtnEncrypt.Font = new Font(mBtnEncrypt.Font.FontFamily, 10);
            // 
            // mBtnDecrypt
            // 
            this.mBtnDecrypt.Location = new System.Drawing.Point(68, 200);
            this.mBtnDecrypt.Name = "mBtnDecrypt";
            this.mBtnDecrypt.Size = new System.Drawing.Size(120, 32);
            this.mBtnDecrypt.TabIndex = 0;
            this.mBtnDecrypt.Text = "Decrypt";
            this.mBtnDecrypt.Click += new System.EventHandler(this.btnDecrypt_OnClick);
            this.mBtnDecrypt.Font = new Font(mBtnDecrypt.Font.FontFamily, 10);
            // 
            // mBtnCreateAsymKey
            // 
            this.mBtnCreateKey.Location = new System.Drawing.Point(68, 100);
            this.mBtnCreateKey.Name = "mBtnCreateAsymKey";
            this.mBtnCreateKey.Size = new System.Drawing.Size(120, 32);
            this.mBtnCreateKey.TabIndex = 0;
            this.mBtnCreateKey.Text = "Create Key";
            this.mBtnCreateKey.Click += new System.EventHandler(this.btnCreateAsmKey_OnClick);
            this.mBtnCreateKey.Font = new Font(mBtnCreateKey.Font.FontFamily, 10);

            // 
            // lblMessages
            // 
            //lblMessages.Location = new Point(280, 50);
            //lblMessages.Size = new System.Drawing.Size(400, 200);
            //lblMessages.Text = "";
            //lblMessages.BorderStyle = BorderStyle.Fixed3D;
            //lblMessages.Width = 200;


            dynamicStatusStrip = new System.Windows.Forms.StatusStrip();
            dynamicStatusStrip.Name = "DynamicStatusStrip";
            dynamicStatusStrip.Text = "statusStrip";
            dynamicStatusStrip.BackColor = Color.White;
            dynamicStatusStrip.Font = new Font("Georgia", 14, FontStyle.Italic);
            dynamicStatusStrip.ForeColor = Color.Gray;
            dynamicStatusStrip.SizingGrip = false;
            
            lblStatus = new ToolStripStatusLabel("");
            lblStatus.BorderSides = ToolStripStatusLabelBorderSides.All;
            lblStatus.BorderStyle = Border3DStyle.Sunken;

            lblStatus.Spring = true;
            //lblStatus.Padding = new Padding((int)(this.Size.Width - 75), 0, 0, 0);


            dynamicStatusStrip.Items.Add(lblStatus);
            



            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);

             
            this.Controls.Add(this.mBtnBrowse);
            this.Controls.Add(this.mBtnEncrypt);
            this.Controls.Add(this.mBtnDecrypt);
            this.Controls.Add(this.mBtnCreateKey);
            //this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.dynamicStatusStrip);
            
            this.Name = "MainForm";
            this.Text = "PKI Example (Ping ID test)";
            this.Load += new System.EventHandler(this.MainForm_Load);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ResumeLayout(false);

        }

        #endregion
    }
}

