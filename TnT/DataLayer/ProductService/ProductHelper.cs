using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.ProductService
{
    public class ProductHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsProductExisting(string Name, string FGCode, string ProductCode)
        {
            try
            {
                var data = db.PackagingAsso.Where(x => x.Name == Name && x.FGCode == FGCode && x.ProductCode == ProductCode).FirstOrDefault();
                if (data != null)
                { return true; }
                else
                { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}