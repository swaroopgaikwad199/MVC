using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using Microsoft.Office.Interop;
using TnT.DataLayer;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TnT.Controllers
{
    public class POCController : Controller
    {
        // GET: POC
        public ActionResult Index() 
        {
            

            ExcellHelper hlp = new ExcellHelper();
            var ds = getDataSetForOfProductandBatch();
            var path = Server.MapPath("~/Content/a1.xlsx");          
            hlp.ExportDataSetToExcel(ds, path);
          
            return View();
        }


     

        private DataSet getDataSetForOfProductandBatch()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            decimal paid = 42388;
            var product = db.PackagingAsso.Where(x => x.PAID == paid).ToList();
            var productDetails = db.PackagingAssoDetails.Where(x => x.PAID == paid).OrderBy(x => x.Id).ToList();

            var dtProd = ToDataTable(product);
            var dtProdDtls = ToDataTable(productDetails);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtProd);
            ds.Tables.Add(dtProdDtls);

            return ds;
        }



        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

      



        //public FileResult Download(string FileID)
        //{
        //    int CurrentFileID = Convert.ToInt32(FileID);
        //    var filesCol = obj.GetFiles();
        //    string CurrentFileName = (from fls in filesCol
        //                              where fls.FileId == CurrentFileID
        //                              select fls.FilePath).First();

        //    string contentType = string.Empty;

        //    if (CurrentFileName.Contains(".pdf"))
        //    {
        //        contentType = "application/pdf";
        //    }

        //    else if (CurrentFileName.Contains(".docx"))
        //    {
        //        contentType = "application/docx";
        //    }
        //    return File(CurrentFileName, contentType, CurrentFileName);
        //}

    }
}