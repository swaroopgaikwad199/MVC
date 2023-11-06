using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace TnT.DataLayer
{
    public class PTPLUidGen
    {
        private string alphanumericCharacters = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";

        private string alphanumericCharactersForEU = "ABCDEFGHKMNPRSTVWXYZ0123456789";
        private string alphanumericCharactersForCryptoCode = "ABCDEFGHKMNPRSTVWXYZ0123456789+/";
        public string GenerateUID(int UIDLength,string selectedJobType)
        {
            if (selectedJobType != "EU" || selectedJobType=="")
            {
                return this.GetRandomString(UIDLength, this.alphanumericCharacters);
            }
            else
            {
                return this.GetRandomString(UIDLength, this.alphanumericCharactersForEU);
            }
        }

        public string GenerateCryptoCode(int UIDLength)
        {

            return this.GetRandomString(UIDLength, this.alphanumericCharactersForCryptoCode);

        }


        private string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            char[] array = characterSet.Distinct<char>().ToArray<char>();
            if (array.Length == 0)
            {
                throw new ArgumentException("characterSet must not be empty", "characterSet");
            }
            byte[] array2 = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(array2);
            char[] array3 = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong num = BitConverter.ToUInt64(array2, i * 8);
                array3[i] = array[(int)(checked((IntPtr)(num % unchecked((ulong)array.Length))))];
            }
            return new string(array3);
        }
    }
}