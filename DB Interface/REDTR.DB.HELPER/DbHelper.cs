using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using PTPLCRYPTORENGINE;
using System.Windows.Forms;
using REDTR.UTILS.SystemIntegrity;
using System.Data.Common;
using REDTR.DB.DAL;
using System.Data;

namespace REDTR.HELPER
{
    public class DbHelper
    {
        //static bool IsUpdateing = false;
        public BLLManager DBManager = new BLLManager();
        private bool DBSaveThread = false;

        public bool CloseDBConn()
        {
            return DBManager.CloseDB();
        }

        //public int ExecuteQuery(string Query)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
        //        return DbProviderHelper.ExecuteNonQuery(oDbCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.TraceError("Error ExecuteQuery :"+ex.Message);
        //        return 0;
        //    }
        //}

        public bool UpdateStatusEndTimeJob(decimal JobID, DateTime date, sbyte state)
        {
            try
            {
                Job jb = new Job();
                jb.JID = JobID;
                jb.JobEndTime = date;
                jb.JobStatus = state;
                if (DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateStatusEndTimeJob, jb) > 0)
                {
                   // Trace.TraceInformation("Close Job:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}.", jb.JobName, jb.PAID, jb.BatchNo, jb.MfgDate, jb.ExpDate, jb.Quantity, jb.SurPlusQty, jb.JobStatus, jb.DetailInfo, jb.JobStartTime, jb.JobEndTime, jb.LabelStartIndex, jb.AutomaticBatchCloser, jb.TID, jb.MLID, jb.TenderText, jb.Remarks, jb.CreatedBy, jb.VerifiedBy, jb.VerifiedDate, jb.LastUpdatedDate, jb.AppId);
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        private Queue<PackagingDetails> PacksQueueSave = new Queue<PackagingDetails>();
        private Thread SavePacksThread = null;
        EventWaitHandle savePackhandle = new AutoResetEvent(false);
        public int AddNewPack(Decimal JobID, Decimal PAID, string DeckType, string SrNo, DateTime PackExpDate, Nullable<bool> IsRejected, string FailedReasons, string NextLevelCode, Nullable<decimal> CaseSeqNum, string SSCC, decimal OperatorId)
        {
            PackagingDetails pack = new PackagingDetails();
            pack.JobID = JobID;
            pack.PAID = PAID;
            pack.PackageTypeCode = DeckType;
          //  SrNo = PTPLCRYPTORENGINE.AESCryptor.Encrypt(SrNo, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);//For Encryption
            pack.Code = SrNo;
            pack.IsRejected = IsRejected;
            //if (NextLevelCode != "FFFFF")
            //{ NextLevelCode = PTPLCRYPTORENGINE.AESCryptor.Decrypt(NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }//For Encryption
            pack.NextLevelCode = NextLevelCode;
            pack.Reason = FailedReasons;
            pack.MfgPackDate = DateTime.Now;
            pack.ExpPackDate = PackExpDate;
            pack.CaseSeqNum = CaseSeqNum;

            if(OperatorId==Globals.SerUserId)
                pack.OperatorId = OperatorId;
            else
                pack.OperatorId = Globals.SerUserId;

            pack.SSCC = SSCC;
            if (DBSaveThread == false)
            {
                return DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.AddNewPckDtls, pack);
            }
            else
            {
                PacksQueueSave.Enqueue(pack);
                if (SavePacksThread == null)
                {
                    SavePacksThread = new Thread(new ParameterizedThreadStart(WaitforPacksToBeSaved));
                    SavePacksThread.Start();
                }
                savePackhandle.Set();
            }
            return 1;
        }
        void WaitforPacksToBeSaved(object node)
        {
            while (true)
            {
                savePackhandle.WaitOne();
                if (PacksQueueSave.Count > 0)
                {
                    PackagingDetails pack = PacksQueueSave.Dequeue();
                    DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.AddNewPckDtls, pack);
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// At The time of job close activity ...here this checks if current deck is Incoplete  or not.
        /// meeans if unmapped child packs are exists ..
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="jobId"></param>
        /// <param name="defaultUid"></param>
        /// <returns></returns>
        public bool IsIncompleteDeck(string deck, decimal jobId, string defaultUid)
        {
            //if (defaultUid != null) { defaultUid = AESCryptor.Encrypt(defaultUid, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey); }
            List<PackagingDetails> PckDtls = DBManager.PackagingDetailsBLL.GetPackagingDetailss(PackagingDetailsBLL.OP.GETInspectedUnmappedUID, jobId.ToString(), defaultUid);
            int Count = PckDtls.FindAll(packDtls => packDtls.PackageTypeCode == deck.ToString()).Count;
            if (Count > 0)
                return true;
            else
                return false;
        }
        public string GetPermission(int Permission_Id) //Added by Arvind on 4.05.2015
        {
            Permissions permison = DBManager.PermissionsBLL.GetPermissionss(Permission_Id);
            return permison.Permission;
        }
        public string GetManualUpdationXML(decimal JobID, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark)
        {
            const string cstrDoubleQuote = "\"";
            string VerType = "";
            if (Type == ManualStatusUpdationTypes.AcceptanceVerification)
                VerType = "ACCEPT TO REJECT(BAD)";
            else if (Type == ManualStatusUpdationTypes.RejectionVerification)
                VerType = "REJECT TO ACCEPT(GOOD)";
            else if (Type == ManualStatusUpdationTypes.ChallengeTest) // For Challenge Test [Sunil 26.12.2015]
                VerType = mRemark;
            else
                VerType = Type.ToString().ToUpper();

            string Xml = "<Action Type=" + cstrDoubleQuote + VerType + cstrDoubleQuote +
                         " User=" + cstrDoubleQuote + UserId + cstrDoubleQuote + " QA=" + cstrDoubleQuote + QAID +
                        cstrDoubleQuote + " ActionAt=" + cstrDoubleQuote + DateTime.Now + cstrDoubleQuote + " Remark=" + cstrDoubleQuote + mRemark + cstrDoubleQuote + "/>";
            return Xml;
        }

        // For Challenge Test [Sunil 24.12.2015]
        public string GetManualUpdationXMLForTest(decimal JobID, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark,string Remark)
        {
            const string cstrDoubleQuote = "\"";
            string VerType = "";
            if (Type == ManualStatusUpdationTypes.AcceptanceVerification)
                VerType = "ACCEPT TO REJECT(BAD)";
            else if (Type == ManualStatusUpdationTypes.RejectionVerification)
                VerType = "REJECT TO ACCEPT(GOOD)";
            else if (Type == ManualStatusUpdationTypes.ChallengeTest)
                VerType = mRemark;
            else if (Type == ManualStatusUpdationTypes.QASampling)
                VerType = mRemark;
            else
                VerType = Type.ToString().ToUpper();

            string Xml = "<Action Type=" + cstrDoubleQuote + VerType + cstrDoubleQuote +
                         " User=" + cstrDoubleQuote + UserId + cstrDoubleQuote + " QA=" + cstrDoubleQuote + QAID +
                        cstrDoubleQuote + " ActionAt=" + cstrDoubleQuote + DateTime.Now + cstrDoubleQuote + " Remark=" + cstrDoubleQuote + Remark + cstrDoubleQuote + "/>";
            return Xml;
        }

        public bool ManualUpdationRemark(decimal JobID, decimal PackID, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark)
        {
            try
            {
                string Xml = GetManualUpdationXML(JobID, Type, UserId, QAID, mRemark);
                PackagingDetails PckDtls = new PackagingDetails();
                //DBManager.PackagingDetailsBLL.GetPackagingDetails(PackagingDetailsBLL.OP.GetDtls, PackID.ToString(), null);
                PckDtls.ManualUpdateDesc = Xml;
                PckDtls.IsManualUpdated = true;
                PckDtls.PackDtlsID = PackID;
                DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.UpdateManualUpdationDesc, PckDtls);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// SAME METHOD IS USED FOR ONLINE QA SAMPLE
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="Code"></param>
        /// <param name="Type"></param>
        /// <param name="UserId"></param>
        /// <param name="QAID"></param>
        /// <param name="mRemark"></param>
        /// <returns></returns>
        public bool OnlineQASampling(decimal JobID, string Code, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark,string Remark)
        {
            try
            {
                //MessageBox.Show("QA SAMPLE MODE", "QA SAMPLE Final", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string Xml = GetManualUpdationXML(JobID, Type, UserId, QAID, Remark);
                //string Xml = GetManualUpdationXMLForTest(JobID, Type, UserId, QAID, mRemark, Remark); //WithGoodBad Result [Modified for Emcure but then decided not to change existing software.
                PackagingDetails PckDtls = new PackagingDetails();
                //DBManager.PackagingDetailsBLL.GetPackagingDetails(PackagingDetailsBLL.OP.GetDtls, PackID.ToString(), null);
                PckDtls.ManualUpdateDesc = Xml;
                PckDtls.IsManualUpdated = true;
                PckDtls.IsDecomission = true;
                PckDtls.Code = Code;
                PckDtls.JobID = JobID;
                DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.AddPackDetlsForQASample, PckDtls);
            }
            catch (Exception)
            {
                Trace.TraceInformation("Update Challenge Test UID Failed {0},{1}", DateTime.Now.ToString(),"Failed");
                return false;
            }
            return true;
        }

        /// <summary>
        /// SAME METHOD IS USED FOR ONLINE QA SAMPLE
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="Code"></param>
        /// <param name="Type"></param>
        /// <param name="UserId"></param>
        /// <param name="QAID"></param>
        /// <param name="mRemark"></param>
        /// <returns></returns>
        public bool OnlineChallengeTest(decimal JobID, string Code, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark,string Remark)
        {
            try
            {
                //MessageBox.Show("QA SAMPLE MODE", "QA SAMPLE Final", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string Xml = GetManualUpdationXMLForTest(JobID, Type, UserId, QAID, mRemark, Remark);
                PackagingDetails PckDtls = new PackagingDetails();
                //DBManager.PackagingDetailsBLL.GetPackagingDetails(PackagingDetailsBLL.OP.GetDtls, PackID.ToString(), null);
                PckDtls.ManualUpdateDesc = Xml;
                PckDtls.IsManualUpdated = true;
                PckDtls.IsDecomission = true;
                PckDtls.Code = Code;
                PckDtls.JobID = JobID;
                DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.AddPackDetlsForQASample, PckDtls);
            }
            catch (Exception)
            {
                Trace.TraceInformation("Update Challenge Test UID Failed {0},{1}", DateTime.Now.ToString(), "Failed");
                return false;
            }
            return true;
        }
        // Added by Sunil on 03.03.2015 for acceptance verification seprately.
        public bool ManualUpdationRemarkForAcceptVeri(decimal JobID, decimal PackID, ManualStatusUpdationTypes Type, Nullable<decimal> UserId, Nullable<decimal> QAID, string mRemark)
        {
            try
            {
                string Xml = GetManualUpdationXML(JobID, Type, UserId, QAID, mRemark);
                PackagingDetails PckDtls = new PackagingDetails();
                //DBManager.PackagingDetailsBLL.GetPackagingDetails(PackagingDetailsBLL.OP.GetDtls, PackID.ToString(), null);
                PckDtls.ManualUpdateDesc = Xml;
                PckDtls.IsManualUpdated = false;
                PckDtls.PackDtlsID = PackID;
                DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(PackagingDetailsBLL.OP.UpdateManualUpdationDesc, PckDtls);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        // End here.

        #region Tertiary_Functions
        public int GetLastSSCC(int JobId)
        {
            int LastSSCC = 0;
            try
            {
                SSCCLineHolder sscc = DBManager.SSCCLineHolderBLL.GetSSCCLineHolder(SSCCLineHolderBLL.SSCCLineHolderOp.GetLastSSCC, JobId);
                if (sscc.LastSSCC.HasValue && sscc.LastSSCC <= sscc.FirstSSCC)
                    LastSSCC = (int)sscc.LastSSCC;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return LastSSCC;
        }
        public int GetLatestSSCC()
        {
            int LastSSCC = 0;
            try
            {
                LastSSCC = DBManager.SSCCLineHolderBLL.GetLatestSSCC();
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return LastSSCC;
        }
        public void SetLastSSCC(int JobID, int LastSSCCPrinted)
        {
            try
            {
                string PackageIndicator = DateTime.Now.Year.ToString();
                PackageIndicator = PackageIndicator.Substring(3, 1);
                SSCCLineHolder sscc = new SSCCLineHolder();
                sscc.PackageIndicator = Convert.ToDecimal(PackageIndicator);
                sscc.JobID = JobID;                                                                 
                sscc.LastSSCC = LastSSCCPrinted;
                DBManager.SSCCLineHolderBLL.InsertOrUpdateSSCCLineHolder(sscc);
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }

        public void SaveSSCCForFreshBatch(int JobID, int LastSSCCPrinted, Int32 FirstSSCCPrinted,string LineCode)
        {
            try
            {
                string PackageIndicator = DateTime.Now.Year.ToString();
                PackageIndicator = PackageIndicator.Substring(3, 1);
                SSCCLineHolder sscc = new SSCCLineHolder();
                sscc.PackageIndicator = Convert.ToDecimal(PackageIndicator);
                sscc.JobID = JobID;
                sscc.LastSSCC = LastSSCCPrinted+1;
                sscc.FirstSSCC = FirstSSCCPrinted;
                sscc.LineCode = LineCode;
                DBManager.SSCCLineHolderBLL.InsertOrUpdateSSCCLineHolder(sscc);
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }

        public int GetLabelStartIndexofJob(decimal jobID, int AppCode)
        {
            int labelStartIndex = 1;
            Job Jb = DBManager.JobBLL.GetJob(JobBLL.JobOp.GetJob, AppCode, jobID.ToString(), null);
            if (Jb.LabelStartIndex != null)
                labelStartIndex = (int)Jb.LabelStartIndex;
            if (labelStartIndex <= 0)
                labelStartIndex = 1;
            return labelStartIndex;
        }
        public bool UpdateLabelStartIndexofJob(decimal jobID, decimal LabelStartIndex, int APPCode)
        {
            try
            {
                Job J = new Job();
                J.JID = jobID;
                J.LabelStartIndex = LabelStartIndex;
                J.AppId = APPCode;
                if (DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateLableStartIndexJob, J) > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }


        public int GetLastDavaFileRunningSeqNo(int m_PI)
        {
            int lastDavaRunningSeq = 0;
            try
            {
                DavaFileRunningSeqNo runSeqNo = DBManager.DavaFileRunningSeqNoBLL.GetDavaFileRunningSeqNos(DavaFileRunningSeqNoBLL.DavaFileRunningSeqNoOp.GETDavaFileRunningSeqNo,m_PI);
                if (runSeqNo.LastRunningSeqNo.HasValue)
                    lastDavaRunningSeq = (int)runSeqNo.LastRunningSeqNo;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return lastDavaRunningSeq;
        }
        public void SetLastDavaFileRunningSeqNo(int LastDavaFileRunningSeqNo)
        {
            try
            {
                DavaFileRunningSeqNo runSeqNo = new DavaFileRunningSeqNo();
                runSeqNo.ID = 1;
               
                runSeqNo.LastRunningSeqNo = LastDavaFileRunningSeqNo;
                DBManager.DavaFileRunningSeqNoBLL.AddDavaFileRunningSeqNo(runSeqNo);
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }


        #endregion END_Tertiary_Functions

        #region UsernRoles_Functions
        public void AddUserTrail(decimal UserId1, Nullable<decimal> UserID2, string Where, string What, string Why, string Remark)
        {
            USerTrail UsrTrl = new USerTrail();
            UsrTrl.AccessedAt = DateTime.Now;

            #region Not Required

            //if (UserId1 != Globals.SerUserId)
            //{
            //    if (Globals.IsFunctionWork == true)
            //    {
            //        UserId1 = Globals.SerVerifyUserId;
            //        Globals.IsFunctionWork = false;
            //    }
            //    else
            //        UserId1 = Globals.SerUserId;
            //}

            #endregion

            if (UserId1 != Globals.SerUserId)
                UserId1 = Globals.SerUserId;

            string Xml = "<Who>" + "<User>" + UserId1 + "</User>";

            if (UserID2 != null)
                Xml = Xml + "<User>" + UserID2 + "</User>";

            Xml = Xml + "</Who>";

            Xml = Xml + "<Where>" + Where + "</Where>" + "<Why>" + Why + "</Why>" + "<What>" + What + "</What>";
            UsrTrl.Reason = Xml;
            if (string.IsNullOrEmpty(Remark))
                Remark = "";
            UsrTrl.Remarks = Remark;
            UsrTrl.LineCode = Globals.LineCode;
            DBManager.USerTrailBLL.AddUSerTrail(UsrTrl,0);
            // Trace.TraceInformation("User Trail=>{0},{1}",UsrTrl.AccessedAt,UsrTrl.Reason);
            //
            // throw new NotImplementedException();
        }
        public string GetRoleName(int RoleId)
        {
            Roles RofUser = DBManager.RolesBLL.GetRoles(RoleId);
            return RofUser.Roles_Name;
        }
        public string GetUserName(decimal UserID)
        {
            Users User = DBManager.UsersBLL.GetUsers(UsersBLL.UsersOp.GetUsers, UserID.ToString());
            return User.UserName;
        }
        #endregion END_UsernRoles_Functions

        public DbDataReader GetReader(string Query)
        {
            try
            {
                DbDataReader dr;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                dr = DbProviderHelper.ExecuteReader(oDbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecureQuery(string Query)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                 return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSet(string Query )
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Application_Functions
        //public string GetJobTypeName(decimal oTypeID)
        //{
        //    JOBType jobType = DBManager.JOBTypeBLL.GetJOBTypeByID(oTypeID);
        //    if (jobType != null)
        //    {
        //        return jobType.Job_Type;
        //    }
        //    return "";
        //}
        public string GetGS1CompCode()
        {
            string CompanyCode = "";
            try
            {
                List<Settings> settings = DBManager.SettingsBLL.GetSettingss();
                CompanyCode = settings[0].CompanyCode;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return CompanyCode;
        }
        public string GetLineCode()
        {
            string LineCode = "";
            try
            {
                List<Settings> settings = DBManager.SettingsBLL.GetSettingss();
                LineCode = settings[0].LineCode;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return LineCode;
        }
        public int GetPlantCode()
        {
            int PlantCode = 0;
            try
            {
                List<Settings> settings = DBManager.SettingsBLL.GetSettingss();
                PlantCode = settings[0].PlantCode;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return PlantCode;
        }
        #endregion END_Application_Functions

        public int ExecuteQuery(string Query)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);// CreateDataAdapter(oDbCommand);
                //ds = DbProviderHelper.FillDataTable(DbAdpt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public enum ManualStatusUpdationTypes
        {
            RejectionVerification,
            Item_to_replace,//ReplaceItem_S,//
            Replaced_Item,//
            Decommisioned,
            UIDVerification,
            QASampling, //QASAMPLING
            AcceptanceVerification, // for acceptance verification 03.03.2015 by sunil
            Closebatch,  // for forcefully batch finish added by Sunil on 19.12.2014
            DAVAExport,
            CameraRejectionVerification, // For Rework [Sunil 18.10.2015]
            ChallengeTest
        }
    }
}
