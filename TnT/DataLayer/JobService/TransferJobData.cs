
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TnT.DataLayer.Line;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.Job;

namespace TnT.DataLayer.JobService
{
    public enum JobTransferErrors
    {
        LineNotAvailable,
        FailureInBulkCopyToLine,
        Other
    }


    public class TransferJobData
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        JobTransferErrors jobTrnsfrError = JobTransferErrors.Other;
        UIDTransferHelper hlpr = new UIDTransferHelper();

        public JobTransferErrors getErrorDetails()
        {
            return jobTrnsfrError;
        }

        public bool Transfer(Job job, string connstr, decimal Client_PAID, decimal Client_JID, string connServer,bool IsExtra)
        {
            try
            {
                string secondLastLevel = "";
               var provider = db.M_Customer.Find(job.CustomerId).ProviderId;
                var TertiaryDec = ProductPackageHelper.getBottomDeck(job.PAID);
                List<string> decks = ProductPackageHelper.getAllDeck(job.JID.ToString());
                List<string> sortlvl = ProductPackageHelper.sorttheLevels(decks);
                if(sortlvl.Count - 2>0)
                {
                    secondLastLevel = sortlvl[sortlvl.Count - 2];
                }
                else
                {
                    secondLastLevel = "MOC";
                }
               
                //if (provider != 2)
                //{
                LineDetails ld = new LineDetails();
                var IsAvailable = ld.IsLineAvailableForTrnfr(job.LineCode);
                var msg = ld.ErrorMessage;

                if (IsAvailable)
                {
                    UIDTransferHelper hlpr = new UIDTransferHelper();
                    List<X_Identities> MTrnfrList = new List<X_Identities>();
                    var mids = db.M_Identities.Where(x => x.JID == job.JID && x.IsExtra == false && x.IsTransfered == false);
                    foreach (var mid in mids)
                    {
                        var UIDdata = db.X_Identities.Where(x => x.MasterId == mid.Id && x.IsTransfered == false).ToList();
                        MTrnfrList.AddRange(UIDdata);
                    }

                    var WholePackagingData = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, Client_PAID, Client_JID,secondLastLevel );
                    var ConvertedData = GeneralDataHelper.convertToDataTable(WholePackagingData);

                    var WholePackagingDataServer = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, job.PAID, job.JID, secondLastLevel);
                    var ConvertedDataServer = GeneralDataHelper.convertToDataTable(WholePackagingDataServer);
                    BulkDataHelper bulkHlpr = new BulkDataHelper();
                    try
                    {
                      
                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedData, connstr))
                        {
                            foreach (var mstrId in mids.ToList())
                            {
                                //bulkHlpr.setFlagToTransferd(mstrId.Id);
                                if(IsExtra)
                                {
                                    mstrId.IsExtra = true;
                                    mstrId.IsTransfered = true;
                                    
                                }
                                else
                                {
                                    mstrId.IsTransfered = true;
                                }
                                db.Entry(mstrId).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //   return true;
                        }
                        else
                        {
                            jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;
                            ExceptionHandler.ExceptionLogger.LogError(jobTrnsfrError.ToString());
                            return false;
                        }
                    }
                    catch(Exception ex)
                    {
                        ExceptionHandler.ExceptionLogger.logException(ex);
                        return false;
                    }
                    try
                    {
                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedDataServer, connServer))
                        {
                            //foreach (var mstrId in mids.ToList())
                            //{
                            //    bulkHlpr.setFlagToTransferd(mstrId.Id);
                            //}
                            return true;
                        }
                        else
                        {
                            jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;
                            ExceptionHandler.ExceptionLogger.LogError(jobTrnsfrError.ToString());
                            return false;
                        }
                    }
                    catch(Exception ex)
                    {
                        ExceptionHandler.ExceptionLogger.logException(ex);
                        return false;
                    }

                }
                else
                {
                    jobTrnsfrError = JobTransferErrors.LineNotAvailable;
                    ExceptionHandler.ExceptionLogger.LogError(jobTrnsfrError.ToString());
                    return false;
                }
                //}
                //else
                //{

                //    return TransferTracelinkData(job, connstr, Client_PAID, Client_JID, TertiaryDec);
                //}
            }
            catch (Exception ex)
            {
                jobTrnsfrError = JobTransferErrors.Other;
                ExceptionHandler.ExceptionLogger.logException(ex);
                return false;
            }


        }

        public bool TransferV1(Job job, string connstr, decimal Client_PAID, decimal Client_JID, string connServer, bool IsExtra)
        {
            try
            {
                var provider = db.M_Customer.Find(job.CustomerId).ProviderId;
                LineDetails ld = new LineDetails();
                //var IsAvailable = ld.IsLineAvailableForTrnfr(job.LineCode);

                //if (!IsAvailable)
                //{
                //    return IsAvailable;
                //}

                UIDTransferHelper uIDTransfer = new UIDTransferHelper();

                var mIdentities = db.M_Identities.Where(x => x.JID == job.JID && x.IsExtra == IsExtra && x.IsTransfered == false);

                List<X_Identities> x_Identities = new List<X_Identities>();
                foreach (var mid in mIdentities)
                {
                    var UIDdata = db.X_Identities.Where(x => x.MasterId == mid.Id && x.IsTransfered == false).ToList();
                    x_Identities.AddRange(UIDdata);
                }
                var PackagingData_Line = hlpr.convertToPackagingDetails(x_Identities, job, "TertiaryDec", Client_PAID, Client_JID, "secondLastLevel");
                DataTable dtPackagingData_Line = GeneralDataHelper.convertToDataTable(PackagingData_Line);

                var PackagingData_Server = hlpr.convertToPackagingDetails(x_Identities, job, "TertiaryDec", job.PAID, job.JID, "secondLastLevel");
                DataTable dtPackagingData_Server = GeneralDataHelper.convertToDataTable(PackagingData_Server);

                BulkDataHelper bulkHlpr = new BulkDataHelper();

                try
                {
                    // INSERT PACK IN LINE
                    if (bulkHlpr.InsertPackagingDetailsForInitial(dtPackagingData_Line, connstr))
                    {
                        foreach (var mstrId in mIdentities.ToList())
                        {
                            if (IsExtra)
                            {
                                //mstrId.IsExtra = true;
                                mstrId.IsTransfered = true;
                            }
                            else
                            {
                                mstrId.IsTransfered = true;
                            }
                            db.Entry(mstrId).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        // INSERT PACK IN SERVER
                        if (bulkHlpr.InsertPackagingDetailsForInitial(dtPackagingData_Server, connServer))
                        {
                            return true;
                        }
                        else
                        {
                            jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;
                            ExceptionHandler.ExceptionLogger.LogError(jobTrnsfrError.ToString());
                            return false;
                        }
                    }
                    else
                    {
                        jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;
                        ExceptionHandler.ExceptionLogger.LogError(jobTrnsfrError.ToString());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                jobTrnsfrError = JobTransferErrors.Other;
                ExceptionHandler.ExceptionLogger.logException(ex);
                return false;
            }
        }

        public bool TransferAdditionalBatchQty(Job job, string connstr, decimal Client_PAID, decimal Client_JID, string connServer, bool IsExtra)
        {
            try
            {
                var provider = db.M_Customer.Find(job.CustomerId).ProviderId;
                var TertiaryDec = ProductPackageHelper.getBottomDeck(job.PAID);
                string secondLastLevel = "";
                List<string> decks = ProductPackageHelper.getAllDeck(job.JID.ToString());
                List<string> sortlvl = ProductPackageHelper.sorttheLevels(decks);
                if (sortlvl.Count - 2 > 0)
                {
                    secondLastLevel = sortlvl[sortlvl.Count - 2];
                }
                else
                {
                    secondLastLevel = "MOC";
                }
                //if (provider != 2)
                //{
                LineDetails ld = new LineDetails();
               // var IsAvailable = ld.IsLineAvailableForTrnfr(job.LineCode);
                var msg = ld.ErrorMessage;

                //if (IsAvailable)
                //{
                    UIDTransferHelper hlpr = new UIDTransferHelper();
                    List<X_Identities> MTrnfrList = new List<X_Identities>();
                    var mids = db.M_Identities.Where(x => x.JID == job.JID && x.IsExtra == true && x.IsTransfered == false);
                    foreach (var mid in mids)
                    {
                        var UIDdata = db.X_Identities.Where(x => x.MasterId == mid.Id && x.IsTransfered == false).ToList();
                        MTrnfrList.AddRange(UIDdata);
                    }

                    var WholePackagingData = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, Client_PAID, Client_JID,secondLastLevel);
                    var ConvertedData = GeneralDataHelper.convertToDataTable(WholePackagingData);

                    var WholePackagingDataServer = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, job.PAID, job.JID,secondLastLevel);
                    var ConvertedDataServer = GeneralDataHelper.convertToDataTable(WholePackagingDataServer);
                    BulkDataHelper bulkHlpr = new BulkDataHelper();
                    try
                    {

                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedData, connstr))
                        {
                            foreach (var mstrId in mids.ToList())
                            {
                                //bulkHlpr.setFlagToTransferd(mstrId.Id);
                                if (IsExtra)
                                {
                                    mstrId.IsExtra = true;
                                    mstrId.IsTransfered = true;

                                }
                                else
                                {
                                    mstrId.IsTransfered = true;
                                }
                                db.Entry(mstrId).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //   return true;
                        }
                        else
                        {
                            jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.ExceptionLogger.logException(ex);
                        return false;
                    }
                    try
                    {
                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedDataServer, connServer))
                        {
                            //foreach (var mstrId in mids.ToList())
                            //{
                            //    bulkHlpr.setFlagToTransferd(mstrId.Id);
                            //}
                            return true;
                        }
                        else
                        {
                            jobTrnsfrError = JobTransferErrors.FailureInBulkCopyToLine;

                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.ExceptionLogger.logException(ex);
                        return false;
                    }

                //}
                //else
                //{
                //    jobTrnsfrError = JobTransferErrors.LineNotAvailable;
                //    return false;
                //}
                //}
                //else
                //{

                //    return TransferTracelinkData(job, connstr, Client_PAID, Client_JID, TertiaryDec);
                //}
            }
            catch (Exception ex)
            {
                jobTrnsfrError = JobTransferErrors.Other;
                ExceptionHandler.ExceptionLogger.logException(ex);
                return false;
            }


        }

        public bool TransferExtraUids(Job job, string connstr, decimal Client_PAID, decimal Client_JID,string connServer)
        {
            try
            {
                var provider = db.M_Customer.Find(job.CustomerId).ProviderId;
                var TertiaryDec = ProductPackageHelper.getBottomDeck(job.PAID);
                string secondLastLevel = "";
                List<string> decks = ProductPackageHelper.getAllDeck(job.JID.ToString());
                List<string> sortlvl = ProductPackageHelper.sorttheLevels(decks);
                if (sortlvl.Count - 2 > 0)
                {
                    secondLastLevel = sortlvl[sortlvl.Count - 2];
                }
                else
                {
                    secondLastLevel = "MOC";
                }

                LineDetails ld = new LineDetails();
                    var IsAvailable = true; //ld.IsLineAvailableForTrnfr(job.LineCode);
                    var msg = ld.ErrorMessage;

                    if (IsAvailable)
                    {
                        UIDTransferHelper hlpr = new UIDTransferHelper();
                        List<X_Identities> MTrnfrList = new List<X_Identities>();
                        var mids = db.M_Identities.Where(x => x.JID == job.JID && x.IsExtra == true && x.IsTransfered == false).OrderByDescending(p => p.CreatedOn);
                        foreach (var mid in mids)
                        {
                            var UIDdata = db.X_Identities.Where(x => x.MasterId == mid.Id && x.IsTransfered == false).ToList();
                            MTrnfrList.AddRange(UIDdata);
                        }
                    DataTable ConvertedData = new DataTable();
                    ConvertedData.Clear();

                    if (provider != 2)
                    {
                        var WholePackagingData = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, Client_PAID, Client_JID, secondLastLevel);
                        ConvertedData = GeneralDataHelper.convertToDataTable(WholePackagingData);
                    }
                    else
                    {
                        var WholePackagingData = hlpr.convertToPackagingDetailsForExtraUIDForTlink(MTrnfrList, job, TertiaryDec, Client_PAID, Client_JID, secondLastLevel);
                        ConvertedData = GeneralDataHelper.convertToDataTable(WholePackagingData);
                    }

                    DataTable ConvertedDataServer = new DataTable();
                    ConvertedDataServer.Clear();
                    if (provider != 2)
                    {
                        var WholePackagingDataServer = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, job.PAID, job.JID, secondLastLevel);
                        ConvertedDataServer = GeneralDataHelper.convertToDataTable(WholePackagingDataServer);
                    }
                    else
                    {
                        var WholePackagingDataServer = hlpr.convertToPackagingDetailsForExtraUIDForTlink(MTrnfrList, job, TertiaryDec, job.PAID, job.JID, secondLastLevel);
                        ConvertedDataServer = GeneralDataHelper.convertToDataTable(WholePackagingDataServer);
                    }

                        BulkDataHelper bulkHlpr = new BulkDataHelper();
                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedData, connstr))
                        {
                            foreach (var mstrId in mids.ToList())
                            {
                                bulkHlpr.setFlagToTransferd(mstrId.Id);
                            }
                        }

                        if (bulkHlpr.InsertPackagingDetailsForInitial(ConvertedDataServer, connServer))
                        {
                            foreach (var mstrId in mids.ToList())
                            {
                                bulkHlpr.setFlagToTransferd(mstrId.Id);
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
               
            }
            catch (Exception)
            {

                throw;
            }

        }


        public bool TransferTracelinkData(Job job, string connstr, decimal Client_PAID, decimal Client_JID, string TertiaryDec)
        {
            LineDetails ld = new LineDetails();
            var IsAvailable = ld.IsLineAvailableForTrnfr(job.LineCode);
            var msg = ld.ErrorMessage;
            string secondLastLevel = "";
            List<string> decks = ProductPackageHelper.getAllDeck(job.JID.ToString());
            List<string> sortlvl = ProductPackageHelper.sorttheLevels(decks);
            if (sortlvl.Count - 2 > 0)
            {
                secondLastLevel = sortlvl[sortlvl.Count - 2];
            }
            else
            {
                secondLastLevel = "MOC";
            }
            if (IsAvailable)
            {
                var jobDtls = db.JobDetails.Where(x => x.JD_JobID == job.JID);

                foreach (var item in jobDtls.ToList())
                {
                    var miden = db.M_Identities.Where(x => x.GTIN == item.JD_GTIN && x.IsExtra == false && x.IsTransfered == false).FirstOrDefault();
                    if (miden != null)
                    {
                        miden.JID = job.JID;
                        db.Entry(miden).State = System.Data.Entity.EntityState.Modified;

                        var idss = db.X_Identities.Where(x => x.MasterId == miden.Id).Take(Convert.ToInt32(job.Quantity) + Convert.ToInt32(job.SurPlusQty));
                        foreach (var uid in idss)
                        {
                            uid.PackTypeCode = item.JD_Deckcode;
                            db.Entry(uid).State = System.Data.Entity.EntityState.Modified;
                        }

                        db.SaveChanges();
                    }

                }
                UIDTransferHelper hlpr = new UIDTransferHelper();
                BulkDataHelper bulkHlpr = new BulkDataHelper();
                List<X_Identities> MTrnfrList = new List<X_Identities>();
                var mids = db.M_Identities.Where(x => x.JID == job.JID);
                foreach (var mid in mids)
                {
                    var UIDdata = db.X_Identities.Where(x => x.MasterId == mid.Id).ToList();
                    MTrnfrList.AddRange(UIDdata);
                }

                foreach (var mstrId in mids.ToList())
                {
                    bulkHlpr.setFlagToTransferd(mstrId.Id);
                }
                var WholePackagingData = hlpr.convertToPackagingDetails(MTrnfrList, job, TertiaryDec, Client_PAID, Client_JID,secondLastLevel);
                var ConvertedData = GeneralDataHelper.convertToDataTable(WholePackagingData);


                return bulkHlpr.InsertPackagingDetailsForInitial(ConvertedData, connstr);

            }
            else
            {
                jobTrnsfrError = JobTransferErrors.LineNotAvailable;
                return false;
            }

        }
    }
}