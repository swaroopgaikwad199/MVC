using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TnT.DataLayer
{
    public class ProductPackageHelper
    {

        /// <summary>
        /// Get last  Deck
        /// </summary>
        /// <param name="PAID"></param>
        /// <returns></returns>
        public static string getTertiarryDeck(decimal PAID,decimal JobId)
        {
            string connectionStr;
            string query = "select PackageTypeCode from PackagingDetails where JobID =" + JobId+" and  PAID="+PAID+" and (SSCC not LIKE '') and NextLevelCode is NULL";
            connectionStr = Utilities.getConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connectionStr);
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }           

        }

        public static string getTertiaryGTIN(decimal PAID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.PackageTypeCode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);
               
                var dec = availablelevels.Last();
                var TertGTIN = db.PackagingAssoDetails.Where(m => m.PAID == PAID && m.PackageTypeCode == dec).Select(x => x.GTIN).FirstOrDefault();
                return TertGTIN;
            }
            else
            {
                return "";
            }

        }

        public static string getTertiaryGTINJb(decimal JID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dataPackagingAssoDetails = db.JobDetails.Where(x => x.JD_JobID == JID).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.JD_Deckcode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);

                var dec = availablelevels.Last();
                var TertGTIN = db.JobDetails.Where(m => m.JD_JobID == JID && m.JD_Deckcode == dec).Select(x => x.JD_GTIN).FirstOrDefault();
                return TertGTIN;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// Get last/Tertiarly  Deck
        /// </summary>
        /// <param name="PAID"></param>
        /// <returns></returns>
        public static string getBottomDeck(decimal PAID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.PackageTypeCode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);
                return availablelevels.Last();
            }
            else
            {
                return "";
            }

        }

        public static string getBottomDeckJB(decimal JID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            // var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).ToList();
            var dataPackagingAssoDetails = db.JobDetails.Where(m => m.JD_JobID == JID).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.JD_Deckcode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);
                return availablelevels.Last();
            }
            else
            {
                return "";
            }

        }

        public static string getTopDeck(decimal PAID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.PackageTypeCode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);
                return availablelevels.FirstOrDefault();
            }
            else
            {
                return "";
            }

        }

        public static string getTopDeckJB(decimal JID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var dataPackagingAssoDetails = db.JobDetails.Where(m => m.JD_JobID == JID).ToList();
            List<string> availablelevels = new List<string>();

            foreach (var item in dataPackagingAssoDetails)
            {
                string code = item.JD_Deckcode;
                if (code == "PPB")
                {
                    availablelevels.Add("PPB");
                }

                if (code == "MOC")
                {
                    availablelevels.Add("MOC");
                }

                if (code == "OBX")
                {
                    availablelevels.Add("OBX");
                }
                if (code == "ISH")
                {
                    availablelevels.Add("ISH");
                }
                if (code == "OSH")
                {
                    availablelevels.Add("OSH");
                }
                if (code == "PAL")
                {
                    availablelevels.Add("PAL");
                }
            }
            if (availablelevels.Count > 0)
            {
                availablelevels = sorttheLevels(availablelevels);
                return availablelevels.FirstOrDefault();
            }
            else
            {
                return "";
            }

        }

        public static string getTopDeck(string JobId)
        {
            try
            {
                decimal JID = Convert.ToDecimal(JobId);
                ApplicationDbContext db = new ApplicationDbContext();
                var paid = db.Job.Find(JID).PAID;
                var datapackageLableMaster = db.PackageLabelMaster.Where(m => m.PAID == paid).ToList();
                List<string> availablelevels = new List<string>();
                //bool isPPB, isMOC, isOBX, isISH, isOSH, isPAL ;
                //retrive the level mainained
                foreach (var item in datapackageLableMaster)
                {
                    string code = item.Code;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }
                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }
                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }
                if (availablelevels.Count > 0)
                {
                    availablelevels = sorttheLevels(availablelevels);
                    return availablelevels.FirstOrDefault();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string getGTIN(decimal PAID, string Level)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var gtin = db.PackagingAssoDetails.Where(p => p.PAID == PAID && p.PackageTypeCode == Level).Select(p => p.GTIN).FirstOrDefault();
                if (!string.IsNullOrEmpty(gtin))
                {
                    return gtin;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                return "";
            }

        }

        public static string getGTINJb(decimal JID, string Level)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var gtin = db.JobDetails.Where(p => p.JD_JobID == JID && p.JD_Deckcode == Level).Select(p => p.JD_GTIN).FirstOrDefault();
                if (!string.IsNullOrEmpty(gtin))
                {
                    return gtin;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                return "";
            }

        }

        public static List<string> sorttheLevels(List<string> levels)
        {
            try
            {
                // string code;
                List<string> output = new List<string>(6);

                for (int i = 0; i < 6; i++)
                {
                    output.Add("");
                    //= "";
                }

                foreach (string code in levels)
                {
                    if (code == "PPB")
                    {
                        output[0] = code;
                    }

                    if (code == "MOC")
                    {
                        output[1] = code;
                    }

                    if (code == "OBX")
                    {
                        output[2] = code;
                    }
                    if (code == "ISH")
                    {
                        output[3] = code;
                    }
                    if (code == "OSH")
                    {
                        output[4] = code;
                    }
                    if (code == "PAL")
                    {
                        output[5] = code;
                    }
                }

                output.RemoveAll(x => x.Equals(""));
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<string> sorttheLevelsDesc(List<string> levels)
        {
            try
            {
                // string code;
                List<string> output = new List<string>(6);

                for (int i = 0; i < 6; i++)
                {
                    output.Add("");
                    //= "";
                }

                foreach (string code in levels)
                {
                    if (code == "PPB")
                    {
                        output[5] = code;
                    }

                    if (code == "MOC")
                    {
                        output[4] = code;
                    }

                    if (code == "OBX")
                    {
                        output[3] = code;
                    }
                    if (code == "ISH")
                    {
                        output[2] = code;
                    }
                    if (code == "OSH")
                    {
                        output[1] = code;
                    }
                    if (code == "PAL")
                    {
                        output[0] = code;
                    }
                }

                output.RemoveAll(x => x.Equals(""));
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<string> getAllDeck(string JobId)
        {
            try
            {
                decimal JID = Convert.ToDecimal(JobId);
                ApplicationDbContext db = new ApplicationDbContext();
                var paid = db.Job.Find(JID).PAID;
                var datapackageLableMaster = db.PackagingAssoDetails.Where(m => m.PAID == paid).OrderBy(x => x.Id).ToList();
                List<string> availablelevels = new List<string>();
                //bool isPPB, isMOC, isOBX, isISH, isOSH, isPAL ;
                //retrive the level mainained
                foreach (var item in datapackageLableMaster)
                {
                    string code = item.PackageTypeCode;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }
                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }
                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }


                return availablelevels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public static List<string> getAllDecks(DateTime FromDt, DateTime ToDt)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                List<string> availableLevels = new List<string>();
                var Jobs = db.Job.Where(j => j.CreatedDate >= FromDt && j.CreatedDate <= ToDt).ToList();
                if (Jobs.Count() > 0)
                {
                    foreach (var jb in Jobs)
                    {                         
                        var lst = getAllDeck(jb.JID.ToString());
                        availableLevels.AddRange(lst);
                    }
                    availableLevels = availableLevels.Distinct().ToList();
                    availableLevels = sorttheLevels(availableLevels);
                    return availableLevels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static void getPacakgingAssoDetails(decimal PAID)
        {

        }

    }
}