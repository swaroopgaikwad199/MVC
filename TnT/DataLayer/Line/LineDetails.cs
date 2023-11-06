using TnT.Models;
using TnT.Models.Home;
using TnT.Models.Job;
using TnT.Models.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Web;
using System.Net.NetworkInformation;

namespace TnT.DataLayer.Line
{
    public class LineDetails
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PackagingAsso product;
        private IEnumerable<PackagingAssoDetails> productAssoDetails;
        private Job job;
        public string ErrorMessage;
        public string qryPackagingDetails = "";
        // 0 : Created, 1 : Running, 2 : Paused, 3: Closed, 4 : Decommissioned, 5: LineClearance,  6: CompleteTransfer, 7: ForcefullyBatchClose
        // 404 : SqlServerError
        
        public List<LiveLinesViewModel> getLiveLine()
        {
            List<LiveLinesViewModel> llm = new List<LiveLinesViewModel>();
            Dictionary<LineLocation, string> InActiveLines = new Dictionary<LineLocation, string>();
            //List<LineLocation> InActiveLines = null;
            Job RunningJob;

            var ExistingLines = GetAllLineFromPlantServer();

            foreach (var item in ExistingLines)
            {
                LiveLinesViewModel obj = new LiveLinesViewModel();
                obj.LineName = item.ID;
                //check server is alive : ping 
                if (IsLineServerAlive(item.LineIP, item.DBName, item.SQLUsername, item.SQLPassword))
                {
                    //connect to server
                    //get the Job 
                    getRunningJobNProductFromLineServer(item.LineIP, item.DBName, item.SQLUsername, item.SQLPassword);
                    RunningJob = job;

                    if (RunningJob != null)
                    {


                        obj.OrderName = job.JobName;
                        obj.ProductName = product.Name;
                        obj.BatchNo = job.BatchNo;
                        obj.Size = job.Quantity + job.SurPlusQty;
                        obj.printingDetails = getPackedDetails(job.JID, item.LineIP, item.DBName, item.SQLUsername, item.SQLPassword);
                        obj.Status = job.JobStatus;
                        //obj.productAssoDetails = productAssoDetails;
                        llm.Add(obj);

                    }
                    else
                    {

                        llm.Add(obj);
                    }

                }
                else
                {
                    obj.Status = 404;
                    obj.Message = ErrorMessage;
                    llm.Add(obj);
                    //InActiveLines.Add(item,ErrorMessage);
                }
            }
            return llm;
        }

        private List<LineLocation> GetAllLineFromPlantServer()
        {
            var line = db.LineLocation.Where(l => l.IsActive == true).ToList();
            return line;
        }


        private void getRunningJobNProductFromLineServer(string Ip, string Db, string user, string pass)
        {
            // 0 : Created, 1 : Running, 2 : Paused, 3: Closed, 4 : Decommissioned, 5: LineClearance,  6: CompleteTransfer, 7: ForcefullyBatchClose

            string qury = "select * from Job where JobStatus = 1";
            DataTable dtJob = new DataTable();
            dtJob = getData(Ip, Db, user, pass, qury);
            if (dtJob.Rows.Count > 0)
            {
                job = ConvertToJobList(dtJob).FirstOrDefault();
                decimal PAID;
                PAID = Convert.ToDecimal(dtJob.Rows[0][2]);
                getProductInfoRunningJob(PAID, Ip, Db, user, pass);
            }
            else
            {
                job = null;
            }


        }

        private void getProductInfoRunningJob(decimal PAID, string Ip, string Db, string user, string pass)
        {
            string qryProduct = string.Format("select * from PackagingAsso where PAID = {0} ", PAID);
            DataTable dtProduct = new DataTable();
            dtProduct = getData(Ip, Db, user, pass, qryProduct);
            product = ConvertToProductList(dtProduct).FirstOrDefault();

            string qryProdAsso = string.Format(@"SELECT DISTINCT TOP(6) PackageTypeCode, Size, GTINCTI, LastUpdatedDate
                                                 FROM            dbo.PackagingAssoDetails
                                                 WHERE(PAID = {0})  ORDER BY LastUpdatedDate DESC  ", PAID);



            DataTable dtProductAssoDetails = getData(Ip, Db, user, pass, qryProdAsso);
            if (dtProductAssoDetails.Rows.Count > 0)
            {

                productAssoDetails = ConvertToProductAssoList(dtProductAssoDetails);
            }
            else
            {
                productAssoDetails = null;
            }
        }



        private List<PrintedPackDetails> getPackedDetails(decimal JID, string Ip, string Db, string User, string Pass)
        {
            List<PrintedPackDetails> lstPPDs = new List<PrintedPackDetails>();
            foreach (var item in productAssoDetails)
            {
                PrintedPackDetails ppd = new PrintedPackDetails();
                ppd.PackagingTypeCode = item.PackageTypeCode;
                ppd.TotalPrintedQty = getPackingDetailsAccLevel(item.PackageTypeCode, JID, Ip, Db, User, Pass);
                lstPPDs.Add(ppd);
            }
            return lstPPDs;
        }

        private int getPackingDetailsAccLevel(string Lvl, decimal JID, string Ip, string Db, string user, string pass)
        {
            string qryPackagingDetails;
            if (Lvl == "MOC")
            {
                qryPackagingDetails = @"select ((select Count(*) from PackagingDetails where PackageTypeCode='" + Lvl + "' and JobID=" + JID + " and IsRejected is not null)+ (SELECT Isnull(NoReadCount,0) from Job where JID = " + JID + ")) as TotalCount";
            }
            else
            {
                qryPackagingDetails = @"select ((select Count(*) from PackagingDetails where PackageTypeCode='" + Lvl + "' and JobID=" + JID + " and IsRejected is not null)) as TotalCount";
            }
            DataTable packedDetails = getData(Ip, Db, user, pass, qryPackagingDetails);
            if (packedDetails.Rows.Count > 0)
            {
                int cnt = Convert.ToInt32(packedDetails.Rows[0][0].ToString());
                return cnt;
            }
            else
            {
                return 0;
            }
        }

        private IEnumerable<PackagingAsso> ConvertToProductList(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new PackagingAsso
                {
                    PAID = Convert.ToDecimal(row["PAID"]),
                    Name = Convert.ToString(row["Name"]),
                    ProductCode = Convert.ToString(row["ProductCode"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"])
                };
            }
        }

        private IEnumerable<PackagingAssoDetails> ConvertToProductAssoList(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new PackagingAssoDetails
                {
                    PackageTypeCode = Convert.ToString(row["PackageTypeCode"]),
                    GTIN = Convert.ToString(row["GTINCTI"]),
                    Size = Convert.ToInt32(row["Size"]),
                    LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"])
                };
            }
        }

        private IEnumerable<Job> ConvertToJobList(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new Job
                {
                    JID = Convert.ToDecimal(row["JID"]),
                    JobName = Convert.ToString(row["JobName"]),
                    PAID = Convert.ToDecimal(row["PAID"]),
                    BatchNo = Convert.ToString(row["BatchNo"]),
                    MfgDate = Convert.ToDateTime(row["MfgDate"]),
                    ExpDate = Convert.ToDateTime(row["ExpDate"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    JobStatus = Convert.ToByte(row["JobStatus"]),
                    JobStartTime = Convert.ToDateTime(row["JobStartTime"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"])
                };
            }
        }


        public bool IsLineServerAlive(string Ip, string Db, string user, string pass)
        {
            string connectionString = string.Format("Data Source={0};Initial Catalog= {1} ;Integrated Security=false; user id ={2}; password={3}", Ip, Db, user, pass);
            if (PingHost(Ip))
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        ErrorMessage = ex.Message;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

        }

        public bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                pingable = false;   // Discard PingExceptions and return false;
            }
            return pingable;
        }

        private DataTable getData(string Ip, string Db, string user, string pass, string query)
        {

            string connectionStr;
            connectionStr = string.Format("Data Source={0};Initial Catalog= {1} ;Integrated Security=false; user id ={2}; password={3}", Ip, Db, user, pass);
            SqlConnection con = new SqlConnection(connectionStr);
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }


        #region For batch Transfer

        public bool checkLine(string LineId)
        {
            var line = db.LineLocation.Find(LineId);
            if (line != null)
            {
                return  IsLineServerAlive(line.LineIP, line.DBName, line.SQLUsername, line.SQLPassword);
            }
            else
            {
                return false;
            }

        }

        public bool IsLineAvailableForTrnfr(string LineId)
        {
            var line = db.LineLocation.Find(LineId);
            if (line != null)
            {
                return IsTransferLineAvailable(line.LineIP, line.DBName, line.SQLUsername, line.SQLPassword);
            }
            else
            {
                ErrorMessage = TnT.LangResource.GlobalRes.ErrorMsgLineServerInfoNotAvailble +" "+ LineId;
                return false;
            }

        }

        private bool IsTransferLineAvailable(string Ip, string Db, string user, string pass)
        {
            string AllowMultipleBatches = Utilities.getAppSettings("AllowMultipleBatchesOnLine");
            string connectionString = string.Format("Data Source={0};Initial Catalog= {1} ;Integrated Security=false; user id ={2}; password={3}", Ip, Db, user, pass);
            if (PingHost(Ip))
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string qry = @"SELECT JobName, BatchNo FROM dbo.Job  WHERE (JobStatus = 1) OR (JobStatus = 2)";
                        //string qry = @"SELECT JobName, BatchNo FROM dbo.Job";
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(qry, connection);
                        da.Fill(ds);

                        if (AllowMultipleBatches == "false")
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ErrorMessage = TnT.LangResource.GlobalRes.TempDataJobPreviousbatchexists;
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    }
                    catch (SqlException ex)
                    {
                        ErrorMessage = ex.Message;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                ErrorMessage = TnT.LangResource.GlobalRes.ErrorMsgLineUnableToping;
                return false;
            }

        }


        #endregion
    }
}