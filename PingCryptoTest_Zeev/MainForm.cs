using PingCryptoTest_Zeev.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PingCryptoTest_Zeev
{
    public partial class MainForm : Form
    {

        
        Button                  mBtnBrowse, mBtnEncrypt, mBtnDecrypt, mBtnCreateKey;
        //ListView                lblMessages;
        StatusStrip             dynamicStatusStrip;
        ToolStripStatusLabel    lblStatus;
        string[]                files;
        const string            iconPath                = "icon.ico";
        utilManager             mUtil;
        const string            mCancelledTxt           = "Browse cancelled.";
        const string            mOpenFolderErrMsg       = "OpenFolderDialog did not complete.";
        const string            mNoFilesMsg             = "No Files found. Click 'Browse...' for .txt files...";
        const string            mEncryptDoneMsg         = "Encrypt done.";
        const string            mEncryptExceptionMsg    = "Encrypt task returned 0.";
        const string            mKeyCreatedMSg          = "Key pair created.";
        const string            mDecryptDoneMsg         = "Decrypt done.";
        const string            mDecryptExceptionMsg    = "Decrypt failed.";
        Task<int>               longRunningEncryptTask, longRunningDecryptTask;
        

        public MainForm()
        {

            InitializeComponent();

            string fullPath = Path.GetFullPath(iconPath);

            using (var stream = File.OpenRead(fullPath))
            {
                //setting app icon
                this.Icon = new Icon(stream);
            }
        }

        
        private int OpenFolderDialog()
        {
            try
            {
                mUtil = utilManager.GetInstance();

                string res = mUtil.startDialog(ref files);

                lblStatus.Text = (res != null) ? res : mCancelledTxt;

                return 1;


            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnBrowse_OnClick(object sender, System.EventArgs e)
        {
            int retVal = OpenFolderDialog();
            
            if (retVal != 1)
            {
                throw new Exception(mOpenFolderErrMsg);
            }

        }


        private void btnCreateAsmKey_OnClick(object sender, System.EventArgs e)
        {
            //Debug.WriteLine("btnCreateAsmKey_OnClick start.");
            mUtil = utilManager.GetInstance();

            try
            {
                // creates and stores a key pair in the key container.
                mUtil.CreateAsymKey(utilManager.GetKeyName);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            lblStatus.Text = mKeyCreatedMSg;
        }

         


        private async void btnEncrypt_OnClick(object sender, System.EventArgs e)
        {
            //Debug.WriteLine("btnEncrypt_OnClick started.");

            if (files == null || files.ToString() == string.Empty)
            {
                MessageBox.Show(mNoFilesMsg);
                return;
            }

            for (int i=0;i< files.Length; i++) {

                longRunningEncryptTask = EncryptFile(files[i]); 

                int[] result = await Task.WhenAll(longRunningEncryptTask);

                if (result.Contains(1))
                {
                    lblStatus.Text = mEncryptDoneMsg;
                } else
                {
                    throw new Exception(mEncryptExceptionMsg);
                }                    
            }
            //Debug.WriteLine("btnEncrypt_OnClick ended.");
        }

        private async void btnDecrypt_OnClick(object sender, System.EventArgs e)
        {
            //Debug.WriteLine("btnDecrypt_OnClick started.");
           

            if (files == null || files.ToString() == string.Empty)
            {
                MessageBox.Show(mNoFilesMsg);
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                longRunningDecryptTask = DecryptFile(files[i]);

                int[] result = await Task.WhenAll(longRunningDecryptTask);

                if (result.Contains(1))
                {
                    lblStatus.Text = mDecryptDoneMsg;
                }
                else
                {
                    throw new Exception(mDecryptExceptionMsg);
                }
            }

            Array.Clear(files, 0, files.Length);
            files = null;
            //Debug.WriteLine("btnDecrypt_OnClick ended.");
        }

        private async Task<int> DecryptFile(string inFile)
        {
            try
            {
                mUtil = utilManager.GetInstance();
                longRunningDecryptTask = mUtil.TryDecrypt(inFile);

                int result = await longRunningDecryptTask;
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }


        public async Task<int> EncryptFile(string inFile)
        {
            try
            {
                mUtil = utilManager.GetInstance();
                longRunningEncryptTask = mUtil.TryEncrypt(inFile);

                int result = await longRunningEncryptTask;

                if (result == 1)
                {
                    lblStatus.Text = mEncryptDoneMsg;
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        


        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

     
    }
}
