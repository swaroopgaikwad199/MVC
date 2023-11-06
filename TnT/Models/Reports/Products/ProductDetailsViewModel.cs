using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models.Product;

namespace TnT.Models.Reports.Products
{
    public class ProductDetailsViewModel
    {

        public string CompanyName { get; set; }

        public string  Address { get; set; }
       
        public string UserName { get; set; }
        public  List<PackagingAsso>  Products { get; set; }
        public   List<PackagingAssoDetails> ProductDetails { get; set; }

    }


   
}