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
        Label                   lblMessages;
        StatusStrip             dynamicStatusStrip;
        ToolStripStatusLabel    lblStatus;
        string[]                files;
        const string            iconPath = "icon.ico";
        utilManager             mUtil;
        const string            mCancelledTxt = "Browse cancelled.";
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

        
        private void createOrOpenFolderDialog()
        {
            
            mUtil = utilManager.GetInstance();

            string res = mUtil.startDialog(ref files);

            lblStatus.Text = (res != null) ? res : mCancelledTxt;

            Debug.WriteLine("Folder Dialog done.");

        }

        private void btnBrowse_OnClick(object sender, System.EventArgs e)
        {
            createOrOpenFolderDialog();
            Debug.WriteLine("");
            //lblStatus.Text = "Browse cancelled.";

        }


        private void btnCreateAsmKey_OnClick(object sender, System.EventArgs e)
        {
            Debug.WriteLine("btnCreateAsmKey_OnClick start.");
            // Stores a key pair in the key container.
            //

            mUtil = utilManager.GetInstance();

            mUtil.CreateAsymKey();

            lblStatus.Text = "Key pair created.";

        }

         


        private async void btnEncrypt_OnClick(object sender, System.EventArgs e)
        {
            Debug.WriteLine("btnEncrypt_OnClick started.");
            Debug.WriteLine("Debug.WriteLine btnEncrypt_OnClick fired");

            if (files == null || files.ToString() == string.Empty)
            {
                MessageBox.Show("No Files found. Click 'Browse...' for .txt files...");

                Debug.WriteLine("No Files.");
                return;
                // throw new exception
            }


            for (int i=0;i< files.Length; i++) {
                //longRunningTask1 = EncryptFile(files[i]);

                longRunningEncryptTask = EncryptFile(files[i]); 

                //int[] result = await Task.WhenAll(longRunningTask1, longRunningTask2);
                int[] result = await Task.WhenAll(longRunningEncryptTask);

                if (result.Contains(1))
                {
                    lblStatus.Text = "Encrypt done.";
                    //TODO: handle error if one of the tasks returned 0.
                    //x = longRunningTask1.Result;
                    //longRunningTask1 = null;
                    //longRunningTask2 = null;

                    
                }
                    
            }

            Debug.WriteLine("btnEncrypt_OnClick ended.");
        }

        private async void btnDecrypt_OnClick(object sender, System.EventArgs e)
        {
            Debug.WriteLine("btnDecrypt_OnClick started.");
           

            if (files == null || files.ToString() == string.Empty)
            {
                MessageBox.Show("No Files found. Click 'Browse...' for .txt files...");

                return;
                // throw new exception
            }


            for (int i = 0; i < files.Length; i++)
            {
                longRunningDecryptTask = DecryptFile(files[i]);

                int[] result = await Task.WhenAll(longRunningDecryptTask);

                if (result.Contains(1))
                {

                    lblStatus.Text = "Decrypt done.";
                    
                }
            }

            Array.Clear(files, 0, files.Length);
            files = null;
            Debug.WriteLine("btnDecrypt_OnClick ended.");
        }

        private async Task<int> DecryptFile(string inFile)
        {
            mUtil = utilManager.GetInstance();

            longRunningDecryptTask = mUtil.TryDecrypt(inFile);


            int result = await longRunningDecryptTask;
            //await mUtil.TryDecrypt(inFile);
            Debug.WriteLine("DecryptFile result: " + result);
            return result;
        }


        public async Task<int> EncryptFile(string inFile)
        {
            Debug.WriteLine("EncryptFile started.");

            // TODO:
            // 1) decide between AES or this RijndaelManaged

            mUtil = utilManager.GetInstance();

            
            longRunningEncryptTask = mUtil.TryEncrypt(inFile);

            int result = await longRunningEncryptTask;
            //use the result 
            Debug.WriteLine("EncryptFile result: " + result);
            return result;

        }

        


        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

     
    }
}
