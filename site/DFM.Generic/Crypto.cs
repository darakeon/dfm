using System;
using System.Text;

namespace Sulamerica.DomainModel.AutenticacaoAdmin
{
    internal static class Crypto
    {
        internal static String Decrypt(String value)
        {
            var toEncryptArray = Convert.FromBase64String(value);

            byte[] resultArray;

            using (var tdes = new TripleDESDisposable())
            {
                resultArray = tdes.Decrypt(toEncryptArray);
            }

            return Encoding.UTF8.GetString(resultArray);
        }

        internal static String Encrypt(String value)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(value);

            byte[] resultArray;

            using (var tdes = new TripleDESDisposable())
            {
                resultArray = tdes.Encrypt(toEncryptArray);
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }



        


    }

}