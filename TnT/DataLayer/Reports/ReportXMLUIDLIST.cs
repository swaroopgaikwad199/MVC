using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using TnT.Models.ImportXml;

namespace TnT.DataLayer.Reports
{
    public class ReportXMLUIDLIST
    {
        bool IsSkipCrystalReport = false;
        string constr = Utilities.getConnectionString("DefaultConnection");
        #region Generate XML Report File from XMLUIDLIST
        public string GenerateXMLReportFile(ModelViewXmlUIDList mv)
        {
            try
            {
                string msg = string.Empty;
                DataSet ds = new DataSet();
                //string query = "Select B.Code,A.BatchNo,A.ExpDate,A.MfgDate,D.GTIN,C.ProductCode from Job A,PackagingDetails B,PackagingAsso C,PackagingAssoDetails D"
                //+ " where A.JID = B.JobID and A.PAID = C.PAID and D.PAID = C.PAID and(B.IsRejected = 0) and(B.IsDecomission is null or B.IsDecomission = 0) and  D.PackageTypeCode='MOC'"
                //+ "and A.JID = " + mv.JobID + " and B.PackageTypeCode = '" + mv.Deck + "' ";
                string query = "Select B.Code,A.BatchNo,A.ExpDate,A.MfgDate,D.GTIN,C.ProductCode from Job A,PackagingDetails B,PackagingAsso C,"
                 + "PackagingAssoDetails D,JobDetails E where A.JID = B.JobID and A.PAID = C.PAID and D.PAID = C.PAID and E.JD_JobID=A.JID and"
                 + " (B.IsRejected = 0) and(B.IsDecomission is null or B.IsDecomission = 0) and  E.JD_Deckcode='"+mv.Deck+"'and A.JID = "+mv.JobID+" and "
                 + " B.PackageTypeCode = '" + mv.Deck+ "'  ";
                ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, query);
                bool IsFirst = true;
                string sFilePath = string.Empty;
                int iReccount = 0;
                //int iNoofRecPerFile = Globals.AppSettings.NoOfRecordsPerFile; //Set NoOfRecords per file in AppSettings File
                int iNoofRecPerFile = Convert.ToInt32(mv.iNoofRecPerFile);  //Set NoOfRecords per file in AppSettings File

                int iNoofFilecnt = 1;
                //string StrReaderId = Globals.AppSettings.ReaderID;                   
                string StrReaderId = mv.StrReaderId;
                string StrCommand = mv.StrCommand;
                string StrDeviceId = mv.StrDeviceId;
                string StrUOM = mv.StrUOM;


                string FILENAME = "PML";


                if (ds.Tables[0].Rows.Count > 0)
                {
                    FILENAME = "PML_" + ds.Tables[0].Rows[0]["BatchNo"].ToString();
                }
                else
                {
                    IsSkipCrystalReport = false;
                    return msg;
                }

                sFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/ImportXML/PML/" + FILENAME + "/");

                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                System.IO.DirectoryInfo di = new DirectoryInfo(sFilePath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                //if (File.Exists(sFilePath + "\\" + FILENAME + ".xml") == true)
                //{
                //    File.Delete(sFilePath + "\\" + FILENAME + ".xml");
                //}

                StreamWriter sw = new StreamWriter(sFilePath + "\\" + FILENAME + ".xml");


                for (Int32 Icntr = 0; Icntr < ds.Tables[0].Rows.Count; Icntr++)
                {

                    if (IsFirst == true)
                    {
                        sw.WriteLine("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "utf-8" + '"' + " ?>");
                        sw.WriteLine("<pmlcore:Sensor xmlns:pmlcore=" + '"' + "urn:autoid:specification:interchange:PMLCore:xml:schema:1" + '"' + " xmlns:pmluid=" + '"' + "urn:autoid:specification:universal:Identifier:xml:schema:1" + '"' + " xmlns:xsi=" + '"' + "http://www.w3.org/2001/XMLSchema-instance" + '"' + " xsi:schemaLocation=" + '"' + "urn:autoid:specification:interchange:PMLCore:xml:schema:1 ./PML/SchemaFiles/Interchange/PMLCore.xsd" + '"' + ">");
                        sw.WriteLine("<pmluid:ID>" + StrDeviceId + "</pmluid:ID>");
                        sw.WriteLine("<pmlcore:Observation>");
                        sw.WriteLine("<pmluid:ID>" + StrReaderId + "</pmluid:ID>");
                        sw.WriteLine("<pmlcore:DateTime>" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "</pmlcore:DateTime>");
                        sw.WriteLine("<pmlcore:Command>" + StrCommand + "</pmlcore:Command>");
                        IsFirst = false;
                    }

                    sw.WriteLine("<pmlcore:Tag>");
                    sw.WriteLine("<pmluid:ID schemeID=" + '"' + "ZGS1_SER" + '"' + ">(01)" + ds.Tables[0].Rows[Icntr]["GTIN"].ToString() + "(21)" + ds.Tables[0].Rows[Icntr]["CODE"].ToString() + "</pmluid:ID>");
                    sw.WriteLine("<pmlcore:Data>");
                    sw.WriteLine("<pmlcore:XML>");
                    sw.WriteLine("<pmlcore:EPCStatus/>");

                    sw.WriteLine("<Memory>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "BATCH_ID" + '"' + ">" + ds.Tables[0].Rows[Icntr]["BatchNo"] + "</DataField>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "EXPIRATION_DATE" + '"' + ">" + Convert.ToDateTime(ds.Tables[0].Rows[Icntr]["ExpDate"].ToString()).ToString("yyMMdd") + "</DataField>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "ZZ_MFD_DATE" + '"' + ">" + Convert.ToDateTime(ds.Tables[0].Rows[Icntr]["MfgDate"].ToString()).ToString("yyMMdd") + "</DataField>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "PRODUCT" + '"' + ">" + ds.Tables[0].Rows[Icntr]["ProductCode"].ToString() + "</DataField>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "GTIN" + '"' + ">" + ds.Tables[0].Rows[Icntr]["GTIN"].ToString() + "</DataField>");
                    sw.WriteLine("<DataField fieldName=" + '"' + "UOM" + '"' + ">" + StrUOM + "</DataField>");

                    sw.WriteLine("</Memory>");
                    sw.WriteLine("</pmlcore:XML>");
                    sw.WriteLine("</pmlcore:Data>");
                    sw.WriteLine("</pmlcore:Tag>");
                    //sw.WriteLine("</pmlcore:Tag>");

                    iReccount++;
                    if (iReccount >= iNoofRecPerFile)
                    {
                        if (Icntr < ds.Tables[0].Rows.Count - 1)
                        {
                            sw.WriteLine("<pmlcore:Data>");
                            sw.WriteLine("<pmlcore:XML>");
                            sw.WriteLine("<ReaderID>" + StrReaderId + "</ReaderID>");
                            sw.WriteLine("</pmlcore:XML>");
                            sw.WriteLine("</pmlcore:Data>");
                            sw.WriteLine("</pmlcore:Observation>");
                            sw.WriteLine("</pmlcore:Sensor>");
                            sw.Close();

                            IsFirst = true;

                            sw = new StreamWriter(sFilePath + "\\" + FILENAME + "_" + iNoofFilecnt.ToString() + ".xml");


                            iReccount = 0;
                            iNoofFilecnt++;
                        }
                    }


                }

                sw.WriteLine("<pmlcore:Data>");
                sw.WriteLine("<pmlcore:XML>");
                sw.WriteLine("<ReaderID>" + StrReaderId + "</ReaderID>");
                sw.WriteLine("</pmlcore:XML>");
                sw.WriteLine("</pmlcore:Data>");
                sw.WriteLine("</pmlcore:Observation>");
                sw.WriteLine("</pmlcore:Sensor>");
                sw.Close();

                return "File Create Success";

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string getFileLocationToStore()
        {
            AppSettingsReader MyReader = new AppSettingsReader();
            string Path = MyReader.GetValue("XMLUIDLIstFileLctn", typeof(string)).ToString();
            return Path;
        }

        #endregion

    }
}