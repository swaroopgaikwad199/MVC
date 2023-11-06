using REDTR.DB.BLL;
using REDTR.UTILS;
using REDTR.UTILS.SystemIntegrity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.Reports
{
    public class ReportJobHelper
    {
        REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();

        private int ChlngTestCnt = 0;
        int ChallengeTestCount = 0;
        int QASampleCount = 0;
        int DecommisionedCount = 0;
        public DataSet RetReportDs(JobBLL.ReportOp op, string Value1, string Value2, DateTime FrmDt, DateTime ToDt, decimal JobId)
        {

            DataSet ds = ObjHelper.DBManager.JobBLL.GetReportDataSet(op, Value1, Value2,1, FrmDt.Date, ToDt.Date, false, -1);
            REDTR.DB.BusinessObjects.Job jobs = ObjHelper.DBManager.JobBLL.GetJob(REDTR.DB.BLL.JobBLL.JobOp.GetJob,1, JobId.ToString(), null);
            var FirstDeck = ProductPackageHelper.getTopDeck(jobs.PAID);
            ChallengeTestCount = GetChallengeTestCNT("CHALLENGE TEST", JobId);
            int NoReadCount = 0;
            if (jobs != null)
                NoReadCount = Convert.ToInt32(jobs.NoReadCount);
            if (op == JobBLL.ReportOp.ForJobdeckProcess)
            {
                TnT_Ds.DeckwiseCountDataTable tbl = new TnT_Ds.DeckwiseCountDataTable();

                tbl.Merge(ds.Tables[0]);

                DataColumnCollection columns = tbl.Columns;
                foreach (DataRow rw in tbl.Rows)
                {
                    object[] values = rw.ItemArray;

                    int index = columns.IndexOf(tbl.PackageTypeCodeColumn);
                    DECKs deck = values[index].ToString().ParseToDeck();
                    REDTR.DB.BusinessObjects.PackageTypeCode pc = ObjHelper.DBManager.PackageTypeCodeBLL.GetPackageTypeCode(deck.ToString());
                    QASampleCount = GetQASampleCNT("QASAMPLING", pc.Code, JobId);
                    DecommisionedCount = GetDecommisionCNT("DECOMMISIONED", pc.Code, JobId);
                    try
                    {
                        if (values[2].ToString() == FirstDeck)
                        {
                            //values[5] = Convert.ToInt32(values[5]) + NoReadCount;
                            values[6] = DecommisionedCount;
                            values[7] = ChallengeTestCount;
                            values[8] = QASampleCount;
                        }
                        else
                        {
                            values[6] = DecommisionedCount;
                            values[8] = QASampleCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Trace.TraceError("{0},{1},{2}", DateTime.Now, ex.Message, ex.StackTrace);
                    }
                    index = columns.IndexOf(tbl.DeckDispNameColumn);
                    values[index] = deck.DisplayName();

                    index = columns.IndexOf(tbl.deckIndexColumn);
                    values[index] = deck.index();

                    rw.ItemArray = values;
                }
                ds = new DataSet();
                ds.Tables.Add(tbl);
            }
            return ds;
        }

        private int GetChallengeTestCNT(string DecommisionType, decimal JobId)
        {
            int ChlngTestCnt = 0;
            try
            {
                string StrSeperator = "User";
                List<REDTR.DB.BusinessObjects.PackagingDetails> PckDtlsLst = ObjHelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetPckDtslForChallengeTest, JobId.ToString(), null);
                List<int> lstCnt = new List<int>();
                //int decommisionRECCount = 0;
                foreach (REDTR.DB.BusinessObjects.PackagingDetails Pck in PckDtlsLst)
                {
                    string XmlString = Pck.ManualUpdateDesc;
                    if (XmlString.Contains(DecommisionType))
                    {
                        //decommisionRECCount++;
                        string SepData = ChallenegTest.SubstringBefore(XmlString, StrSeperator);//XmlString.Split('|').Last();
                        string CountOFChallenegeTest = SepData.Split('|').Last();
                        string CNT = CountOFChallenegeTest.Substring(0, CountOFChallenegeTest.Length - 2);
                        lstCnt.Add(Convert.ToInt32(CNT.Trim()));
                        ChlngTestCnt = lstCnt.Max();
                    }
                }
                return ChlngTestCnt;//decommisionRECCount++
            }
            catch (Exception ex)
            {
                ChlngTestCnt = 0;
            }
            return ChlngTestCnt;
        }

        private int GetQASampleCNT(string DecommisionType, string deck, decimal JobId) // deck added for detail job information to show proper QA sample count deckwise. [06.10.2016])
        {
            try
            {
                ChlngTestCnt = 0;
                List<REDTR.DB.BusinessObjects.PackagingDetails> PckDtlsLst = ObjHelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetCountForDeckwiseQAnDecomissionedSample, JobId.ToString(), deck);
                foreach (REDTR.DB.BusinessObjects.PackagingDetails Pck in PckDtlsLst)
                {
                    string XmlString = Pck.ManualUpdateDesc;
                    if (XmlString.Contains(DecommisionType))
                        ChlngTestCnt++;

                }
                return ChlngTestCnt;
            }
            catch (Exception ex)
            {
                ChlngTestCnt = 0;
            }
            return ChlngTestCnt;
        }


        private int GetDecommisionCNT(string DecommisionType, string deck, decimal JobId) // deck added for detail job informationto show proper QA sample count deckwise. [06.10.2016]
        {
            try
            {
                ChlngTestCnt = 0;
                List<REDTR.DB.BusinessObjects.PackagingDetails> PckDtlsLst = ObjHelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetCountForDeckwiseQAnDecomissionedSample, JobId.ToString(), deck);
                foreach (REDTR.DB.BusinessObjects.PackagingDetails Pck in PckDtlsLst)
                {
                    string XmlString = Pck.ManualUpdateDesc;
                    if (XmlString.Contains(DecommisionType))
                        ChlngTestCnt++;

                }
                return ChlngTestCnt;
            }
            catch (Exception ex)
            {
                ChlngTestCnt = 0;

            }
            return ChlngTestCnt;
        }

    }
}
