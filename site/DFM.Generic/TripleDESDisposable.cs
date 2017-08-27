using System;
using System.Security.Cryptography;
using System.Text;

namespace Sulamerica.DomainModel.AutenticacaoAdmin
{
    class TripleDESDisposable : IDisposable
    {
        private const String key = "";

        private readonly TripleDESCryptoServiceProvider tripleDES;


        internal TripleDESDisposable()
        {
            //if hashing was used get the hash code with regards to your key
            var hashmd5 = new MD5CryptoServiceProvider();

            var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice
            hashmd5.Clear();

            tripleDES = new TripleDESCryptoServiceProvider
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }
        

        internal byte[] Encrypt(byte[] toEncryptArray)
        {
            var cTransform = tripleDES.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }


        internal byte[] Decrypt(byte[] toEncryptArray)
        {
            var cTransform = tripleDES.CreateDecryptor();

            //transform the specified region of bytes array to resultArray
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }


        internal void Dispose()
        {
            //Release resources held by TripleDes Encryptor                
            tripleDES.Clear();
        }


    }
}
