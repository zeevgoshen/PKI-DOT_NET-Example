
Ping Identity PKI home test, Zeev G.
*----------------------------------*
14/04/20

1. Please run PingCryptoTest_Zeev.exe as administrator, in some cases it is needed.

2. 2 new sub-directories will be created inside the selected folder - "enc", "dec".

3. "enc" folder will include the encrypted files.

4. "dec" folder will include the newly decrypted files, identical to the original .txt files.

5. Public and Private keys are be created by RSACryptoServiceProvider and will be stored and retrieved from the local Machine store.

6. NUnit through Visual Studio was used for additional testing.





