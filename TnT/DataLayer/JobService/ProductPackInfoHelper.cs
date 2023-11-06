using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.JOB;
using REDTR.UTILS;
using REDTR.UTILS.SystemIntegrity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.JobService
{
    public class ProductPackInfoHelper
    {
        List<PackInfo> LstpackInfo;
        List<PackageLabelAsso> LstPackLabelInfo;
        private Decimal m_jobId = 0;
        private Decimal m_PAId = 0;
        int m_BatchQty = 0;
        int m_SurplusQty = 0;

        public ProductPackInfoHelper( decimal JobId,decimal PAID,int BatchQty,int Surplus)
        {
            m_jobId = JobId;
            m_PAId = PAID;
            m_BatchQty = BatchQty;
            m_SurplusQty = Surplus;

            RefreshPackInfo();
        }

        private bool IsProdLoadForJob = true;

        REDTR.HELPER.DbHelper dbhelper = new REDTR.HELPER.DbHelper();
        private List<PackInfo> RefreshPackInfo()
        {
            try
            {
                if (m_jobId > 0)
                {
                    Job job = dbhelper.DBManager.JobBLL.GetJob(REDTR.DB.BLL.JobBLL.JobOp.GetJob, 1, m_jobId.ToString(), null);

                    LstpackInfo = new List<PackInfo>();
                    LstPackLabelInfo = new List<PackageLabelAsso>();
                    List<JobDetails> Jd = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, m_jobId, -1);
                    if (Jd.Count > 0)
                    {
                        foreach (JobDetails item in Jd)
                        {
                            //if (item.JD_Deckcode.ParseToDeck().IsExists() == false)
                            //    continue;
                            PackInfo PckInfo = new PackInfo();
                            PckInfo.PPN = item.JD_PPN;
                            PckInfo.GTIN = item.JD_GTIN;
                            PckInfo.GTINCTI = item.JD_GTINCTI;
                            PckInfo.Deck = item.JD_Deckcode.ParseToDeck();
                            PckInfo.Size = item.JD_DeckSize;
                            PckInfo.BundleQty = item.BundleQty;
                            PckInfo.MRP = item.JD_MRP;
                            PckInfo.FGCode = item.JD_FGCode; // Newly by Murtaza[14.09.2015].
                            LstpackInfo.Add(PckInfo);

                            List<PackageLabelAsso> PLLst = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetPackageLabelDetails), m_PAId.ToString(), item.JD_Deckcode.ParseToDeck().ToString());
                            foreach (PackageLabelAsso item1 in PLLst)
                            {
                                PackageLabelAsso PckLinfo = new PackageLabelAsso();
                                PckLinfo.PAID = item1.PAID;
                                PckLinfo.JobTypeID = item1.JobTypeID;
                                PckLinfo.Code = item1.Code;
                                PckLinfo.LabelName = item1.LabelName;
                                PckLinfo.Filter = item1.Filter;
                                PckLinfo.LastUpdatedDate = item1.LastUpdatedDate;
                                LstPackLabelInfo.Add(PckLinfo);
                            }
                        }
                    }
                    else
                    {
                        LstpackInfo = new List<PackInfo>();
                        LstPackLabelInfo = new List<PackageLabelAsso>(); //05Sept2015 Sunil
                        List<PackagingAssoDetails> PDLst = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(m_PAId);
                        int iCnt = 1;
                        foreach (PackagingAssoDetails item in PDLst)
                        {
                            //if (item.PackageTypeCode.ParseToDeck().IsExists() == false)
                            //    continue;
                            if (IsProdLoadForJob == true && item.Size == -1)
                                continue;
                            PackInfo PckInfo = new PackInfo();
                            PckInfo.GTIN = item.GTIN;
                            PckInfo.GTINCTI = item.GTINCTI;
                            PckInfo.Deck = item.PackageTypeCode.ParseToDeck();
                            PckInfo.Size = (int)item.Size;   //Convert.ToInt32(DGPacks[iCnt, (int)DGPacksEnum.PCMAPRw].Value);//  
                            PckInfo.BundleQty = (int)item.BundleQty; //Convert.ToInt32(DGPacks[iCnt, (int)DGPacksEnum.BUNDLINGRw].Value);  
                            PckInfo.MRP = item.MRP;
                            PckInfo.UIDsToPRint = 0;
                            LstpackInfo.Add(PckInfo);
                            iCnt++;
                            List<PackageLabelAsso> PLLst = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetPackageLabelDetails), m_PAId.ToString(), item.PackageTypeCode.ParseToDeck().ToString());
                            foreach (PackageLabelAsso item1 in PLLst)
                            {
                                PackageLabelAsso PckLinfo = new PackageLabelAsso();
                                PckLinfo.PAID = item1.PAID;
                                PckLinfo.JobTypeID = item1.JobTypeID;
                                PckLinfo.Code = item1.Code;
                                PckLinfo.LabelName = item1.LabelName;
                                PckLinfo.Filter = item1.Filter;
                                PckLinfo.LastUpdatedDate = item1.LastUpdatedDate;
                                LstPackLabelInfo.Add(PckLinfo);
                            }
                        }
                    }

                    RefreshQty(job.Quantity, (int)job.SurPlusQty, job);
                }
                else
                {
                    LstpackInfo = new List<PackInfo>();
                    LstPackLabelInfo = new List<PackageLabelAsso>(); //05Sept2015 Sunil
                    List<PackagingAssoDetails> PDLst = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(m_PAId);
                    int iCnt = 1;
                    foreach (PackagingAssoDetails item in PDLst)
                    {
                        if (item.PackageTypeCode.ParseToDeck().IsExists() == false)
                            continue;
                        if (IsProdLoadForJob == true && item.Size == -1)
                            continue;
                        PackInfo PckInfo = new PackInfo();
                        PckInfo.GTIN = item.GTIN;
                        PckInfo.GTINCTI = item.GTINCTI;
                        PckInfo.Deck = item.PackageTypeCode.ParseToDeck();
                        PckInfo.Size = (int)item.Size;   //Convert.ToInt32(DGPacks[iCnt, (int)DGPacksEnum.PCMAPRw].Value);//  
                        PckInfo.BundleQty = (int)item.BundleQty; //Convert.ToInt32(DGPacks[iCnt, (int)DGPacksEnum.BUNDLINGRw].Value);  
                        PckInfo.MRP = item.MRP;
                        PckInfo.UIDsToPRint = 0;
                        LstpackInfo.Add(PckInfo);
                        iCnt++;
                        List<PackageLabelAsso> PLLst = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetPackageLabelDetails), m_PAId.ToString(), item.PackageTypeCode.ParseToDeck().ToString());
                        foreach (PackageLabelAsso item1 in PLLst)
                        {
                            PackageLabelAsso PckLinfo = new PackageLabelAsso();
                            PckLinfo.PAID = item1.PAID;
                            PckLinfo.JobTypeID = item1.JobTypeID;
                            PckLinfo.Code = item1.Code;
                            PckLinfo.LabelName = item1.LabelName;
                            PckLinfo.Filter = item1.Filter;
                            PckLinfo.LastUpdatedDate = item1.LastUpdatedDate;
                            LstPackLabelInfo.Add(PckLinfo);
                        }
                    }
                    RefreshQty(m_BatchQty, m_SurplusQty);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return LstpackInfo;
        }

        public void RefreshQty(int BatchQty, int SurplusQty)
        {
            try
            {
                m_BatchQty = BatchQty;
                m_SurplusQty = SurplusQty;
                int m_TotalQty = BatchQty + SurplusQty;

                if (LstpackInfo == null)
                    return;
                //else
                //{
                //    // UPDATED FOR NON PROPER ENTRIS IN JOBDETAILS TABLE [07.10.2016]
                //    LstpackInfo.Clear();
                //    foreach (Jobdeck jdec in Jobdeck.jDecks)
                //    {
                //        PackInfo item = LstpackInfo.Find(item1 => item1.Deck == jdec.deck);
                //        LstpackInfo.Add(item);
                //    }
                //}
                for (int i = 0; i < LstpackInfo.Count; i++)
                {
                    int AcceptedQnt = dbhelper.DBManager.PackagingDetailsBLL.GetGoodCountOfJob(m_jobId, LstpackInfo[i].Deck.ToString());
                    int BadQnt = dbhelper.DBManager.PackagingDetailsBLL.GetBadCountOfJob(m_jobId, LstpackInfo[i].Deck.ToString());
                    BatchQty = GetPerValue(BatchQty, LstpackInfo[i].Size);
                    m_TotalQty = (int)(GetPerValue(m_TotalQty, LstpackInfo[i].Size));
                    int TPUID = m_TotalQty - AcceptedQnt > 0 ? m_TotalQty - AcceptedQnt : 0;
                    int TotalUIdsForPrint = m_TotalQty - (AcceptedQnt + BadQnt) > 0 ? m_TotalQty - (AcceptedQnt + BadQnt) : 0;

                  
                    LstpackInfo[i].BatchQty = BatchQty;
                    LstpackInfo[i].TotalQty = m_TotalQty;
                    if (LstpackInfo[i].Deck.IsFirstDeck() == false)
                    {
                        TPUID += Globals.AppSettings.LooseShiperCnt;
                        m_TotalQty += Globals.AppSettings.LooseShiperCnt;
                        BatchQty += Globals.AppSettings.LooseShiperCnt;
                        LstpackInfo[i].BatchQty = BatchQty;
                        LstpackInfo[i].TotalQty = m_TotalQty;
                        LstpackInfo[i].RemainigUIDsToPrint = TotalUIdsForPrint + Globals.AppSettings.LooseShiperCnt; //[09.01.2015]
                        LstpackInfo[i].UIDsToPRint = TPUID;
                    }
                    else
                    {
                        LstpackInfo[i].RemainigUIDsToPrint = TotalUIdsForPrint; //[09.01.2015]
                        LstpackInfo[i].UIDsToPRint = TPUID;
                    }


                    //int ColumnIndex = GetColumnIndex(LstpackInfo[i].Deck);
                    //if (ColumnIndex > -1)
                    //{
                    //    DGPacks[ColumnIndex, (int)DGPacksEnum.BatchQtyRw].Value = BatchQty.ToString();
                    //    DGPacks[ColumnIndex, (int)DGPacksEnum.UIDsToPRINTRw].Value = TPUID.ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1},{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }


        public void RefreshQty(int BatchQty, int SurplusQty, Job JobInfo)
        {
            try
            {
                m_BatchQty = BatchQty;
                m_SurplusQty = SurplusQty;
                int m_TotalQty = BatchQty + SurplusQty;
              

                if (LstpackInfo == null)
                    return;
              
                for (int i = 0; i < LstpackInfo.Count; i++)
                {
                    
                    int AcceptedQnt = dbhelper.DBManager.PackagingDetailsBLL.GetGoodCountOfJob(m_jobId, LstpackInfo[i].Deck.ToString());
                    m_BatchQty = GetPerValue(m_BatchQty, LstpackInfo[i].Size);
                    m_TotalQty = (int)(GetPerValue(m_TotalQty, LstpackInfo[i].Size));
                    int TPUID = 0;
                  
                    if (JobInfo.AutomaticBatchCloser == true && BatchQty == AcceptedQnt)
                        TPUID = 0;
                    else
                        TPUID = m_TotalQty - AcceptedQnt > 0 ? m_TotalQty - AcceptedQnt : 0;


                    if (LstpackInfo[i].Deck.IsFirstDeck() == false)
                    {
                        TPUID += Globals.AppSettings.LooseShiperCnt;
                        m_TotalQty += Globals.AppSettings.LooseShiperCnt;
                        m_BatchQty += Globals.AppSettings.LooseShiperCnt;
                      
                    }
                    LstpackInfo[i].BatchQty = m_BatchQty;
                    LstpackInfo[i].TotalQty = m_TotalQty;
                    LstpackInfo[i].UIDsToPRint = TPUID;
                    //int ColumnIndex = GetColumnIndex(LstpackInfo[i].Deck);
                    //if (ColumnIndex > -1)
                    //{
                    //    DGPacks[ColumnIndex, (int)DGPacksEnum.BatchQtyRw].Value = m_BatchQty.ToString();
                    //    DGPacks[ColumnIndex, (int)DGPacksEnum.UIDsToPRINTRw].Value = TPUID.ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }


        private int GetPerValue(int totalQty, int PackQty)
        {
            if (PackQty > 0)
                return (int)(totalQty % PackQty) == 0 ? (int)(totalQty / PackQty) : (int)(totalQty / PackQty) + 1;
            else
                return totalQty;
        }



        public List<PackInfo> GetPackInfoLst()
        {
            return LstpackInfo;
        }

    }
}