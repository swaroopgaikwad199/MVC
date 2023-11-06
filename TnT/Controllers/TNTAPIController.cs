using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.Product;
using TnT.Models.Job;
using System.Xml;
using System.Web.Http;
using TnT.Models;
using REDTR.JOB;
using REDTR.UTILS;
using REDTR.DB.BLL;
using PTPLCRYPTORENGINE;
using TnT.DataLayer.Line;
using TnT.DataLayer;
using TnT.DataLayer.Security;
using System.Data.Entity;
using TnT.DataLayer.JobService;
using TnT.Models.Code;
using System.Data;
using REDTR.HELPER;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using TnT.DataLayer.RFXCELService;
using TnT.Models.TraceLinkImporter;

namespace TnT.Controllers
{
    public class TNTAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        int firstCount = 0;
        int secondCount = 0;

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5", "value6" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult ExtraUIDs(int JobID, int Line_JobId, int Line_PAID, int Quantity, string PackagingLevel, bool IsTertiary, bool IsLoose, string LineCode = "")
        {
            try
            {

                if (JobID <= 0)
                {
                    return BadRequest("Please Provide JobID ");
                }
                if (Line_JobId <= 0)
                {
                    return BadRequest("Please Provide Line JobId");
                }
                if (Line_PAID <= 0)
                {
                    return BadRequest("Please Provide Line PAID");
                }
                if (Quantity <= 0)
                {
                    return BadRequest("Please Provide Quantity");
                }
                if (string.IsNullOrEmpty(LineCode))
                {
                    return BadRequest("Please Provide LineCode");
                }
                if (string.IsNullOrEmpty(PackagingLevel))
                {
                    return BadRequest("Please Provide PackagingLevel");
                }

                Job job = db.Job.Find(JobID);
                if (job == null)
                {
                    return BadRequest(TnT.LangResource.GlobalRes.TempDataReceiverInvalidReq);
                }

                var customer = db.M_Customer.Find(job.CustomerId);
                var provider = db.M_Providers.Find(customer.ProviderId).Id;
                var IAC_CIN = db.Settings.FirstOrDefault();
                var ExtraUIDStat = "";
                bool result = ImportExtraUID(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, provider);
             
                if (result == true)
                {
                    ExtraUIDStat += "Extra UIDs generated.";
                }
                else
                {
                    ExtraUIDStat += "Fail to generate Extra UIDs.";
                }

                string Type = db.JOBTypes.Where(j => j.TID == job.TID).Select(j => j.Job_Type).FirstOrDefault();

                var LineServer = db.LineLocation.Where(x => x.ID == job.LineCode).FirstOrDefault();
                string ConnectionStr = "";
                if (LineServer != null)
                {
                    ConnectionStr = @"Data Source=" + LineServer.LineIP + ";" + "Initial Catalog=" + LineServer.DBName + ";Persist Security Info=True;User ID=" + LineServer.SQLUsername + ";Password=" + LineServer.SQLPassword + ";MultipleActiveResultSets=True";
                }

                using (var connection = new SqlConnection(ConnectionStr))
                {

                    connection.Open();
                    try
                    {
                        string Query = "Select count(*) AS CO from PackagingDetails where PAID = " + Line_PAID + " AND JobId = " + Line_JobId;
                        SqlCommand cmd = new SqlCommand(Query, connection);
                        firstCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                string ConnectionServer = Utilities.getConnectionString("DefaultConnection");
                TransferJobData tjd = new TransferJobData();
               // tjd.TransferExtraUids(job, ConnectionStr, Line_PAID, (decimal)Line_JobId, ConnectionServer);
                tjd.TransferV1(job, ConnectionStr, Line_PAID, (decimal)Line_JobId, ConnectionServer, true);



                using (var connection = new SqlConnection(ConnectionStr))
                {

                    connection.Open();
                    try
                    {
                        string Query = "Select count(*) AS CO from PackagingDetails where PAID = " + Line_PAID + " AND JobId = " + Line_JobId;
                        SqlCommand cmd = new SqlCommand(Query, connection);
                        secondCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                ConnectionStr = string.Empty;

                if (secondCount > firstCount)
                {
                    return Ok(ExtraUIDStat);
                }
                else
                {
                    string elseMsg = TnT.LangResource.GlobalRes.TrailTNTApiLineUidNotUpdated;
                    return Ok(elseMsg);
                }



            }
            catch (Exception ex)
            {
                return BadRequest(TnT.LangResource.GlobalRes.TempDataReceiverInvalidReq);

            }
        }

        public bool ImportExtraUID(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            var provider = db.M_Providers.Where(x => x.Id == ProviderId).FirstOrDefault();
            UIDCustomTypeHelper ctHelpr = new UIDCustomTypeHelper();
            UIDCustomType ctype = UIDCustomTypeHelper.convertToUIDCustomType(provider.Code);
            if (ctype == UIDCustomType.PROP)
            {
                return GenerateExtraUIds(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose);
            }
            else
            {
                return GenerateExtraUIdsUIDExhuastV1(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);
            }

            //bool result = false;
            //switch (ctype)
            //{
            //    case UIDCustomType.PROP:
            //        return GenerateExtraUIds(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose);

            //    case UIDCustomType.TLINK:
            //        return GenerateTLExtraUIds(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);

            //    case UIDCustomType.RFXL:
            //        return GenerateRFXCELExtraUIds(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);

            //    case UIDCustomType.TKEY:
            //        return GenerateTracekeyExtraUIds(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);

            //    case UIDCustomType.NA:
            //        return false;
            //}
            //return result;
        }

        private bool GenerateExtraUIds(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateExtraUIDData(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose);
        }

        private bool GenerateTLExtraUIds(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateTLExtraUIDData(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);
        }

        private bool GenerateRFXCELExtraUIds(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateRFXCELExtraUIDData(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);
        }

        public bool GenerateExtraUIdsUIDExhuastV1(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            DbHelper dbhelper = new DbHelper();

            //var pkgasso = db.PackagingAsso.Where(x => x.PAID == job.PAID).FirstOrDefault();
            var pkgassoDetails = db.PackagingAssoDetails.FirstOrDefault(x => x.PAID == job.PAID && x.PackageTypeCode == PackagingLevel);
            string firstLevel = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id).FirstOrDefault().PackageTypeCode;
            string tertiaryLevel = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderByDescending(x => x.Id).FirstOrDefault().PackageTypeCode;
            var customer = db.M_Customer.FirstOrDefault(x => x.Id == job.CustomerId);
            var selectedJobType = db.JOBTypes.FirstOrDefault(x => x.TID == job.TID);

            bool isCodeFromStore = false;
            if ((pkgassoDetails.PackageTypeCode == firstLevel) ||
                (tertiaryLevel == PackagingLevel && customer.IsSSCC) ||
                (firstLevel != PackagingLevel && tertiaryLevel != PackagingLevel && Convert.ToBoolean(customer.IsProvideCodeForMiddleDeck)))
            {
                isCodeFromStore = true;
            }
            if (isCodeFromStore)
            {
                string qery = "SELECT GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore WHERE IsUsed = 0 AND GTIN ='" + pkgassoDetails.GTIN + "' GROUP BY GTIN";

                if (selectedJobType.Job_Type == "RUSSIA" && pkgassoDetails.PackageTypeCode == firstLevel)
                {
                    qery = "SELECT GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore WHERE CryptoCode IS NOT NULL AND IsUsed = 0 AND GTIN ='"
                            + pkgassoDetails.GTIN + "' GROUP BY GTIN";
                }

                DataSet ds = dbhelper.GetDataSet(qery);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    { return false; }// "NO SERIAL NUMBER FOUND FOR " + item.GTIN; 
                }

                int availableSerialNo = 0;
                int.TryParse(ds.Tables[0].Rows[0]["CNT"].ToString(), out availableSerialNo);
                if (Quantity > availableSerialNo)
                {
                    return false;
                }
            }

            M_Identities ids = new M_Identities();
            ids.CreatedOn = DateTime.Now;
            ids.CustomerId = (int)job.CustomerId;
            ids.GTIN = pkgassoDetails.GTIN;
            ids.PPN = pkgassoDetails.PPN;
            ids.JID = job.JID;
            ids.PackageTypeCode = pkgassoDetails.PackageTypeCode;
            ids.IsTransfered = false;
            ids.IsExtra = true;
            db.M_Identities.Add(ids);
            db.SaveChanges();

            DataTable DtconvertedData = new DataTable();
            DtconvertedData.Columns.Add("MasterId", typeof(int));
            DtconvertedData.Columns.Add("SerialNo", typeof(string));
            DtconvertedData.Columns.Add("CodeType", typeof(int));
            DtconvertedData.Columns.Add("PackTypeCode", typeof(string));
            DtconvertedData.Columns.Add("IsTransfered", typeof(int));
            DtconvertedData.Columns.Add("CryptoCode", typeof(string));
            DtconvertedData.Columns.Add("PublicKey", typeof(string));
            BulkDataHelper bulkHlpr = new BulkDataHelper();

            if (isCodeFromStore) // GET CODE FROM STORE
            {
                List<X_TracelinkUIDStore> tlStore = new List<X_TracelinkUIDStore>();
                if (selectedJobType.Job_Type == "RUSSIA" && pkgassoDetails.PackageTypeCode == firstLevel)
                {
                    tlStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == pkgassoDetails.GTIN && x.IsUsed == false && !string.IsNullOrEmpty(x.CryptoCode)).Take(Quantity).ToList();
                }
                else
                {
                    tlStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == pkgassoDetails.GTIN && x.IsUsed == false).Take(Quantity).ToList();
                }
                foreach (var item in tlStore)
                {
                    DtconvertedData.Rows.Add(new object[] { ids.Id, item.SerialNo, false, pkgassoDetails.PackageTypeCode, false, item.CryptoCode, item.PublicKey });
                    bulkHlpr.setFlagToTransferdUID(item.Id);
                }
            }
            else // GENERATE CODE BY PROPIX
            {
                // RUSSIA / SAP = OTHER THAN FIRST DECK == SSCC BY PROPIX
                if ((selectedJobType.Job_Type == "RUSSIA" && pkgassoDetails.PackageTypeCode != firstLevel) ||
                    (pkgassoDetails.PackageTypeCode == tertiaryLevel))
                {
                    TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                    IDGenrationFactory obj = new IDGenrationFactory();

                    var IAC_CIN = db.Settings.FirstOrDefault();
                    var cust = db.M_Customer.FirstOrDefault(x => x.Id == job.CustomerId);

                    ssccForMiddleDek.GenerateMiddleDekSSCCs((int)job.JID, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, false, cust.SSCCExt, Convert.ToInt32(job.CustomerId), Quantity);

                    var lstPropixSerialNo = ssccForMiddleDek.getMasterData();

                    string typeCode = pkgassoDetails.PackageTypeCode;
                    if (typeCode == tertiaryLevel)
                    {
                        typeCode = "SSC";
                    }

                    foreach (var sscc in lstPropixSerialNo)
                    {
                        DtconvertedData.Rows.Add(new object[] { ids.Id, sscc.Key, false, typeCode, false, string.Empty, string.Empty });
                    }
                }
                else
                {
                    // CODE GENERATED BY PROPIX THER THAN MOC == SSCC FOR SAP AND RUSSIA
                    IDGenrationFactory obj = new IDGenrationFactory();
                    var lstPropixSerialNo = obj.generateIDs(Quantity, 13, selectedJobType.Job_Type);

                    foreach (var srcode in lstPropixSerialNo)
                    {
                        DtconvertedData.Rows.Add(new object[] { ids.Id, srcode, false, pkgassoDetails.PackageTypeCode, false, string.Empty, string.Empty });
                    }
                }
            }

            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
        }

        private bool GenerateTracekeyExtraUIds(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateTraceKeyExtraUIDData(job, Quantity, PackagingLevel, LineCode, IsTertiary, IsLoose, ProviderId);
        }

        private bool GenerateSSCCs(Job job, string IAC_CIN, string LoosExt, string SSCCExt, int CustId)
        {
            var JobTYpe = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
            var selectedJobType = JobTYpe.Job_Type;

            DemoSSCC demo = new DemoSSCC();
            int cnt = demo.getSSCCToPrint(job);
            var tertLvl = demo.getTertDeck();

            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID && x.PackageTypeCode == tertLvl).FirstOrDefault();


            M_Identities mids = new M_Identities();
            mids.CreatedOn = DateTime.Now;
            mids.CustomerId = (int)job.CustomerId;
            mids.GTIN = pacAssoDet.GTIN;
            mids.JID = job.JID;
            mids.PackageTypeCode = pacAssoDet.PackageTypeCode;
            mids.IsExtra = false;
            mids.IsTransfered = false;
            db.M_Identities.Add(mids);
            db.SaveChanges();


            IDGenrationFactory obj = new IDGenrationFactory();
            var UIDsSSCC = obj.generateSSCC((int)job.JID, cnt, selectedJobType, IAC_CIN, LoosExt, SSCCExt, CustId);
            Dictionary<string, string> masterList = new Dictionary<string, string>();
            if (UIDsSSCC.Count > 0)
            {
                Dictionary<string, string> ids = new Dictionary<string, string>();
                foreach (var item in UIDsSSCC)
                {
                    ids.Add(item, "SSC");
                }
                masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);


                IDDataGeneratorUtility util = new IDDataGeneratorUtility();
                var ConvertedData = util.converUIDData(masterList, mids.Id, tertLvl);

                DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(ConvertedData);

                BulkDataHelper bulkHlpr = new BulkDataHelper();
                return bulkHlpr.InsertUIDIdenties(DtconvertedData);
            }
            return false;

        }

    }
}