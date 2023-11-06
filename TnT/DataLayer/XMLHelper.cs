using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{
    public class XMLHelper
    {

        public DataSet readXML(string FileName)
        {
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/DataFiles/" + FileName + "");
                DataSet ds = new DataSet();
                ds.ReadXml(filePath);
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public void writeXML(DataSet ds)
        {
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Files/output.xml");
                ds.WriteXml(filePath);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}