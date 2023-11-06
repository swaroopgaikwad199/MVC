using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace TnT.Models.Vendor
{
    public class M_Vendor
    {
        public M_Vendor()
        {
            IsActive = true;
            ServiceKey = APIKey.getAPIKey();
        }
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name= "M_VendorCompanyName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "M_VendorContactPerson", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string  ContactPerson { get; set; }

        [Required]
        [Display(Name = "M_VendorContactNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ContactNo { get; set; }

        [Required]
        [Display(Name = "M_VendorEmail", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "M_VendorAddress", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Address { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "M_VendorKey", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ServiceKey { get; set; }
    }


    class APIKey
    {
        public static string getAPIKey()
        {
            
            return RNGCharacterMask(9);
        }

        private static string RNGCharacterMask(int KeyLenght)
        {
            int maxSize = KeyLenght;
            
            //int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }



    }
    
}