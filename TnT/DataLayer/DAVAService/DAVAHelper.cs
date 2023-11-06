using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web.UI.WebControls;
using REDTR.CODEMGR;
using REDTR.HELPER;
using REDTR.DB.BusinessObjects;
using REDTR.DB.BLL;
using PTPLUidGen;
using PTPLCRYPTORENGINE;

namespace TnT.DataLayer.DAVAService
{
    public class DAVA
    {
        public enum TypeOfUpload
        {
            PRO,//ProductDetails = "PRO",
            DPM,//DistributionPointDetails = "DPM",
            BAT,//BatchDetails = "BAT",
            TSP,//ProductionDetails = "TSP",//(Tertiary, Secondary ,Primary  Production and packaging details data)
            TER,//Only Tertiary Pack details="TER"
            MOV,//MovementDetails = "MOV"
            TNS

        }

        /// <summary>
        /// Packages exempted from requirement of bar-coding
        ///on Primary and secondary pack and upload of
        ///Primary and secondary pack(s) information on
        ///central DAVA portal:
        ///Exempted Categories: 
        /// </summary>
        public enum ExemptionCode
        {
            E21,//Consignments containing drugs/devices for export purposes to countries eligible for exemption from bar-coding as per rule 21 of “Drugs and Cosmetic Act” withspecific notification number.
            EME,//Merchant exporter’s consignment containing drugs/ devices from multiple manufacturers
            ELL//Drugs/devices having bar coding as per importing countries barcoding requirements.
        }

        public string GetFileName(string Manufacturercode, TypeOfUpload typeofUpload)
        {
            try
            {
                int Runningseqno = SetNextRunningseqno();
                string FileCreationdate = DateTime.Now.ToString("ddMMyyyy"); //FileCreationdate="DDMMYYYY" Format
                string DAVAFileName = string.Empty;
                DAVAFileName = Manufacturercode.PadRight(10, '0') + typeofUpload.ToString() + FileCreationdate + Convert.ToString(Runningseqno).PadLeft(3, '0');
                //0890011700            TSP                   27082014                    001

                return DAVAFileName;
            }
            catch (Exception ex) { throw ex; }

        }

        private DavaRunningSeqNoMgr m_DavaRunningSeqNoMgr;
        private int m_RunningSeqNo = 0;

        public int SetNextRunningseqno()
        {
            m_DavaRunningSeqNoMgr = new DavaRunningSeqNoMgr();

            m_DavaRunningSeqNoMgr.SetNextRunnningSeqNo();
            m_RunningSeqNo = m_DavaRunningSeqNoMgr.GetNextRunnningSeqNo(SSCCMgr.SSCCIncreamentMode.PreFetch);

            return m_RunningSeqNo;
        }
        public static string ImageToBase64(string filename)
        {


            long originalSizeInBytes = 0;
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                originalSizeInBytes = fs.Read(buffer, 0, (int)fs.Length);
                var base64 = Convert.ToBase64String(buffer);
                long base64EncodedSize = 4 * originalSizeInBytes / 3;
                return base64.ToString();

            }

        }



    }

    class DAVAProductHelper
    {
        DbHelper m_dbhelper = new DbHelper();
        DAVA m_Dava = new DAVA();
        DAVAExportFileTagsInfo m_davaExportFileTagsInfo = new DAVAExportFileTagsInfo();
        string m_CompanyCode = string.Empty;
        string sFilePath = string.Empty;
        private double FileSizeInKB = 0.0;
        private double FileSizeInMB = 0.0;
        public bool ISXMLFileSplit = false;

        public DAVAStatus CreateProduct(DataTable dtPackagingAsso)
        {
            int iproduct = 0;
            string PAID = string.Empty;
            string ProductType = string.Empty;
            string ProductCode = string.Empty;
            string PRODUCT_NAME = string.Empty;
            string GenericName = string.Empty;
            string Composition = string.Empty;
            string ScheduledDrug = string.Empty;
            string DoseUsage = string.Empty;
            string REMARK = string.Empty;
            string PRODUCT_IMAGE = string.Empty;
            string PRODUCT_STATUS = string.Empty;
            string MaxDavaFileSize = Utilities.getAppSettings("DavaFileSizeInMB");

            DataTable dtPA = new DataTable("PackageAsso");
            dtPA = dtPackagingAsso; //m_dbhelper.DBManager.PackagingAssoBLL.PackagingAssoDAO.GetProductdataTableforXML((int)PackagingAssoBLL.PackagingAssoOp.GetProductforXML, null, null);
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.PRO);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.PRO.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.PRODUCT.ToString();
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            StreamWriter sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<PRODUCTS_LIST>");
            sw.WriteLine("<ENVELOPE>");
            sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
            sw.WriteLine("<MANUFACTURER_CODE>" + m_CompanyCode + "</MANUFACTURER_CODE>");

            foreach (DataRow dr in dtPA.Rows)
            {
                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.PRO);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.PRO.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<PRODUCTS_LIST>");
                    sw.WriteLine("<ENVELOPE>");
                    sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    sw.WriteLine("<MANUFACTURER_CODE>" + m_CompanyCode + "</MANUFACTURER_CODE>");
                    ISXMLFileSplit = false;
                }

                PAID = dr["PAID"].ToString();

                if (dr["PRODUCT_CODE"].ToString().Contains(m_CompanyCode))
                {
                    ProductType = "O";
                }
                else
                {
                    ProductType = "L";
                }

                ProductCode = dr["PRODUCT_CODE"].ToString();
                PRODUCT_NAME = dr["PRODUCT_NAME"].ToString();
                GenericName = dr["GENERIC_NAME"].ToString();
                Composition = dr["COMPOSITION"].ToString();
                ScheduledDrug = dr["SCHEDULED"].ToString();
                DoseUsage = dr["USAGE"].ToString();
                REMARK = dr["REMARK"].ToString();
                PRODUCT_IMAGE = dr["PRODUCT_IMAGE"].ToString();
                PRODUCT_STATUS = dr["PRODUCT_STATUS"].ToString();

                sw.WriteLine("<PRODUCT>");
                sw.WriteLine("<PRODUCT_TYPE>" + ProductType + "</PRODUCT_TYPE>");
                sw.WriteLine("<PRODUCT_CODE>" + ProductCode + "</PRODUCT_CODE>");
                sw.WriteLine("<PRODUCT_NAME>" + PRODUCT_NAME + "</PRODUCT_NAME>");
                sw.WriteLine("<GENERIC_NAME>" + GenericName + "</GENERIC_NAME>");
                sw.WriteLine("<COMPOSITION>" + Composition + "</COMPOSITION>");
                sw.WriteLine("<SCHEDULED>" + ScheduledDrug + "</SCHEDULED>");
                sw.WriteLine("<USAGE>" + DoseUsage + "</USAGE>");
                if (string.IsNullOrEmpty(REMARK))
                {
                    sw.WriteLine("<REMARK/>");
                }
                else
                {
                    sw.WriteLine("<REMARK>" + REMARK + "</REMARK>");
                }
                if (string.IsNullOrEmpty(PRODUCT_IMAGE))
                {
                    sw.WriteLine("<PRODUCT_IMAGE/>");
                }
                else
                {
                    sw.WriteLine("<PRODUCT_IMAGE>" + PRODUCT_IMAGE + "</PRODUCT_IMAGE>");
                }
                sw.WriteLine("<PRODUCT_STATUS>" + PRODUCT_STATUS + "</PRODUCT_STATUS>");  /// Added By Amol 16/06/2016
                sw.WriteLine("</PRODUCT>");

                PackagingAsso PA = new PackagingAsso();
                PA.PAID = int.Parse(PAID);
                PA.DAVAPortalUpload = true;
                m_dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.UpdatePackagingAssoforDavaExport, PA);
                iproduct++;


                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.8 && FileSizeInMB <= Convert.ToDouble(MaxDavaFileSize))
                {
                    sw.WriteLine("</ENVELOPE>");
                    sw.WriteLine("</ProductionInfo>");
                    sw.Close();
                    ISXMLFileSplit = true;
                }
            }
            if (FileSizeInMB < 1.8)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</PRODUCTS_LIST>");
                sw.Close();
            }


            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();
            int id = AddDavaFileTagsInfo();
            return DAVAStatus.Success;
        }

        private int AddDavaFileTagsInfo()
        {
            m_davaExportFileTagsInfo.CreatedDate = DateTime.Parse(DateTime.Now.ToString());
            return m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.AddDAVAExportFileTagsInfo(m_davaExportFileTagsInfo);
        }
    }


    class DAVABatchHelper
    {
        DbHelper m_dbhelper = new DbHelper();
        DAVA m_Dava = new DAVA();
        DAVAExportFileTagsInfo m_davaExportFileTagsInfo = new DAVAExportFileTagsInfo();
        string sFilePath = string.Empty;
        string m_CompanyCode = string.Empty;
        private double FileSizeInKB = 0.0;
        private double FileSizeInMB = 0.0;
        public bool ISXMLFileSplit = false;

        public DAVAStatus createBatchDAVA(DataTable dtJob, bool IsAllBatch, bool IsExemptBarcode, string ExpemptCountry, string ExpemptcountryCode,string EXEMPTION_NOTIFICATION_AND_DATE)
        {

            string NUMBER = string.Empty;
            string PRODUCT_CODE = string.Empty;
            string BATCH_SIZE = string.Empty;
            string EXPIRY_DATE = string.Empty;
            string UNIT_PRICE = string.Empty;
            string BATCH_FOR_EXPORT = string.Empty;
            string BATCH_STATUS = string.Empty;
            string EXEMPTED_FROM_BARCODING = string.Empty;
            //string EXEMPTION_NOTIFICATION_AND_DATE = string.Empty;
            string EXEMPTED_COUNTRY_CODE = string.Empty;
            string MaxDavaFileSize = Utilities.getAppSettings("DavaFileSizeInMB");

            DataTable ds = new DataTable();
            ds = dtJob;
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.BAT);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.BAT.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.BATCH.ToString();
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            StreamWriter sws; //new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
            sws = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
            sws.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sws.WriteLine("<BATCHINFO>");
            sws.WriteLine("<ENVELOPE>");
            sws.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
            sws.WriteLine("<MANUFACTURER_CODE>" + m_CompanyCode + "</MANUFACTURER_CODE>");

            int ibatch = 0;
            foreach (DataRow dr in ds.Rows)
            {

                NUMBER = dr["NUMBER"].ToString();

                //if (NUMBER.Length < 3)
                //{
                //    continue;
                //}

                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.BAT);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.BAT.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sws = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sws.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sws.WriteLine("<BATCHINFO>");
                    sws.WriteLine("<ENVELOPE>");
                    sws.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    sws.WriteLine("<MANUFACTURER_CODE>" + m_CompanyCode + "</MANUFACTURER_CODE>");
                    ISXMLFileSplit = false;
                }

                PRODUCT_CODE = dr["PRODUCT_CODE"].ToString();
                BATCH_SIZE = dr["BATCH_SIZE"].ToString();
                EXPIRY_DATE = dr["EXPIRY_DATE"].ToString();
                UNIT_PRICE = dr["UNIT_PRICE"].ToString();
                if (string.IsNullOrEmpty(UNIT_PRICE))
                { UNIT_PRICE = "0.00"; }
                BATCH_FOR_EXPORT = dr["BATCH_FOR_EXPORT"].ToString();
                BATCH_STATUS = dr["BATCH_STATUS"].ToString();
                if (IsAllBatch)
                {
                    EXEMPTED_FROM_BARCODING = "N";
                    EXEMPTION_NOTIFICATION_AND_DATE = string.Empty;
                    EXEMPTED_COUNTRY_CODE = string.Empty;
                }
                else
                {
                    if (IsExemptBarcode)
                    {
                        EXEMPTED_FROM_BARCODING = "Y";
                       // EXEMPTION_NOTIFICATION_AND_DATE = "EXEMPT for " + ExpemptCountry + DateTime.Now.ToString();
                      
                        EXEMPTED_COUNTRY_CODE = ExpemptcountryCode;
                    }
                    else
                    {
                        EXEMPTED_FROM_BARCODING = "N";
                        EXEMPTION_NOTIFICATION_AND_DATE = string.Empty;
                        EXEMPTED_COUNTRY_CODE = string.Empty;
                    }
                }
                sws.WriteLine("<BATCH>");
                sws.WriteLine(" <NUMBER>" + NUMBER + "</NUMBER>");
                sws.WriteLine("<PRODUCT_CODE>" + PRODUCT_CODE + "</PRODUCT_CODE>");
                sws.WriteLine("<BATCH_SIZE>" + BATCH_SIZE + "</BATCH_SIZE>");
                sws.WriteLine("<EXPIRY_DATE>" + EXPIRY_DATE + "</EXPIRY_DATE>");
                sws.WriteLine(" <UNIT_PRICE>" + UNIT_PRICE + "</UNIT_PRICE>");
                sws.WriteLine("<BATCH_FOR_EXPORT>" + BATCH_FOR_EXPORT + "</BATCH_FOR_EXPORT>");
                sws.WriteLine("<EXEMPTED_FROM_BARCODING>" + EXEMPTED_FROM_BARCODING + "</EXEMPTED_FROM_BARCODING>");
                sws.WriteLine(" <EXEMPTION_NOTIFICATION_AND_DATE>" + EXEMPTION_NOTIFICATION_AND_DATE + "</EXEMPTION_NOTIFICATION_AND_DATE>");
                sws.WriteLine("<EXEMPTED_COUNTRY_CODE>" + EXEMPTED_COUNTRY_CODE + "</EXEMPTED_COUNTRY_CODE>");
                sws.WriteLine("<BATCH_STATUS>" + BATCH_STATUS + "</BATCH_STATUS>");
                sws.WriteLine("</BATCH>");

                Job o_job = new Job();
                o_job.BatchNo = NUMBER;
                o_job.DAVAPortalUpload = true;
                o_job.JID = Convert.ToInt32(dr["jid"]);
                m_dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateDavaPortalJob, o_job);
                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.8 && FileSizeInMB <= Convert.ToDouble(MaxDavaFileSize))
                {
                    sws.WriteLine("</ENVELOPE>");
                    sws.WriteLine("</BATCHINFO>");
                    sws.Close();
                    ISXMLFileSplit = true;
                }
                ibatch++;
                //ToolStripPrograss(ibatch);

            }

            if (FileSizeInMB < 1.8)
            {
                sws.WriteLine("</ENVELOPE>");
                sws.WriteLine("</BATCHINFO>");
                sws.Close();
            }
            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();
            int id = AddDavaFileTagsInfo();
            //msg = " BATCH DETAILS " + FILENAME + ".xml";
            //m_dbhelper.AddUserTrail(CurrentUser.ID, null, REDTR.UTILS.USerTrailWHERE.TnT, REDTR.UTILS.USerTrailWHAT.DavaFileRegeneration, msg, null);
            //XMLWebBrowser.Navigate(sFilePath + "\\" + FILENAME + ".xml");
            //toolStripStatusLabel1.Text = "FILE GENERATED AT:";
            //toolStripStatusLabel1.Text += sFilePath + "\\" + FILENAME + ".xml";

            //if (IsAllBatch)
            //{
            //    //MessageBoxEx.Show("FILE GENERATED FOR DAVA SUCCESSFULLY!", "INFORMATION", MessageBoxEx.MessageBoxButtonsEx.OK, 0);
            //    return;
            //}
            return DAVAStatus.Success;
            //if ((!IsAllBatch) && (rbProduction.Checked || rbTerExempted.Checked))
            //{
            //    if (MessageBoxEx.Show("BATCH XML GENERATED SUCCESSFULLY! \r\r DO YOU WANT TO GENERATE PRODUCTION DATA XML", "INFORMATION", MessageBoxEx.MessageBoxButtonsEx.YesNo, 0) == DialogResult.Yes)
            //    {
            //        panelButtton.Enabled = true;
            //        progressBar1.Value = 0;
            //        progressBar1.Visible = false;
            //        if (lblJobType.Text == "DGFT")
            //        {
            //            Xmlfiletype = XMLFILEType.PRODUCTION;
            //        }
            //        else
            //        {
            //            Xmlfiletype = XMLFILEType.TEREXEMPTED;
            //        }
            //    }
            //}
            //if (progressBar1.Value == ds.Rows.Count)
            //{
            //    panelButtton.Enabled = false;
            //    this.Cursor = Cursors.Default;
            //    progressBar1.Visible = false;
            //    toolStripStatusLabel1.Text = "XML FILE IS GENERATED SUCCESSFULLY";
            //}
        }

        private int AddDavaFileTagsInfo()
        {
            m_davaExportFileTagsInfo.CreatedDate = DateTime.Parse(DateTime.Now.ToString());
            return m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.AddDAVAExportFileTagsInfo(m_davaExportFileTagsInfo);
        }

    }


    class DAVAProductionHelper
    {

        DbHelper m_dbhelper = new DbHelper();
        DAVA m_Dava = new DAVA();
        List<PackagingDetails> LstBatchData = new List<PackagingDetails>();
        DAVAExportFileTagsInfo m_davaExportFileTagsInfo = new DAVAExportFileTagsInfo();
        string m_CompanyCode = string.Empty;
        string sFilePath = string.Empty;
        private double FileSizeInKB = 0.0;
        private double FileSizeInMB = 0.0;
        public bool ISXMLFileSplit = false;
        int m_jobId = 0;



        public DAVAStatus CreateProductionDAVA(string BatchNo, List<Job> LstJb, List<PackagingDetails> SetWholeSSCC, bool IsWholeBatch, bool IsExemptedBarcode, string ExemtedBarcode)
        {
            if (LstJb.Count > 0)
            {
                foreach (Job jb in LstJb)
                {
                    m_jobId = (int)jb.JID;
                }
            }
            UIDGen mPTPLCodes = new UIDGen();
            //mPTPLCodes.InitUIDGen((int)DECKs.MOC, getFileLocation());
            List<JobDetails> LstJobDetails = m_dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, m_jobId, 1);
            string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL " +
                                  " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0) AND JOBID =" + m_jobId;
            LstBatchData = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);

            //string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND PACKAGETYPECODE='ISH'  AND ISREJECTED IS NOT NULL " +
            //                " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0) AND JOBID =" + JobId;
            //m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);
            string ProductCode = string.Empty;
            string BatchNoXMl = BatchNo;
            List<PackagingDetails> Terpckdetails = new List<PackagingDetails>();
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TSP);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TSP.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.PRODUCTION.ToString();
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.PRODUCTION.ToString() + "\\" + BatchNoXMl;
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            StreamWriter sw;
            sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");

            //sw = new StreamWriter(Globals.DAVAFilePath + "\\" + FILENAME + ".xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<ProductionInfo>");
            sw.WriteLine("<ENVELOPE>");
            sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");


            if (IsWholeBatch)
            {
                Terpckdetails = SetWholeSSCC;

                if (Terpckdetails.Count == 0 || Terpckdetails == null)
                {
                    //MessageBoxEx.Show("NO PRODUCTION DATA AVAILABLE FOR EXPORT FOR THE SELECTED BATCH!!!", "INFORMATION", MessageBoxEx.MessageBoxButtonsEx.OK, 0);
                    return DAVAStatus.NoProductionDataAvailable;
                }
            }
            else
            {
                Terpckdetails = SetWholeSSCC;
                if (!IsWholeBatch && SetWholeSSCC.Count == 0)
                {
                    //MessageBoxEx.Show("PLEASE SCAN ATLEAST 1 SSCC CODE TO GENERATE XML FILE...", "INFORMATION", MessageBoxEx.MessageBoxButtonsEx.OK, 0);
                    return DAVAStatus.NoSSCCsAvailable;
                }
                if (Terpckdetails.Count == 0)
                {
                    return DAVAStatus.NoProductionDataAvailable;
                }
            }

            int i = 0;
            //panelButtton.Enabled = false;
            //progressBar1.Maximum = Terpckdetails.Count;
            //progressBar1.Visible = true;
            //toolStripStatusLabel1.Text = "FILE IS GENERATING.PLEASE WAIT...!!!";
            //Application.DoEvents();
            foreach (PackagingDetails pc in Terpckdetails)
            {
                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TSP);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TSP.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<ProductionInfo>");
                    sw.WriteLine("<ENVELOPE>");
                    sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    ISXMLFileSplit = false;
                }
                TreeNode NextLevelCode = new TreeNode();
                NextLevelCode.Text = pc.SSCC;
                int packagingLevelCount = LstJobDetails.Count;  //m_dbhelper.DBManager.JobBLL.JobDAO.GetNoOfDecks(pc.SSCC);
                if (LstJobDetails.Count > 0)
                    ProductCode = LstJobDetails[0].JD_ProdCode;
                BatchNoXMl = BatchNo;
                if (packagingLevelCount == 2)
                {
                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> SecpckDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code);   //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int subItemCount = SecpckDetails.Count;
                    sw.WriteLine("<SubItemCnt>" + subItemCount + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    //DataTable dt = new DataTable();
                    //dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatch_ProductCode((int)JobBLL.JobOp.GetProductBatch,cmbBatchNo.SelectedValue.ToString() ,null, null);

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ProductCode = dr["ProductCode"].ToString();
                    //    BatchNoXMl = dr["BatchNo"].ToString();
                    //}
                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    int PrimaryCount = 0;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;
                    foreach (PackagingDetails sc in SecpckDetails)
                    {
                        TreeNode TreeSecpckDetails = new TreeNode();
                        TreeSecpckDetails.Text = sc.Code;
                        NextLevelCode.ChildNodes.Add(TreeSecpckDetails);
                        sw.WriteLine("<Secondary>");
                        sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                        sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                        // List<PrimaryPackDummys> PrimarypckDetails = m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVAPriamryPacks(PackagingDetailsBLL.OP.GetPrimaryCodesFromSecSrNo, pc.JobID, sc.Code);
                        //int NoOfPrimaries = PrimarypckDetails.Count();
                        sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                        //mRedCodes.GenerateUID(12);
                        string uid = string.Empty;
                        for (int ix = 0; ix < PrimaryCount; ix++)
                        {
                            TreeNode TreePripckDetails = new TreeNode();
                            uid = mPTPLCodes.GenerateUID(12);
                            TreePripckDetails.Text = uid;
                            TreeSecpckDetails.ChildNodes.Add(TreePripckDetails);
                            sw.WriteLine("<PriSrNo>" + uid + "</PriSrNo>");
                        }
                        //foreach (PrimaryPackDummys ppb in PrimarypckDetails)
                        //{
                        //    TreeNode TreePripckDetails = new TreeNode();
                        //    TreePripckDetails.Text = ppb.Code;
                        //    TreeSecpckDetails.Nodes.Add(TreePripckDetails);

                        //    sw.WriteLine("<PriSrNo>" + ppb.Code + "</PriSrNo>");
                        //}
                        sw.WriteLine("</Secondary>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");
                    // ProductionTree.Nodes.Add(NextLevelCode);
                }

                if (packagingLevelCount == 3)
                {
                    //foreach (PackagingDetails pc in Terpckdetails)
                    //{
                    //    TreeNode TerNode = new TreeNode();
                    //    TerNode.Text = pc.SSCC;
                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> OuterPackDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code);  // m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int SubItemCnt = OuterPackDetails.Count;
                    sw.WriteLine("<SubItemCnt>" + SubItemCnt + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    int PrimaryCount = 0;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;
                    //DataTable dt = new DataTable();
                    //dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatch_ProductCode((int)JobBLL.JobOp.GetProductBatch, m_jobId.ToString(), null, null);

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ProductCode = dr["ProductCode"].ToString();
                    //    BatchNoXMl = dr["BatchNo"].ToString();
                    //}
                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    foreach (PackagingDetails OuterPackDetailss in OuterPackDetails)
                    {
                        TreeNode OuterNode = new TreeNode();
                        OuterNode.Text = OuterPackDetailss.Code;
                        NextLevelCode.ChildNodes.Add(OuterNode);
                        sw.WriteLine("<IntermediateLevel2>");
                        sw.WriteLine("<Level2SrNo>" + OuterPackDetailss.Code + "</Level2SrNo>");
                        List<PackagingDetails> SecpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == OuterPackDetailss.Code);   //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, OuterPackDetailss.Code);
                        int SecondaryCnt = SecpackDetails.Count;
                        sw.WriteLine("<SecondaryCnt>" + SecondaryCnt + "</SecondaryCnt>");
                        foreach (PackagingDetails sc in SecpackDetails)
                        {
                            TreeNode SecpckDetails = new TreeNode();
                            SecpckDetails.Text = sc.Code;
                            OuterNode.ChildNodes.Add(SecpckDetails);
                            sw.WriteLine("<Secondary>");
                            sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                            sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                            //List<PrimaryPackDummys> PrimarypckDetails =  //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVAPriamryPacks(PackagingDetailsBLL.OP.GetPrimaryCodesFromSecSrNo, pc.JobID, sc.Code);
                            //int NoOfPrimaries = PrimarypckDetails.Count();
                            sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                            //string uid = string.Empty;
                            //for (int ix = 0; ix < PrimaryCount; ix++)
                            //{
                            //    TreeNode PripckDetails = new TreeNode();
                            //    uid = mPTPLCodes.GenerateUID(12);
                            //    PripckDetails.Text = uid;
                            //    SecpckDetails.ChildNodes.Add(PripckDetails);
                            //    sw.WriteLine("<PriSrNo>" + uid + "</PriSrNo>");
                            //}
                            sw.WriteLine("</Secondary>");
                        }
                        sw.WriteLine("</IntermediateLevel2>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");
                    // ProductionTree.Nodes.Add(NextLevelCode);
                }

                if (packagingLevelCount == 4)
                {
                    //foreach (PackagingDetails pc in Terpckdetails)
                    //{
                    //    TreeNode TerNode = new TreeNode();
                    //    TerNode.Text = pc.SSCC;
                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> IshpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int SubItemCnt = IshpackDetails.Count();
                    sw.WriteLine("<SubItemCnt>" + SubItemCnt + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    //DataTable dt = new DataTable();
                    //dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatch_ProductCode((int)JobBLL.JobOp.GetProductBatch, m_jobId.ToString(), null, null);

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ProductCode = dr["ProductCode"].ToString();
                    //    BatchNoXMl = dr["BatchNo"].ToString();
                    //}
                    int PrimaryCount = 1;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;
                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    foreach (PackagingDetails IshpackDetailss in IshpackDetails)
                    {
                        TreeNode ISHNode = new TreeNode();
                        ISHNode.Text = IshpackDetailss.Code;
                        NextLevelCode.ChildNodes.Add(ISHNode);
                        sw.WriteLine("<IntermediateLevel3>");
                        sw.WriteLine("<Level3SrNo>" + IshpackDetailss.Code + "</Level3SrNo>");
                        List<PackagingDetails> outerPackDetialss = LstBatchData.FindAll(item => item.NextLevelCode == IshpackDetailss.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, IshpackDetailss.Code);
                        int L2Cnt = outerPackDetialss.Count;
                        sw.WriteLine("<L2Cnt>" + L2Cnt + "</L2Cnt>");
                        foreach (PackagingDetails outerPackDetials in outerPackDetialss)
                        {
                            TreeNode OuterNode = new TreeNode();
                            OuterNode.Text = outerPackDetials.Code;
                            ISHNode.ChildNodes.Add(OuterNode);
                            sw.WriteLine("<IntermediateLevel2>");
                            sw.WriteLine("<Level2SrNo>" + outerPackDetials.Code + "</Level2SrNo>");
                            List<PackagingDetails> SecpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == outerPackDetials.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, outerPackDetials.Code);
                            int SecondaryCnt = SecpackDetails.Count;
                            sw.WriteLine("<SecondaryCnt>" + SecondaryCnt + "</SecondaryCnt>");
                            foreach (PackagingDetails sc in SecpackDetails)
                            {
                                TreeNode SecpckDetails = new TreeNode();
                                SecpckDetails.Text = sc.Code;
                                OuterNode.ChildNodes.Add(SecpckDetails);
                                sw.WriteLine("<Secondary>");
                                sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                                sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                                //List<PrimaryPackDummys> PrimarypckDetails = m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVAPriamryPacks(PackagingDetailsBLL.OP.GetPrimaryCodesFromSecSrNo, pc.JobID, sc.Code);
                                //int NoOfPrimaries = PrimarypckDetails.Count();
                                sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                                string uid = string.Empty;
                                for (int ix = 0; ix < PrimaryCount; ix++)
                                {
                                    TreeNode PripckDetails = new TreeNode();
                                    uid = mPTPLCodes.GenerateUID(12);
                                    PripckDetails.Text = uid;
                                    SecpckDetails.ChildNodes.Add(PripckDetails);
                                    sw.WriteLine("<PriSrNo>" + uid + "</PriSrNo>");
                                }
                                //foreach (PrimaryPackDummys ppb in PrimarypckDetails)
                                //{
                                //    TreeNode PripckDetails = new TreeNode();
                                //    PripckDetails.Text = ppb.Code;
                                //    SecpckDetails.Nodes.Add(PripckDetails);
                                //    sw.WriteLine("<PriSrNo>" + ppb.Code + "</PriSrNo>");
                                //}
                                sw.WriteLine("</Secondary>");
                            }
                            sw.WriteLine("</IntermediateLevel2>");
                        }
                        sw.WriteLine("</IntermediateLevel3>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");
                    // ProductionTree.Nodes.Add(NextLevelCode);
                }
                i++;
                //ToolStripPrograss(i);

                //UpdateStatus(i);
                GC.Collect();
                //Here update davaportal UID flag comes...
                //string reason = "Exported to DAVA Portal";
                //UpdateExportedParentUIDs(pc.PackDtlsID,pc.Code,reason ,pc.PackageTypeCode);
                UpdateDAVAStatus(pc.PackageTypeCode, pc.Code, pc.JobID);

                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.2 && FileSizeInMB >= 4.0)
                {
                    sw.WriteLine("</ENVELOPE>");
                    sw.WriteLine("</ProductionInfo>");
                    sw.Close();
                    ISXMLFileSplit = true;
                    //msg = " PRODUCTION " + FILENAME + ".xml";
                    //m_dbhelper.AddUserTrail(CurrentUser.ID, null, REDTR.UTILS.USerTrailWHERE.TnT, REDTR.UTILS.USerTrailWHAT.DavaFileRegeneration, msg, null);
                }
            }
            if (FileSizeInMB < 1.2)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }
            if (FileSizeInMB > 1.2 && FileSizeInMB <= 4.0)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }
            //if (progressBar1.Value == Terpckdetails.Count)
            //{
            //    this.Cursor = Cursors.Default;
            //    panelButtton.Enabled = true;
            //    progressBar1.Visible = false;
            //    btnSSCCcounter.BaseColor = Color.Silver;
            //   // toolStripStatusLabel1.Text = "Xml File is generated Successfully";
            //    MessageBoxEx.Show("File generated for Dava Successfully!", "Information", MessageBoxEx.MessageBoxButtonsEx.OK, 0);
            //}

            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();

            int id2 = AddDavaFileTagsInfo();
            AddBatchForDavaExport(id2, BatchNo, IsExemptedBarcode, m_jobId, ExemtedBarcode);

            //XMLWebBrowser.Navigate(sFilePath + "\\" + FILENAME + ".xml");
            //toolStripStatusLabel1.Text = "File Generated at:";
            //toolStripStatusLabel1.Text += sFilePath + "\\" + FILENAME + ".xml";
            //if (progressBar1.Value == Terpckdetails.Count)
            //{
            //    this.Cursor = Cursors.Default;
            //    panelButtton.Enabled = true;
            //    progressBar1.Visible = false;
            //    btnSSCCcounter.BaseColor = Color.Silver;
            //    MessageBoxEx.Show("FILE GENERATED FOR DAVA SUCCESSFULLY!", "INFORMATION", MessageBoxEx.MessageBoxButtonsEx.OK, 0);
            //    lblActualBatchQuantity.Text = "00";
            //    lblBatchQuantity.Text = "00";
            //    lblExportedBatchquantity.Text = "00";
            //    lblRemainingbatchQuantity.Text = "00";
            //    cmbBatchNo.SelectedIndex = -1; lblJobType.Text = "";
            //}
            SetWholeSSCC.Clear();
            return DAVAStatus.Success;
            //progressBar1.Value = 0;
            //panelButtton.Enabled = true;
            //dataGridView1.Rows.Clear();
            //progressBar1.Visible = false;
        }



        



        public DAVAStatus CreateProductionDAVATNS(string BatchNo, List<Job> LstJb, List<PackagingDetails> SetWholeSSCC, bool IsWholeBatch, bool IsExemptedBarcode, string ExemtedBarcode)
        {
            if (LstJb.Count > 0)
            {
                foreach (Job jb in LstJb)
                {
                    m_jobId = (int)jb.JID;
                }
            }
            UIDGen mPTPLCodes = new UIDGen();

            List<JobDetails> LstJobDetails = m_dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, m_jobId, 1);
            string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL " +
                                  " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0) AND JOBID =" + m_jobId;
            LstBatchData = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);


            string ProductCode = string.Empty;
            string BatchNoXMl = BatchNo;
            List<PackagingDetails> Terpckdetails = new List<PackagingDetails>();
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TNS);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TNS.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.TNS.ToString();
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.TNS.ToString() + "\\" + BatchNoXMl;
            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }
            StreamWriter sw;
            sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");

            //sw = new StreamWriter(Globals.DAVAFilePath + "\\" + FILENAME + ".xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<ProductionInfo>");
            sw.WriteLine("<ENVELOPE>");
            sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");


            if (IsWholeBatch)
            {
                Terpckdetails = SetWholeSSCC;

                if (Terpckdetails.Count == 0 || Terpckdetails == null)
                {
                    return DAVAStatus.NoProductionDataAvailable;
                }
            }
            else
            {
                Terpckdetails = SetWholeSSCC;
                if (!IsWholeBatch && SetWholeSSCC.Count == 0)
                {
                    return DAVAStatus.NoSSCCsAvailable;
                }
                if (Terpckdetails.Count == 0)
                {
                    return DAVAStatus.NoProductionDataAvailable;
                }
            }

            int i = 0;

            foreach (PackagingDetails pc in Terpckdetails)
            {
                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TNS);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TNS.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<ProductionInfo>");
                    sw.WriteLine("<ENVELOPE>");
                    sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    ISXMLFileSplit = false;
                }
                TreeNode NextLevelCode = new TreeNode();
                NextLevelCode.Text = pc.SSCC;
                int packagingLevelCount = LstJobDetails.Count;  //m_dbhelper.DBManager.JobBLL.JobDAO.GetNoOfDecks(pc.SSCC);
                if (LstJobDetails.Count > 0)
                    ProductCode = LstJobDetails[0].JD_ProdCode;
                BatchNoXMl = BatchNo;
                if (packagingLevelCount == 2)
                {
                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> SecpckDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code);   //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int subItemCount = SecpckDetails.Count;
                    sw.WriteLine("<SubItemCnt>" + subItemCount + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    int PrimaryCount = 0;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;
                    foreach (PackagingDetails sc in SecpckDetails)
                    {
                        TreeNode TreeSecpckDetails = new TreeNode();
                        TreeSecpckDetails.Text = sc.Code;
                        NextLevelCode.ChildNodes.Add(TreeSecpckDetails);
                        sw.WriteLine("<Secondary>");
                        sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                        sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                        sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                        sw.WriteLine("</Secondary>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");
                }

                if (packagingLevelCount == 3)
                {

                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> OuterPackDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code);  // m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int SubItemCnt = OuterPackDetails.Count;
                    sw.WriteLine("<SubItemCnt>" + SubItemCnt + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    int PrimaryCount = 0;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;

                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    foreach (PackagingDetails OuterPackDetailss in OuterPackDetails)
                    {
                        TreeNode OuterNode = new TreeNode();
                        OuterNode.Text = OuterPackDetailss.Code;
                        NextLevelCode.ChildNodes.Add(OuterNode);
                        sw.WriteLine("<IntermediateLevel2>");
                        sw.WriteLine("<Level2SrNo>" + OuterPackDetailss.Code + "</Level2SrNo>");
                        List<PackagingDetails> SecpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == OuterPackDetailss.Code);   //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, OuterPackDetailss.Code);
                        int SecondaryCnt = SecpackDetails.Count;
                        sw.WriteLine("<SecondaryCnt>" + SecondaryCnt + "</SecondaryCnt>");
                        foreach (PackagingDetails sc in SecpackDetails)
                        {
                            TreeNode SecpckDetails = new TreeNode();
                            SecpckDetails.Text = sc.Code;
                            OuterNode.ChildNodes.Add(SecpckDetails);
                            sw.WriteLine("<Secondary>");
                            sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                            sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                            sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                            sw.WriteLine("</Secondary>");
                        }
                        sw.WriteLine("</IntermediateLevel2>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");

                }

                if (packagingLevelCount == 4)
                {
                    //foreach (PackagingDetails pc in Terpckdetails)
                    //{
                    //    TreeNode TerNode = new TreeNode();
                    //    TerNode.Text = pc.SSCC;
                    sw.WriteLine("<Tertiary>");
                    sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                    List<PackagingDetails> IshpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == pc.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                    int SubItemCnt = IshpackDetails.Count();
                    sw.WriteLine("<SubItemCnt>" + SubItemCnt + "</SubItemCnt>");
                    sw.WriteLine("<Product>");
                    //DataTable dt = new DataTable();
                    //dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatch_ProductCode((int)JobBLL.JobOp.GetProductBatch, m_jobId.ToString(), null, null);

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ProductCode = dr["ProductCode"].ToString();
                    //    BatchNoXMl = dr["BatchNo"].ToString();
                    //}
                    int PrimaryCount = 1;
                    if (LstJb.Count > 0)
                        PrimaryCount = LstJb[0].PrimaryPCMapCount;
                    sw.WriteLine("<ProductCode>" + ProductCode + "</ProductCode>");

                    foreach (PackagingDetails IshpackDetailss in IshpackDetails)
                    {
                        TreeNode ISHNode = new TreeNode();
                        ISHNode.Text = IshpackDetailss.Code;
                        NextLevelCode.ChildNodes.Add(ISHNode);
                        sw.WriteLine("<IntermediateLevel3>");
                        sw.WriteLine("<Level3SrNo>" + IshpackDetailss.Code + "</Level3SrNo>");
                        List<PackagingDetails> outerPackDetialss = LstBatchData.FindAll(item => item.NextLevelCode == IshpackDetailss.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, IshpackDetailss.Code);
                        int L2Cnt = outerPackDetialss.Count;
                        sw.WriteLine("<L2Cnt>" + L2Cnt + "</L2Cnt>");
                        foreach (PackagingDetails outerPackDetials in outerPackDetialss)
                        {
                            TreeNode OuterNode = new TreeNode();
                            OuterNode.Text = outerPackDetials.Code;
                            ISHNode.ChildNodes.Add(OuterNode);
                            sw.WriteLine("<IntermediateLevel2>");
                            sw.WriteLine("<Level2SrNo>" + outerPackDetials.Code + "</Level2SrNo>");
                            List<PackagingDetails> SecpackDetails = LstBatchData.FindAll(item => item.NextLevelCode == outerPackDetials.Code); //m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, outerPackDetials.Code);
                            int SecondaryCnt = SecpackDetails.Count;
                            sw.WriteLine("<SecondaryCnt>" + SecondaryCnt + "</SecondaryCnt>");
                            foreach (PackagingDetails sc in SecpackDetails)
                            {
                                TreeNode SecpckDetails = new TreeNode();
                                SecpckDetails.Text = sc.Code;
                                OuterNode.ChildNodes.Add(SecpckDetails);
                                sw.WriteLine("<Secondary>");
                                sw.WriteLine("<SecSrNo>" + sc.Code + "</SecSrNo>");
                                sw.WriteLine("<BatchNo>" + BatchNoXMl + "</BatchNo>");
                                //List<PrimaryPackDummys> PrimarypckDetails = m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVAPriamryPacks(PackagingDetailsBLL.OP.GetPrimaryCodesFromSecSrNo, pc.JobID, sc.Code);
                                //int NoOfPrimaries = PrimarypckDetails.Count();
                                sw.WriteLine("<NoOfPrimaries>" + PrimaryCount + "</NoOfPrimaries>");
                                
                                sw.WriteLine("</Secondary>");
                            }
                            sw.WriteLine("</IntermediateLevel2>");
                        }
                        sw.WriteLine("</IntermediateLevel3>");
                    }
                    sw.WriteLine("</Product>");
                    sw.WriteLine("</Tertiary>");
                    // ProductionTree.Nodes.Add(NextLevelCode);
                }
                i++;

                GC.Collect();

                UpdateDAVAStatus(pc.PackageTypeCode, pc.Code, pc.JobID);

                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.2 && FileSizeInMB >= 4.0)
                {
                    sw.WriteLine("</ENVELOPE>");
                    sw.WriteLine("</ProductionInfo>");
                    sw.Close();
                    ISXMLFileSplit = true;

                }
            }
            if (FileSizeInMB < 1.2)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }
            if (FileSizeInMB > 1.2 && FileSizeInMB <= 4.0)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }


            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();

            int id2 = AddDavaFileTagsInfo();
            AddBatchForDavaExport(id2, BatchNo, IsExemptedBarcode, m_jobId, ExemtedBarcode);


            SetWholeSSCC.Clear();
            return DAVAStatus.Success;

        }


     

        private int AddDavaFileTagsInfo()//string FileName,string FileType)
        {
            //m_davaExportFileTagsInfo.FILENAME = FileName;
            //m_davaExportFileTagsInfo.TypeofUpload = FileType;
            m_davaExportFileTagsInfo.CreatedDate = DateTime.Parse(DateTime.Now.ToString());
            return m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.AddDAVAExportFileTagsInfo(m_davaExportFileTagsInfo);

        }
        private void UpdateDAVAStatus(string deck, string code, decimal JobId)
        {
            try
            {
                //code = AESCryptor.Encrypt(code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);

                string Query = "update PackagingDetails Set DavaPortalUpload =1 where JobId=" + JobId + " AND code ='" + code + "'";
                m_dbhelper.ExecuteQuery(Query);
            }
            catch (Exception ex)
            {
                //Trace.TraceError("Error At Updating DAVA status :" + ex.Message);
            }
        }

        string BatchStatus = "";
        public void AddBatchForDavaExport(int Production_Id, string BatchNo, bool IsExemptedBarcode, int JobId, string ExemptedCountryCode)
        {
            DAVAExportDetails davaExport = new DAVAExportDetails();
            davaExport.BatchName = BatchNo;
            davaExport.BatchQuantity = 0;
            davaExport.BatchStatus = BatchStatus;
            davaExport.ExemptedFromBarcoding = IsExemptedBarcode;
            davaExport.ProductionInfo_Id = Production_Id;
            if (IsExemptedBarcode)
            {
                davaExport.ExemptionDate = DateTime.Parse(DateTime.Now.ToString());
                davaExport.ExemptedCountryCode = ExemptedCountryCode;
            }
            m_jobId = davaExport.JobId = Convert.ToInt32(JobId); //m_dbhelper.DBManager.DAVAExportDetailsBLL.DAVAExportDetailsDAO.GetjobIdOfBatchNo(cmbBatchNo.Text);
            davaExport.ProductCode = m_dbhelper.DBManager.DAVAExportDetailsBLL.DAVAExportDetailsDAO.GetProductCodeOfBatchNo(JobId);
            davaExport.LastUpdatedDate = DateTime.Parse(DateTime.Now.ToString());
            davaExport.PrimaryPackPCMap = 0;
            m_dbhelper.DBManager.DAVAExportDetailsBLL.AddDAVAExportDetails(davaExport);

        }

    }


    class DAVATertiaryExemptionHelper
    {

        DbHelper m_dbhelper = new DbHelper();
        DAVA m_Dava = new DAVA();
        List<PackagingDetails> LstBatchData = new List<PackagingDetails>();
        DAVAExportFileTagsInfo m_davaExportFileTagsInfo = new DAVAExportFileTagsInfo();
        string m_CompanyCode = string.Empty;
        string sFilePath = string.Empty;
        private double FileSizeInKB = 0.0;
        private double FileSizeInMB = 0.0;
        public bool ISXMLFileSplit = false;
        int m_jobId = 0;
        List<PackagingDetails> Terpckdetails = new List<PackagingDetails>();

        public DAVAStatus CreateTerExempDAVAOld(int JobId, string BatchNo, List<PackagingDetails> SetWholeSSCC, bool IsExemptBarcode, string ExemptionCode, string CountryCode)
        {
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TER);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TER.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.TEREXEMPTED.ToString();
            string MaxDavaFileSize = Utilities.getAppSettings("DavaFileSizeInMB");

            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }

            StreamWriter sw;
            sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<ProductionInfo>");
            sw.WriteLine("<ENVELOPE>");
            sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
            if (!string.IsNullOrEmpty(ExemptionCode))
            {
                sw.WriteLine("<ExemptionCode>" + ExemptionCode + "</ExemptionCode>");
            }
    
            Terpckdetails = SetWholeSSCC;

            foreach (PackagingDetails pc in Terpckdetails)
            {
                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TER);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TER.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<ProductionInfo>");
                    sw.WriteLine("<ENVELOPE>");
                    sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    if (!string.IsNullOrEmpty(ExemptionCode))
                    {
                        sw.WriteLine("<ExemptionCode>" + ExemptionCode + "</ExemptionCode>");
                    }
                    
                    ISXMLFileSplit = false;
                }


                TreeNode NextLevelCode = new TreeNode();
                NextLevelCode.Text = pc.SSCC;
                sw.WriteLine("<Tertiary>");
                sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                List<PackagingDetails> SecpckDetails = m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                int subItemCount = SecpckDetails.Count();
                TreeNode subItemCountnode = new TreeNode();
                subItemCountnode.Text = "subItemCount- " + subItemCount.ToString();
                NextLevelCode.ChildNodes.Add(subItemCountnode);
                sw.WriteLine("<SubItemCnt>" + subItemCount + "</SubItemCnt>");
                sw.WriteLine("</Tertiary>");
                string reason = "Exported to DAVA Portal";
                UpdateExportedParentUIDs(pc.PackDtlsID, pc.Code, reason, pc.PackageTypeCode, JobId);
                UpdateDAVAStatus(pc.PackageTypeCode, pc.Code, pc.JobID);//Changed Status of DAVA for Tertiary Exempted -By Pranita
                //ProductionTree.Nodes.Add(NextLevelCode);
                //msg = " TERTIARY EXEMPTED " + FILENAME + ".xml";
                //m_dbhelper.AddUserTrail(CurrentUser.ID, null, REDTR.UTILS.USerTrailWHERE.TnT, REDTR.UTILS.USerTrailWHAT.DavaFileRegeneration, msg, null);
                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.8 && FileSizeInMB <= Convert.ToDouble(MaxDavaFileSize))
                {
                    sw.WriteLine("</ENVELOPE>");
                    sw.WriteLine("</ProductionInfo>");
                    sw.Close();
                    ISXMLFileSplit = true;
                }
            }

            if (FileSizeInMB < 1.8)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }

            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();
            int id3 = AddDavaFileTagsInfo();
            AddBatchForDavaExport(id3, JobId, BatchNo, IsExemptBarcode, CountryCode);
            SetWholeSSCC.Clear();
            return DAVAStatus.Success;

        }

        public DAVAStatus CreateTerExempDAVA(int JobId, string BatchNo, List<PackagingDetails> SetWholeSSCC, bool IsExemptBarcode, string ExemptionCode, string CountryCode, int lvl)
        {
            m_CompanyCode = DAVAUtility.getCompanyCode();
            string FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TER);
            m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TER.ToString();
            m_davaExportFileTagsInfo.FILENAME = FILENAME;
            sFilePath = DAVAUtility.getFileLocationToStore() + "\\" + XMLFILEType.TEREXEMPTED.ToString();
            string MaxDavaFileSize = Utilities.getAppSettings("DavaFileSizeInMB");

            if (!Directory.Exists(sFilePath))
            {
                Directory.CreateDirectory(sFilePath);
            }

            StreamWriter sw;
            sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<ProductionInfo>");
            sw.WriteLine("<ENVELOPE>");
            sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
            if (!string.IsNullOrEmpty(ExemptionCode))
            {
                sw.WriteLine("<ExemptionCode>" + ExemptionCode + "</ExemptionCode>");
            }
            int subItemCount = 0;
            Terpckdetails = SetWholeSSCC;

            foreach (PackagingDetails pc in Terpckdetails)
            {
                string qryPacks = "declare @GetParent integer" +
" select @GetParent = " + lvl + ";" +
            "; with cte as (" +
"select 1 lvl, P1.NextLevelCode, P1.Code, P1.PackageTypeCode, P1.JobID,P1.MfgPackDate,P1.ExpPackDate,P1.LastUpdatedDate,P1.SSCC from PackagingDetails P1" +
  " where P1.JobID=" + JobId + " AND SSCC like ('" + pc.SSCC + "') and IsUsed=1    union all" +
" select cte.lvl + 1, P2.NextLevelCode, P2.Code, P2.PackageTypeCode, P2.JobID,P2.MfgPackDate,P2.ExpPackDate,P2.LastUpdatedDate,P2.SSCC from PackagingDetails P2" +
 " join cte on cte.Code = P2.NextLevelCode  where P2.JobID=" + JobId + ")" +
" select distinct(Code) from cte c join JobDetails j on c.JobID = j.JD_JobID and c.PackageTypeCode = 'MOC'";
                DataSet ds = m_dbhelper.GetDataSet(qryPacks);
                subItemCount = ds.Tables[0].Rows.Count;
                FileSizeInKB = 0.0;
                FileSizeInMB = 0.0;
                if (ISXMLFileSplit == true)
                {
                    FILENAME = m_Dava.GetFileName(m_CompanyCode, DAVA.TypeOfUpload.TER);
                    m_davaExportFileTagsInfo.TypeofUpload = DAVA.TypeOfUpload.TER.ToString();
                    m_davaExportFileTagsInfo.FILENAME = FILENAME;
                    sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<ProductionInfo>");
                    sw.WriteLine("<FILENAME>" + FILENAME + "</FILENAME>");
                    if (!string.IsNullOrEmpty(ExemptionCode))
                    {
                        sw.WriteLine("<ExemptionCode>" + ExemptionCode + "</ExemptionCode>");
                    }
                    sw.WriteLine("<ENVELOPE>");
                    ISXMLFileSplit = false;
                }


                //TreeNode NextLevelCode = new TreeNode();
                //NextLevelCode.Text = pc.SSCC;
                sw.WriteLine("<Tertiary>");
                sw.WriteLine("<SSCC>" + pc.SSCC + "</SSCC>");
                //List<PackagingDetails> SecpckDetails = m_dbhelper.DBManager.PackagingDetailsBLL.GetDAVASecondaryPacks(PackagingDetailsBLL.OP.GetSecondaryCodesFromTerSrNo, pc.JobID, pc.Code);
                // subItemCount = SecpckDetails.Count();
                //TreeNode subItemCountnode = new TreeNode();
                //subItemCountnode.Text = "subItemCount- " + subItemCount.ToString();
                //NextLevelCode.ChildNodes.Add(subItemCountnode);
                sw.WriteLine("<SubItemCnt>" + subItemCount + "</SubItemCnt>");
                sw.WriteLine("</Tertiary>");
                string reason = "Exported to DAVA Portal";
                UpdateExportedParentUIDs(pc.PackDtlsID, pc.Code, reason, pc.PackageTypeCode, JobId);
                UpdateDAVAStatus(pc.PackageTypeCode, pc.Code, pc.JobID);//Changed Status of DAVA for Tertiary Exempted -By Pranita
                //ProductionTree.Nodes.Add(NextLevelCode);
                //msg = " TERTIARY EXEMPTED " + FILENAME + ".xml";
                //m_dbhelper.AddUserTrail(CurrentUser.ID, null, REDTR.UTILS.USerTrailWHERE.TnT, REDTR.UTILS.USerTrailWHAT.DavaFileRegeneration, msg, null);
                FileInfo fInfo = new FileInfo(sFilePath + "\\" + FILENAME + ".xml");
                FileSizeInKB = fInfo.Length / 1024;
                FileSizeInMB = FileSizeInKB / 1024;

                if (FileSizeInMB >= 1.8 && FileSizeInMB <= Convert.ToDouble(MaxDavaFileSize))
                {
                    sw.WriteLine("</ENVELOPE>");
                    sw.WriteLine("</ProductionInfo>");
                    sw.Close();
                    ISXMLFileSplit = true;
                }
            }

            if (FileSizeInMB < 1.8)
            {
                sw.WriteLine("</ENVELOPE>");
                sw.WriteLine("</ProductionInfo>");
                sw.Close();
            }

            m_davaExportFileTagsInfo.FileData = new StreamReader(sFilePath + "\\" + FILENAME + ".xml", Encoding.ASCII).ReadToEnd();
            int id3 = AddDavaFileTagsInfo();
            // AddBatchForDavaExport(id3, JobId, BatchNo, IsExemptBarcode, CountryCode);
            SetWholeSSCC.Clear();
            return DAVAStatus.Success;

        }

        private void UpdateDAVAStatus(string deck, string code, decimal JobId)
        {
            try
            {
                //code = AESCryptor.Encrypt(code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);

                string Query = "update PackagingDetails Set DavaPortalUpload =1 where JobId=" + JobId + " AND code ='" + code + "'";
                m_dbhelper.ExecuteQuery(Query);
            }
            catch (Exception ex)
            {
                //Trace.TraceError("Error At Updating DAVA status :" + ex.Message);
            }
        }
        private void UpdateExportedParentUIDs(decimal packId, string PackCode, string Remark, string Deck, int JobId)
        {
            try
            {
                PackagingDetails Pck = new PackagingDetails();
                Pck.Code = PackCode;
                Pck.PackageTypeCode = Deck.ToString();
                Pck.JobID = int.Parse(JobId.ToString());
                Pck.DAVAPortalUpload = true;
                m_dbhelper.DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.UpdateParentUIDdavaportalupload, Pck);
                m_dbhelper.ManualUpdationRemark(int.Parse(JobId.ToString()), packId, DbHelper.ManualStatusUpdationTypes.Decommisioned, null, null, Remark);

                UpdateExportedChildUIDs(Deck, PackCode, Remark, JobId);

            }
            catch (Exception ex)
            {
                //Trace.TraceError("{0},{1}{2}{3}", DateTime.Now.ToString(), ex.Message, ex.StackTrace, "QA SAMPLING");
            }
        }

        private void UpdateExportedChildUIDs(string deck, string Ncode, string reason, int JobId)
        {
            //if (IsLableGen == true)
            //    return;
            string EncryptedNcode = "";
            EncryptedNcode = Ncode;
            if (EncryptedNcode != null) { EncryptedNcode = AESCryptor.Encrypt(EncryptedNcode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
            List<PackagingDetails> lstpackdtls = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetDtlsOfJobNNextLevelCode, JobId.ToString(), EncryptedNcode);

            if (lstpackdtls.Count == 0)
                return;

            foreach (PackagingDetails item in lstpackdtls)
            {
                UpdateExportedChildUIDs(item.PackageTypeCode, item.Code, reason, JobId);
            }

            string manualupdateDescXML = m_dbhelper.GetManualUpdationXML(int.Parse(JobId.ToString()), DbHelper.ManualStatusUpdationTypes.DAVAExport, null, null, reason);
            PackagingDetails Pck = new PackagingDetails();
            Pck.NextLevelCode = Ncode;
            Pck.JobID = int.Parse(JobId.ToString());
            Pck.PackageTypeCode = deck.ToString();
            Pck.ManualUpdateDesc = manualupdateDescXML;
            Pck.DAVAPortalUpload = true;
            m_dbhelper.DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.UpdateChildUIDdavaportalupload, Pck);
        }

        private int AddDavaFileTagsInfo()
        {
            m_davaExportFileTagsInfo.CreatedDate = DateTime.Parse(DateTime.Now.ToString());
            return m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.AddDAVAExportFileTagsInfo(m_davaExportFileTagsInfo);

        }

        private void AddBatchForDavaExport(int Production_Id, int JobId, string BatchNo, bool IsExemptBarcode, string CountryCode)
        {
            DAVAExportDetails davaExport = new DAVAExportDetails();
            davaExport.BatchName = BatchNo;
            davaExport.BatchQuantity = 0;
            davaExport.BatchStatus = "";
            davaExport.ExemptedFromBarcoding = IsExemptBarcode;
            davaExport.ProductionInfo_Id = Production_Id;
            if (IsExemptBarcode)
            {
                davaExport.ExemptionDate = DateTime.Parse(DateTime.Now.ToString());
                davaExport.ExemptedCountryCode = CountryCode;
            }
            m_jobId = davaExport.JobId = JobId;
            davaExport.ProductCode = m_dbhelper.DBManager.DAVAExportDetailsBLL.DAVAExportDetailsDAO.GetProductCodeOfBatchNo(JobId);
            davaExport.LastUpdatedDate = DateTime.Parse(DateTime.Now.ToString());
            davaExport.PrimaryPackPCMap = 0;
            m_dbhelper.DBManager.DAVAExportDetailsBLL.AddDAVAExportDetails(davaExport);

        }

    }
}