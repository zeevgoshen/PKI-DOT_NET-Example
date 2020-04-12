using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingCryptoTest_Zeev.Tests
{

    [TestFixture]
    class MainFormTests
    {
        string[] files;

        [TestCase]
        public void testFormFunctions()
        {
            MainForm mf = new MainForm();

            Task<int> longRunningEncryptTask = mf.EncryptFile(files[0]);
        }


    }
}
