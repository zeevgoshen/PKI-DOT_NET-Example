using NUnit.Framework;
using PingCryptoTest_Zeev.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingCryptoTest_Zeev.Tests
{

   
    [TestFixture]
    class utilManagerTests
    {

        const string mTestPath = @"C:\testing_pki\osx_vm_instructions.txt";
        const string mTestDecryptPath = @"C:\testing_pki\dec\osx_vm_instructions.txt";
        int res;
        string[] files;


        [TestCase]
        public void utilManagerInstanceEqualityTest()
        {
            utilManager firstManager = utilManager.GetInstance();
            utilManager secondManager = utilManager.GetInstance();


            Assert.AreSame(firstManager, secondManager);
            //Assert.AreEqual(firstManager, secondManager);
        }

        [TestCase]
        public async Task encryptTestWrongFileName()
        {
            files = new string[2];
            utilManager firstManager = utilManager.GetInstance();
            res = await firstManager.TryEncrypt("fgfgfgf");
            Assert.AreEqual(res, 0);
            return;

        }

        [TestCase]
        public async Task encryptTestRightFileName()
        {
            utilManager firstManager = utilManager.GetInstance();
            res = await firstManager.TryEncrypt(mTestPath);
            FileAssert.Exists(mTestPath);
            Assert.AreEqual(res, 1);
            return;
        }


        [TestCase]
        public void testCreateAsymKeys()
        {
            utilManager firstManager = utilManager.GetInstance();
            string empty = "new";

            Assert.Fail(empty, firstManager.CreateAsymKey(null));

        }
        

        [TestCase]
        public void decryptTest()
        {
            
            try
            {
                FileAssert.AreEqual(mTestPath, mTestDecryptPath);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
        }

    }
}
