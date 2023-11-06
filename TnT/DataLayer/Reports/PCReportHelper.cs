using PTPL.Resources.Properties;
using PTPLCRYPTORENGINE;
using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.Reports
{

    public class PCReportHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
        public DataSet getPCRptDS(decimal JID)
        {
            TnT_Ds dsTnT = new TnT_Ds();
            bool IsShowRpt = false;

            List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
            //    var jb = db.PackagingDetails.Where(x => x.JobID == JID).GroupBy(x => x.PackageTypeCode).Select(n => new { Text = n.Key, Value = n.Count() }).OrderBy(n=>n.Value).ToList();
            var jb = ProductPackageHelper.getAllDeck(JID.ToString());
            jb = ProductPackageHelper.sorttheLevelsDesc(jb);
            var decks = LstJobDetails.Select(x => x.JD_Deckcode);
            if (jb.Count == 3)
            {
                string Query1 = "Set Statistics time on Select cast(TBLMOC.JobID as nvarchar(100))  as JobID,TBLISH.Code as " + jb[0] + "Code,TBLOBX.Code as " + jb[1] + "Code,TBLMOC.Code as " + jb[2] + "Code,TBLISH.SSCC as SSCC,cast(TBLMOC.JobID as nvarchar(100))  as JobId,TBLISH.SSCC as " + jb[0] + "Code from ((Select JobID, Code, NextLevelCode, SSCC from PackagingDetails where IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[0] + "' and JobID = " + JID + ")TBLISH inner join (Select JobID, Code, NextLevelCode from PackagingDetails where IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[1] + "' and JobID = " + JID + " )TBLOBX on TBLISH.Code = TBLOBX.NextLevelCode inner join (Select JobID, Code, NextLevelCode from PackagingDetails where IsRejected= 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[2] + "' and JobID = " + JID + " )TBLMOC on TBLOBX.Code = TBLMOC.NextLevelCode) order by " + jb[1] + "Code Set Statistics time off";
                DbHelper m_dbhelper = new DbHelper();
                DataSet ds = m_dbhelper.GetDataSet(Query1);
                if (!jb.Any(x => x == "OBX"))
                {
                    ds.Tables[0].Columns.RemoveAt(1);
                    ds.Tables[0].Columns.Add("OBXCode", typeof(string)).SetOrdinal(2);
                }

                if (!jb.Any(x => x == "PAL"))
                {
                  
                    ds.Tables[0].Columns.RemoveAt(6);
                    ds.Tables[0].Columns.Add("PALCode", typeof(string)).SetOrdinal(6);

                }

                if (!jb.Any(x => x == "ISH"))
                {
                    ds.Tables[0].Columns.RemoveAt(6);
                    ds.Tables[0].Columns["OBXCode"].ColumnName = "ISHCode";
                    ds.Tables[0].Columns.Add("OBXCode", typeof(string)).SetOrdinal(3);
                    ds.Tables[0].Columns[1].SetOrdinal(6);
                }

                List<JobDetails> TempLstJobDetails = new List<JobDetails>();
                TempLstJobDetails = LstJobDetails;

                string EncryptedData = Resources.UIDDefault;
                DataSet dsPck = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPckBoxRelationship, JID.ToString(), EncryptedData);
                dsPck.Tables[0].AcceptChanges();


                //ObjHelper.DBManager.PackagingDetailsBLL.DeleteTempPCMAP();
                //List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = new List<REDTR.DB.BusinessObjects.PackagingDetails>();
                //JobDetails JTerDeck = LstJobDetails.Find(item => item.JD_Deckcode == "PAL");
                //bool IsPAL = false;
                //bool IsISH = false;
                //bool IsOSH = false;

                //if (JTerDeck != null)
                //{
                //    PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "PAL", "NULL");
                //    IsPAL = true;
                //}
                //else
                //{
                //    if (decks.Contains("OSH"))
                //    {
                //        JobDetails JTerDeck1 = TempLstJobDetails.Find(item => item.JD_Deckcode == "OSH");
                //        if (JTerDeck1 != null)
                //        {
                //            PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OSH", "NULL");
                //            IsOSH = true;
                //        }
                //    }
                //    else if (decks.Contains("ISH"))
                //    {

                //        JobDetails JTerDeck1 = TempLstJobDetails.Find(item => item.JD_Deckcode == "ISH");
                //        if (JTerDeck1 != null)
                //        {
                //            PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", "NULL");
                //            IsISH = true;
                //        }
                //    }

                //}




                //// For ISH
                //List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = new List<REDTR.DB.BusinessObjects.PackagingDetails>();
                //for (int i = 0; i < PckISHCode.Count; i++)
                //{
                //    JobDetails JNonTerDeck = null;
                //    if (decks.Contains("OSH"))
                //    {
                //        JNonTerDeck = LstJobDetails.Find(item => item.JD_Deckcode == "OSH");
                //    }
                //    else if (decks.Contains("ISH"))
                //    {
                //        JNonTerDeck = LstJobDetails.Find(item => item.JD_Deckcode == "ISH");
                //    }


                //    if (JNonTerDeck != null && IsISH == false && IsOSH == false)
                //    {
                //        if (decks.Contains("OSH"))
                //        {
                //            PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OSH", PckISHCode[i].Code);
                //        }
                //        else if (decks.Contains("ISH"))
                //        {
                //            PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", PckISHCode[i].Code);
                //        }
                //    }
                //    else
                //    {
                //        JobDetails JTerDeck1 = LstJobDetails.Find(item => item.JD_Deckcode == "OBX");
                //        if (JTerDeck1 != null)
                //        {
                //            PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OBX", PckISHCode[i].Code);
                //        }
                //    }


                //    // For OBX
                //    for (int j = 0; j < PckOBXCode.Count; j++)
                //    {
                //        List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                //        for (int k = 0; k < PckMOCCode.Count; k++)
                //        {
                //            if (IsPAL)
                //            {
                //                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), PckISHCode[i].Code, PckOBXCode[j].Code, string.Empty, PckMOCCode[k].Code, PckISHCode[i].SSCC);
                //            }
                //            else
                //            {
                //                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), string.Empty, PckISHCode[i].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckISHCode[i].SSCC);

                //            }
                //        }
                //    }
                //}

                //DataSet TempPCM = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetTempPCMAP, JID.ToString(), null);

                //IsOSH = false;
                //IsISH = false;
                //IsPAL = false;
                IsShowRpt = (dsPck.Tables[0].Rows.Count > 0);
                dsTnT.PackBoxesRelationship.Rows.Clear();


                if (IsShowRpt)
                {
                    DataView view = ds.Tables[0].DefaultView;
                    view.Sort = "SSCC ASC";
                    DataTable sortedPCM = view.ToTable();


                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                    //dsTnT.TempPCMAP.Merge(TempPCM.Tables[0]);
                    dsTnT.TempPCMAP.Merge(sortedPCM);

                    return dsTnT;
                }
                else
                {
                    return null;
                }
            }
            else if (LstJobDetails.Count == 4)
            {



                string Query = "Set Statistics time on Select cast(TBLPAL.JobID as nvarchar(100))  as JobID,TBLISH.Code as ISHCode,TBLOBX.Code as OBXCode,TBLMOC.Code as MOCCode,TBLPAL.SSCC as SSCC,cast(TBLPAL.JobID as nvarchar(100))  as JobId,TBLPAL.SSCC as PALCode from ((Select JobID, Code, NextLevelCode, SSCC from PackagingDetails where IsRejected= 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[0] + "' and JobID=" + JID + ")TBLPAL inner join  (Select JobID, Code, NextLevelCode from PackagingDetails where IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[1] + "' and JobID=" + JID + ")TBLISH on TBLPAL.Code = TBLISH.NextLevelCode inner join (Select JobID, Code, NextLevelCode from PackagingDetails where IsRejected= 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[2] + "' and JobID=" + JID + ")TBLOBX on TBLISH.Code = TBLOBX.NextLevelCode inner join  (Select JobID, Code, NextLevelCode from PackagingDetails where IsRejected= 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[3] + "' and JobID=" + JID + ")TBLMOC on TBLOBX.Code = TBLMOC.NextLevelCode) Set Statistics time off";
                DbHelper m_dbhelper = new DbHelper();
                DataSet ds = m_dbhelper.GetDataSet(Query);

                string EncryptedData = Resources.UIDDefault;
                //if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                DataSet dsPck = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPckBoxRelationship,
                     JID.ToString(), EncryptedData);


                //for (int i = 0; i < dsPck.Tables[0].Rows.Count; i++)
                //{
                //    dsPck.Tables[0].Rows[i]["Code"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["Code"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //    dsPck.Tables[0].Rows[i]["NextLevelCode"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["NextLevelCode"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                dsPck.Tables[0].AcceptChanges();
                IsShowRpt = (ds.Tables[0].Rows.Count > 0);
                dsTnT.PackBoxesRelationship.Rows.Clear();
                if (IsShowRpt)
                {
                    DataSet ds1 = new DataSet();
                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                    //  dsTnT.TempPCMAP.Merge(ds.Tables[0]);
                    var temp = ds.Tables[0].AsEnumerable().OrderBy(x => x.Field<string>("SSCC")).ThenBy(y => y.Field<string>("ISHCode")).ThenBy(z => z.Field<string>("OBXCode")).CopyToDataTable();
                    //  dsTnT.TempPCMAP.OrderBy(x => x.SSCC).ThenBy(y => y.ISHCode).ThenBy(z => z.OBXCode).CopyToDataTable();
                    dsTnT.TempPCMAP.Merge(temp);
                    return dsTnT;
                }
                else
                {
                    return null;
                }
                // For PCMAP

                // For Delete the TempPCMAP
                //ObjHelper.DBManager.PackagingDetailsBLL.DeleteTempPCMAP();

                //List<REDTR.DB.BusinessObjects.PackagingDetails> PckPALCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "PAL", "NULL");
                //// For ISH
                //for (int i = 0; i < PckPALCode.Count; i++)
                //{
                //    // List<PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(Convert.ToInt32(m_JObID), "ISH", "NULL");
                //    List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", PckPALCode[i].Code);

                //    for (int m = 0; m < PckISHCode.Count; m++)
                //    {
                //        List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OBX", PckISHCode[m].Code);

                //        // For OBX
                //        for (int j = 0; j < PckOBXCode.Count; j++)
                //        {
                //            List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                //            for (int k = 0; k < PckMOCCode.Count; k++)
                //            {
                //                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), PckPALCode[i].Code, PckISHCode[m].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckPALCode[i].SSCC);
                //            }
                //        }
                //    }
                //}
                //DataSet TempPCM = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetTempPCMAP,
                //    JID.ToString(), null);
                //// End

                //IsShowRpt = (TempPCM.Tables[0].Rows.Count > 0);
                //dsTnT.PackBoxesRelationship.Rows.Clear();
                //if (IsShowRpt)
                //{
                //    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                //    dsTnT.TempPCMAP.Merge(TempPCM.Tables[0]);
                //    return dsTnT;
                //}
                //else
                //{
                //    return null;
                //}
            }
            else if (LstJobDetails.Count == 5)
            {



                string Query = " Select cast(TBLPAL.JobID as nvarchar(100))  as JobID,TBLPAL.SSCC,TBLPAL.Code as PALCode,TBLOSH.Code as OSHCode,TBLISH.Code as ISHCode,TBLOBX.Code as OBXCode,TBLMOC.Code as MOCCode from" +
" ((Select JobID, Code, NextLevelCode, SSCC from PackagingDetails where JobID= " + JID + " and IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[0] + "')TBLPAL inner join" +
              "(Select Code, NextLevelCode from PackagingDetails where JobID = " + JID + " and IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[1] + "')TBLOSH on TBLPAL.Code = TBLOSH.NextLevelCode inner join" +
                         "(Select Code, NextLevelCode from PackagingDetails where JobID= " + JID + " and IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[2] + "')TBLISH on TBLOSH.Code = TBLISH.NextLevelCode inner join" +
                                   "(Select Code, NextLevelCode from PackagingDetails where JobID= " + JID + " and IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[3] + "')TBLOBX on TBLISH.Code = TBLOBX.NextLevelCode inner join" +
                                             "(Select Code, NextLevelCode from PackagingDetails where JobID= " + JID + " and IsRejected = 0 and IsDecomission = 0 and PackageTypeCode = '" + jb[4] + "')TBLMOC on TBLOBX.Code = TBLMOC.NextLevelCode)";
                DbHelper m_dbhelper = new DbHelper();
                DataSet ds = m_dbhelper.GetDataSet(Query);

                string EncryptedData = Resources.UIDDefault;
                //if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                DataSet dsPck = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPckBoxRelationship,
                     JID.ToString(), EncryptedData);


                //for (int i = 0; i < dsPck.Tables[0].Rows.Count; i++)
                //{
                //    dsPck.Tables[0].Rows[i]["Code"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["Code"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //    dsPck.Tables[0].Rows[i]["NextLevelCode"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["NextLevelCode"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                dsPck.Tables[0].AcceptChanges();
                IsShowRpt = (ds.Tables[0].Rows.Count > 0);
                dsTnT.PackBoxesRelationship.Rows.Clear();
                if (IsShowRpt)
                {
                    DataSet ds1 = new DataSet();
                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                    //  dsTnT.TempPCMAP.Merge(ds.Tables[0]);
                    var temp = ds.Tables[0].AsEnumerable().OrderBy(x => x.Field<string>("SSCC")).ThenBy(y => y.Field<string>("ISHCode")).ThenBy(z => z.Field<string>("OBXCode")).CopyToDataTable();
                    //  dsTnT.TempPCMAP.OrderBy(x => x.SSCC).ThenBy(y => y.ISHCode).ThenBy(z => z.OBXCode).CopyToDataTable();
                    dsTnT.TempPCMAP.Merge(temp);
                    return dsTnT;
                }
                else
                {
                    return null;
                }
                // For PCMAP

                // For Delete the TempPCMAP
                //ObjHelper.DBManager.PackagingDetailsBLL.DeleteTempPCMAP();

                //List<REDTR.DB.BusinessObjects.PackagingDetails> PckPALCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "PAL", "NULL");
                //// For ISH
                //for (int i = 0; i < PckPALCode.Count; i++)
                //{
                //    // List<PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(Convert.ToInt32(m_JObID), "ISH", "NULL");
                //    List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", PckPALCode[i].Code);

                //    for (int m = 0; m < PckISHCode.Count; m++)
                //    {
                //        List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OBX", PckISHCode[m].Code);

                //        // For OBX
                //        for (int j = 0; j < PckOBXCode.Count; j++)
                //        {
                //            List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                //            for (int k = 0; k < PckMOCCode.Count; k++)
                //            {
                //                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), PckPALCode[i].Code, PckISHCode[m].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckPALCode[i].SSCC);
                //            }
                //        }
                //    }
                //}
                //DataSet TempPCM = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetTempPCMAP,
                //    JID.ToString(), null);
                //// End

                //IsShowRpt = (TempPCM.Tables[0].Rows.Count > 0);
                //dsTnT.PackBoxesRelationship.Rows.Clear();
                //if (IsShowRpt)
                //{
                //    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                //    dsTnT.TempPCMAP.Merge(TempPCM.Tables[0]);
                //    return dsTnT;
                //}
                //else
                //{
                //    return null;
                //}
            }
            else
            {
                string EncryptedData = Resources.UIDDefault;
                //if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                DataSet dsPck = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPckBoxRelationship2Deck, JID.ToString(), EncryptedData);


                //for (int i = 0; i < dsPck.Tables[0].Rows.Count; i++)
                //{
                //    dsPck.Tables[0].Rows[i]["Code"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["Code"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //    dsPck.Tables[0].Rows[i]["NextLevelCode"] = PTPLCRYPTORENGINE.AESCryptor.Decrypt(dsPck.Tables[0].Rows[i]["NextLevelCode"].ToString(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                dsPck.Tables[0].AcceptChanges();

                IsShowRpt = (dsPck.Tables[0].Rows.Count > 0);
                if (IsShowRpt)
                {
                    dsTnT.PackBoxesRelationship.Rows.Clear();
                    dsTnT.REC_Count.Rows.Clear();
                    dsTnT.ReconciliationCount.Rows.Clear();
                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);


                    return dsTnT;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}