using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingCryptoTest_Zeev.src
{
    public class utilManager
    {
        private static readonly object syncLock = new object();
        private static utilManager mInstance;


        // Declare CspParmeters and RsaCryptoServiceProvider
        //
        CspParameters cspp = new CspParameters();
        RSACryptoServiceProvider rsa;

        // Path variables for source, encryption, and
        // decryption folders. Must end with a backslash.

        #region Path vars 

        //const string DecrFolder = @"F:\roms\Desktop\Zeev\0_zeev\";
        const string decPref = @"\dec\";
        //const string    EncrFolder = @"F:\roms\Desktop\Zeev\0_zeev\enc\";

        const string mTestPath = @"F:\roms\Desktop\Zeev\0_zeev\";
        const string encPref = @"enc\";
        const string encExtension = ".enc";

        const string encPref2 = @"\enc";
        string fileName = string.Empty;
        int startFileNameIndex;
        string newPath;
        string outFile;


        #endregion

        // Key container name for
        // private/public key value pair.
        private string      keyName             = "Key01";
        const string        mFilesFoundMsg      = "Files found: ";
        const string        mKeyPairCreated     = "KeyPair created in Container - ";
        const string        mErrorMsg           = "Error caused exception: ";
        const string        mUserCancelledMsg   = "User cancelled.";
        const string        mFileTypes          = "*.txt";
        const string        mGeneralMessage     = "Message";
        const string        mBackSlashEscaped   = "\\";
        const string        mKeyNameNullErrMsg  = "Key name is null.";
        private utilManager()
        {

        }


        public static string GetKeyName { get { return mInstance.keyName; } set { mInstance.keyName = value; } }
        
        public static utilManager GetInstance()
        {
            if (mInstance == null) // first check
            {
                lock (syncLock)
                {
                    if (mInstance == null) // second check
                    {
                        mInstance = new utilManager();
                    }

                }
            }
            return mInstance;
        }


        internal string startDialog(ref string [] files)
        {
            FolderBrowserDialog mOpenFolderDialog = null;
            
            if (mOpenFolderDialog == null)
            {
                mOpenFolderDialog = new FolderBrowserDialog();
            }
            DialogResult result = mOpenFolderDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(mOpenFolderDialog.SelectedPath))
            {
      
                try { 
                    files = Directory.GetFiles(mOpenFolderDialog.SelectedPath, mFileTypes, SearchOption.AllDirectories);
                    System.Windows.Forms.MessageBox.Show(mFilesFoundMsg + files.Length.ToString(), mGeneralMessage);
                    return mOpenFolderDialog.SelectedPath;
                }
                catch (Exception ex) {
                    return mErrorMsg + ex.Message; }
            } else
            {
                // Cancel pressed case
                if ((files != null) && (files.Length > 0))
                    return files[0].Substring(0, files[0].LastIndexOf(mBackSlashEscaped));

                return mUserCancelledMsg;
            }

            
        }

        internal async Task<int> TryDecrypt(string inFile)
        {
            try
            {

                RijndaelManaged rjndl = new RijndaelManaged();
                rjndl.KeySize = 256;
                rjndl.BlockSize = 256;
                rjndl.Mode = CipherMode.CBC;

                // Create byte arrays to get the length of
                // the encrypted key and IV.
                // These values were stored as 4 bytes each
                // at the beginning of the encrypted package.
                byte[] LenK = new byte[4];
                byte[] LenIV = new byte[4];

                // Consruct the file name for the decrypted file.
                string decPath = inFile.Substring(0, inFile.LastIndexOf(mBackSlashEscaped)) + decPref;
             

                string encPath = inFile.Substring(0, inFile.LastIndexOf(mBackSlashEscaped)) + encPref2; 
                encPath += inFile.Substring(inFile.LastIndexOf(mBackSlashEscaped));
                fileName = inFile.Substring(inFile.LastIndexOf(mBackSlashEscaped));

                string newinFile = encPath.Substring(0, encPath.LastIndexOf(".")) + encExtension;

                if (!File.Exists(newinFile))
                {
                    return 0;
                }
                // newinFile is the encoded file to be read and decrypted
                //
                using (FileStream inFs = new FileStream(newinFile, FileMode.Open))
                {

                    inFs.Seek(0, SeekOrigin.Begin);
                    inFs.Seek(0, SeekOrigin.Begin);
                    inFs.Read(LenK, 0, 3);
                    inFs.Seek(4, SeekOrigin.Begin);
                    inFs.Read(LenIV, 0, 3);

                    // Convert the lengths to integer values.
                    int lenK = BitConverter.ToInt32(LenK, 0);
                    int lenIV = BitConverter.ToInt32(LenIV, 0);

                    // Determine the start postition of
                    // the ciphter text (startC)
                    // and its length(lenC).
                    int startC = lenK + lenIV + 8;
                    int lenC = (int)inFs.Length - startC;

                    // Create the byte arrays for
                    // the encrypted Rijndael key,
                    // the IV, and the cipher text.
                    byte[] KeyEncrypted = new byte[lenK];
                    byte[] IV = new byte[lenIV];

                    // Extract the key and IV
                    // starting from index 8
                    // after the length values.
                    inFs.Seek(8, SeekOrigin.Begin);
                    inFs.Read(KeyEncrypted, 0, lenK);
                    inFs.Seek(8 + lenK, SeekOrigin.Begin);
                    inFs.Read(IV, 0, lenIV);
                    Directory.CreateDirectory(decPath);


                    //TODO: remove the 3 lines pf rsa
                    //
                    cspp.KeyContainerName = keyName;
                    cspp.Flags = CspProviderFlags.UseMachineKeyStore;
                    rsa = new RSACryptoServiceProvider(cspp);
                    rsa.PersistKeyInCsp = true;

                    //RSACryptoServiceProvider.UseMachineKeyStore = true;

                    // Use RSACryptoServiceProvider
                    // to decrypt the Rijndael key.
                    byte[] KeyDecrypted = await Task.Run(() => rsa.Decrypt(KeyEncrypted, false));

                    // Decrypt the key.
                    ICryptoTransform transform = rjndl.CreateDecryptor(KeyDecrypted, IV);


                    // Decrypt the cipher text from
                    // from the FileSteam of the encrypted
                    // file (inFs) into the FileStream
                    // for the decrypted file (outFs).
                    using (FileStream outFs = new FileStream(decPath + fileName, FileMode.Create))
                    {


                        int count = 0;
                        int offset = 0;

                        // blockSizeBytes can be any arbitrary size.
                        int blockSizeBytes = rjndl.BlockSize / 8;
                        byte[] data = new byte[blockSizeBytes];

                        // By decrypting a chunk a time,
                        // you can save memory and
                        // accommodate large files.

                        // Start at the beginning
                        // of the cipher text.
                        inFs.Seek(startC, SeekOrigin.Begin);
                        using (CryptoStream outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                        {
                            do
                            {
                                count = inFs.Read(data, 0, blockSizeBytes);
                                offset += count;
                                outStreamDecrypted.Write(data, 0, count);
                            }
                            while (count > 0);

                            outStreamDecrypted.FlushFinalBlock();
                            outStreamDecrypted.Close();
                        }
                        outFs.Close();
                    }
                    inFs.Close();
                    return 1;
             
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        internal int CreateAsymKey(string keyName)
        {
            try
            {
                if (keyName == null)
                    throw new Exception(mKeyNameNullErrMsg);

                cspp.KeyContainerName = keyName;
                cspp.Flags = CspProviderFlags.UseMachineKeyStore;
                rsa = new RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                MessageBox.Show(mKeyPairCreated + keyName);

                return 1;

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            //RSAParameters rsaKeyInfo = rsa.ExportParameters(false);

        }

        public async Task<int> TryEncrypt(string inFile)
        {
            
            try
            {

                RijndaelManaged rjndl = new RijndaelManaged();
                rjndl.KeySize = 256;
                rjndl.BlockSize = 256;
                rjndl.Mode = CipherMode.CBC;
                ICryptoTransform transform = rjndl.CreateEncryptor();

                // Use RSACryptoServiceProvider to
                // enrypt the Rijndael key.
                // rsa is previously instantiated: 
                //    rsa = new RSACryptoServiceProvider(cspp);

                cspp.KeyContainerName = keyName;
                cspp.Flags = CspProviderFlags.UseMachineKeyStore;
                rsa = new RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;
                              
                byte[] keyEncrypted = await Task.Run(() =>  rsa.Encrypt(rjndl.Key, false));


                // Create byte arrays to contain
                // the length values of the key and IV.
                byte[] LenK = new byte[4];
                byte[] LenIV = new byte[4];

                int lKey = keyEncrypted.Length;
                LenK = BitConverter.GetBytes(lKey);
                int lIV = rjndl.IV.Length;
                LenIV = BitConverter.GetBytes(lIV);

                // Write the following to the FileStream
                // for the encrypted file (outFs):
                // - length of the key
                // - length of the IV
                // - ecrypted key
                // - the IV
                // - the encrypted cipher content



                #region Creating enc folder and file
                
                // Creating the destination directory
                startFileNameIndex = inFile.LastIndexOf("\\") + 1;
                newPath = inFile.Substring(0, startFileNameIndex) + encPref; 
                Directory.CreateDirectory(newPath);

                // Fullpath for creating a new encrypted file with extension ".enc"
                newPath += inFile.Substring(startFileNameIndex, inFile.LastIndexOf(".") - startFileNameIndex) + encExtension;              
                outFile = newPath;

                #endregion 

                
                // If the file already exists, it will be overwritten
                using (FileStream outFs = new FileStream(outFile, FileMode.Create))
                {

                    outFs.Write(LenK, 0, 4);
                    outFs.Write(LenIV, 0, 4);
                    outFs.Write(keyEncrypted, 0, lKey);
                    outFs.Write(rjndl.IV, 0, lIV);

                    // Now write the cipher text using
                    // a CryptoStream for encrypting.
                    using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                    {

                        // By encrypting a chunk at
                        // a time, you can save memory
                        // and accommodate large files.
                        int count = 0;
                        int offset = 0;

                        // blockSizeBytes can be any arbitrary size.
                        int blockSizeBytes = rjndl.BlockSize / 8;
                        byte[] data = new byte[blockSizeBytes];
                        int bytesRead = 0;

                        //
                        using (FileStream inFs = new FileStream(inFile, FileMode.Open))
                        {
                            do
                            {
                                count = inFs.Read(data, 0, blockSizeBytes);
                                offset += count;
                                outStreamEncrypted.Write(data, 0, count);
                                bytesRead += blockSizeBytes;
                            }
                            while (count > 0);
                            inFs.Close();
                        }
                        outStreamEncrypted.FlushFinalBlock();
                        outStreamEncrypted.Close();
                    }
                    outFs.Close();
                }

                return 1;
            }
            catch(Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                return 0;
            }

        }


    }
}
