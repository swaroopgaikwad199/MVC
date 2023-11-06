using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using PTPLCRYPTORENGINE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Services;

namespace TnT
{
    /// <summary>
    /// Summary description for BatchDataService
    /// </summary>
    [WebService(Namespace = "http://192.168.10.238/tnt1.5/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BatchDataService : System.Web.Services.WebService
    {
        string constr= Utilities.getConnectionString("DefaultConnection");
        [WebMethod]
        public string GetBatchData(string username,string password,string jobname, string pkTypeCode)
        {
            string qry = "select UserName,Password from users where UserName='" + username + "'";
            DataSet ds1= SqlHelper.ExecuteDataset(constr, CommandType.Text, qry);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string pwd = AESCryptor.Decrypt(ds1.Tables[0].Rows[0]["Password"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                string uname = ds1.Tables[0].Rows[0]["UserName"].ToString();
                if (pwd == password && username == uname)
                {
                    string query = "select JD_GTIN,FORMAT(j.ExpDate,'yyMM00'),j.JobName,p.Code from job j inner join JobDetails jb on j.JID=jb.JD_JobID inner join PackagingDetails p on p.JobID=j.JID where j.JobName='" + jobname + "' and p.PackageTypeCode='" + pkTypeCode + "' and jb.JD_Deckcode='" + pkTypeCode + "'";
                    DataSet ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, query);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject("Data Not Found", Newtonsoft.Json.Formatting.Indented);
                    }
                }
                else
                {
                     return JsonConvert.SerializeObject("Error Authentication Failed", Newtonsoft.Json.Formatting.Indented);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("Error Authentication Failed", Newtonsoft.Json.Formatting.Indented);
            }
        }
    }
}
