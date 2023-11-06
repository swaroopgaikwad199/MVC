using System;
using System.Collections.Generic;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class PackagingDetailsBLL
    {
        public enum OP
        {
            /// <summary>
            /// //[@Flag]
            /// </summary>
            GetDtlss = 1,

            /// <summary>
            /// [@Flag,@Value as PackageTypeCode]
            /// </summary>
            GetDtlsOfPckType = 2,

            /// <summary>
            /// [@Flag,@Value as PackDtlsID]
            /// </summary>
            GetDtls = 3,

            /// <summary>
            /// [@Flag,@Value as JobId]
            /// </summary>
            GetDtlsOfJob = 4,

            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Pack type]
            /// </summary>
            GetRejDtlsPckType = 5,

            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Pack type]
            /// </summary>
            GetAccDtlsPckagType = 6,

            GetShipperQty = 50, // For getting Case No.
            /// <summary>
            /// 
            /// </summary>
            GETGOODCnt = 7,

            /// <summary>
            /// 
            /// </summary>
            GETBadCnt = 8,

            /// <summary>
            /// GETGOODCntForRelationCAM
            /// </summary>
            GETGOODCntForRelationCAM = 35,

            AddPackDetailss=15,

            /// <summary>
            /// Insert OR Update PackagingDetails for Online QA Sampling.[30.11.2015]
            /// </summary>
            AddPackDetlsForQASample=16,

            /// <summary>
            /// GETBadCntForRelationCAM
            /// </summary>
            GETBadCntForRelationCAM = 36,

            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Code]
            /// </summary>
            GetDtlsOfJobNCode = 9,
            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Code]
            /// </summary>
            GetDtlsOfJobNCodeForReplace = 52,

            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Pack type]
            /// </summary>
            GetDtlsOfJobNPckType = 10,

            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfJobNSSCC = 11,

            GetNextLevelCodeForLoose = 2, // NextLevelCode of loose.

            /// <summary>
            /// 

            /// 
            /// </summary>
            GetPckBoxRelationship = 12,
            // For Retriving 2 Decks record
            GetPckBoxRelationship2Deck = 54,
            /// <summary>
            /// [JobID,PackageTypeCode]
            /// </summary>
            GetPackSSCC = 13,

            /// <summary>
            /// /this will gives you cnt with DECOMMISSION
            /// </summary>
            GETPrintedLblCnt = 14,

            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfJbNPckTypeWithDcmsion = 15,

            /// <summary>
            /// 
            /// </summary>
            GET_VerifiedUIDStatus = 16,

            /// <summary>
            /// 
            /// </summary>
            GETDS_FailureReasonOfCode = 17,

            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfJobNNextLevelCode = 18,

            /// <summary>
            /// 
            /// </summary>
            ForDetailJobIformation = 19,

            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfPackCode = 20,
            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfPackChildCode = 51,

            /// <summary>
            /// 
            /// </summary>
            GetDtlsOfQASAMPLINGOf_JBnCode = 21,

            /// <summary>
            /// 
            /// </summary>
            GetDS_ofManualUpdationReason = 22,

            /// <summary>
            /// 
            /// </summary>
            GetDecomissionedDtls = 23,

            /// <summary>
            /// 
            /// </summary>
            GETDecomissionedCnt = 24,

            /// <summary>
            /// 
            /// </summary>
            GETUIDsToMAP = 25,

            /// <summary>
            /// GETPrintedUIDs
            /// </summary>
            GETPrintedUIDs = 34,

            /// <summary>
            /// [@Flag,@Value as JobID,@Value1 as Default NExt Level UID]
            /// </summary>
            GETInspectedUnmappedUID = 26,

            /// <summary>
            /// 
            /// </summary>
            GETDS_FailureTypeCounts = 27,

            /// <summary>
            /// 
            /// </summary>
            GetNotInspectedCount = 28,

            /// <summary>
            /// 
            /// </summary>
            GetUID4SSCCnDeck = 29,

            /// <summary>
            /// 
            /// </summary>
            GetUID4SSCC = 30,

            /// <summary>
            /// 
            /// </summary>
            GetIncompleteParents = 2,

            /// <summary>
            /// 
            /// </summary>
            GetPacksOfGreaterThanLUDOfJob= 32,
            /// <summary>
            /// 
            /// </summary>
            GetPacksOfGreaterThanLUD = 33,

            /// <summary>
            /// 
            /// </summary>

            GetInspSkipPcks4Reason = 34,

            GetDtlsOfJbNPckTypeWithDcmsionBad = 37,     //For UID List By Tushar
            GetBadReason = 38,      // For Bad Reasons  By Tushar
            GetSkipInsp = 39,       // For SKIP the inspection  By Tushar

            GetUIDManualupdatestatus = 40, // For Uid Status by Dipesh

            GetPackagingDetail1 = 41,  // Add for Get Total UIDs from db

            GetTempPCMAP = 53,      // For TempPCMAP
            //GetBadNotVeri = 54,   //   For Bad and Not Verified UID Testing (By Dipesh 13 Aug,2015)

            //GetReasonsForRejVerificaion=55,

            //GetScannedUIDList=56,

            GetPrintedCount=65,//57

            //GetChildDecks = 66,//58    // For Loose Shipper SSCC Generation (By Ansuman on 5th Sept 2015)           
            GetCodesForPrimaryPackDummy=60,//For Priimary Dummy Entries

            GetChildDecks = 61,    // For Loose Shipper SSCC Generation (By Ansuman on 5th Sept 2015)

            GetCodesForTSP = 55,//For Priimary Dummy Entries
            
            GetSSCC=56,

            GetSecondaryCodesFromTerSrNo=57,

            GetPrimaryCodesFromSecSrNo = 58,

            GetPrimaryCountofScannedSSCC=59,
            
            /// <summary>
            /// [@Flag,@Value as JobId and @Value1 as LineId] on 26.09.2015 by sunil.
            /// </summary>
            GetLineWiseDetails = 67,

            GetRcResultForMcStart=68, // fOR MC start Emcure 3 deck [20.10.2015]

        //    GetRcResultForMcStartSSCC = 77, // fOR MC start Emcure 3 deck [20.10.2015]


            GetPackagingDetailsForSync=69,

            GetUnmaapedGoodUIDs = 70,

            GetNRNUCnt = 73, // FOR RETRIVING BAD COUNT FOR Not Read/Not Used [Ansu 28.11.2015]

            GetPckDtslForChallengeTest = 74, // FOR RETRIVING DETAILS FOR CHALLENGE TEST [SUNIL 24.12.2015]

            GetLooseShipperCount = 75, // FOR LOOSE SHIPPER COUNT [Sunil 07.01.2015]
            GetLooseShipper_Count = 1,
            GetUnmaapedGoodUIDsRC = 76,

            GetUnmappedGoodUIDsLooseOuter = 78, // FOR LOOSE OUTER PRINT [Sunil 01.10.16]

            /// <summary>
            /// // FOR RETRIVING QA SAMPLE AND DECOMMISIONED COUNT DECKWISE [Sunil 06.10.2016]
            /// </summary>
            GetCountForDeckwiseQAnDecomissionedSample = 80,


            /// <summary>
            /// [@Flag,@Value as JobId,@Value1 as Code for acceptance verification] [Sunil 
            /// </summary>
            GetDtlsOfJobNCodeForAcceptanceVerification = 81,

            /// <summary>
            /// 
            /// </summary>
            AddNewPckDtls = 1,

            /// <summary>
            /// /[NextLevelCode,PackDtlsID]
            /// Replace Item
            /// </summary>
            UpdateNextLevelCodeOfPckID = 2,


            /// <summary>
            ///used at rejection verification
            //[IsRejected,LastUpdatedDate,PackDtlsID]
            /// </summary>
            UpdateStatus = 3,

            /// <summary>
            ///[SSCCVerificationStatus and sscc]
            /// </summary>
            UpdateSSCCVerified = 4,

            /// <summary>
            /// [Code and PackagetypeCode]
            /// </summary>
            UpdateDecomissionDtls = 5,

            /// <summary>
            /// [ManualUpdateDesc and pack ID]
            /// </summary>
            UpdateManualUpdationDesc = 6,
          
            /// <summary>
            /// [ManualUpdateDesc and job ID and decomission==1 managed in query]
            /// </summary>
            UpdateDecommissionforJobId = 7,

            /// <summary>
            /// [nextLevelCode ,@code]
            /// </summary>
            UpdatePCMAp = 8,

            /// <summary>
            /// [IsRejected,Reason,Code]
            /// </summary>
            UpdateResultStatusnReason = 9,

            /// <summary>
            /// [IsRejected,Reason,Code]
            /// </summary>
            UpdateRelationResult= 12,
            /// <summary>
            /// [sscc,CaseNo,Code]
            /// </summary>
            UpdateTertiary = 10,

            /// <summary>
            ///Decommissiondetails of nextlevelcode
            ///[ManualUpdateDesc and NextLevelCode and job ID, decomission==1 managed in query]
            /// </summary>
            UpdateDecommissionForNextLevelCode = 11,


            UpdateParentUIDdavaportalupload=13,

            UpdateChildUIDdavaportalupload = 14,

            UpdateSync = 17,
            /// <summary>
            /// [@Flag,@Value as PackCode]
            /// </summary>
            DeletePacks = 1,

            /// <summary>
            /// 
            /// </summary>
            DeleteAllPacks = 2
         
        }

        private PackagingDetailsDAO _PackagingDetailsDAO;
        public PackagingDetailsDAO PackagingDetailsDAO
        {
            get { return _PackagingDetailsDAO; }
            set { _PackagingDetailsDAO = value; }
        }

        public PackagingDetailsBLL()
        {
            PackagingDetailsDAO = new PackagingDetailsDAO();
        }
        public List<PackagingDetails> GetPackagingDetailss(string Query)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailssQuery(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingDetails> GetPackagingDetailss(OP Op, string ParamVal, string ParamVal1)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailss((int)Op, ParamVal, ParamVal1, ParamVal1); // Updated 'ParamVal1' instead of null. [Sunil 06.10.2016]
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingDetails> GetPackagingDetailssForSync(OP Op, string ParamVal, string ParamVal1)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailssForSync((int)Op, ParamVal, ParamVal1, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Add by Dipesh For Total UIDs
        //public List<PackagingDetails> GetPackagingDetail1(OP Op, string ParamVal, string ParamVal1)
        //{
        //    try
        //    {
        //        return PackagingDetailsDAO.GetPackagingDetailss((int)Op, ParamVal, ParamVal1, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<PackagingDetails> GetPackagingDetailss(Nullable<DateTime> lastUpdatedDate, string format, decimal jobID)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return PackagingDetailsDAO.GetPackagingDetailss((int)OP.GetPacksOfGreaterThanLUDOfJob, jobID.ToString(), dt, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingDetails> GetPackagingDetailss(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return PackagingDetailsDAO.GetPackagingDetailss((int)OP.GetPacksOfGreaterThanLUD, dt, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FOR RETRIVING PACKAGING DETAILS LINEWISE [Sunil 26.09.2015]
        /// </summary>
        /// <param name="Op"></param>
        /// <param name="ParamVal"></param>
        /// <param name="ParamVal1"></param>
        /// <returns></returns>
        public List<PackagingDetails> GetPackagingDetailsByLine(OP Op, string ParamVal, string ParamVal1)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailss((int)Op, ParamVal, ParamVal1, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetNextLevelOfJobForLoose(OP Op, string ParamVal, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                return PackagingDetailsDAO.GetNextLevOfJobelForLoose((int)Op, ParamVal, ParamVal1, ParamVal2, ParamVal3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  For Retriving Packaging Details count as per given condition [05.12.2016]
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="deck"></param>
        /// <param name="NextLevelCode"></param>
        /// <returns></returns>
        public int RetriveCountFromPackagingDetailss(decimal? JobID, string deck, string NextLevelCode)
        {
            try
            {
                return PackagingDetailsDAO.RetriveCountFromPackagingDetails(JobID, deck, NextLevelCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  For Retriving RC Packaging Details count as per given condition [12.01.2016]
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="deck"></param>
        /// <param name="NextLevelCode"></param>
        /// <returns></returns>
        public int RetriveRCCountFromPackagingDetailss(decimal? JobID, string deck,string CommandString)
        {
            try
            {
                return PackagingDetailsDAO.RetriveRCCountFromPackagingDetails(JobID, deck, CommandString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GEtDecomissionedPackCount(decimal JobID, string PackType)
        {

            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETDecomissionedCnt, JobID.ToString(), PackType, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetNextLevelPacks( OP Op, decimal JobID,string PackType)
        {

            try
            {
                return PackagingDetailsDAO.GetnextLevelPacks((int)OP.GetCodesForPrimaryPackDummy, JobID.ToString(), PackType, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }



        public List<PackagingDetails> GetDAVASSCCLevelPacks(OP Op, decimal JobID)
        {

            try
            {
                return PackagingDetailsDAO.GetDAVASSCCLevelPacks((int) Op, JobID.ToString(), null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<PackagingDetails> GetDAVASecondaryPacks(OP Op, decimal JobID,string TerSrNo)
        {

            try
            {
                return PackagingDetailsDAO.GetDAVASecondaryPacks((int)Op, JobID.ToString(), TerSrNo, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<PrimaryPackDummys> GetDAVAPriamryPacks(OP Op, decimal JobID, string SecSrNo)
        {

            try
            {
                return PackagingDetailsDAO.GetDAVAPrimaryPacks((int)Op, JobID.ToString(), SecSrNo, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public PackagingDetails GetPackagingDetails(OP Op, string ParamVal, string ParamVal1)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetails((int)Op, ParamVal, ParamVal1, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Emcure Three deck [20.10.2015]
        /// </summary>
        /// <param name="Op"></param>
        /// <param name="ParamVal"></param>
        /// <param name="ParamVal1"></param>
        /// <param name="ParamVal2"></param>
        /// <returns></returns>
        public PackagingDetails GetPackagingDetails(OP Op, string ParamVal, string ParamVal1,string ParamVal2)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetails((int)Op, ParamVal, ParamVal1, ParamVal2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FOR EMCURE THREE DECK [07.01.2015]
        /// </summary>
        /// <param name="Op"></param>
        /// <param name="ParamVal"></param>
        /// <param name="ParamVal1"></param>
        /// <param name="ParamVal2"></param>
        /// <returns></returns>
        public List<PackagingDetails> GetPackagingDetailss(OP Op, string ParamVal, string ParamVal1, string ParamVal2)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailss((int)Op, ParamVal, ParamVal1, ParamVal2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// TO GET BATCH COUNT [SUNIL 07.01.2015]
        /// </summary>
        /// <param name="Op"></param>
        /// <param name="ParamVal"></param>
        /// <param name="ParamVal1"></param>
        /// <param name="ParamVal2"></param>
        /// <returns></returns>
        public int GetMappedCountOfJob(OP Op, string ParamVal, string ParamVal1, string ParamVal2)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)Op, ParamVal, ParamVal1, ParamVal2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMappedCountOfJobForLoose(OP Op, string ParamVal, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJobForLoose((int)Op, ParamVal, ParamVal1, ParamVal2, ParamVal3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PackagingDetails GetUID4SSCC(OP Op, string ParamVal1)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetails((int)Op, ParamVal1, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSet(OP Op, string ParamVal1, string ParamVal2)
        {
            try
            {
                return PackagingDetailsDAO.GetDataSet((int)Op, ParamVal1, ParamVal2, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetGoodCountOfJob(decimal m_jobId, string PackTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETGOODCnt, m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetPrintedLblCount(decimal m_jobId, string PackTypeCode) //printed 
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETPrintedLblCnt, m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetBadCountOfJob(decimal m_jobId, string PackTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETBadCnt, m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // By Ansuman For NRNU Cnt [21.11.2015]
        public int GetNRNUCountOfJob(decimal m_jobId, string PackTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GetNRNUCnt, m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetBadCountOfJobForRelationCamera(decimal m_jobId, string PackTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETBadCntForRelationCAM, m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetGoodCountOfJobForRelationCamera(decimal m_jobId, string PackTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETGOODCntForRelationCAM , m_jobId.ToString(), PackTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetFailureCountOfJob(decimal m_jobId, string PackTypeCode, float FailureTypecode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GETDS_FailureTypeCounts, m_jobId.ToString(), PackTypeCode, FailureTypecode.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetInspSkippedCountOfJob(decimal m_jobId, string PackTypeCode, float FailureTypecode)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GetSkipInsp, m_jobId.ToString(), PackTypeCode, FailureTypecode.ToString()); // GetInspSkipPcks4Reason replaced by GetSkipInsp [Sunil 23.11.2015]
            }
            catch (Exception ex)
            {
                //throw ex;
                return 0;
            }
        }
        public int GetNotInspectedCount(decimal m_JObID, string oPackageTypeCode, string NotInspText)
        {
            try
            {
                return PackagingDetailsDAO.GetCountDetailsOfJob((int)OP.GetNotInspectedCount, m_JObID.ToString(), oPackageTypeCode, NotInspText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetPrintedCount(string m_JObID, string oPackageTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetPrintedCount(m_JObID, oPackageTypeCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdatePackagingDetails(OP Op, PackagingDetails PckDtls)
        {
            try
            {
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails((int)Op, PckDtls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdatePackagingDetails(int Op, PackagingDetails PckDtls)
        {
            try
            {
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails(Op, PckDtls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemovePackagingDetails(OP Op, string PckCode)
        {
            try
            {
                return PackagingDetailsDAO.RemovePackagingDetails((int)Op, PckCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingDetails> GetDetailJobIformation(decimal JobID, string PackageTypeCode)
        {
            try
            {
                return PackagingDetailsDAO.GetPackagingDetailss((int)OP.ForDetailJobIformation, JobID.ToString(), PackageTypeCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GETUIDsToMAP(decimal JobID, string PackageTypecode, string DefaultNextLevelUID)
        {
            try
            {
                return PackagingDetailsDAO.GETUIDs((int)OP.GETUIDsToMAP, JobID.ToString(), PackageTypecode, DefaultNextLevelUID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetUnmappedInspected_UIDs(decimal JobID, string PackageTypecode, string DefaultNextLevelUID)
        {
            try
            {
                return PackagingDetailsDAO.GETUIDs((int)OP.GETInspectedUnmappedUID, JobID.ToString(), PackageTypecode, DefaultNextLevelUID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetUnmappedPrinted_UIDs(decimal JobID, string PackageTypecode, string DefaultNextLevelUID)
        {
            try
            {
                return PackagingDetailsDAO.GETUIDs((int)OP.GETPrintedUIDs, JobID.ToString(), PackageTypecode, DefaultNextLevelUID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingDetails> DeserializePackagingDetailss(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<PackagingDetails>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializePackagingDetailss(string Path, List<PackagingDetails> PackagingDetailss)
        {
            try
            {
                GenericXmlSerializer<List<PackagingDetails>>.Serialize(PackagingDetailss, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // By tushar
        public List<string> GetBADReason(string JobID, string Code, string UID)
        {
            try
            {
                return PackagingDetailsDAO.GEtBadUIDReason((int)OP.GetBadReason, JobID, Code, UID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // By tushar
        public List<string> GetSkipInsp(string JobID, string Code, string UID)
        {
            try
            {
                return PackagingDetailsDAO.GEtBadUIDReason((int)OP.GetSkipInsp, JobID, Code, UID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // By Dipesh 12/05/2015
        public string GetUIDManualupdatestatus(string JobID, string Code, string UID)
        {
            try
            {
                return PackagingDetailsDAO.GetUIDManualstatus((int)OP.GetUIDManualupdatestatus, JobID, Code, UID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       


        // For PCMAP
        public List<PackagingDetails> GetPCMAP(int level, int JobID, string PackageTypeCode, string DefaultNextLevelUID)
        {
            try
            {
                return PackagingDetailsDAO.GETPCMAP(level, JobID, PackageTypeCode, DefaultNextLevelUID);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // For insert TempPCMAP
        public bool InsertTempPCMAP(int JID, string PALCode, string ISHCode, string OBXCode, string MOCCode, string SSCC)
        {
            try
            {
                return PackagingDetailsDAO.InsertTempPCMAP(JID,PALCode,ISHCode, OBXCode, MOCCode, SSCC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // For Delete TempPCMAP
        public bool DeleteTempPCMAP()
        {
            try
            {
                return PackagingDetailsDAO.DeleteTempPCMAP();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
