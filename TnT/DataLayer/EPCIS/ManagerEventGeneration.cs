using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.DataLayer.EPCISHelper;
using TnT.Models;
using REDTR.HELPER;
using System.Data;

namespace TnT.DataLayer.EPCIS
{
    public class ManagerEventGeneration
    {
        public string Manufacturer, DosageForm, Strength, ContainerSize, DrugName, Batch, MfgPackDate, ExpPackDate, Quantity;
        public List<string> ProductGTINs;
        public string SenderCompanyName, SenderStreet1, Senderstreet2, SenderstateOrRegion, Sendercity, SenderpostalCode, Sendercountry, SenderLicense, SenderLicenseState, SenderLicenseAgency, SenderReadPointGLN, SenderReadPointGLN_Ext, SenderBizLocGLN, SenderBizLocGLN_Ext;
        public string RecieverCompanyName, RecieverStreet1, Recieverstreet2, RecieverstateOrRegion, Recievercity, RecieverpostalCode, Recievercountry, RecieverLicense, RecieverLicenseState, RecieverLicenseAgency, RecieverReadPointGLN, RecieverReadPointGLN_Ext, RecieverBizLocGLN, RecieverBizLocGLN_Ext;
        EPCISConfig config = new EPCISConfig();
        //EventGeneration eg = new EventGeneration();

        ApplicationDbContext db = new ApplicationDbContext();

        private DbHelper dbhelper = new DbHelper();
        List<string> Codeslst = new List<string>();

        public void StandardBuisnessDocumentHeader(int ProductID, int JobId, int SenderId, int RecieverId, string PCkCodeType)
        {
            string PCkCode_Type = PCkCodeType;
            int paid = ProductID;
            DataTable dtProduct = new DataTable();
            DataTable dtProductDetails = new DataTable();
            DataTable dtJob = new DataTable();
            DataTable dtpackCodes = new DataTable();
            List<string> mylst = new List<string>();
            dtProduct = dbhelper.DBManager.PackagingAssoBLL.PackagingAssoDAO.GetProductMasterData(paid);

            DataRow dr = dtProduct.Rows[0];

            Manufacturer = dr["Manufacturer"].ToString();
            DosageForm = dr["DosageForm"].ToString();
            Strength = dr["Strength"].ToString();
            ContainerSize = dr["ContainerSize"].ToString();
            DrugName = dr["DrugName"].ToString();
            // dtProductDetails = GeneralDataHelper.convertToDataTable(db.PackagingAssoDetails.Where(x => x.PAID == ProductID && x.PackageTypeCode == PCkCodeType.ToString()).ToList());   //dbhelper.DBManager.PackagingAssoDetailsBLL.PackagingAssoDetailsDAO.GetProductGTIN(paid, PCkCodeType);
            //DataRow drGtin = dtProductDetails.Rows[0];
            //ProductGTIN = drGtin["GTIN"].ToString();
            int CompanyCodeLength = db.Settings.FirstOrDefault().CompanyCode.Length;
            EPCIS_XMLs_Generation.EPCISConfig epcConfig = new EPCIS_XMLs_Generation.EPCISConfig();
            ProductGTINs = new List<string>();
            foreach (var item in db.PackagingAssoDetails.Where(x => x.PAID == ProductID).Select(x => x.GTIN).ToList())
            {
                ProductGTINs.Add(epcConfig.GetEPCClassSGTIN(item, CompanyCodeLength));
            }
            //ProductGTINs = db.PackagingAssoDetails.Where(x => x.PAID == ProductID).Select(x => x.GTIN).ToList();
            int JOBid = JobId;
            dtJob = dbhelper.DBManager.JobBLL.JobDAO.GetJobMasterData(JOBid);
            DataRow drJob = dtJob.Rows[0];
            Batch = drJob["BatchNo"].ToString();
            MfgPackDate = drJob["MfgDate"].ToString();
            ExpPackDate = drJob["ExpDate"].ToString();
            Quantity = drJob["Quantity"].ToString();

            int senderID = SenderId;
            //dtSender = dbhelper.DBManager.SettingsBLL.SettingsDAO.GetSenderDetails(senderID);

            var sendr = db.Settings.Find(senderID);
            var jobData = db.Job.Where(x => x.JID == JobId).FirstOrDefault();
            try
            {
                var lineLocationData = db.LineLocation.Where(l => l.ID == jobData.LineCode).FirstOrDefault();
                if(lineLocationData==null)
                {
                    return;
                }

                //DataRow dr1 = dtSender.Rows[0];

                SenderCompanyName = sendr.CompanyName;
                SenderStreet1 = sendr.Street;
                Senderstreet2 = "";
                if (sendr.StateOrRegion != 0)
                {
                    SenderstateOrRegion = db.S_State.Find(sendr.StateOrRegion).StateName;
                }
                Sendercity = sendr.City;
                SenderpostalCode = sendr.PostalCode;
                Sendercountry = db.Country.Find(sendr.Country).CountryName; //dr1["CountryName"].ToString();
                SenderLicense = sendr.License;
                SenderLicenseState = sendr.LicenseState;
                SenderLicenseAgency = sendr.LicenseAgency;
                SenderReadPointGLN = lineLocationData.ReadGLN; //dr1["ReadPointGLN"].ToString();
                SenderReadPointGLN_Ext = lineLocationData.GLNExtension;   //dr1["ReadPointGLN_Ext"].ToString();
                SenderBizLocGLN = sendr.GLN;
                SenderBizLocGLN_Ext = "001";
                SenderReadPointGLN = config.GetEPCGLN(SenderReadPointGLN, SenderReadPointGLN_Ext);
                SenderBizLocGLN = config.GetEPCGLN(SenderBizLocGLN, SenderBizLocGLN_Ext);

                DataTable dtReciever = new DataTable();
                int recieverId = RecieverId;
                var cust = db.M_Customer.Where(cm => cm.Id == recieverId).FirstOrDefault();
                //dtReciever = GeneralDataHelper.convertToDataTable(cust); //dbhelper.DBManager.ClientDetailsBLL.ClientDetailsDAO.GetRecieverDetails(recieverId);
                //DataRow dr2 = dtReciever.Rows[0];
                RecieverCompanyName = cust.CompanyName;
                RecieverStreet1 = cust.street1;
                Recieverstreet2 = cust.street2;  //dr2["street2"].ToString();
                RecieverstateOrRegion = cust.M_State.StateName;
                Recievercity = cust.city;
                RecieverpostalCode = cust.postalCode;
                Recievercountry = cust.M_Country.CountryName;
                RecieverLicense = cust.License;
                RecieverLicenseState = cust.LicenseState;
                RecieverLicenseAgency = cust.LicenseAgency;
                RecieverBizLocGLN = cust.BizLocGLN;
                RecieverBizLocGLN_Ext = cust.BizLocGLN_Ext;
                // RecieverReadPointGLN = dr1["ReadPointGLN"].ToString();
                // RecieverReadPointGLN_Ext = dr1["ReadPointGLN_Ext"].ToString();
                RecieverReadPointGLN = RecieverBizLocGLN;
                RecieverReadPointGLN_Ext = RecieverBizLocGLN_Ext;
                RecieverReadPointGLN = config.GetEPCGLN(RecieverReadPointGLN, RecieverReadPointGLN_Ext);
                RecieverBizLocGLN = config.GetEPCGLN(RecieverBizLocGLN, RecieverBizLocGLN_Ext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}