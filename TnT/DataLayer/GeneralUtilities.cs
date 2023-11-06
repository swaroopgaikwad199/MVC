using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer
{
    public class GeneralUtilities
    {
        

        public static List<string> getDateFormats()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dts = db.S_DateFormats.Select(x => x.Format).ToList();
            return dts;
        }

        public static List<string> getOrientationTypes()
        {
            List<string> orientations = new List<string>();
            orientations.Add("Landscape");
            orientations.Add("Portrait");
            return orientations;
        }

        public static List<string> getDPIs()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dts = db.S_DPI.Select(x => x.dpi.Trim()).ToList();            
            return dts;
        }

        public static List<string> getZPLFonts()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dts = db.S_ZPLFonts.Select(x => x.font).ToList();
            return dts;
        }

        public static List<string> getSystemFonts()
        {

            List<string> sysfonts = new List<string>();
            foreach (FontFamily oneFontFamily in FontFamily.Families)
            {
                sysfonts.Add(oneFontFamily.Name);
            }
            return sysfonts;

        }
        public static Dictionary<string, string> getPaperSize()
        {
           // List<string> paperSizes = new List<string>();
           // List<string> lstPaperSizes = new List<string>();

            Dictionary<string, string> papers = new Dictionary<string, string>();
            PrinterSettings settings = new PrinterSettings();
           
            foreach (PaperSize size in settings.PaperSizes)
                papers.Add(size.PaperName, size.Width + " X " + size.Height);

            //paperSizes.Add(size.PaperName);                

            return papers;
        }
        

    }

    public static class GeneralDataHelper
    {
        public static System.Data.DataTable convertToDataTable<T>(this IList<T> data)
        {
            try
            {


                PropertyDescriptorCollection properties =
                    TypeDescriptor.GetProperties(typeof(T));
                System.Data.DataTable table = new System.Data.DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    System.Data.DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }

                return table;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    
}