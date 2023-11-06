using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{
    public class JSONHelper
    {

        public DataSet readJSON(string FileName)
        {
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/DataFiles/" + FileName + "");
                string JsonData = System.IO.File.ReadAllText(filePath);
                DataSet ds = new DataSet();
                ds = JsonConvert.DeserializeObject<DataSet>(JsonData);
                //ds.ReadXml(filePath);
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
          
        }        

    }
}