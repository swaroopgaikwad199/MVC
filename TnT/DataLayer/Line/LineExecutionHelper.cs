using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TnT.Models;
using TnT.Models.Home;
using TnT.Models.Job;

namespace TnT.DataLayer.Line
{
    // 0 : Created, 1 : Running, 2 : Paused, 3: Closed, 4 : Decommissioned, 5: LineClearance,  6: CompleteTransfer, 7: ForcefullyBatchClose
    // 404 : SqlServerError , 101 : Not Reachable, 102 : Idle , 103 : No Data in ExecutionDataTbl
    public class LineExecutionHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public LineChartViewModel getLineChartExecution(string LineLocId)
        {
            var Exstingline = GetLineFromPlantServer(LineLocId);
            return getLineChart(Exstingline.ID);
        }
        public LiveLinesViewModel getLiveLineExecution(string LineLocId)
        {
            var Exstingline = GetLineFromPlantServer(LineLocId);
            return getLiveLine(Exstingline.ID);
        }
        public List<LiveLinesViewModel> getLiveLineExecution()
        {
            List<LiveLinesViewModel> llm = new List<LiveLinesViewModel>();
            // Dictionary<LineLocation, string> InActiveLines = new Dictionary<LineLocation, string>();

            var ExistingLines = GetAllLineFromPlantServer();

            foreach (var item in ExistingLines)
            {
                var ll = getLiveLine(item.ID);
                llm.Add(ll);
            }
            return llm;
        }


        private LiveLinesViewModel getLiveLine(string LineLocId)
        {
            var item = GetLineFromPlantServer(LineLocId);
            LiveLinesViewModel obj = new LiveLinesViewModel();
            string LineRealName = db.LineLocation.Where(x => x.ID == item.ID).FirstOrDefault().LineName;

            if (LineRealName == null || LineRealName == "")
            {
                LineRealName = item.ID;
            }
            obj.LineName = LineRealName;
            // check line batches in ExecutionData
            var executionData = db.ExecutionData.Where(x => x.LineCode == item.ID).OrderByDescending(x => x.LastUpdatedDate).ToList();

            if (executionData.Count() != 0)
            {

                var latest = executionData.FirstOrDefault();
                obj.Status = getStatus(item.LineIP, latest.JobStatus);
                obj.ProductName = latest.ProductName;
                obj.BatchNo = latest.BatchNo;
               
                //var edata = executionData.Where(x => x.BatchNo == latest.BatchNo).OrderBy(x => x.LastUpdatedDate).ToList();
                //List<PrintStatusInfo> printingDtls = new List<PrintStatusInfo>();
                //foreach (var entry in edata)
                //{
                //    printingDtls.Add(new PrintStatusInfo() { printDT = entry.LastUpdatedDate, packDetails = getPackDtls(entry.PackagingLevel) });
                //}
                //obj.printingExecutionDetails = printingDtls;

                var lineLoc = db.LineLocation.Where(x => x.ID == latest.LineCode).FirstOrDefault();
                LineRealName = lineLoc.LineName;
                if (LineRealName == null || LineRealName == "")
                {
                    LineRealName = latest.LineCode;
                }
                obj.LineId = lineLoc.ID;
                obj.LineName = LineRealName;
                obj.OrderName = latest.JobName;
                obj.Size = latest.Quantity;
                obj.printingDetails = getPackDtls(latest.PackagingLevel);

                //obj.IdleTime = getIdleTimeForJob(latest.LineCode, latest.JobName);
                return obj;
            }
            else
            {
                obj.Status = 103;
                obj.Message = TnT.LangResource.GlobalRes.ShwMsgLineNotActive;
                return obj;
            }
        }

        private LineChartViewModel getLineChart(string LineLocId)
        {
            var item = GetLineFromPlantServer(LineLocId);
            LineChartViewModel obj = new LineChartViewModel();
            string LineRealName = db.LineLocation.Where(x => x.ID == item.ID).FirstOrDefault().LineName;

            if (string.IsNullOrEmpty(LineRealName))
            {
                LineRealName = item.ID;
            }

            obj.LineName = LineRealName;
            // check line batches in ExecutionData
            var executionData = db.ExecutionData.Where(x => x.LineCode == item.ID).OrderByDescending(x => x.LastUpdatedDate).ToList();

            var latest = executionData.FirstOrDefault();

            var edata = executionData.Where(x => x.BatchNo == latest.BatchNo).OrderBy(x => x.LastUpdatedDate).ToList();
            List<PrintStatusInfo> printingDtls = new List<PrintStatusInfo>();
            foreach (var entry in edata)
            {
                printingDtls.Add(new PrintStatusInfo() { printDT = entry.LastUpdatedDate, packDetails = getPackDtls(entry.PackagingLevel) });
            }
            obj.printingExecutionDetails = printingDtls;

            LineRealName = db.LineLocation.Where(x => x.ID == latest.LineCode).FirstOrDefault().LineName;

            if (LineRealName == null || LineRealName == "")
            {
                LineRealName = latest.LineCode;
            }
            obj.LineName = LineRealName;
            obj.printingDetails = getPackDtls(latest.PackagingLevel);


            return obj;

        }

        private TimeSpan getIdleTimeForJob(string LineCode, string JobName)
        {
            TimeSpan ts;
            var idlTimes = db.LineIdelTime.Where(x => x.LineCode == LineCode && x.JobName == JobName);
            if (idlTimes != null)
            {
                var conversionTime = idlTimes.Select(x => x.IdlTime);
                List<int> result = conversionTime.Select(int.Parse).ToList();
                var total = Convert.ToInt32(result.Sum());
                ts = new TimeSpan(0, 0, total);
            }
            else
            {
                ts = new TimeSpan(0, 0, 0);
            }



            return ts;
        }

        // 0 : Created, 1 : Running, 2 : Paused, 3: Closed, 4 : Decommissioned, 5: LineClearance,  6: CompleteTransfer, 7: ForcefullyBatchClose
        // 404 : SqlServerError , 101 : Not Reachable, 102 : Idle , 103 : No Data in ExecutionData
        private int getStatus(string ServerIP, int status)
        {
            int statusId;
            LineDetails ld = new LineDetails();
            if (ld.PingHost(ServerIP))
            {
                statusId = status;
                //int exeIdleTimeSeconds = Convert.ToInt32(Utilities.getAppSettings("ExecutionIdleTimeSeconds"]);
                //var dif = LastUpdated - PrevUpdated;

                //TimeSpan IdolDifference = TimeSpan.FromSeconds(exeIdleTimeSeconds);

                //if (IdolDifference > dif)
                //{
                //    statusId = 102;                   
                //}
                //else
                //{
                //    statusId = 1;
                //}
            }
            else
            {
                statusId = 3;
            }

            return statusId;
        }

        private List<PrintedPackDetails> getPackDtls(string PckDtlsString)
        {
            List<PrintedPackDetails> Listpcks = new List<PrintedPackDetails>();

            PckDtlsString = Regex.Replace(PckDtlsString, @"\s+", "");
            var chr = new Char[] { '|', '|' };
            var pcksArr = PckDtlsString.Split(chr);

            if (pcksArr != null)
            {
                foreach (var item in pcksArr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var dtl = item.Split(':');
                        if (dtl != null)
                        {

                            if (!string.IsNullOrEmpty(dtl[0]))
                            {
                                PrintedPackDetails pd = new PrintedPackDetails();

                                var dec = dtl[0];
                                int cnt = 0;
                                if (!string.IsNullOrEmpty(dtl[1]))
                                {
                                    cnt = Convert.ToInt32(dtl[1]);
                                }

                                pd.PackagingTypeCode = dec;
                                pd.TotalPrintedQty = cnt;
                                Listpcks.Add(pd);
                            }
                        }

                    }

                }
            }
            return Listpcks;
        }

        private List<LineLocation> GetAllLineFromPlantServer()
        {
            var line = db.LineLocation.Where(l => l.IsActive == true).ToList();
            return line;
        }
        private LineLocation GetLineFromPlantServer(string LineLocId)
        {
            var line = db.LineLocation.Where(x => x.ID == LineLocId && x.IsActive == true).FirstOrDefault();
            return line;
        }
    }
}