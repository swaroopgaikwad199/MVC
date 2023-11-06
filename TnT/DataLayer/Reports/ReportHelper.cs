using TnT.Models.Reports.Jobs;
using REDTR.DB.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Code;
using Westwind.Web.Mvc;
using REDTR.JOB;
using REDTR.HELPER;
using REDTR.UTILS.SystemIntegrity;
using PTPLCRYPTORENGINE;
using PTPL.Resources.Properties;
using TnT.Models.Reports;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Xml.Linq;

namespace TnT.DataLayer.Reports
{
    public class ReportHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();


        string CompanyName = string.Empty;
        string CompanyAddrs = string.Empty;
        string UserName = string.Empty;
        public ReportHelper()
        {
            CompanyName = db.Settings.Select(s => s.CompanyName).FirstOrDefault();
            CompanyAddrs = db.Settings.Select(s => s.Address).FirstOrDefault();

        }

        #region Rpt Summary

        public string getSummary(DateTime frmDt, DateTime toDt, string lvl, string username)
        {
            //bool IsFirstLevel = false;
            //List<string> availableLevels = new List<string>();
            //REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
            //RptProductWiseViewModel vm = new RptProductWiseViewModel();
            RptSummaryViewModel vm = new RptSummaryViewModel();
            //List<JobSummary> jobDtls = new List<JobSummary>();
            //var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            //if (Jobs.Count() > 0)
            //{
            //    bool has;
            //    vm.CompanyName = CompanyName;
            //    vm.Address = CompanyAddrs;
            //    vm.Username=username;
            //    foreach (var jb in Jobs)
            //    {
            //        var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == jb.PAID);
            //        if (lvl != "NA")
            //        {

            //            has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
            //        }
            //        else
            //        {
            //            has = true;
            //        }
            //        if (has == true)
            //        {
            //            JobSummary jobsum = new JobSummary();
            //            jobsum.Job = jb;
            //            jobsum.product = db.PackagingAsso.Find(jb.PAID);
            //            jobsum.GTINtertiary = ProductPackageHelper.getBottomDeck(jb.PAID);

            //            if (lvl == "NA")
            //            {
            //                IsFirstLevel = true;
            //                lvl = ProductPackageHelper.getTopDeck(jb.PAID);
            //            }

            //            if (lvl == "MOC")
            //            {
            //                IsFirstLevel = true;
            //            }
            //            jobsum.DECK = lvl;
            //            jobsum.GTIN = ProductPackageHelper.getGTIN(jb.PAID, lvl);

            //            DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, jb.JID.ToString(), lvl, null, null, null, false, -1);

            //            if (DS.Tables[0].Rows.Count > 0)
            //            {
            //                int badCountV = Convert.ToInt32(DS.Tables[0].Rows[0][2]);
            //                jobsum.GoodCount = badCountV;// + (int)jb.NoReadCount;
            //                if (IsFirstLevel == true)
            //                {
            //                    jobsum.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + Convert.ToInt32(DS.Tables[0].Rows[0][6]);
            //                }
            //                else
            //                {
            //                    jobsum.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]);
            //                }
            //                jobsum.decommisionedCount = Convert.ToInt32(DS.Tables[0].Rows[0][4].ToString());
            //                jobsum.notVerified = Convert.ToInt32(DS.Tables[0].Rows[0][5].ToString());
            //                jobsum.Total = 0;
            //            }
            //            else
            //            {
            //                jobsum.BadCount = 0;
            //                jobsum.GoodCount = 0;
            //                jobsum.decommisionedCount = 0;
            //                jobsum.notVerified = 0;
            //                jobsum.Total = 0;
            //            }

            //            jobDtls.Add(jobsum);
            //            var lst = ProductPackageHelper.getAllDeck(jb.JID.ToString());
            //            availableLevels.AddRange(lst);
            //        }
            //    }
            //    vm.JobInfo = jobDtls;
            //    availableLevels = availableLevels.Distinct().ToList();
            //    availableLevels.Insert(0, "Select");
            //    vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            vm = getSummaryPDF(frmDt, toDt, lvl, username);
            if (vm == null)
            {
                //var data =TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                var data = "No Data";
                return data;
            }
            else
            {
                var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptSummary.cshtml", vm);
                return data;
            }
            //}
            //else
            //{
            //    return "No Data";
            //}
        }
        public RptSummaryViewModel getSummaryPDF(DateTime frmDt, DateTime toDt, string lvl, string username)
        {
            bool IsFirstLevel = false;
            List<string> availableLevels = new List<string>();
            REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
            //RptProductWiseViewModel vm = new RptProductWiseViewModel();
            RptSummaryViewModel vm = new RptSummaryViewModel();
            List<JobSummary> jobDtls = new List<JobSummary>();
            var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            if (Jobs.Count() > 0)
            {
                bool has;
                vm.CompanyName = CompanyName;
                vm.Address = CompanyAddrs;
                vm.Username = username;
                foreach (var jb in Jobs)
                {
                    var jbdetail = db.JobDetails.Where(x => x.JD_JobID == jb.JID);
                    if (lvl != "NA")
                    {

                        has = jbdetail.Any(x => x.JD_Deckcode == lvl);
                    }
                    else
                    {
                        has = true;
                    }
                    if (has == true)
                    {
                        JobSummary jobsum = new JobSummary();
                        jobsum.Job = jb;
                        jobsum.product = db.PackagingAsso.Find(jb.PAID);

                        jobsum.GTINtertiary = ProductPackageHelper.getBottomDeckJB(jb.JID);

                        if (lvl == "NA")
                        {
                            IsFirstLevel = true;
                            lvl = ProductPackageHelper.getTopDeckJB(jb.JID);
                        }

                        if (lvl == "MOC")
                        {
                            IsFirstLevel = true;
                        }
                        jobsum.jb = db.JobDetails.Where(x => x.JD_JobID == jb.JID && x.JD_Deckcode == lvl).FirstOrDefault();
                        jobsum.DECK = lvl;
                        jobsum.GTIN = ProductPackageHelper.getGTINJb(jb.JID, lvl);

                        DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, jb.JID.ToString(), lvl, null, null, null, false, -1);

                        if (DS.Tables[0].Rows.Count > 0)
                        {
                            int badCountV = Convert.ToInt32(DS.Tables[0].Rows[0][2]);
                            jobsum.GoodCount = badCountV;// + (int)jb.NoReadCount;
                            if (IsFirstLevel == true)
                            {
                                jobsum.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + Convert.ToInt32(DS.Tables[0].Rows[0][6]);
                            }
                            else
                            {
                                jobsum.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]);
                            }
                            jobsum.decommisionedCount = Convert.ToInt32(DS.Tables[0].Rows[0][4].ToString());
                            jobsum.notVerified = Convert.ToInt32(DS.Tables[0].Rows[0][5].ToString());
                            jobsum.Total = 0;
                        }
                        else
                        {
                            jobsum.BadCount = 0;
                            jobsum.GoodCount = 0;
                            jobsum.decommisionedCount = 0;
                            jobsum.notVerified = 0;
                            jobsum.Total = 0;
                        }

                        jobDtls.Add(jobsum);
                        var lst = ProductPackageHelper.getAllDeck(jb.JID.ToString());
                        availableLevels.AddRange(lst);
                    }
                }
                vm.JobInfo = jobDtls;
                availableLevels = availableLevels.Distinct().ToList();
                availableLevels.Insert(0, "Select");
                vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });

                //var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptSummary.cshtml", vm);
                return vm;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Rpt Details with Operator

        DataSet DSRptData;


        public string getDetailsWithOprtr(DateTime frmDt, DateTime toDt, string lvl, string UserName)
        {
            string data = string.Empty;
            RptDtlsWithOptrViewModel vm = new RptDtlsWithOptrViewModel();
            vm = getDetailsWithOprtrpdf(frmDt, toDt, lvl, UserName);
            if (vm != null)
            {
                if (vm.JobOpretors.Count > 0)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDtlWithOptr.cshtml", vm);
                }
                else
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            return data;
            //var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            //if (Jobs.Count() > 0)
            //{

            //    List<string> availableLevels = new List<string>();
            //    availableLevels = ProductPackageHelper.getAllDecks(frmDt, toDt);
            //    if (lvl == "NA")
            //    {
            //        lvl = availableLevels.First();
            //    }
            //    DSRptData = getReportDs(JobBLL.ReportOp.ForOperatorDtls, lvl, null, frmDt, toDt);


            //    if (DSRptData.Tables[0].Rows.Count > 0)
            //    {
            //        RptDtlsWithOptrViewModel vm = new RptDtlsWithOptrViewModel();
            //        List<OptrBatchDetails> jobWrkDtls = new List<OptrBatchDetails>();
            //        vm.CompanyName = CompanyName;
            //        vm.Address = CompanyAddrs;
            //        vm.UserName = UserName;
            //        bool has;
            //        var jobIds = DSRptData.Tables[0].AsEnumerable().Select(row => new { JobId = row.Field<decimal>("JID") }).Distinct();
            //        foreach (var item in jobIds)
            //        {
            //            var id = Convert.ToDecimal(item.JobId);
            //            var pkgAssoD = from j1 in db.Job join pk in db.PackagingAssoDetails on j1.PAID equals pk.PAID where j1.JID == id select new Packagetpe { phk = pk.PackageTypeCode };
            //            //var Job = db.Job.Find(id);
            //            //var Product = db.PackagingAsso.Find(Job.PAID);
            //            //var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == Job.PAID);
            //            if (lvl != "NA")
            //            {
            //                // has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
            //                has = pkgAssoD.Any(x => x.phk == lvl);
            //            }
            //            else
            //            {
            //                has = true;
            //            }
            //            if (has == true)
            //            {
            //                OptrBatchDetails obj = getDetailsofJobNOprtr(id);
            //                jobWrkDtls.Add(obj);
            //            }
            //        }
            //        vm.JobOpretors = jobWrkDtls;
            //        if (jobWrkDtls.Count > 0)
            //        {
            //            vm.Packinglevel = lvl;
            //            availableLevels.Insert(0, "Select");
            //            vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
            //            //vm.Packaginlevels.Where(x => x.Value == "0").First();
            //            var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDtlWithOptr.cshtml", vm);
            //            return data;
            //        }
            //        else
            //        {
            //            return "No Data";
            //        }
            //    }
            //    else
            //    {
            //        return "No Data";
            //    }
            //}
            //else
            //{
            //    return "No Data";
            //}
        }

        public RptDtlsWithOptrViewModel getDetailsWithOprtrpdf(DateTime frmDt, DateTime toDt, string lvl, string UserName)
        {

            var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            if (Jobs.Count() > 0)
            {

                List<string> availableLevels = new List<string>();
                availableLevels = ProductPackageHelper.getAllDecks(frmDt, toDt);
                if (lvl == "NA")
                {
                    lvl = availableLevels.FirstOrDefault();
                }
                DSRptData = getReportDs(JobBLL.ReportOp.ForOperatorDtls, lvl, null, frmDt, toDt);


                if (DSRptData.Tables[0].Rows.Count > 0)
                {
                    RptDtlsWithOptrViewModel vm = new RptDtlsWithOptrViewModel();
                    List<OptrBatchDetails> jobWrkDtls = new List<OptrBatchDetails>();
                    vm.CompanyName = CompanyName;
                    vm.Address = CompanyAddrs;
                    vm.UserName = UserName;
                    bool has;
                    var jobIds = DSRptData.Tables[0].AsEnumerable().Select(row => new { JobId = row.Field<decimal>("JID") }).Distinct();
                    foreach (var item in jobIds)
                    {
                        var id = Convert.ToDecimal(item.JobId);
                        var pkgAssoD = from j1 in db.Job join pk in db.PackagingAssoDetails on j1.PAID equals pk.PAID where j1.JID == id select new Packagetpe { phk = pk.PackageTypeCode };
                        //var Job = db.Job.Find(id);
                        //var Product = db.PackagingAsso.Find(Job.PAID);
                        //var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == Job.PAID);
                        if (lvl != "NA")
                        {
                            // has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
                            has = pkgAssoD.Any(x => x.phk == lvl);
                        }
                        else
                        {
                            has = true;
                        }
                        if (has == true)
                        {
                            OptrBatchDetails obj = getDetailsofJobNOprtr(id);
                            jobWrkDtls.Add(obj);
                        }
                    }
                    vm.JobOpretors = jobWrkDtls;
                    if (jobWrkDtls.Count > 0)
                    {
                        vm.Packinglevel = lvl;
                        availableLevels.Insert(0, "Select");
                        vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
                        //vm.Packaginlevels.Where(x => x.Value == "0").First();
                        var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDtlWithOptr.cshtml", vm);
                        return vm;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private OptrBatchDetails getDetailsofJobNOprtr(decimal JID)
        {
            var Job = db.Job.Find(JID);
            var Product = db.PackagingAsso.Find(Job.PAID);
            var jbdetail = db.JobDetails.Where(x => x.JD_JobID == JID).FirstOrDefault();
            OptrBatchDetails obd = new OptrBatchDetails();
            obd.BatchNo = Job.BatchNo;
            obd.ProductCode = jbdetail.JD_ProdCode; //Product.ProductCode;
            obd.ProductName = jbdetail.JD_ProdName; //Product.Name;


            List<OperatorBatchWork> workers = new List<OperatorBatchWork>();
            var wdata = from myRow in DSRptData.Tables[0].AsEnumerable()
                        where myRow.Field<decimal>("JID") == JID
                        select myRow;

            if (wdata.Count() > 0)
            {
                foreach (DataRow item in wdata)
                {
                    OperatorBatchWork wrk = new OperatorBatchWork();
                    if (string.IsNullOrEmpty(item[1].ToString())) wrk.GoodCnt = 0; else wrk.GoodCnt = Convert.ToInt32(item[1]);
                    if (string.IsNullOrEmpty(item[2].ToString())) wrk.BadCnt = 0; else wrk.BadCnt = Convert.ToInt32(item[2]);
                    if (string.IsNullOrEmpty(item[6].ToString())) wrk.OperatorName = "Not available"; else wrk.OperatorName = item[6].ToString();
                    if (string.IsNullOrEmpty(item[8].ToString())) wrk.NoRead = 0; else wrk.NoRead = Convert.ToInt32(item[8]);
                    workers.Add(wrk);
                }
            }
            obd.OperatorWorkings = workers;
            return obd;
        }

        private DataSet getReportDs(JobBLL.ReportOp op, string Value1, string Value2, DateTime frmDt, DateTime toDt)
        {
            DataSet ds = ObjHelper.DBManager.JobBLL.GetReportDataSet(op, Value1, Value2, 1, frmDt.Date, toDt.Date, false, -1);
            return ds;
        }

        #endregion

        #region Operator Statistics

        public string getOprtrStats(DateTime frmDt, DateTime toDt, string lvl, string UserName)
        {
            string data = string.Empty;
            RptOptrStatsViewModel vm = new RptOptrStatsViewModel();
            vm = getOprtrStatsPdf(frmDt, toDt, lvl, UserName);
            if (vm != null)
            {
                if (vm.OpretorStats.Count > 0)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptOperatorStats.cshtml", vm);
                }
                else
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            return data;
            //var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            //if (Jobs.Count() > 0)
            //{

            //    List<string> availableLevels = new List<string>();
            //    availableLevels = ProductPackageHelper.getAllDecks(frmDt, toDt);
            //    if (lvl == "NA")
            //    {
            //        lvl = availableLevels.First();
            //    }
            //    DSRptData = getReportDs(JobBLL.ReportOp.ForOperatorDtls, lvl, null, frmDt, toDt);


            //    if (DSRptData.Tables[0].Rows.Count > 0)
            //    {
            //        RptOptrStatsViewModel vm = new RptOptrStatsViewModel();
            //        List<OperatorStats> oprtrSts = new List<OperatorStats>();

            //        vm.CompanyName = CompanyName;
            //        vm.Address = CompanyAddrs;
            //        vm.UserName = UserName;
            //        List<BatchStats> batchSts = new List<BatchStats>();
            //        var Operators = DSRptData.Tables[0].AsEnumerable().Select(row => new { OId = row.Field<decimal>("OperatorId") }).Distinct();
            //        bool has;

            //        foreach (var item in Operators)
            //        {
            //            var jobIds = DSRptData.Tables[0].AsEnumerable().Where(row => row.Field<decimal>("OperatorId") == item.OId).Select(row => new { JobId = row.Field<decimal>("JID") }).Distinct();
            //            OperatorStats oprtSts = new OperatorStats();
            //            var Nm = DSRptData.Tables[0].AsEnumerable().Where(row => row.Field<decimal>("OperatorId") == item.OId).Select(row => new { Name = row.Field<string>("OperatorName") }).First();
            //            oprtSts.OperatorName = Nm.Name;

            //            foreach (var jb in jobIds)
            //            {
            //                var pkgAssoD = from j1 in db.Job join pk in db.PackagingAssoDetails on j1.PAID equals pk.PAID where j1.JID == jb.JobId select new Packagetpe { phk = pk.PackageTypeCode };
            //                //var Job = db.Job.Find(jb.JobId);
            //                //var Product = db.PackagingAsso.Find(Job.PAID);
            //                //var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == Job.PAID);
            //                if (lvl != "NA")
            //                {
            //                    //has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
            //                    has = pkgAssoD.Any(x => x.phk == lvl);
            //                }
            //                else
            //                {
            //                    has = true;
            //                }
            //                if (has == true)
            //                {
            //                    BatchStats btcSts = geOprtrBatches(jb.JobId, lvl, Nm.Name);
            //                    batchSts.Add(btcSts);
            //                }
            //            }
            //            oprtSts.BatchStats = batchSts;
            //            oprtrSts.Add(oprtSts);
            //        }


            //        vm.OpretorStats = oprtrSts;

            //        vm.Packinglevel = lvl;
            //        availableLevels.Insert(0, "Select");
            //        vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            //        var data = "";
            //        if (batchSts.Count > 0)
            //        {
            //            data = ViewRenderer.RenderPartialView("~/Views/Reports/RptOperatorStats.cshtml", vm);
            //        }
            //        else
            //        {
            //            data = "No Data";
            //        }
            //        return data;
            //    }
            //    else
            //    {
            //        return "No Data";
            //    }
            //}
            //else
            //{
            //    return "No Data";
            //}
        }

        public RptOptrStatsViewModel getOprtrStatsPdf(DateTime frmDt, DateTime toDt, string lvl, string UserName)
        {

            var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            if (Jobs.Count() > 0)
            {

                List<string> availableLevels = new List<string>();
                availableLevels = ProductPackageHelper.getAllDecks(frmDt, toDt);
                if (lvl == "NA")
                {
                    lvl = availableLevels.FirstOrDefault();
                }
                DSRptData = getReportDs(JobBLL.ReportOp.ForOperatorDtls, lvl, null, frmDt, toDt);


                if (DSRptData.Tables[0].Rows.Count > 0)
                {
                    RptOptrStatsViewModel vm = new RptOptrStatsViewModel();
                    List<OperatorStats> oprtrSts = new List<OperatorStats>();

                    vm.CompanyName = CompanyName;
                    vm.Address = CompanyAddrs;
                    vm.UserName = UserName;
                    List<BatchStats> batchSts = new List<BatchStats>();
                    var Operators = DSRptData.Tables[0].AsEnumerable().Select(row => new { OId = row.Field<decimal>("OperatorId") }).Distinct();
                    bool has;

                    foreach (var item in Operators)
                    {
                        var jobIds = DSRptData.Tables[0].AsEnumerable().Where(row => row.Field<decimal>("OperatorId") == item.OId).Select(row => new { JobId = row.Field<decimal>("JID") }).Distinct();
                        OperatorStats oprtSts = new OperatorStats();
                        var Nm = DSRptData.Tables[0].AsEnumerable().Where(row => row.Field<decimal>("OperatorId") == item.OId).Select(row => new { Name = row.Field<string>("OperatorName") }).FirstOrDefault();
                        oprtSts.OperatorName = Nm.Name;

                        foreach (var jb in jobIds)
                        {
                            var pkgAssoD = from j1 in db.Job join pk in db.PackagingAssoDetails on j1.PAID equals pk.PAID where j1.JID == jb.JobId select new Packagetpe { phk = pk.PackageTypeCode };
                            //var Job = db.Job.Find(jb.JobId);
                            //var Product = db.PackagingAsso.Find(Job.PAID);
                            //var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == Job.PAID);
                            if (lvl != "NA")
                            {
                                //has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
                                has = pkgAssoD.Any(x => x.phk == lvl);
                            }
                            else
                            {
                                has = true;
                            }
                            if (has == true)
                            {
                                BatchStats btcSts = geOprtrBatches(jb.JobId, lvl, Nm.Name);
                                batchSts.Add(btcSts);
                            }
                        }
                        oprtSts.BatchStats = batchSts;
                        oprtrSts.Add(oprtSts);
                    }


                    vm.OpretorStats = oprtrSts;

                    vm.Packinglevel = lvl;
                    availableLevels.Insert(0, "Select");
                    vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
                    var data = "";
                    if (batchSts.Count > 0)
                    {
                        return vm;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private BatchStats geOprtrBatches(decimal JID, string Decklevel, string oprname)
        {
            var Job = db.Job.Find(JID);
            var Product = db.PackagingAsso.Find(Job.PAID);
            var jb = db.JobDetails.Where(x => x.JD_JobID == JID).FirstOrDefault();
            BatchStats obd = new BatchStats();
            obd.BatchNo = Job.BatchNo;
            obd.ProductCode = jb.JD_ProdCode; //Product.ProductCode;
            obd.ProductName = jb.JD_ProdName; //Product.Name;
            obd.oprName = oprname;
            List<OperatorBatchWork> workers = new List<OperatorBatchWork>();
            var data = from myRow in DSRptData.Tables[0].AsEnumerable()
                       where (myRow.Field<decimal>("JID") == JID && myRow.Field<string>("OperatorName") == oprname)
                       select myRow;
            //DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, JID.ToString(), Decklevel, null, null, null, false, -1);
            //if (DS.Tables[0].Rows.Count > 0)
            //{
            //    if (string.IsNullOrEmpty(DS.Tables[0].Rows[0][3].ToString())) obd.GoodCnt = 0; else obd.GoodCnt = Convert.ToInt32(DS.Tables[0].Rows[0][3].ToString());
            //    if (string.IsNullOrEmpty(DS.Tables[0].Rows[0][2].ToString())) obd.BadCnt = 0; else obd.BadCnt = Convert.ToInt32(DS.Tables[0].Rows[0][2].ToString());
            //}
            if (data.Count() > 0)
            {
                foreach (DataRow item in data)
                {
                    if (string.IsNullOrEmpty(item[1].ToString())) obd.GoodCnt = 0; else obd.GoodCnt = Convert.ToInt32(item[2].ToString());
                    if (string.IsNullOrEmpty(item[2].ToString())) obd.BadCnt = 0; else obd.BadCnt = Convert.ToInt32(item[1].ToString());
                    if (string.IsNullOrEmpty(item[8].ToString())) obd.NoRead = 0; else obd.NoRead = Convert.ToInt32(item[8]);
                }
            }

            return obd;
        }


        #endregion

        #region Rpt Detailed Jobs
        public string getDetails(DateTime frmDt, DateTime toDt, string UserName)
        {
            string data = string.Empty;
            DbHelper m_dbhelper = new DbHelper();
            try
            {
                RptDetailViewModel vm = new RptDetailViewModel();
                //                var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();

                //                if (Jobs.Count() > 0)
                //                {
                //                    vm.CompanyName = CompanyName;
                //                    vm.Address = CompanyAddrs;
                //                    vm.UserName = UserName;
                //                    List<JobInfoDetails> jidLst = new List<JobInfoDetails>();
                //                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];

                //                    Query = "select j.JobName BatchNo,p.Name ProductName,AutobatchClose=case j.AutomaticBatchCloser when 1 then 'True' else 'False' end,j.MfgDate MfgDate,j.ExpDate ExpDate,j.Quantity Qty,u.UserName CreatedBy,ur.UserName VerifiedBy,"
                //+ "Status =case j.JobStatus"
                //+ " when 0 then 'Created'"
                //+ "when 1 then 'Running'"
                //+ "when 2 then 'Paused'"
                //+ "when 3 then 'Closed'"
                //+ "when 4 then 'Decommissioned'"
                //+ "when 5 then 'LineClearance'"
                //+ "when 6 then 'CompleteTransfer'"
                //+ "when 7 then 'ForcefullyBatchClose' end from job j inner join PackagingAsso p on j.PAID = p.PAID full outer join Users u on u.ID = j.CreatedBy full outer join users ur on ur.ID = j.VerifiedBy where j.CreatedDate >=CONVERT(datetime, '" + frmDt + "',103) and j.CreatedDate <= CONVERT(datetime,'" + toDt + "',103)";

                //                    DataSet ds = m_dbhelper.GetDataSet(Query);
                //                    var lst = ds.Tables[0].AsEnumerable().Select(dataRow => new JobInfoDetails { ProductName = dataRow.Field<string>("ProductName"), Btachno = dataRow.Field<string>("BatchNo"), AutoBatchClose = dataRow.Field<string>("AutobatchClose"), MfgDate = dataRow.Field<DateTime>("MfgDate"), ExpDate = dataRow.Field<DateTime>("ExpDate"), Qty = dataRow.Field<int>("Qty"), CreatedBy = dataRow.Field<string>("CreatedBy"), VerifiedBy = dataRow.Field<string>("VerifiedBy"), Status = dataRow.Field<string>("Status") }).ToList();

                //                    vm.Jobs = lst;
                //                    var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetails.cshtml", vm);
                //                    Trace.Write("This is an action in my page.PAUL");
                //                    Trace.TraceInformation("Successfully sended");
                //                    Trace.TraceError("Successfully  Error tested ");
                //                    Trace.TraceWarning("Successfully Warning tested ");
                //                    return data;
                //                }
                //                else
                //                {
                //                    return "No Data";
                //                }
                vm = getDetailsPdf(frmDt, toDt, UserName);
                if (vm.CompanyName != null)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetails.cshtml", vm);
                }
                else
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
                return data;
            }
            catch (Exception ex)
            {
                Trace.Write(ex.StackTrace);
                // return (TnT.LangResource.GlobalRes.toastrLblLytDsgNoData);
                return "No Data";
            }

        }

        public RptDetailViewModel getDetailsPdf(DateTime frmDt, DateTime toDt, string UserName)
        {
            string Query = string.Empty;
            DbHelper m_dbhelper = new DbHelper();
            try
            {
                RptDetailViewModel vm = new RptDetailViewModel();
                var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();

                if (Jobs.Count() > 0)
                {
                    vm.CompanyName = CompanyName;
                    vm.Address = CompanyAddrs;
                    vm.UserName = UserName;
                    List<JobInfoDetails> jidLst = new List<JobInfoDetails>();
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];

                    Query = "select distinct(j.BatchNo) as BatchNo,jb.JD_ProdName ProductName,AutobatchClose=case j.AutomaticBatchCloser when 1 then 'True' else 'False' end,j.MfgDate MfgDate,j.ExpDate ExpDate,j.Quantity Qty,u.UserName CreatedBy,ur.UserName VerifiedBy,"
+ "Status =case j.JobStatus"
+ " when 0 then 'Created'"
+ "when 1 then 'Running'"
+ "when 2 then 'Paused'"
+ "when 3 then 'Closed'"
+ "when 4 then 'Decommissioned'"
+ "when 5 then 'LineClearance'"
+ "when 6 then 'CompleteTransfer'"
+ "when 7 then 'ForcefullyBatchClose' end from job j inner join PackagingAsso p on j.PAID = p.PAID full outer join Users u on u.ID = j.CreatedBy full outer join users ur on ur.ID = j.VerifiedBy inner join JobDetails jb on j.JID=jb.JD_JobID  where j.CreatedDate >=CONVERT(datetime, '" + frmDt + "',103) and j.CreatedDate <= CONVERT(datetime,'" + toDt + "',103)";

                    DataSet ds = m_dbhelper.GetDataSet(Query);
                    var lst = ds.Tables[0].AsEnumerable().Select(dataRow => new JobInfoDetails { ProductName = dataRow.Field<string>("ProductName"), Btachno = dataRow.Field<string>("BatchNo"), AutoBatchClose = dataRow.Field<string>("AutobatchClose"), MfgDate = dataRow.Field<DateTime>("MfgDate"), ExpDate = dataRow.Field<DateTime>("ExpDate"), Qty = dataRow.Field<int>("Qty"), CreatedBy = dataRow.Field<string>("CreatedBy"), VerifiedBy = dataRow.Field<string>("VerifiedBy"), Status = dataRow.Field<string>("Status") }).ToList();

                    vm.Jobs = lst;
                    var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetails.cshtml", vm);
                    Trace.Write("This is an action in my page.PAUL");
                    Trace.TraceInformation(TnT.LangResource.GlobalRes.TraceWarnSended);
                    Trace.TraceError(TnT.LangResource.GlobalRes.TraceWarnErrorTested);
                    Trace.TraceWarning(TnT.LangResource.GlobalRes.TraceWarningTested);
                    return vm;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.StackTrace);
                return null;
            }

        }
        #endregion

        #region Rpt Product Wise

        public string getProductWise(DateTime frmDt, DateTime toDt, string lvl, string Username)
        {
            string data = string.Empty;
            //bool IsFirstLvl = false;
            //List<string> availableLevels = new List<string>();
            //REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
            RptProductWiseViewModel vm = new RptProductWiseViewModel();


            //var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();

            //if (Jobs.Count() > 0)
            //{
            //    vm.CompanyName = CompanyName;
            //    vm.Address = CompanyAddrs;
            //    vm.UserName = Username;
            //    bool has;
            //    List<decimal> prds = new List<decimal>();
            //    //foreach (var item in Jobs)
            //    //{
            //    //    prds.Add(item.PAID);
            //    //}
            //    prds = Jobs.Select(x => x.PAID).Distinct().ToList();
            //    //prds = prds.Distinct().ToList();
            //    List<ProductInfoStats> prodInfoStats = new List<ProductInfoStats>();
            //    foreach (var paid in prds)
            //    {
            //        var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == paid);
            //        if (lvl != "NA")
            //        {
            //            has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
            //        }
            //        else
            //        {
            //            has = true;
            //        }
            //        if (has == true)
            //        {
            //            ProductInfoStats prod = new ProductInfoStats();

            //            prod.product = db.PackagingAsso.Find(paid);
            //            prod.GTINtertiary = ProductPackageHelper.getBottomDeck(paid);
            //            if (lvl == "NA")
            //            {
            //                IsFirstLvl = true;
            //                lvl = ProductPackageHelper.getTopDeck(paid);
            //            }

            //            if (lvl == "MOC")
            //            {
            //                IsFirstLvl = true;
            //            }

            //            prod.GTIN = ProductPackageHelper.getGTIN(paid, lvl);
            //            prod.DECK = lvl;

            //            List<JobInfoStats> jobInfoSts = new List<JobInfoStats>();
            //            var job = Jobs.Where(x => x.PAID == paid).ToList();
            //            for (int i = 0; i < job.Count; i++)
            //            {
            //                //if (jb.PAID == paid)
            //                //{
            //                DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, job[i].JID.ToString(), lvl, null, null, null, false, -1);
            //                JobInfoStats ji = new JobInfoStats();
            //                ji.Job = job[i];
            //                if (DS.Tables[0].Rows.Count > 0)
            //                {
            //                    if (IsFirstLvl)
            //                    {
            //                        ji.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3].ToString()) + Convert.ToInt32(job[i].NoReadCount);
            //                    }
            //                    else
            //                    {
            //                        ji.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3].ToString());
            //                    }

            //                    ji.GoodCount = Convert.ToInt32(DS.Tables[0].Rows[0][2].ToString());
            //                    ji.decommisionedCount = Convert.ToInt32(DS.Tables[0].Rows[0][4].ToString());
            //                    ji.notVerified = Convert.ToInt32(DS.Tables[0].Rows[0][5].ToString());
            //                    ji.Total = 0;
            //                }
            //                else
            //                {
            //                    ji.BadCount = 0;
            //                    ji.GoodCount = 0;
            //                    ji.decommisionedCount = 0;
            //                    ji.notVerified = 0;
            //                    ji.Total = 0;
            //                }
            //                jobInfoSts.Add(ji);
            //                var lst = ProductPackageHelper.getAllDeck(job[i].JID.ToString());
            //                availableLevels.AddRange(lst);

            //                //  }
            //            }
            //            prod.Jobs = jobInfoSts;
            //            prodInfoStats.Add(prod);
            //        }
            //    }
            //    vm.Products = prodInfoStats;
            //    availableLevels = availableLevels.Distinct().ToList();
            //    availableLevels.Insert(0, "Select");
            //    vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            vm = getProductWisePDF(frmDt, toDt, lvl, Username);
            if (vm == null)
            {
                // data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            else
            {
                if (vm.Products != null)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptProductWise.cshtml", vm);
                }
                else
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            return data;
            //}
            //else
            //{
            //    return "No Data";
            //}
        }

        public RptProductWiseViewModel getProductWisePDF(DateTime frmDt, DateTime toDt, string lvl, string Username)
        {
            bool IsFirstLvl = false;
            List<string> availableLevels = new List<string>();
            REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
            RptProductWiseViewModel vm = new RptProductWiseViewModel();


            // var Jobs = db.Job.Where(j => j.CreatedDate >= frmDt && j.CreatedDate <= toDt).ToList();
            var products = (from jb in db.Job join pkg in db.PackagingAsso on jb.PAID equals pkg.PAID where jb.CreatedDate >= frmDt && jb.CreatedDate <= toDt select new { pkg.PAID, pkg.Name }).Distinct().ToList();

            if (products.Count() > 0)
            {
                vm.CompanyName = CompanyName;
                vm.Address = CompanyAddrs;
                vm.UserName = Username;
                bool has;
                List<decimal> prds = new List<decimal>();
                //foreach (var item in Jobs)
                //{
                //    prds.Add(item.PAID);
                //}
                //  prds = Jobs.Select(x => x.PAID).Distinct().ToList();
                //prds = prds.Distinct().ToList();
                List<ProductInfoStats> prodInfoStats = new List<ProductInfoStats>();
                foreach (var jb in products)
                {
                    var pkgAssoD = db.PackagingAssoDetails.Where(x => x.PAID == jb.PAID).OrderBy(x => x.Id);
                    if (lvl != "NA")
                    {
                        if (lvl != "")
                        {
                            has = pkgAssoD.Any(x => x.PackageTypeCode == lvl);
                        }
                        else
                        {
                            has = true;
                        }
                    }
                    else
                    {
                        has = true;
                    }
                    if (has == true)
                    {
                        ProductInfoStats prod = new ProductInfoStats();
                        prod.product = db.PackagingAsso.Where(x => x.PAID == jb.PAID).FirstOrDefault();
                        prod.jb = db.PackagingAssoDetails.Where(x => x.PAID == jb.PAID).FirstOrDefault();
                        prod.GTINtertiary = ProductPackageHelper.getBottomDeck(jb.PAID);
                        if (lvl == "NA")
                        {
                            IsFirstLvl = true;
                            lvl = ProductPackageHelper.getTopDeck(jb.PAID);
                        }

                        if (lvl == "MOC")
                        {
                            IsFirstLvl = true;
                        }

                        prod.GTIN = ProductPackageHelper.getGTIN(jb.PAID, lvl);
                        prod.DECK = lvl;

                        List<JobInfoStats> jobInfoSts = new List<JobInfoStats>();
                        var job = db.Job.Where(x => x.PAID == jb.PAID && x.CreatedDate >= frmDt && x.CreatedDate <= toDt).ToList();
                        prod.totalbatchcnt = Convert.ToString(job.Count);
                        for (int i = 0; i < job.Count; i++)
                        {
                            //if (jb.PAID == paid)
                            //{
                            DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, job[i].JID.ToString(), lvl, null, null, null, false, -1);
                            JobInfoStats ji = new JobInfoStats();
                            ji.Job = job[i];
                            if (DS.Tables[0].Rows.Count > 0)
                            {
                                if (IsFirstLvl)
                                {
                                    ji.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3].ToString()) + Convert.ToInt32(job[i].NoReadCount);
                                }
                                else
                                {
                                    ji.BadCount = Convert.ToInt32(DS.Tables[0].Rows[0][3].ToString());
                                }

                                ji.GoodCount = Convert.ToInt32(DS.Tables[0].Rows[0][2].ToString());
                                ji.decommisionedCount = Convert.ToInt32(DS.Tables[0].Rows[0][4].ToString());
                                ji.notVerified = Convert.ToInt32(DS.Tables[0].Rows[0][5].ToString());
                                ji.Total = 0;
                            }
                            else
                            {
                                ji.BadCount = 0;
                                ji.GoodCount = 0;
                                ji.decommisionedCount = 0;
                                ji.notVerified = 0;
                                ji.Total = 0;
                            }
                            jobInfoSts.Add(ji);
                            var lst = ProductPackageHelper.getAllDeck(job[i].JID.ToString());
                            availableLevels.AddRange(lst);
                            prod.totalSrn = prod.totalSrn + job[i].Quantity;
                            //  }
                        }
                        prod.Jobs = jobInfoSts;
                        prodInfoStats.Add(prod);
                    }
                }
                vm.Products = prodInfoStats;
                availableLevels = availableLevels.Distinct().ToList();
                availableLevels.Insert(0, "Select");
                vm.Packaginlevels = availableLevels.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });

                //var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptProductWise.cshtml", vm);
                return vm;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Rpt UID List
        public string getUIDs(decimal JID, string lvl, string UserName)
        {
            var data = "";
            RptUIDsViewModel vm = new RptUIDsViewModel();
            vm = getUIDsPDF(JID, lvl, UserName);
            if (vm != null)
            {
                if (vm.packagingDetails.Count > 0)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptUIdList.cshtml", vm);
                }
                else
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            else
            {
                data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //vm.job = db.Job.Find(JID);
            //var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            //vm.Jobtype = jobtype.Job_Type;
            //vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            //vm.DivisionCode = db.LineLocation.Where(l => l.ID == vm.job.LineCode).Select(l => l.DivisionCode).First();
            //vm.product = db.PackagingAsso.Find(vm.job.PAID);
            //var packagingAssoCount = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).Count();
            //if (packagingAssoCount > 0)
            //{
            //    vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).First();
            //    //vm.packagingDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == jid).ToList();
            //    var pakDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == JID && p.PackageTypeCode == lvl && p.IsDecomission == false).OrderByDescending(x => x.IsRejected).ToList();
            //    //vm.packagingDetails = DecryptedUids(pakDetails);
            //    vm.packagingDetails = pakDetails;
            //    var levles = ProductPackageHelper.getAllDeck(JID.ToString());
            //    levles.Insert(0, "Select");
            //    vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
            //    if (pakDetails.Count <= 0)
            //    {
            //        data = "No Data";
            //    }
            //    else
            //    {
            //        data = ViewRenderer.RenderPartialView("~/Views/Reports/RptUIdList.cshtml", vm);
            //    }
            //}
            //else
            //{
            //    data = "<h3 style='text - align:center; padding - bottom: 50px; text-align:center;'>No data available for " + lvl + " level</h3>";
            //}

            return data;
        }


        public RptUIDsViewModel getUIDsPDF(decimal JID, string lvl, string UserName)
        {
            var data = "";
            RptUIDsViewModel vm = new RptUIDsViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.job = db.Job.Find(JID);
            var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            vm.Jobtype = jobtype.Job_Type;
            vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            vm.DivisionCode = db.LineLocation.Where(l => l.ID == vm.job.LineCode).Select(l => l.DivisionCode).FirstOrDefault();
            vm.product = db.PackagingAsso.Find(vm.job.PAID);
            var packagingAssoCount = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).Count();
            var jbdetail = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == lvl).Count();
            if (jbdetail > 0)
            {
                //  vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).First();
                vm.jbDetails = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == lvl).FirstOrDefault();
                //vm.packagingDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == jid).ToList();
                var pakDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == JID && p.PackageTypeCode == lvl).OrderBy(x => x.LastUpdatedDate).ToList();
                //vm.packagingDetails = DecryptedUids(pakDetails);
                vm.packagingDetails = pakDetails;
                var levles = ProductPackageHelper.getAllDeck(JID.ToString());
                levles.Insert(0, "Select");
                vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
                if (pakDetails.Count <= 0)
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
                else
                {
                    return vm;
                }
            }
            else
            {
                data = "No data available for " + lvl + " level";
            }

            return null;
        }

        private List<PackagingDetails> DecryptedUids(List<PackagingDetails> lst)
        {
            foreach (var item in lst)
            {
                item.Code = PTPLCRYPTORENGINE.AESCryptor.Decrypt(item.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            }
            return lst;
        }
        #endregion

        #region Rpt Bad UID List
        public string getBadUIDs(decimal JID, string lvl, string UserName)
        {

            var data = "";
            RptUIDsViewModel vm = new RptUIDsViewModel();
            vm = getBadUIDsPdf(JID, lvl, UserName);
            if (vm != null)
            {
                if (vm.UIDFailReasonsDetails.Count > 0)
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptBadUIdList.cshtml", vm);
                }
                else
                {
                    //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                    data = "No Data";
                }
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            // vm.CompanyName = CompanyName;
            // vm.Address = CompanyAddrs;
            // vm.UserName = UserName;
            // vm.job = db.Job.Find(JID);
            // var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            // vm.Jobtype = jobtype.Job_Type;
            // vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            // vm.DivisionCode = vm.LineDetails.DivisionCode;
            //// vm.DivisionCode = db.LineLocation.Where(l => l.ID == vm.job.LineCode).Select(l => l.DivisionCode).First();
            // vm.product = db.PackagingAsso.Find(vm.job.PAID);
            // var packagingAssoCount = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).ToList();
            // if (packagingAssoCount.Count > 0)
            // {
            //     string bottomlvl = ProductPackageHelper.getBottomDeck(vm.product.PAID);
            //     // vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).First();
            //     vm.packagingAsso = packagingAssoCount.Where(x => x.PAID == vm.product.PAID && x.PackageTypeCode == lvl).First();
            //     //vm.packagingDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == jid).ToList();
            //     var pakDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == JID && p.PackageTypeCode == lvl && p.IsRejected == true && p.IsManualUpdated == false).ToList();
            //     //vm.packagingDetails = DecryptedUids(pakDetails);
            //     vm.packagingDetails = pakDetails;

            //     List<UIDFailReason> UIDFailReason = new List<UIDFailReason>();
            //     List<UIDFailReasonDetails> lstUIDFailReasonDetails = new List<UIDFailReasonDetails>();
            //     foreach (var item in pakDetails)
            //     {
            //         DbHelper m_dbhelper = new DbHelper();

            //         string Query = "select " +
            //                        " T.FReason.value('@TYPE', 'Nvarchar(max)') AS FailedID, Code,LastUpdatedDate, T.FReason.value('@ISFAILED', 'Nvarchar(max)') as [Status], Code,SSCC" +
            //                        " from " +
            //                        " PackagingDetails " +
            //                        " cross apply " +
            //                        " Reason.nodes('/INSPECTION') as T(FReason) " +
            //                        " where JobID= CAST(" + JID + " as Numeric) and PackageTypeCode='" + lvl + "' and IsRejected = 1 and IsRejected is not null " +
            //                        " and Code ='" + item.Code + "' " +
            //                        " and T.FReason.value('@ISFAILED', 'Nvarchar(max)') != '' " +
            //                        " and T.FReason.value('@SKIP', 'Nvarchar(max)') = 'False'";

            //         DataSet ds = m_dbhelper.GetDataSet(Query);
            //         DataTable DtCodeReason = ds.Tables[0];

            //         DataTable DtDistinctCodes;
            //         DataView vw = new DataView(DtCodeReason);
            //         if (lvl == bottomlvl)
            //         {
            //              DtDistinctCodes = vw.ToTable(true, "SSCC");
            //         }
            //         else
            //         {
            //              DtDistinctCodes = vw.ToTable(true, "Code");
            //         }

            //         foreach (DataRow dcod in DtDistinctCodes.Rows)
            //         {
            //             UIDFailReasonDetails uid = new UIDFailReasonDetails();
            //             DataTable fldIds;
            //             if (lvl==bottomlvl)
            //             {
            //                 uid.Code = dcod["SSCC"].ToString();
            //                 fldIds = DtCodeReason.AsEnumerable()
            //                .Where(r => r.Field<string>("SSCC") == uid.Code).CopyToDataTable();
            //             }
            //             else
            //             {
            //                 uid.Code = dcod["Code"].ToString();
            //                 fldIds = DtCodeReason.AsEnumerable()
            //                .Where(r => r.Field<string>("Code") == uid.Code).CopyToDataTable();
            //             }
            //             //uid.Code = dcod["Code"].ToString();




            //             List<string> lstFldIds = new List<string>();
            //             foreach (DataRow fid in fldIds.Rows)
            //             {
            //                 var fida = fid["FailedID"].ToString();
            //                 lstFldIds.Add(fida);
            //             }

            //             List<status> lstStatus = new List<status>();
            //             foreach (DataRow fid in fldIds.Rows)
            //             {
            //                 status s = new status();
            //                 s.feild = fid["FailedID"].ToString();
            //                 s.stat = fid["Status"].ToString();
            //                 lstStatus.Add(s);
            //             }
            //             uid.FailedIds = lstFldIds;
            //             uid.status = lstStatus;

            //             lstUIDFailReasonDetails.Add(uid);
            //         }

            //         vm.UIDFailReasonsDetails = lstUIDFailReasonDetails;

            //         //foreach (DataRow rw in DtCodeReason.Rows)
            //         //{
            //         //   UIDFailReason UIDRes = new UIDFailReason();
            //         //   UIDRes.FailedID = rw[0].ToString();
            //         //   UIDRes.Code = rw[1].ToString();
            //         //   UIDRes.LastUpdatedDate = rw[2].ToString();
            //         //   UIDRes.status = rw[3].ToString();
            //         //   UIDFailReason.Add(UIDRes);

            //         //}
            //         //vm.UIDFailReasons = UIDFailReason;
            //     }

            //     var levles = ProductPackageHelper.getAllDeck(JID.ToString());
            //     levles.Insert(0, "Select");
            //     vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
            //     if (pakDetails.Count <= 0)
            //     {
            //         data = "No Data";
            //     }
            //     else
            //     {
            //         if (vm.UIDFailReasonsDetails.Count > 0)
            //         {
            //             data = ViewRenderer.RenderPartialView("~/Views/Reports/RptBadUIdList.cshtml", vm);
            //         }
            //         else
            //         {
            //             data = "No Data";
            //         }
            //     }
            // }
            // else
            // {
            //     data = "<h3 style='text - align:center; padding - bottom: 50px; text-align:center;'>No data available for " + lvl + " level</h3>";
            // }

            return data;
        }

        public RptUIDsViewModel getBadUIDsPdf(decimal JID, string lvl, string UserName)
        {

            RptUIDsViewModel vm = new RptUIDsViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.job = db.Job.Find(JID);
            var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            vm.Jobtype = jobtype.Job_Type;
            vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            vm.DivisionCode = vm.LineDetails.DivisionCode;
            // vm.DivisionCode = db.LineLocation.Where(l => l.ID == vm.job.LineCode).Select(l => l.DivisionCode).First();
            vm.product = db.PackagingAsso.Find(vm.job.PAID);
            var packagingAssoCount = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).ToList();
            var jbDetails = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == lvl).ToList();
            if (jbDetails.Count > 0)
            {
                string bottomlvl = ProductPackageHelper.getBottomDeckJB(JID);
                // vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).First();
                vm.jbDetails = jbDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == lvl).FirstOrDefault();
                //vm.packagingDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == jid).ToList();
                var pakDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == JID && p.PackageTypeCode == lvl && (p.IsRejected == true) && p.IsDecomission == false).ToList();
                //vm.packagingDetails = DecryptedUids(pakDetails);
                vm.packagingDetails = pakDetails;

                //List<UIDFailReason> UIDFailReason = new List<UIDFailReason>();
                List<UIDFailReasonDetails> lstUIDFailReasonDetails = new List<UIDFailReasonDetails>();
                XDocument xml = null;
                IEnumerable<XElement> lst = null;
                foreach (var item in pakDetails)
                {
                    xml = XDocument.Parse("<root>" + item.Reason + "</root>");
                    lst = xml.Root.Elements().Where(x => x.Attribute("SKIP").Value == "False" && (!string.IsNullOrEmpty(x.Attribute("ISFAILED").Value)));

                    UIDFailReasonDetails uid = new UIDFailReasonDetails();
                    uid.Code = (lvl == bottomlvl) ? item.SSCC : item.Code;
                    uid.FailedIds = new List<string>();
                    uid.status = new List<status>();
                    foreach (var node in lst)
                    {
                        uid.FailedIds.Add(node.Attribute("TYPE").Value);
                        uid.status.Add(new status() { feild = node.Attribute("TYPE").Value, stat = node.Attribute("ISFAILED").Value });
                    }
                    lstUIDFailReasonDetails.Add(uid);


                    #region OLD LOGIC
                    /*
                    DbHelper m_dbhelper = new DbHelper();

                    string Query = "select " +
                                   " T.FReason.value('@TYPE', 'Nvarchar(max)') AS FailedID, Code,LastUpdatedDate, T.FReason.value('@ISFAILED', 'Nvarchar(max)') as [Status], Code,SSCC" +
                                   " from " +
                                   " PackagingDetails " +
                                   " cross apply " +
                                   " Reason.nodes('/INSPECTION') as T(FReason) " +
                                   " where JobID= CAST(" + JID + " as Numeric) and PackageTypeCode='" + lvl + "' and IsRejected = 1 and IsRejected is not null " +
                                   " and Code ='" + item.Code + "' " +
                                   " and T.FReason.value('@ISFAILED', 'Nvarchar(max)') != '' " +
                                   " and T.FReason.value('@SKIP', 'Nvarchar(max)') = 'False'";

                    DataSet ds = m_dbhelper.GetDataSet(Query);
                    DataTable DtCodeReason = ds.Tables[0];

                    DataTable DtDistinctCodes;
                    DataView vw = new DataView(DtCodeReason);
                    if (lvl == bottomlvl && packagingAssoCount.Count() > 1)
                    {
                        DtDistinctCodes = vw.ToTable(true, "SSCC");
                    }
                    else
                    {
                        DtDistinctCodes = vw.ToTable(true, "Code");
                    }

                    foreach (DataRow dcod in DtDistinctCodes.Rows)
                    {
                        UIDFailReasonDetails uid = new UIDFailReasonDetails();
                        DataTable fldIds;
                        if (lvl == bottomlvl  && packagingAssoCount.Count() > 1)
                        {
                            uid.Code = dcod["SSCC"].ToString();
                            fldIds = DtCodeReason.AsEnumerable()
                           .Where(r => r.Field<string>("SSCC") == uid.Code).CopyToDataTable();
                        }
                        else
                        {
                            uid.Code = dcod["Code"].ToString();
                            fldIds = DtCodeReason.AsEnumerable()
                           .Where(r => r.Field<string>("Code") == uid.Code).CopyToDataTable();
                        }
                        //uid.Code = dcod["Code"].ToString();




                        List<string> lstFldIds = new List<string>();
                        foreach (DataRow fid in fldIds.Rows)
                        {
                            var fida = fid["FailedID"].ToString();
                            lstFldIds.Add(fida);
                        }

                        List<status> lstStatus = new List<status>();
                        foreach (DataRow fid in fldIds.Rows)
                        {
                            status s = new status();
                            s.feild = fid["FailedID"].ToString();
                            s.stat = fid["Status"].ToString();
                            lstStatus.Add(s);
                        }
                        uid.FailedIds = lstFldIds;
                        uid.status = lstStatus;

                        lstUIDFailReasonDetails.Add(uid);
                    }
                    */
                    vm.UIDFailReasonsDetails = lstUIDFailReasonDetails;

                    #endregion

                }

                var levles = ProductPackageHelper.getAllDeck(JID.ToString());
                levles.Insert(0, "Select");
                vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString(), Selected = false });
                if (pakDetails.Count <= 0)
                {
                    return null;
                }
                else
                {
                    if (vm.UIDFailReasonsDetails.Count > 0)
                    {
                        return vm;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }

        }
        #endregion
        #region Rpt Detailed Job Info

        public string getDetailedJobInformation(decimal JID, DateTime FromDate, DateTime ToDate, string UserName)
        {
            var data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptDetailedJobInfoViewModel vm = new RptDetailedJobInfoViewModel();
            vm = getDetailedJobInformationPdf(JID, FromDate, ToDate, UserName);
            if (vm.ProductionSummary.Count > 0)
            {
                data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetailedJobInfo.cshtml", vm);
            }
            else
            {
                data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //vm.Job = db.Job.Find(JID);
            //var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
            //vm.jobtype = jobtype.Job_Type;
            //vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
            //vm.LocationDetails = db.LineLocation.Find(vm.Job.LineCode);
            //vm.NoReadCount = vm.Job.NoReadCount.ToString();
            //JobState state = (JobState)vm.Job.JobStatus;
            //vm.Status = state.ToString();
            //if (vm.Job.CreatedBy != null)
            //{
            //    var user = db.Users.Where(u => u.ID == vm.Job.CreatedBy).ToList();
            //    if (user.Count() > 0)
            //    {
            //        vm.CreatedBy = db.Users.Where(u => u.ID == vm.Job.CreatedBy).Select(u => u.UserName).First();
            //    }
            //}
            //if (vm.Job.VerifiedBy != null)
            //{
            //    var user = db.Users.Where(u => u.ID == vm.Job.VerifiedBy).ToList();
            //    if (user.Count() > 0)
            //    {
            //        vm.VerifiedBy = db.Users.Where(u => u.ID == vm.Job.VerifiedBy).Select(u => u.UserName).First();
            //    }
            //}
            //else
            //{
            //    data = "Batch is not verified";
            //    return data;
            //}
            //ReportJobHelper rjh = new ReportJobHelper();
            //DataSet dsDtlj = rjh.RetReportDs(JobBLL.ReportOp.ForJobdeckProcess, JID.ToString(), null, FromDate, ToDate, JID);
            //List<ProductionSummaryDeckWise> lstProSumLst = new List<ProductionSummaryDeckWise>();
            //foreach (DataRow item in dsDtlj.Tables[0].Rows)
            //{
            //    ProductionSummaryDeckWise obj = new ProductionSummaryDeckWise();
            //    obj.Deck = item[2].ToString();
            //    obj.GoodCnt = Convert.ToInt32(item[4].ToString());
            //    obj.RejectedCnt = Convert.ToInt32(item[5].ToString());
            //    obj.Total = obj.GoodCnt + obj.RejectedCnt;
            //    obj.Decommisioned = Convert.ToInt32(item[6].ToString());
            //    obj.QASample = Convert.ToInt32(item[8].ToString());
            //    if (item[7].ToString() != "")
            //        obj.ChallengeTest = Convert.ToInt32(item[7].ToString());
            //    else
            //        obj.ChallengeTest = 0;
            //    obj.TotalUsedCartons = obj.Total + obj.Decommisioned + obj.QASample + obj.ChallengeTest;
            //    //double percent = (double)obj.RejectedCnt / (obj.GoodCnt + obj.QASample + obj.Decommisioned) * 100;
            //    double percent = ((double)obj.RejectedCnt / obj.TotalUsedCartons) * 100;
            //    if (Convert.ToString(percent) == "NaN")
            //    {
            //        obj.RejectionPercent = 0;
            //    }
            //    else
            //    {
            //        obj.RejectionPercent = percent; //Convert.ToInt32(item[0].ToString());
            //    }
            //    lstProSumLst.Add(obj);
            //}

            //var sorted = ProductPackageHelper.sorttheLevels(lstProSumLst.Select(x => x.Deck).ToList());

            //var qry = from d in sorted
            //          join s in lstProSumLst on d equals s.Deck
            //          select s;

            //var ordLst = qry.ToList();

            //lstProSumLst = ordLst;

            //vm.ProductionSummary = lstProSumLst;

            //if (lstProSumLst.Count > 0)
            //{
            //    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetailedJobInfo.cshtml", vm);
            //}
            //else
            //{
            //    data = "No Data";
            //}
            return data;
        }

        public RptDetailedJobInfoViewModel getDetailedJobInformationPdf(decimal JID, DateTime FromDate, DateTime ToDate, string UserName)
        {

            RptDetailedJobInfoViewModel vm = new RptDetailedJobInfoViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.Job = db.Job.Find(JID);
            var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
            vm.jobtype = jobtype.Job_Type;
            vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
            vm.jbDetails = db.JobDetails.Where(x => x.JD_JobID == JID).FirstOrDefault();
            vm.LocationDetails = db.LineLocation.Find(vm.Job.LineCode);
            vm.NoReadCount = vm.Job.NoReadCount.ToString();
            int extra = (from m in db.M_Identities join x in db.X_Identities on m.Id equals x.MasterId where m.IsTransfered == true && m.IsExtra == true && m.JID == JID && x.IsTransfered == true && m.PackageTypeCode == "MOC" select x.CodeType).Count();
            vm.ExtraCount = extra.ToString();
            JobState state = (JobState)vm.Job.JobStatus;
            vm.Status = state.ToString();
            if (vm.Job.CreatedBy != null)
            {
                var user = db.Users.Where(u => u.ID == vm.Job.CreatedBy).ToList();
                if (user.Count() > 0)
                {
                    vm.CreatedBy = db.Users.Where(u => u.ID == vm.Job.CreatedBy).Select(u => u.UserName).FirstOrDefault();
                }
            }
            if (vm.Job.VerifiedBy != null)
            {
                var user = db.Users.Where(u => u.ID == vm.Job.VerifiedBy).ToList();
                if (user.Count() > 0)
                {
                    vm.VerifiedBy = db.Users.Where(u => u.ID == vm.Job.VerifiedBy).Select(u => u.UserName).FirstOrDefault();
                }
            }
            //else
            //{
            //    //data = "Batch is not verified";
            //    //return data;
            //    return null;
            //}
            ReportJobHelper rjh = new ReportJobHelper();
            DataSet dsDtlj = rjh.RetReportDs(JobBLL.ReportOp.ForJobdeckProcess, JID.ToString(), null, FromDate, ToDate, JID);
            List<ProductionSummaryDeckWise> lstProSumLst = new List<ProductionSummaryDeckWise>();
            foreach (DataRow item in dsDtlj.Tables[0].Rows)
            {
                ProductionSummaryDeckWise obj = new ProductionSummaryDeckWise();
                obj.Deck = item[2].ToString();
                obj.GoodCnt = Convert.ToInt32(item[4].ToString());
                obj.RejectedCnt = Convert.ToInt32(item[5].ToString());
                obj.Total = obj.GoodCnt + obj.RejectedCnt;
                obj.Decommisioned = Convert.ToInt32(item[6].ToString());
                obj.QASample = Convert.ToInt32(item[8].ToString());
                obj.NRNU = Convert.ToInt32(item[9].ToString());
                if (item[7].ToString() != "")
                    obj.ChallengeTest = Convert.ToInt32(item[7].ToString());
                else
                    obj.ChallengeTest = 0;

                obj.TotalUsedCartons = obj.Total + obj.Decommisioned + obj.QASample + obj.ChallengeTest + obj.NRNU;
                double percent = (double)obj.RejectedCnt / (obj.GoodCnt + obj.RejectedCnt) * 100;
                //       double percent = ((double)obj.RejectedCnt / obj.TotalUsedCartons) * 100;
                if (Convert.ToString(percent) == "NaN")
                {
                    obj.RejectionPercent = 0;
                }
                else
                {
                    obj.RejectionPercent = percent; //Convert.ToInt32(item[0].ToString());
                }
                lstProSumLst.Add(obj);
            }

            var sorted = ProductPackageHelper.sorttheLevels(lstProSumLst.Select(x => x.Deck).ToList());

            var qry = from d in sorted
                      join s in lstProSumLst on d equals s.Deck
                      select s;

            var ordLst = qry.ToList();

            lstProSumLst = ordLst;

            vm.ProductionSummary = lstProSumLst;

            if (lstProSumLst.Count > 0)
            {
                //data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDetailedJobInfo.cshtml", vm);
                return vm;
            }
            else
            {
                return null;
            }
            return null;
        }
        #endregion


        #region Rpt Decommisioned UIDs

        public string getDecommisionedUIDs(decimal JID, string lvl, string UserName)
        {
            var data = "";
            //TnT_Ds dsTnT = new TnT_Ds();
            RptUIDsViewModel vm = new RptUIDsViewModel();
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //vm.job = db.Job.Find(JID);
            //var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            //vm.Jobtype = jobtype.Job_Type;
            //vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            //vm.product = db.PackagingAsso.Find(vm.job.PAID);
            //vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).First();
            //////vm.packagingDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == jid).ToList();
            ////var pakDetails = db.PackagingDetails.Where(p => p.PAID == vm.job.PAID && p.JobID == JID && p.PackageTypeCode == lvl).ToList();
            ////vm.packagingDetails = DecryptedUids(pakDetails);


            //List<REDTR.DB.BusinessObjects.PackagingDetails> Pck = ObjHelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetDecomissionedDtls, JID.ToString(), lvl);
            //FillUidList(Pck, ref dsTnT,JID);

            //List<PackagingDetails> pkgDtls = new List<PackagingDetails>();
            //foreach (DataRow item in dsTnT.Tables["UIDList"].Rows)
            //{
            //    PackagingDetails pkd = new PackagingDetails();
            //    pkd.Code = item[1].ToString();
            //    pkd.MfgPackDate = Convert.ToDateTime(item[2].ToString());
            //    //Uisng Bad Image as status 
            //    pkd.BadImage = item[3].ToString();
            //    pkgDtls.Add(pkd);
            //}
            //vm.packagingDetails = pkgDtls;

            //var levles = ProductPackageHelper.getAllDeck(JID.ToString());
            //levles.Insert(0, "Select");
            //vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            vm = getDecommisionedUIDsPDF(JID, lvl, UserName);

            int cnt = 1;
            List<string> lstString = new List<string>();
            foreach (PackagingDetails pack in vm.packagingDetails)
            {
                lstString.Clear();
                if (pack.BadImage.Contains("CHALLENGE"))
                {
                    lstString = pack.BadImage.Split('|').ToList();
                    if (lstString.Count > 0)
                    {
                        lstString.RemoveAt(lstString.Count - 1);
                        lstString.Add(" " + cnt.ToString());
                        cnt++;
                        pack.BadImage = string.Join("|", lstString);
                    }
                }
            }

            if (vm != null)
            {
                if (vm.packagingDetails.Count <= 0)
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
                else
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptDecomUIdList.cshtml", vm);
                }
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            return data;


        }
        public RptUIDsViewModel getDecommisionedUIDsPDF(decimal JID, string lvl, string UserName)
        {

            TnT_Ds dsTnT = new TnT_Ds();
            RptUIDsViewModel vm = new RptUIDsViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.job = db.Job.Find(JID);
            var jobtype = db.JOBTypes.Where(x => x.TID == vm.job.TID).FirstOrDefault();
            vm.Jobtype = jobtype.Job_Type;
            vm.LineDetails = db.LineLocation.Find(vm.job.LineCode);
            vm.product = db.PackagingAsso.Find(vm.job.PAID);
            vm.packagingAsso = db.PackagingAssoDetails.Where(p => p.PAID == vm.product.PAID && p.PackageTypeCode == lvl).FirstOrDefault();
            vm.jbDetails = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == lvl).FirstOrDefault();

            List<REDTR.DB.BusinessObjects.PackagingDetails> Pck = ObjHelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GetDecomissionedDtls, JID.ToString(), lvl);
            //FillUidList(Pck, ref dsTnT, JID);

            var lvls = ProductPackageHelper.getAllDeck(JID.ToString());
            lvls = ProductPackageHelper.sorttheLevelsDesc(lvls).ToList();
            List<PackagingDetails> pkgDtls = new List<PackagingDetails>();
            for (int i = 0; i < Pck.Count; i++) //sscc records inserted
            {
                string UidStatus = GetUIdStatus(Pck[i]);
                if (UidStatus != null)
                {
                    PackagingDetails pkd = new PackagingDetails();
                    if (Pck[i].PackageTypeCode == lvls[0])
                    {
                        if (lvls.Count == 1)
                        {
                            pkd.Code = Pck[i].Code;
                            //ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].Code, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                        }
                        else
                        {
                            pkd.Code = Pck[i].SSCC;
                            //ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].SSCC, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                        }
                    }
                    else
                    {
                        //Pck[i].Code = PTPLCRYPTORENGINE.AESCryptor.Decrypt(Pck[i].Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                        pkd.Code = Pck[i].Code;
                        //ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].Code, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                    }
                    pkd.MfgPackDate = Convert.ToDateTime(Pck[i].LastUpdatedDate);
                    //Uisng Bad Image as status 
                    pkd.BadImage = UidStatus;
                    pkd.Remarks = Pck[i].Remarks;
                    pkgDtls.Add(pkd);
                }
            }

            //foreach (DataRow item in dsTnT.Tables["UIDList"].Rows)
            //{
            //    PackagingDetails pkd = new PackagingDetails();
            //    pkd.Code = item[1].ToString();
            //    pkd.MfgPackDate = Convert.ToDateTime(item[2].ToString());
            //    //Uisng Bad Image as status 
            //    pkd.BadImage = item[3].ToString();
            //    pkgDtls.Add(pkd);
            //}
            vm.packagingDetails = pkgDtls;

            var levles = ProductPackageHelper.getAllDeck(JID.ToString());
            levles.Insert(0, "Select");
            vm.Packaginlevels = levles.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });

            if (vm.packagingDetails.Count <= 0)
            {
                return null;
            }
            else
            {
                return vm;
            }
            return null;
        }
        private void FillUidList(List<REDTR.DB.BusinessObjects.PackagingDetails> Pck, ref TnT_Ds ds, decimal jid)
        {
            var lvls = ProductPackageHelper.getAllDeck(jid.ToString());
            lvls = ProductPackageHelper.sorttheLevelsDesc(lvls).ToList();
            for (int i = 0; i < Pck.Count; i++) //sscc records inserted
            {
                string UidStatus = GetUIdStatus(Pck[i]);
                if (UidStatus != null)
                {
                    if (Pck[i].PackageTypeCode == lvls[0])
                    {
                        if (lvls.Count == 1)
                        {
                            ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].Code, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                        }
                        else
                        {
                            ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].SSCC, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                        }

                    }
                    else
                    {
                        //Pck[i].Code = PTPLCRYPTORENGINE.AESCryptor.Decrypt(Pck[i].Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                        ds.UIDList.Rows.Add(Pck[i].JobID, Pck[i].Code, Convert.ToDateTime(Pck[i].LastUpdatedDate), UidStatus);
                    }
                }

            }
        }
        private string GetUIdStatus(REDTR.DB.BusinessObjects.PackagingDetails pdls)
        {
            string UIDStatus = string.Empty;
            if (pdls.IsDecomission == true || pdls.ManualUpdateDesc != null)
                //  UIDStatus = Resources.UID_Decomissioned;
                UIDStatus = ObjHelper.DBManager.PackagingDetailsBLL.GetUIDManualupdatestatus(       // Code Change by Dipesh
                pdls.JobID.ToString(), pdls.PackageTypeCode, pdls.Code);

            else if (Globals.VendorAppAsso.isLABELGENERATOR)
            {
                UIDStatus = Resources.NonVerified;

                bool IsVerified = ObjHelper.DBManager.PackagingDetailsBLL.GEtVerifiedUIDStatus(
                pdls.JobID.ToString(), pdls.Code);

                if (IsVerified == true)
                    UIDStatus = Resources.Verified;
            }
            else if (Globals.VendorAppAsso.isPACKiTRACKnTRACE)
            {
                // Add by Dipesh 14.05.2015
                UIDStatus = Resources.UIDGood;
                if (pdls.IsRejected == true)
                    UIDStatus = Resources.UIDBAD;
                if (pdls.IsRejected == null && Globals.AppSettings.AllowNotVerifiedUID == true)
                    UIDStatus = "NR/NU";
                if (pdls.IsRejected == null && Globals.AppSettings.AllowNotVerifiedUID == false)
                    UIDStatus = null;
            }
            return UIDStatus;
        }



        #endregion

        #region Rpt Parent Child

        public string getParentChildReport(decimal JID, string UserName)
        {
            //PCReportHelper hlp = new PCReportHelper();

            //var ds = hlp.getPCRptDS(JID);
            //if (ds == null)
            //{
            //    return "No Data";
            //}
            string data = string.Empty;
            RptPCRelationshipViewModel vm = new RptPCRelationshipViewModel();
            vm = getParentChildReportPdf(JID, UserName);
            if (vm != null)
            {
                data = ViewRenderer.RenderPartialView("~/Views/Reports/RptPCTable.cshtml", vm);
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //vm.Job = db.Job.Find(JID);
            //var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
            //vm.Jobtype = jobtype.Job_Type;
            //vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
            //vm.LineDetails = db.LineLocation.Find(vm.Job.LineCode);
            //if (ds.Tables["TempPCMAP"].Rows.Count > 0)
            //{


            //    DataTable PCData = ds.Tables["TempPCMAP"];
            //    //decrypt DataTable

            //    var Levels = ProductPackageHelper.getAllDeck(JID.ToString());
            //    Levels = ProductPackageHelper.sorttheLevelsDesc(Levels);
            //    //List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
            //    //List<string> Levels = LstJobDetails.Select(x => x.JD_Deckcode).Distinct().ToList();
            //    var SSCCs = PCData.AsEnumerable().Select(x => x.Field<string>("SSCC")).Distinct().ToList();
            //    //Levels = ProductPackageHelper.sorttheLevels(Levels);
            //    //Levels.Reverse();

            //    vm.PkgLevels = Levels;
            //    vm.TertiaryLevel = ProductPackageHelper.getTertiarryDeck(vm.Job.PAID, JID);
            //    vm.TertiaryGTIN = ProductPackageHelper.getTertiaryGTIN(vm.Job.PAID);
            //    vm.PCRela = getPCMTempData(PCData);
            //    var ssccCnt = vm.PCRela.Where(x => x.SSCC != "").Select(x => x.SSCC).Count();
            //    string data;
            //    if (ssccCnt > 0)
            //    {
            //        data = ViewRenderer.RenderPartialView("~/Views/Reports/RptPCTable.cshtml", vm);
            //    }
            //    else
            //    {
            //        data = " No Data";
            //    }

            //    return data;
            //}
            //else if (ds.Tables["PackBoxesRelationship"].Rows.Count > 0)
            //{
            //    //RptPCRelationshipViewModel vm = new RptPCRelationshipViewModel();
            //    //vm.CompanyName = CompanyName;
            //    //vm.Address = CompanyAddrs;
            //    //vm.UserName = UserName;
            //    //vm.Job = db.Job.Find(JID);
            //    //var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
            //    //vm.Jobtype = jobtype.Job_Type;
            //    //vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
            //    //vm.LineDetails = db.LineLocation.Find(vm.Job.LineCode);

            //    DataTable PCData = ds.Tables["PackBoxesRelationship"];

            //    List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
            //    List<string> Levels = LstJobDetails.Select(x => x.JD_Deckcode).Distinct().ToList();
            //    var SSCCs = PCData.AsEnumerable().Select(x => x.Field<string>("SSCC")).Distinct().ToList();
            //    Levels = ProductPackageHelper.sorttheLevels(Levels);
            //    Levels.Reverse();
            //    vm.PkgLevels = Levels;
            //    vm.TertiaryLevel = ProductPackageHelper.getTertiarryDeck(vm.Job.PAID, JID);
            //    vm.TertiaryGTIN = ProductPackageHelper.getTertiaryGTIN(vm.Job.PAID);

            //    vm.PCRela = getPCMTempDataFor2Levels(PCData, vm.TertiaryLevel);


            //    var ssccCnt = vm.PCRela.Where(x => x.SSCC != "" && x.IsUsed == true).Select(x => x.SSCC).Count();
            //    string data;

            //    if (ssccCnt > 0)
            //    {
            //        data = ViewRenderer.RenderPartialView("~/Views/Reports/RptPCTable.cshtml", vm);
            //    }
            //    else
            //    {
            //        data = "No Data";
            //    }

            //    return data;
            //}
            //else
            //{
            //    return "No Data";
            //}

            return data;
        }

        public RptPCRelationshipViewModel getParentChildReportPdf(decimal JID, string UserName)
        {
            PCReportHelper hlp = new PCReportHelper();

            var ds = hlp.getPCRptDS(JID);
            if (ds == null)
            {
                return null;
            }
            RptPCRelationshipViewModel vm = new RptPCRelationshipViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.Job = db.Job.Find(JID);
            var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
            vm.Jobtype = jobtype.Job_Type;
            vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
            vm.jbDetails = db.JobDetails.Where(x => x.JD_JobID == JID).FirstOrDefault();
            vm.LineDetails = db.LineLocation.Find(vm.Job.LineCode);
            if (ds.Tables["TempPCMAP"].Rows.Count > 0)
            {


                DataTable PCData = ds.Tables["TempPCMAP"];
                //decrypt DataTable

                var Levels = ProductPackageHelper.getAllDeck(JID.ToString());
                Levels = ProductPackageHelper.sorttheLevelsDesc(Levels);
                //List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
                //List<string> Levels = LstJobDetails.Select(x => x.JD_Deckcode).Distinct().ToList();
                var SSCCs = PCData.AsEnumerable().Select(x => x.Field<string>("SSCC")).Distinct().ToList();
                //Levels = ProductPackageHelper.sorttheLevels(Levels);
                //Levels.Reverse();

                vm.PkgLevels = Levels;
                vm.TertiaryLevel = ProductPackageHelper.getTertiarryDeck(vm.Job.PAID, JID);
                vm.TertiaryGTIN = ProductPackageHelper.getTertiaryGTINJb(vm.Job.JID);
                vm.PCRela = getPCMTempData(PCData);
                var ssccCnt = vm.PCRela.Where(x => x.SSCC != "").Select(x => x.SSCC).Count();
                string data;
                if (ssccCnt > 0)
                {
                    return vm;
                }
                else
                {
                    return null;
                }

                return null;
            }
            else if (ds.Tables["PackBoxesRelationship"].Rows.Count > 0)
            {
                //RptPCRelationshipViewModel vm = new RptPCRelationshipViewModel();
                //vm.CompanyName = CompanyName;
                //vm.Address = CompanyAddrs;
                //vm.UserName = UserName;
                //vm.Job = db.Job.Find(JID);
                //var jobtype = db.JOBTypes.Where(x => x.TID == vm.Job.TID).FirstOrDefault();
                //vm.Jobtype = jobtype.Job_Type;
                //vm.Product = db.PackagingAsso.Find(vm.Job.PAID);
                //vm.LineDetails = db.LineLocation.Find(vm.Job.LineCode);

                DataTable PCData = ds.Tables["PackBoxesRelationship"];

                List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
                List<string> Levels = LstJobDetails.Select(x => x.JD_Deckcode).Distinct().ToList();
                var SSCCs = PCData.AsEnumerable().Select(x => x.Field<string>("SSCC")).Distinct().ToList();
                Levels = ProductPackageHelper.sorttheLevels(Levels);
                Levels.Reverse();
                vm.PkgLevels = Levels;
                vm.TertiaryLevel = ProductPackageHelper.getTertiarryDeck(vm.Job.PAID, JID);
                vm.TertiaryGTIN = ProductPackageHelper.getTertiaryGTIN(vm.Job.PAID);

                vm.PCRela = getPCMTempDataFor2Levels(PCData, vm.TertiaryLevel);


                var ssccCnt = vm.PCRela.Where(x => x.SSCC != "" && x.IsUsed == true).Select(x => x.SSCC).Count();
                string data;

                if (ssccCnt > 0)
                {
                    return vm;
                }
                else
                {
                    return null;
                }

                return null;
            }
            else
            {
                return null;
            }


        }
        public List<PCRela> getPCMTempDataFor2Levels(DataTable PCData, string LastLevel)
        {
            List<PCRela> PCList = new List<PCRela>();

            foreach (DataRow item in PCData.Rows)
            {
                PCRela pc = new PCRela();
                if (LastLevel == "OSH")
                {
                    pc.OSHCode = item[4].ToString();
                }
                else if (LastLevel == "ISH")
                {
                    pc.ISHCode = item[4].ToString();
                }
                else if (LastLevel == "OBX")
                {
                    pc.OBXCode = item[4].ToString();
                }

                pc.MOCCode = item[4].ToString();
                pc.SSCC = item[10].ToString();
                if (item.ItemArray[12] != null && item.ItemArray[12].ToString() != "")
                {
                    pc.IsUsed = Convert.ToBoolean(item.ItemArray[12]);
                }
                else
                {
                    pc.IsUsed = false;
                }

                PCList.Add(pc);
            }
            return PCList;

        }


        //public List<PCRela> getPCMTempDataFor2Levels(DataTable PCData)
        //{
        //    List<PCRela> PCList = new List<PCRela>();

        //    foreach (DataRow item in PCData.Rows)
        //    {
        //        PCRela pc = new PCRela();

        //        pc.ISHCode = item[4].ToString();
        //        pc.MOCCode = item[4].ToString();
        //        pc.SSCC = item[10].ToString();
        //        PCList.Add(pc);
        //    }
        //    return PCList;

        //}
        public List<PCRela> getPCMTempData(DataTable PCData)
        {
            List<PCRela> PCList = new List<PCRela>();

            foreach (DataRow item in PCData.Rows)
            {
                PCRela pc = new PCRela();
                if (item[0].ToString() != item[5].ToString())
                {
                    pc.PALCode = item[5].ToString();
                }
                if (item[6].ToString() != "")
                {
                    pc.OSHCode = item[6].ToString();
                }
                pc.ISHCode = item[1].ToString();
                pc.OBXCode = item[2].ToString();
                pc.MOCCode = item[3].ToString();
                //pc.PPBCode = item[0].ToString();
                pc.SSCC = item[4].ToString();
                PCList.Add(pc);
            }
            return PCList;

        }

        private List<string> getPckgLvls(decimal JID)
        {
            List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);
            var lvl = LstJobDetails.Select(x => x.JD_Deckcode).Distinct().ToList();
            return lvl;
        }


        private DataSet getPCRptDS(decimal JID)
        {
            TnT_Ds dsTnT = new TnT_Ds();
            bool IsShowRpt = false;

            List<REDTR.DB.BusinessObjects.JobDetails> LstJobDetails = ObjHelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JID, -1);

            if (LstJobDetails.Count == 3)
            {
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

                // For PCMAP
                // For Delete the TempPCMAP
                ObjHelper.DBManager.PackagingDetailsBLL.DeleteTempPCMAP();

                //List<PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(Convert.ToInt32(m_JObID), "ISH", "NULL");

                List<string> lvls = LstJobDetails.Select(x => x.JD_Deckcode).ToList();


                if (lvls.Contains("PAL") && lvls.Contains("ISH") && lvls.Contains("MOC"))
                {
                    List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "PAL", "NULL");
                    // For ISH
                    for (int i = 0; i < PckISHCode.Count; i++)
                    {
                        List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", PckISHCode[i].Code);
                        for (int j = 0; j < PckOBXCode.Count; j++)
                        {
                            List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                            for (int k = 0; k < PckMOCCode.Count; k++)
                            {
                                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), string.Empty, PckISHCode[i].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckISHCode[i].SSCC);
                            }
                        }
                    }
                }
                else
                {
                    List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", "NULL");
                    // For ISH
                    for (int i = 0; i < PckISHCode.Count; i++)
                    {
                        List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OBX", PckISHCode[i].Code);
                        for (int j = 0; j < PckOBXCode.Count; j++)
                        {
                            List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                            for (int k = 0; k < PckMOCCode.Count; k++)
                            {
                                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), string.Empty, PckISHCode[i].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckISHCode[i].SSCC);
                            }
                        }
                    }
                }

                ////////////////////////////////////////////



                ////////////////////////////////////////////

                DataSet TempPCM = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetTempPCMAP, JID.ToString(), null);
                // End

                IsShowRpt = (dsPck.Tables[0].Rows.Count > 0);
                dsTnT.PackBoxesRelationship.Rows.Clear();
                if (IsShowRpt)
                {
                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                    dsTnT.TempPCMAP.Merge(TempPCM.Tables[0]);

                    return dsTnT;
                }
                else
                {
                    return null;
                }
            }
            else if (LstJobDetails.Count == 4)
            {
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

                // For PCMAP

                // For Delete the TempPCMAP
                ObjHelper.DBManager.PackagingDetailsBLL.DeleteTempPCMAP();

                List<REDTR.DB.BusinessObjects.PackagingDetails> PckPALCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "PAL", "NULL");
                // For ISH
                for (int i = 0; i < PckPALCode.Count; i++)
                {
                    // List<PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(Convert.ToInt32(m_JObID), "ISH", "NULL");
                    List<REDTR.DB.BusinessObjects.PackagingDetails> PckISHCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "ISH", PckPALCode[i].Code);

                    for (int m = 0; m < PckISHCode.Count; m++)
                    {
                        List<REDTR.DB.BusinessObjects.PackagingDetails> PckOBXCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "OBX", PckISHCode[m].Code);

                        // For OBX
                        for (int j = 0; j < PckOBXCode.Count; j++)
                        {
                            List<REDTR.DB.BusinessObjects.PackagingDetails> PckMOCCode = ObjHelper.DBManager.PackagingDetailsBLL.GetPCMAP(LstJobDetails.Count, Convert.ToInt32(JID), "MOC", PckOBXCode[j].Code);
                            for (int k = 0; k < PckMOCCode.Count; k++)
                            {
                                ObjHelper.DBManager.PackagingDetailsBLL.InsertTempPCMAP(Convert.ToInt32(JID), PckPALCode[i].Code, PckISHCode[m].Code, PckOBXCode[j].Code, PckMOCCode[k].Code, PckPALCode[i].SSCC);
                            }
                        }
                    }
                }
                DataSet TempPCM = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetTempPCMAP,
                JID.ToString(), null);
                // End

                IsShowRpt = (TempPCM.Tables[0].Rows.Count > 0);
                dsTnT.PackBoxesRelationship.Rows.Clear();
                if (IsShowRpt)
                {
                    dsTnT.PackBoxesRelationship.Merge(dsPck.Tables[0]);
                    dsTnT.TempPCMAP.Merge(TempPCM.Tables[0]);
                    return dsTnT;
                }
                else
                {
                    return null;
                }
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


        #endregion

        #region Rpt Job wise SSCC

        public string getJobWiseSSCCs(decimal JID, string UserName)
        {
            //var Job = db.Job.Find(JID);
            var data = "";
            RptJobWiseSSCCViewModel vm = new RptJobWiseSSCCViewModel();
            vm = getJobWiseSSCCsPDF(JID, UserName);
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //vm.job = Job;
            //var jobtype = db.JOBTypes.Where(x => x.TID == Job.TID).FirstOrDefault();
            //vm.Jobtype = jobtype.Job_Type;
            //vm.product = db.PackagingAsso.Find(Job.PAID);
            //vm.LineDetails = db.LineLocation.Find(Job.LineCode);

            //string tertiary = ProductPackageHelper.getTertiarryDeck(Job.PAID, JID);
            //DataSet ds = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPackSSCC, JID.ToString(), tertiary);


            //List<JobWiseSSCCc> lstSSCCs = new List<JobWiseSSCCc>();
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    JobWiseSSCCc obj = new JobWiseSSCCc();
            //    obj.PackagingDate = Convert.ToDateTime(row[20].ToString());
            //    obj.SSCC = row[11].ToString();
            //    obj.SSCCVerification = Convert.ToBoolean(row[12].ToString());
            //    lstSSCCs.Add(obj);
            //}
            //var sscclst = lstSSCCs.OrderBy(x => x.PackagingDate).ToList();
            //vm.SSCCs = sscclst;
            if (vm != null)
            {
                if (vm.SSCCs.Count <= 0)
                {
                    data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
                else
                {
                    data = ViewRenderer.RenderPartialView("~/Views/Reports/RptJobWiseSSCC.cshtml", vm);
                }
            }
            else
            {
                //data = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                data = "No Data";
            }
            return data;
        }


        public RptJobWiseSSCCViewModel getJobWiseSSCCsPDF(decimal JID, string UserName)
        {
            var Job = db.Job.Find(JID);
            var data = "";
            RptJobWiseSSCCViewModel vm = new RptJobWiseSSCCViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.job = Job;
            var jobtype = db.JOBTypes.Where(x => x.TID == Job.TID).FirstOrDefault();
            vm.Jobtype = jobtype.Job_Type;
            vm.product = db.PackagingAsso.Find(Job.PAID);
            vm.jbDetail = db.JobDetails.Where(x => x.JD_JobID == JID).FirstOrDefault();
            vm.LineDetails = db.LineLocation.Find(Job.LineCode);

            string tertiary = ProductPackageHelper.getTertiarryDeck(Job.PAID, JID);
            DataSet ds = ObjHelper.DBManager.PackagingDetailsBLL.GetDataSet(REDTR.DB.BLL.PackagingDetailsBLL.OP.GetPackSSCC, JID.ToString(), tertiary);


            List<JobWiseSSCCc> lstSSCCs = new List<JobWiseSSCCc>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                JobWiseSSCCc obj = new JobWiseSSCCc();
                obj.PackagingDate = Convert.ToDateTime(row[20].ToString());
                obj.SSCC = row[11].ToString();
                obj.SSCCVerification = Convert.ToBoolean(row[12].ToString());
                lstSSCCs.Add(obj);
            }
            var sscclst = lstSSCCs.OrderBy(x => x.PackagingDate).ToList();
            vm.SSCCs = sscclst;
            if (lstSSCCs.Count <= 0)
            {
                return null;
            }
            else
            {
                return vm;
            }
            return null;
        }

        #endregion

        #region Rpt Audit Trails

        public string getAuditTrailRpt(DateTime frmDt, DateTime toDt, DateTime frmSDate, DateTime toSDate, string Type, string lineCode, string UserName, string Uid, string Activity)
        {
            string Query = string.Empty;
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptAuditTrailsViewModel vm = new RptAuditTrailsViewModel();
            //vm = getAuditTrailRptPdf(frmDt, toDt, frmSDate, toSDate, Type, lineCode, UserName, Uid, Activity);

            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.FrmDt = frmDt;
            vm.ToDt = toDt;
            vm.lineLocation = lineCode;
            vm.Type = Type;
            vm.UID = Uid;
            DbHelper m_dbhelper = new DbHelper();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (Type == "Line")
            {
                vm.RptType = TnT.LangResource.GlobalRes.RptTrailLine;
                if (Activity != "")
                {
                    int activityId = Convert.ToInt32(Activity);

                    var activity = db.S_Activity.Where(x => x.Id == activityId).FirstOrDefault();

                    vm.Activity = activity.Activity;
                }

                string Where = string.Empty;

                if (Uid != "")
                {
                    int Userid = Convert.ToInt32(Uid);

                    Where = " and UserId=" + Userid;
                }
                if (Activity != "")
                {
                    Where = " and Actions=" + Convert.ToInt32(Activity);
                }
                if (Uid != "" && Activity != "")
                {
                    int Userid = Convert.ToInt32(Uid);
                    Where = " and UserId=" + Userid + " and Actions=" + Convert.ToInt32(Activity);
                }
                if (Where != "")
                {
                    Query = "select s.Activity,s.AccessedAt,u.UserName,r.Roles_Name,s.Reason from USerTrail s inner join Users u on u.ID=s.UserId Left Outer join roles r on r.ID=s.RoleID where AccessedAt>=CONVERT(datetime,'" + frmDt + "',103) and AccessedAt <= CONVERT(datetime,'" + toDt + "',103) " + Where + " and LineCode='" + lineCode + "' order by AccessedAt";
                }
                else
                {
                    Query = "select s.Activity,s.AccessedAt,u.UserName,r.Roles_Name,s.Reason from USerTrail s inner join Users u on u.ID=s.UserId Left Outer join roles r on r.ID=s.RoleID where AccessedAt>=CONVERT(datetime,'" + frmDt + "',103) and AccessedAt <= CONVERT(datetime,'" + toDt + "',103)  and LineCode='" + lineCode + "' order by AccessedAt";
                }

                DataSet ds = m_dbhelper.GetDataSet(Query);
                var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new ATrailings { UserName = dataRow.Field<string>("UserName"), Reason = dataRow.Field<string>("Reason").ToUpper(), UserType = dataRow.Field<string>("Roles_Name"), Time = dataRow.Field<DateTime>("AccessedAt"), Activity = dataRow.Field<string>("Activity") }).ToList();
                vm.Trails = empList;

                if (empList.Count > 0)
                {
                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptAuditTrails.cshtml", vm);
                }
                else
                {
                    RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }

            }
            else if (Type == "Server")
            {
                vm.Activity = Activity;
                vm.RptType = TnT.LangResource.GlobalRes.RptTrailServer;
                string where = "";
                if (Uid != "")
                {
                    int Userid = Convert.ToInt32(Uid);

                    where = " and UserId=" + Userid;
                }
                if (Activity != "")
                {
                    where = " and Activity like '%" + Activity + "%'";
                }
                if (Uid != "" && Activity != "")
                {
                    int Userid = Convert.ToInt32(Uid);
                    where = " and UserId=" + Userid + " and Activity like '%" + Activity + "%'";
                }
                if (where != "")
                {
                    Query = "select s.Message,s.ActitvityTime,u.UserName,r.Roles_Name from ServerSideTrails s inner join Users u on u.ID=s.UserId Left outer join Roles r on r.ID=s.RoleId  where ActitvityTime>=CONVERT(datetime,'" + frmDt + "',103) and ActitvityTime <= CONVERT(datetime,'" + toDt + "',103) " + where + " order by ActitvityTime";
                }
                else
                {
                    Query = "select s.Message,s.ActitvityTime,u.UserName,r.Roles_Name from ServerSideTrails s inner join Users u on u.ID=s.UserId Left outer join Roles r on r.ID=s.RoleId where ActitvityTime>=CONVERT(datetime,'" + frmDt + "',103) and ActitvityTime <= CONVERT(datetime,'" + toDt + "',103) order by ActitvityTime";
                }

                DataSet ds = m_dbhelper.GetDataSet(Query);
                //var trails = db.ServerSideTrails.Where(j => j.ActitvityTime >= frmSDate && j.ActitvityTime <= toSDate).ToList();
                //var UsersInfos = db.Users;
                //var UserRoles = db.Roles;

                var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new ATrailings { UserName = dataRow.Field<string>("UserName"), Reason = dataRow.Field<string>("Message").ToUpper(), UserType = dataRow.Field<string>("Roles_Name"), Time = dataRow.Field<DateTime>("ActitvityTime") }).ToList();



                //foreach (var item in trails)
                //{
                //    ATrailings trl = new ATrailings();
                //    if (item.UserId != 0)
                //    {
                //        var usr = UsersInfos.Find(item.UserId);
                //        var UserType = UserRoles.Find(usr.RoleID);

                //        if (usr != null && UserType != null)
                //        {
                //            trl.UserName = usr.UserName;
                //            trl.Time = item.ActitvityTime;
                //            int i = item.Message.IndexOf(" ") + 1;
                //            string strW = item.Message.Substring(i);
                //            trl.UserType = UserType.Roles_Name;
                //            //trl.Reason =usr.UserName+" "+ strW.ToUpper();
                //            trl.Reason = item.Message.ToUpper();


                //            Atrls.Add(trl);
                //        }
                //    }
                //    else
                //    {
                //        trl.UserName = "Unknown";
                //        trl.Time = item.ActitvityTime;
                //        trl.Reason = item.Message.ToUpper();
                //        trl.UserType = "Unknown";
                //        Atrls.Add(trl);
                //    }

                //}
                vm.Trails = empList;
                if (empList.Count > 0)
                {
                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptAuditTrails.cshtml", vm);
                }
                else
                {
                    RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            return RptData;
        }

        public RptAuditTrailsViewModel getAuditTrailRptPdf(DateTime frmDt, DateTime toDt, DateTime frmSDate, DateTime toSDate, string Type, string lineCode, string UserName, string Uid, string Activity)
        {
            string Query = string.Empty;
            int ActivityLine = 0;
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptAuditTrailsViewModel vm = new RptAuditTrailsViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.FrmDt = frmDt;
            vm.ToDt = toDt;
            vm.Type = Type;
            vm.UID = Uid;
            vm.lineLocation = lineCode;
            vm.Activity = Activity;
            DbHelper m_dbhelper = new DbHelper();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (Type == "Line")
            {
                vm.RptType = TnT.LangResource.GlobalRes.RptTrailLine;
                if (Activity != "")
                {


                    var activity = db.S_Activity.Where(x => x.Activity == Activity).FirstOrDefault();

                    vm.Activity = activity.Activity;
                    vm.ActivityId = Convert.ToString(activity.Id);
                }

                string Where = string.Empty;

                if (Uid != "")
                {
                    int Userid = Convert.ToInt32(Uid);

                    Where = " and UserId=" + Userid;
                }
                if (Activity != "")
                {
                    Where = " and Actions=" + Convert.ToInt32(vm.ActivityId);
                }
                if (Uid != "" && Activity != "")
                {
                    int Userid = Convert.ToInt32(Uid);
                    Where = " and UserId=" + Userid + " and Actions=" + Convert.ToInt32(vm.ActivityId);
                }
                if (Where != "")
                {
                    Query = "select s.Activity,s.AccessedAt,u.UserName,r.Roles_Name,s.Reason from USerTrail s inner join Users u on u.ID=s.UserId Left Outer join roles r on r.ID=s.RoleID where AccessedAt>=CONVERT(datetime,'" + frmDt + "',103) and AccessedAt <= CONVERT(datetime,'" + toDt + "',103) " + Where + " and LineCode='" + lineCode + "'";
                }
                else
                {
                    Query = "select s.Activity,s.AccessedAt,u.UserName,r.Roles_Name,s.Reason from USerTrail s inner join Users u on u.ID=s.UserId Left Outer join roles r on r.ID=s.RoleID where AccessedAt>=CONVERT(datetime,'" + frmDt + "',103) and AccessedAt <= CONVERT(datetime,'" + toDt + "',103)  and LineCode='" + lineCode + "'";
                }

                DataSet ds = m_dbhelper.GetDataSet(Query);
                var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new ATrailings { UserName = dataRow.Field<string>("UserName"), Reason = dataRow.Field<string>("Reason").ToUpper(), UserType = dataRow.Field<string>("Roles_Name"), Time = dataRow.Field<DateTime>("AccessedAt"), Activity = dataRow.Field<string>("Activity") }).ToList();
                vm.Trails = empList;

                if (empList.Count > 0)
                {
                    return vm;
                }
                else
                {
                    return null;
                }

            }
            else if (Type == "Server")
            {
                vm.Activity = Activity;
                vm.RptType = TnT.LangResource.GlobalRes.RptTrailServer;
                string where = "";
                if (Uid != "")
                {
                    int Userid = Convert.ToInt32(Uid);

                    where = " and UserId=" + Userid;
                }
                if (Activity != "")
                {
                    where = " and Activity like '%" + Activity + "%'";
                }
                if (Uid != "" && Activity != "")
                {
                    int Userid = Convert.ToInt32(Uid);
                    where = " and UserId=" + Userid + " and Activity like '%" + Activity + "%'";
                }
                if (where != "")
                {
                    Query = "select s.Message,s.ActitvityTime,u.UserName,r.Roles_Name from ServerSideTrails s inner join Users u on u.ID=s.UserId Left outer join Roles r on r.ID=s.RoleId  where ActitvityTime>=CONVERT(datetime,'" + frmDt + "',103) and ActitvityTime <= CONVERT(datetime,'" + toDt + "',103) " + where + "";
                }
                else
                {
                    Query = "select s.Message,s.ActitvityTime,u.UserName,r.Roles_Name from ServerSideTrails s inner join Users u on u.ID=s.UserId Left outer join Roles r on r.ID=s.RoleId where ActitvityTime>=CONVERT(datetime,'" + frmDt + "',103) and ActitvityTime <= CONVERT(datetime,'" + toDt + "',103) ";
                }

                DataSet ds = m_dbhelper.GetDataSet(Query);


                var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new ATrailings { UserName = dataRow.Field<string>("UserName"), Reason = dataRow.Field<string>("Message").ToUpper(), UserType = dataRow.Field<string>("Roles_Name"), Time = dataRow.Field<DateTime>("ActitvityTime") }).ToList();

                vm.Trails = empList;
                if (empList.Count > 0)
                {
                    return vm;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        #endregion

        #region Rpt User Details
        public string getUserDetail(string status, string UserName)
        {
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptUserViewModel vm = new RptUserViewModel();
            vm = getUserDetailPdf(status, UserName);
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //DbHelper m_dbhelper = new DbHelper();
            //string Where = string.Empty;
            //if (status == "Active")
            //{
            //    Where = "Active=1";
            //    vm.ReportType = true;
            //}
            //else
            //{
            //    Where = "Active=0 and  Users.ID!=0 ";
            //    vm.ReportType = false;
            //}
            //string Query = "select UserName1,CreatedDate,Active,Roles_Name,LastUpdatedDate from Users inner join Roles on Roles.ID=USERs.RoleID where " + Where + "order by UserName asc";

            //DataSet ds = m_dbhelper.GetDataSet(Query);
            //var userdata = ds.Tables[0];
            //List<UDetail> user = new List<UDetail>();
            //foreach (var item in userdata.Rows)
            //{
            //    UDetail ur = new UDetail();
            //    ur.UserName = ((System.Data.DataRow)item).ItemArray[0].ToString();
            //    ur.Status = status;
            //    ur.RoleName = ((System.Data.DataRow)item).ItemArray[3].ToString();
            //    CultureInfo enUS = new CultureInfo("en-US");

            //    ur.CreatedDate = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[1]);
            //    ur.LastUpdated = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[4]);
            //    user.Add(ur);
            //}
            //vm.UsersDetail = user;
            if (vm != null)
            {
                if (vm.UsersDetail.Count > 0)
                {
                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptUserDetails.cshtml", vm);
                }
                else
                {
                    RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }


            return RptData;
        }

        public RptUserViewModel getUserDetailPdf(string status, string UserName)
        {

            RptUserViewModel vm = new RptUserViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            vm.Status = status;
            DbHelper m_dbhelper = new DbHelper();
            string Where = string.Empty;
            if (status == "Active")
            {
                Where = "Active=1";
                vm.ReportType = true;
            }
            else
            {
                Where = "Active=0 and  Users.ID!=0 ";
                vm.ReportType = false;
            }
            string Query = "select UserName,UserName1,CreatedDate,Active,Roles_Name,LastUpdatedDate from Users inner join Roles on Roles.ID=USERs.RoleID where " + Where + "order by UserName asc";

            DataSet ds = m_dbhelper.GetDataSet(Query);
            var userdata = ds.Tables[0];
            List<UDetail> user = new List<UDetail>();
            foreach (var item in userdata.Rows)
            {
                UDetail ur = new UDetail();
                ur.UserName = ((System.Data.DataRow)item).ItemArray[1].ToString();
                ur.UserId = ((System.Data.DataRow)item).ItemArray[0].ToString();
                ur.Status = status;
                ur.RoleName = ((System.Data.DataRow)item).ItemArray[4].ToString();
                CultureInfo enUS = new CultureInfo("en-US");

                ur.CreatedDate = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[2]);
                ur.LastUpdated = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[5]);
                user.Add(ur);
            }
            vm.UsersDetail = user;
            if (user.Count > 0)
            {
                return vm;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Rpt Dava Generation Status
        public string getDavaStatus(string rptType, string status, string Username)
        {

            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptDavaViewModel vm = new RptDavaViewModel();
            vm = getDavaStatusPdf(rptType, status, Username);
            if (vm != null)
            {
                if (vm.DavaData.Count > 0)
                {
                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptDavaDetails.cshtml", vm);
                }
                else
                {
                    RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                }
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            //            DataSet ds;
            //            int done = 0;
            //            string query = string.Empty;
            //            vm.CompanyName = CompanyName;
            //            vm.Address = CompanyAddrs;
            //            vm.UserNAme = Username;
            //            DbHelper m_dbhelper = new DbHelper();
            //            vm.ReportType = rptType;
            //            if (rptType == "Product")
            //            {
            //                if (status == "Generated")
            //                {
            //                    query = "select * from PackagingAsso where DAVAPortalUpload = 1 ";
            //                }
            //                else
            //                {
            //                    query = "select * from PackagingAsso where DAVAPortalUpload = 0";
            //                }
            //            }
            //            else
            //            {
            //                if (status == "Generated" || status == "Partial")
            //                {
            //                    //                    query = "Select BatchNo,JID from Vw_GetBatchForDAVA"
            //                    //+ " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 1) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null)";
            //                    if (status == "Partial")
            //                    {
            //                        query = "   Select BatchNo, JID from Vw_GetBatchForDAVA where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 0 or PackagingDetails.DavaPortalUpload is null) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null) ";
            //                    }
            //                    else if (status == "Generated")
            //                    {
            //                        query = "   Select BatchNo, JID from Vw_GetBatchForDAVA where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 1) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null) ";
            //                    }
            //                }
            //                else if (status == "NotGenerated")
            //                {
            //                    query = "Select BatchNo,JID from Vw_GetBatchForDAVA"
            //+ " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 0 or PackagingDetails.DavaPortalUpload is null) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null and IsUsed=1)";
            //                }

            //            }
            //            ds = m_dbhelper.GetDataSet(query);
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                List<DavaData> lstdata = new List<DavaData>();
            //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //                {
            //                    DavaData da = new DavaData();
            //                    if (rptType == "Product")
            //                    {
            //                        da.Name = ds.Tables[0].Rows[i]["Name"].ToString();
            //                        if (status == "Generated")
            //                        {
            //                            da.Status = status;
            //                        }
            //                        else
            //                        {
            //                            da.Status = "Not Generated";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        string TertiaryDeck = "";
            //                        int jid = Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]);
            //                        var pdata = db.Job.Where(x => x.JID == jid).FirstOrDefault();
            //                        TertiaryDeck = ProductPackageHelper.getTertiarryDeck(pdata.PAID, pdata.JID);
            //                        DataSet ds1;
            //                        query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and (DavaPortalUpload=0 or DavaPortalUpload is null) and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
            //                        ds1 = m_dbhelper.GetDataSet(query);
            //                        if (status != "NotGenerated")
            //                        {

            //                            query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
            //                            DataSet ds2 = m_dbhelper.GetDataSet(query);

            //                            if (status == "Generated")
            //                            {
            //                                if (ds1.Tables[0].Rows.Count == 0)
            //                                {
            //                                    da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
            //                                    da.Status = status;
            //                                    da.Qty = ds2.Tables[0].Rows.Count;
            //                                }
            //                            }
            //                            else if (status == "Partial")
            //                            {
            //                                if (ds1.Tables[0].Rows.Count > 0)
            //                                {

            //                                    query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and DavaPortalUpload=1 and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
            //                                    DataSet ds3 = m_dbhelper.GetDataSet(query);
            //                                    if (ds3.Tables[0].Rows.Count > 0)
            //                                    {
            //                                        da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
            //                                        da.Status = status;
            //                                        da.Qty = ds2.Tables[0].Rows.Count;
            //                                        da.SSCCDone = ds3.Tables[0].Rows.Count;
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and DavaPortalUpload=1 and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
            //                            DataSet ds3 = m_dbhelper.GetDataSet(query);
            //                            if (ds3.Tables[0].Rows.Count == 0)
            //                            {
            //                                da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
            //                                da.Qty = ds1.Tables[0].Rows.Count;
            //                                da.Status = "Not Generated";
            //                            }
            //                        }
            //                    }
            //                    if (da.Name != null)
            //                    {
            //                        lstdata.Add(da);
            //                    }
            //                }
            //                vm.DavaData = lstdata;
            //                if (lstdata.Count > 0)
            //                {
            //                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptDavaDetails.cshtml", vm);
            //                }
            //                else
            //                {
            //                    RptData = "No Data";
            //                }
            //            }
            //            else
            //            {
            //                RptData = "No Data";
            //            }
            return RptData;
        }


        public RptDavaViewModel getDavaStatusPdf(string rptType, string status, string Username)
        {
            DataSet ds;
            int done = 0;
            string query = string.Empty;
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            RptDavaViewModel vm = new RptDavaViewModel();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserNAme = Username;
            DbHelper m_dbhelper = new DbHelper();
            vm.ReportType = rptType;
            vm.stat = status;
            if (rptType == "Product")
            {
                if (status == "Generated")
                {
                    query = "select * from PackagingAsso where DAVAPortalUpload = 1 ";
                }
                else
                {
                    query = "select * from PackagingAsso where DAVAPortalUpload = 0";
                }
            }
            else
            {
                if (status == "Generated" || status == "Partial")
                {
                    //                    query = "Select BatchNo,JID from Vw_GetBatchForDAVA"
                    //+ " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 1) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null)";
                    if (status == "Partial")
                    {
                        query = "   Select BatchNo, JID from Vw_GetBatchForDAVA where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 0 or PackagingDetails.DavaPortalUpload is null) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null) ";
                    }
                    else if (status == "Generated")
                    {
                        query = "   Select BatchNo, JID from Vw_GetBatchForDAVA where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 1) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null) ";
                    }
                }
                else if (status == "NotGenerated")
                {
                    query = "Select BatchNo,JID from Vw_GetBatchForDAVA"
                    + " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 0 or PackagingDetails.DavaPortalUpload is null) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null and IsUsed=1)";
                }

            }
            ds = m_dbhelper.GetDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<DavaData> lstdata = new List<DavaData>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DavaData da = new DavaData();
                    if (rptType == "Product")
                    {
                        da.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        if (status == "Generated")
                        {
                            da.Status = status;
                        }
                        else
                        {
                            da.Status = "Not Generated";
                        }
                    }
                    else
                    {
                        string TertiaryDeck = "";
                        int jid = Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]);
                        var pdata = db.Job.Where(x => x.JID == jid).FirstOrDefault();
                        TertiaryDeck = ProductPackageHelper.getTertiarryDeck(pdata.PAID, pdata.JID);
                        DataSet ds1;
                        query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and (DavaPortalUpload=0 or DavaPortalUpload is null) and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
                        ds1 = m_dbhelper.GetDataSet(query);
                        if (status != "NotGenerated")
                        {

                            query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
                            DataSet ds2 = m_dbhelper.GetDataSet(query);

                            if (status == "Generated")
                            {
                                if (ds1.Tables[0].Rows.Count == 0)
                                {
                                    da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
                                    da.Status = status;
                                    da.Qty = ds2.Tables[0].Rows.Count;
                                }
                            }
                            else if (status == "Partial")
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {

                                    query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and DavaPortalUpload=1 and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
                                    DataSet ds3 = m_dbhelper.GetDataSet(query);
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
                                        da.Status = status;
                                        da.Qty = ds2.Tables[0].Rows.Count;
                                        da.SSCCDone = ds3.Tables[0].Rows.Count;
                                    }
                                }
                            }
                        }
                        else
                        {
                            query = "select * from PackagingDetails where JobID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["JID"]) + " and SSCC is not null and DavaPortalUpload=1 and PackageTypeCode='" + TertiaryDeck + "' and IsUsed=1";
                            DataSet ds3 = m_dbhelper.GetDataSet(query);
                            if (ds3.Tables[0].Rows.Count == 0)
                            {
                                da.Name = ds.Tables[0].Rows[i]["BatchNo"].ToString();
                                da.Qty = ds1.Tables[0].Rows.Count;
                                da.Status = "Not Generated";
                            }
                        }
                    }
                    if (da.Name != null)
                    {
                        lstdata.Add(da);
                    }
                }
                vm.DavaData = lstdata;
                if (lstdata.Count > 0)
                {
                    return vm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Rpt UID Detail
        public string getUIDDetail(string detail, string Type, string UserName)
        {

            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            if (Type != "--Select--")
            {
                if (detail != "")
                {
                    string mReason = string.Empty;
                    string mInspectionSkipped = string.Empty;
                    DataSet ds;
                    RptUIDDetailViewModel vm = new RptUIDDetailViewModel();
                    vm.CompanyName = CompanyName;
                    vm.Address = CompanyAddrs;
                    vm.UserName = UserName;
                    vm.Type = Type;
                    DbHelper m_dbhelper = new DbHelper();
                    string where = string.Empty;

                    PackagingDetails pkdetail;
                    if (Type == "UID")
                    {
                        pkdetail = db.PackagingDetails.Where(x => x.Code == detail).FirstOrDefault();
                    }
                    else
                    {
                        pkdetail = db.PackagingDetails.Where(x => x.SSCC.Contains(detail)).FirstOrDefault();
                    }
                    var jobdeatil = db.Job.Where(x => x.JID == pkdetail.JobID).FirstOrDefault();
                    var jd = db.JobDetails.Where(x => x.JD_JobID == pkdetail.JobID && x.JD_Deckcode == pkdetail.PackageTypeCode).FirstOrDefault();
                    var packAsso = db.PackagingAsso.Where(x => x.PAID == pkdetail.PAID).FirstOrDefault();
                    var linedetail = db.LineLocation.Where(x => x.ID == jobdeatil.LineCode).FirstOrDefault();
                    var oper = db.Users.Where(x => x.ID == pkdetail.OperatorId).FirstOrDefault();
                    var jobtype = db.JOBTypes.Where(x => x.TID == jobdeatil.TID).FirstOrDefault();
                    vm.JobType = jobtype.Job_Type;
                    vm.JID = Convert.ToInt32(pkdetail.JobID);
                    vm.TID = jobdeatil.TID;
                    vm.BPR = jobdeatil.JobName;
                    vm.BatchNo = jobdeatil.BatchNo;
                    vm.ProductName = jd.JD_ProdName;
                    vm.ProductCode = jd.JD_ProdName; //packAsso.ProductCode;
                    vm.MfgDate = Convert.ToDateTime(jobdeatil.MfgDate);
                    vm.ExpDate = Convert.ToDateTime(jobdeatil.ExpDate);
                    vm.BatchQty = Convert.ToInt32(jobdeatil.Quantity);
                    vm.JobWithUid = Convert.ToString(jobdeatil.JobWithUID);
                    vm.LocationCode = linedetail.LocationCode;
                    vm.DivCode = linedetail.DivisionCode;
                    vm.PlantCode = linedetail.PlantCode;
                    vm.LineCode = linedetail.LineCode;
                    vm.UIDCode = pkdetail.Code;
                    vm.PackagingType = pkdetail.PackageTypeCode;
                    vm.PackagingDate = Convert.ToDateTime(pkdetail.LastUpdatedDate);

                    if (oper != null)
                    {
                        vm.Operator = oper.UserName;
                    }
                    if (pkdetail.IsRejected == false && pkdetail.IsDecomission == false)
                    {
                        vm.Status = "Good";
                    }
                    else
                    {
                        if (pkdetail.IsDecomission == true)
                            vm.Status = "Decommissioned";
                        else
                            vm.Status = "Bad";
                    }
                    if (Type == "UID")
                    {
                        string code = detail;
                        for (int i = 0; i < 4; i++)
                        {
                            var next = db.PackagingDetails.Where(x => x.Code == code).Single();
                            if (!string.IsNullOrEmpty(next.SSCC))
                            {
                                vm.SSCC = next.SSCC;
                                break;
                            }
                            else if (next.NextLevelCode == "FFFFF")
                            {
                                break;
                            }
                            code = next.NextLevelCode;
                        }
                    }
                    else
                    {
                        vm.SSCC = pkdetail.SSCC;
                    }
                    if (vm.SSCC != null)
                    {
                        var SSCCVerifystatus = db.PackagingDetails.Where(x => x.SSCC.Contains(vm.SSCC)).FirstOrDefault();
                        if (SSCCVerifystatus != null)
                        {
                            vm.SSCCVerifiedStatus = Convert.ToString(SSCCVerifystatus.SSCCVarificationStatus);
                        }
                    }
                    vm.CaseNo = Convert.ToString(pkdetail.CaseSeqNum);

                    vm.parentCode = pkdetail.NextLevelCode;
                    string EncryptedData = detail;
                    if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                    DataSet ds1 = m_dbhelper.DBManager.PackagingDetailsBLL.GetDataSet(PackagingDetailsBLL.OP.GETDS_FailureReasonOfCode, vm.UIDCode, null);


                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        float mType = 0;
                        float.TryParse(ds1.Tables[0].Rows[i][2].ToString(), out mType);
                        if (ds1.Tables[0].Rows[i][3].ToString() == "False" && ds1.Tables[0].Rows[i][4].ToString() == "True")
                            mReason += InspectionFailed.RetTypeDesc(mType) + ",";// +"\n";

                        if (ds1.Tables[0].Rows[i][3].ToString() == "False" && ds1.Tables[0].Rows[i][4].ToString() == "False")
                            mInspectionSkipped += InspectionFailed.RetTypeDesc(mType) + ",";
                    }
                    if (!string.IsNullOrEmpty(mReason))
                        mReason = mReason.Remove(mReason.Length - 1, 1);

                    vm.FailureReason = mReason;
                    if (!string.IsNullOrEmpty(mInspectionSkipped))
                        mInspectionSkipped = mInspectionSkipped.Remove(mInspectionSkipped.Length - 1, 1);
                    vm.InspectionSet = mInspectionSkipped;

                    var gtin = db.JobDetails.Where(x => x.JD_JobID == vm.JID && x.JD_Deckcode == vm.PackagingType).FirstOrDefault();
                    vm.GTIN = gtin.JD_GTIN;
                    var childcode = db.PackagingDetails.Where(x => x.JobID == vm.JID && x.NextLevelCode == vm.UIDCode).ToList();
                    if (childcode != null)
                    {
                        List<ChildCode> lst = new List<ChildCode>();
                        for (int i = 0; i < childcode.Count(); i++)
                        {
                            ChildCode ch = new ChildCode();
                            ch.SrNo = childcode[i].Code;
                            lst.Add(ch);
                        }
                        vm.ChildCode = lst;
                    }

                    RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptUidValidation.cshtml", vm);
                }
                else
                {
                    RptData = TnT.LangResource.GlobalRes.toastrRptuIdDetailsEnterdata;
                }
            }
            else
            {
                RptData = "Please Select type";
            }
            //}
            //else
            //{
            //    RptData = "No Data";
            //}
            return RptData;
        }

        public RptUIDDetailViewModel getUIDDetailPdf(string detail, string Type, string UserName)
        {


            if (Type != "--Select--")
            {
                if (detail != "")
                {
                    string mReason = string.Empty;
                    string mInspectionSkipped = string.Empty;
                    DataSet ds;
                    RptUIDDetailViewModel vm = new RptUIDDetailViewModel();
                    vm.CompanyName = CompanyName;
                    vm.Address = CompanyAddrs;
                    vm.UserName = UserName;
                    vm.Type = Type;
                    DbHelper m_dbhelper = new DbHelper();
                    string where = string.Empty;

                    PackagingDetails pkdetail;
                    if (Type == "UID")
                    {
                        pkdetail = db.PackagingDetails.Where(x => x.Code == detail).FirstOrDefault();
                    }
                    else
                    {
                        pkdetail = db.PackagingDetails.Where(x => x.SSCC.Contains(detail)).FirstOrDefault();
                    }
                    var jobdeatil = db.Job.Where(x => x.JID == pkdetail.JobID).FirstOrDefault();
                    var packAsso = db.PackagingAsso.Where(x => x.PAID == pkdetail.PAID).FirstOrDefault();
                    var linedetail = db.LineLocation.Where(x => x.ID == jobdeatil.LineCode).FirstOrDefault();
                    var oper = db.Users.Where(x => x.ID == pkdetail.OperatorId).FirstOrDefault();
                    var jobtype = db.JOBTypes.Where(x => x.TID == jobdeatil.TID).FirstOrDefault();
                    vm.JobType = jobtype.Job_Type;
                    vm.JID = Convert.ToInt32(pkdetail.JobID);
                    vm.TID = jobdeatil.TID;
                    vm.BPR = jobdeatil.JobName;
                    vm.BatchNo = jobdeatil.BatchNo;
                    vm.ProductName = packAsso.Name;
                    vm.ProductCode = packAsso.ProductCode;
                    vm.MfgDate = Convert.ToDateTime(jobdeatil.MfgDate);
                    vm.ExpDate = Convert.ToDateTime(jobdeatil.ExpDate);
                    vm.BatchQty = Convert.ToInt32(jobdeatil.Quantity);
                    vm.JobWithUid = Convert.ToString(jobdeatil.JobWithUID);
                    vm.LocationCode = linedetail.LocationCode;
                    vm.DivCode = linedetail.DivisionCode;
                    vm.PlantCode = linedetail.PlantCode;
                    vm.LineCode = linedetail.LineCode;
                    vm.UIDCode = pkdetail.Code;
                    vm.PackagingType = pkdetail.PackageTypeCode;
                    vm.PackagingDate = Convert.ToDateTime(pkdetail.LastUpdatedDate);

                    if (oper != null)
                    {
                        vm.Operator = oper.UserName;
                    }
                    if (pkdetail.IsRejected == false && pkdetail.IsDecomission == false)
                    {
                        vm.Status = "Good";
                    }
                    else
                    {
                        if (pkdetail.IsDecomission == true)
                            vm.Status = "Decommissioned";
                        else
                            vm.Status = "Bad";
                    }
                    if (Type == "UID")
                    {
                        string code = detail;
                        for (int i = 0; i < 4; i++)
                        {
                            var next = db.PackagingDetails.Where(x => x.Code == code).Single();
                            if (!string.IsNullOrEmpty(next.SSCC))
                            {
                                vm.SSCC = next.SSCC;
                                break;
                            }
                            else if (next.NextLevelCode == "FFFFF")
                            {
                                break;
                            }
                            code = next.NextLevelCode;
                        }
                    }
                    else
                    {
                        vm.SSCC = pkdetail.SSCC;
                    }
                    if (vm.SSCC != null)
                    {
                        var SSCCVerifystatus = db.PackagingDetails.Where(x => x.SSCC.Contains(vm.SSCC)).FirstOrDefault();
                        if (SSCCVerifystatus != null)
                        {
                            vm.SSCCVerifiedStatus = Convert.ToString(SSCCVerifystatus.SSCCVarificationStatus);
                        }
                    }
                    vm.CaseNo = Convert.ToString(pkdetail.CaseSeqNum);

                    vm.parentCode = pkdetail.NextLevelCode;
                    string EncryptedData = detail;
                    if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                    DataSet ds1 = m_dbhelper.DBManager.PackagingDetailsBLL.GetDataSet(PackagingDetailsBLL.OP.GETDS_FailureReasonOfCode, vm.UIDCode, null);


                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        float mType = 0;
                        float.TryParse(ds1.Tables[0].Rows[i][2].ToString(), out mType);
                        if (ds1.Tables[0].Rows[i][3].ToString() == "False" && ds1.Tables[0].Rows[i][4].ToString() == "True")
                            mReason += InspectionFailed.RetTypeDesc(mType) + ",";// +"\n";

                        if (ds1.Tables[0].Rows[i][3].ToString() == "False" && ds1.Tables[0].Rows[i][4].ToString() == "False")
                            mInspectionSkipped += InspectionFailed.RetTypeDesc(mType) + ",";
                    }
                    if (!string.IsNullOrEmpty(mReason))
                        mReason = mReason.Remove(mReason.Length - 1, 1);

                    vm.FailureReason = mReason;
                    if (!string.IsNullOrEmpty(mInspectionSkipped))
                        mInspectionSkipped = mInspectionSkipped.Remove(mInspectionSkipped.Length - 1, 1);
                    vm.InspectionSet = mInspectionSkipped;

                    var gtin = db.JobDetails.Where(x => x.JD_JobID == vm.JID && x.JD_Deckcode == vm.PackagingType).FirstOrDefault();
                    vm.GTIN = gtin.JD_GTIN;
                    var childcode = db.PackagingDetails.Where(x => x.JobID == vm.JID && x.NextLevelCode == vm.UIDCode).ToList();
                    if (childcode != null)
                    {
                        List<ChildCode> lst = new List<ChildCode>();
                        for (int i = 0; i < childcode.Count(); i++)
                        {
                            ChildCode ch = new ChildCode();
                            ch.SrNo = childcode[i].Code;
                            lst.Add(ch);
                        }
                        vm.ChildCode = lst;
                    }

                    return vm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            //}
            //else
            //{
            //    RptData = "No Data";
            //}

        }

        private void FailureReasonAndInspectionDtls(string Code, bool IsRejected)
        {
            try
            {
                DbHelper m_dbhelper = new DbHelper();
                string mReason = string.Empty;
                string mInspectionSkipped = string.Empty;
                string EncryptedData = Code;
                if (EncryptedData != null) { EncryptedData = AESCryptor.Encrypt(EncryptedData, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
                DataSet ds = m_dbhelper.DBManager.PackagingDetailsBLL.GetDataSet(PackagingDetailsBLL.OP.GETDS_FailureReasonOfCode, EncryptedData, null);
                TnT_Ds TrackDs = new TnT_Ds();
                TrackDs.InspectionDetails.Merge(ds.Tables[0]);

                for (int i = 0; i < TrackDs.InspectionDetails.Rows.Count; i++)
                {
                    float mType = 0;
                    float.TryParse(TrackDs.InspectionDetails[i].TYPE.ToString(), out mType);
                    if (TrackDs.InspectionDetails[i].SKIP == false && TrackDs.InspectionDetails[i].ISFAILED == true && (IsRejected == true))
                        mReason += InspectionFailed.RetTypeDesc(mType) + ",";// +"\n";

                    if (TrackDs.InspectionDetails[i].SKIP == false && TrackDs.InspectionDetails[i].ISFAILED == false)
                        mInspectionSkipped += InspectionFailed.RetTypeDesc(mType) + ",";
                }
                if (!string.IsNullOrEmpty(mReason))
                    mReason = mReason.Remove(mReason.Length - 1, 1);
                mReason += " Failed";

                if (!string.IsNullOrEmpty(mInspectionSkipped))
                    mInspectionSkipped = mInspectionSkipped.Remove(mInspectionSkipped.Length - 1, 1);
            }
            catch (Exception ex)
            {
                Trace.TraceError("FailureReasonAndInspectionDtls{0},{1},{2}", DateTime.Now, ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region AvialbleSerialNo Count
        public string getSerailCount(string UserName)
        {
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;

            RptAvailableSerialNo vm = new RptAvailableSerialNo();
            vm = getSerailCountPdf(UserName);
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //DbHelper m_dbhelper = new DbHelper();
            //string Query = "select distinct(x.GTIN) as GTIN,count(x.SerialNo) as serialNumber from X_TracelinkUIDStore X inner join M_TracelinkRequest M on x.TLRequestId=m.Id where x.IsUsed=0 and m.ProviderId=2  group by x.GTIN";
            //DataSet ds = m_dbhelper.GetDataSet(Query);
            //var SerialNodata = ds.Tables[0];
            //List<SerailNo> SrNo = new List<SerailNo>();
            //foreach (var item in SerialNodata.Rows)
            //{
            //    SerailNo sr = new SerailNo();
            //    sr.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
            //    sr.SerailNoCount = ((System.Data.DataRow)item).ItemArray[1].ToString();
            //    SrNo.Add(sr);
            //}
            //vm.SrNoCount = SrNo;
            if (vm.SrNoCount.Count > 0)
            {
                RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptAvailableSerialNo.cshtml", vm);
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            return RptData;
        }
        public RptAvailableSerialNo getSerailCountPdf(string UserName)
        {


            RptAvailableSerialNo vm = new RptAvailableSerialNo();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            DbHelper m_dbhelper = new DbHelper();
            string Query = "select distinct(x.GTIN) as GTIN,count(x.SerialNo) as serialNumber from X_TracelinkUIDStore X inner join M_TracelinkRequest M on x.TLRequestId=m.Id where x.IsUsed=0 and m.ProviderId=2  group by x.GTIN";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            var SerialNodata = ds.Tables[0];
            List<SerailNo> SrNo = new List<SerailNo>();
            foreach (var item in SerialNodata.Rows)
            {
                SerailNo sr = new SerailNo();
                sr.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
                sr.SerailNoCount = ((System.Data.DataRow)item).ItemArray[1].ToString();
                SrNo.Add(sr);
            }
            vm.SrNoCount = SrNo;
            if (SrNo.Count > 0)
            {
                return vm;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Tracelink request report
        public string GetTlinkRequest(string UserName)
        {
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;

            RptTlinkRequest vm = new RptTlinkRequest();
            vm = GetTlinkRequestPdf(UserName);
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //DbHelper m_dbhelper = new DbHelper();
            //string Query = "select t.GTIN,t.Quatity,t.RequestedOn,c.CompanyName  from M_TracelinkRequest t inner join M_Customer c on t.CustomerId=c.Id where t.ProviderId=2";
            //DataSet ds = m_dbhelper.GetDataSet(Query);
            //var RequestedData = ds.Tables[0];
            //List<RequestedGtin> Rdata = new List<RequestedGtin>();
            //foreach (var item in RequestedData.Rows)
            //{
            //    RequestedGtin rp = new RequestedGtin();
            //    rp.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
            //    rp.Quantity = Convert.ToInt32(((System.Data.DataRow)item).ItemArray[1]);
            //    rp.RequestedOn = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[2]);
            //    rp.Customer = ((System.Data.DataRow)item).ItemArray[3].ToString();
            //    Rdata.Add(rp);
            //}
            //vm.RGTIN = Rdata;
            if (vm.RGTIN.Count() > 0)
            {

                RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptTlinkRequest.cshtml", vm);
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            return RptData;
        }

        public RptTlinkRequest GetTlinkRequestPdf(string UserName)
        {

            RptTlinkRequest vm = new RptTlinkRequest();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            DbHelper m_dbhelper = new DbHelper();
            string Query = "select t.GTIN,t.Quatity,t.RequestedOn,c.CompanyName  from M_TracelinkRequest t inner join M_Customer c on t.CustomerId=c.Id where t.ProviderId=2";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            var RequestedData = ds.Tables[0];
            List<RequestedGtin> Rdata = new List<RequestedGtin>();
            foreach (var item in RequestedData.Rows)
            {
                RequestedGtin rp = new RequestedGtin();
                rp.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
                rp.Quantity = Convert.ToInt32(((System.Data.DataRow)item).ItemArray[1]);
                rp.RequestedOn = Convert.ToDateTime(((System.Data.DataRow)item).ItemArray[2]);
                rp.Customer = ((System.Data.DataRow)item).ItemArray[3].ToString();
                Rdata.Add(rp);
            }
            vm.RGTIN = Rdata;
            if (Rdata.Count() > 0)
            {

                return vm;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region UnusedSrNo
        public string GetUnusedSrno(string UserName)
        {
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;

            RptUnusedSrNo vm = new RptUnusedSrNo();
            vm = GetUnusedSrnoPdf(UserName);
            //vm.CompanyName = CompanyName;
            //vm.Address = CompanyAddrs;
            //vm.UserName = UserName;
            //DbHelper m_dbhelper = new DbHelper();
            //string Query = "select x.GTIN,x.SerialNo from X_TracelinkUIDStore x inner join M_TracelinkRequest m on x.TLRequestId=m.Id where x.IsUsed=0 and m.ProviderId=2 ";
            //DataSet ds = m_dbhelper.GetDataSet(Query);
            //var UnusedData = ds.Tables[0];
            //List<UnusedSrno> Udata = new List<UnusedSrno>();
            //foreach (var item in UnusedData.Rows)
            //{
            //    UnusedSrno rp = new UnusedSrno();
            //    rp.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
            //    rp.SrNo = ((System.Data.DataRow)item).ItemArray[1].ToString();

            //    Udata.Add(rp);
            //}
            //vm.UnSrNo = Udata;
            if (vm.UnSrNo.Count() > 0)
            {

                RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptUnusedSrNo.cshtml", vm);
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
            return RptData;
        }

        public RptUnusedSrNo GetUnusedSrnoPdf(string UserName)
        {


            RptUnusedSrNo vm = new RptUnusedSrNo();
            vm.CompanyName = CompanyName;
            vm.Address = CompanyAddrs;
            vm.UserName = UserName;
            DbHelper m_dbhelper = new DbHelper();
            string Query = "select x.GTIN,x.SerialNo from X_TracelinkUIDStore x inner join M_TracelinkRequest m on x.TLRequestId=m.Id where x.IsUsed=0 and m.ProviderId=2 ";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            var UnusedData = ds.Tables[0];
            List<UnusedSrno> Udata = new List<UnusedSrno>();
            foreach (var item in UnusedData.Rows)
            {
                UnusedSrno rp = new UnusedSrno();
                rp.GTIN = ((System.Data.DataRow)item).ItemArray[0].ToString();
                rp.SrNo = ((System.Data.DataRow)item).ItemArray[1].ToString();

                Udata.Add(rp);
            }
            vm.UnSrNo = Udata;
            if (Udata.Count() > 0)
            {

                return vm;
            }
            else
            {
                return null;
            }

        }
        #endregion
    }
}

public class Packagetpe
{
    public string phk { get; set; }
}
