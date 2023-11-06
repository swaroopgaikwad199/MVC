using Microsoft.ApplicationBlocks.Data;
using PTPL.SSCC_Gen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TnT.DataLayer.JobService;
using TnT.DataLayer.TracelinkService;
using TnT.Models;
using TnT.Models.Customer;
using TnT.Models.Job;

namespace TnT.DataLayer
{
    public class IDGenrationFactory
    {
        private List<string> Uids;
        private List<string> SSCC;
        private List<string> SSCC1;
        private SSCCHelper ssccHelperObj;
        public int Quantity;
        private int PPNTransportNumberUID = 13;

        public List<string> generateIDs(int Quantity, int Lenght,string selectedJobType)
        {
            try
            {
                PTPLUidGen obj = new PTPLUidGen();
                Uids = new List<string>();

                string uid;
                for (int i = 0; i < Quantity; i++)
                {
                    uid = obj.GenerateUID(Lenght, selectedJobType);
                    Uids.Add(uid);
                }

                return Uids;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> generateCryptoCode(int Quantity, int Lenght)
        {
            try
            {
                PTPLUidGen obj = new PTPLUidGen();
                Uids = new List<string>();

                string uid;
                for (int i = 0; i < Quantity; i++)
                {
                    uid = obj.GenerateCryptoCode(Lenght);
                    Uids.Add(uid);
                }

                return Uids;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //    public List<string> generateSSCC(int JobID, int requestedUidsQty, string selectedJobType, string IAC_CIN)
        //    {
        //        try
        //        {
        //            if (selectedJobType == "DGFT" || selectedJobType == "CIP" || selectedJobType == "DSCSA" || selectedJobType== "Russia")
        //            {
        //                string CompanyCode, PlantCode;
        //                int LastSSCC;
        //                ssccHelperObj = new SSCCHelper();
        //                DataRow dr = SSCCHelper.getCompanyDetails();
        //                CompanyCode = dr[0].ToString();
        //                PlantCode = dr[1].ToString();
        //                //Quantity = ssccHelperObj.getQuantity(JobID);

        //                int sscc = ssccHelperObj.getLastSSCC();
        //                LastSSCC = (sscc == 0) ? 1 : sscc;

        //                if (requestedUidsQty > 0)
        //                {
        //                    //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);


        //                    int newFirstSSCC = LastSSCC + 1;
        //                    int newLastSSCC = newFirstSSCC + Quantity;

        //                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC))
        //                    {
        //                        SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
        //                        return SSCC;
        //                    }
        //                    else
        //                    {
        //                        return null;
        //                    }
        //                }
        //                else
        //                {
        //                    return null;
        //                }

        //            }
        //            else if (selectedJobType == "PPN")
        //            {
        //                PTPLUidGen.UIDGen obj = new PTPLUidGen.UIDGen();
        //                Uids = new List<string>();

        //                string uid;
        //                for (int i = 0; i < requestedUidsQty; i++)
        //                {
        //                    uid = obj.GenerateUID(PPNTransportNumberUID);
        //                    Uids.Add(IAC_CIN + uid);
        //                }

        //                return Uids;
        //            }
        //            else {
        //                return null;
        //            }

        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private List<string> complianceCommonGenrtr(int JobID, int requestedUidsQty)
        {
            string CompanyCode, PlantCode;
            int LastSSCC;
            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);

            int sscc = ssccHelperObj.getLastSSCC("SSCC");
            LastSSCC =  ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode,sscc);

            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);


                int newFirstSSCC = LastSSCC + 1;
                int newLastSSCC = LastSSCC + requestedUidsQty;

                if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC"))
                {
                    //SSCC = PTPL.SSCC_Gen.SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                    SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, newFirstSSCC);
                    return SSCC;
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
        private List<string> complianceCommonGenrtr(int JobID, int requestedUidsQty, string LoosExt, string SSCCExt, int CustomerId)
        {
            int newFirstSSCC=0;
            string CompanyCode, PlantCode;
            int LastSSCC, EndSSCC = 0;
            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);

            int sscc = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);

            LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode,sscc);

            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);
                
              
                    newFirstSSCC = LastSSCC + 1;
                
                int newLastSSCC = LastSSCC + requestedUidsQty;

                if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC", SSCCExt, CustomerId))
                {
                    //SSCC = PTPL.SSCC_Gen.SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                    SSCC= TracelinkService.SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, newFirstSSCC, SSCCExt);
                    return SSCC;
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

        private List<string> complianceCommonGenrtrTLink(int JobID, int requestedUidsQty, string SSCCExt, int CustomerId,string pkglvl, bool isLoose)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            string CompanyCode, PlantCode;
            int LastSSCC, ssccRangeLength, sscctoprint;
            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);


            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);
                //int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                //if (pkglvl != "DSCSA")
                {
                    sscctoprint = requestedUidsQty;
                }
                //else
                //{
                //    sscctoprint = requestedUidsQty - looseshiper;
                //}

              //Commented by Kiran
                //if (looseshiper>0)
                //{
                //    sscc = ssccHelperObj.getLastSSCC("Loos");
                //    LastSSCC = (sscc == 0) ? 1 : sscc;
                //    int newFirstSSCC = LastSSCC + 1;
                //    int newLastSSCC = LastSSCC + looseshiper;

                //    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "Loos"))
                //    {
                //        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                //        SSCC1= TracelinkService.SSCCGeneration.GenerateSSCC(looseshiper, CompanyCode, PlantCode, LastSSCC,"Loos");
                      
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}
                //else
                //{
                //    return null;
                //}

                if (sscctoprint > 0)
                {
                    
                    LastSSCC = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);
                    //ssccRangeLength = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + sscctoprint;

                    //int totalWidth = 17 - (1 + CompanyCode.Length /*+ PlantCode.Length*/);

                    //do
                    //{
                    //    if (newLastSSCC.ToString().Length >= totalWidth)
                    //    {
                    //        LastSSCC = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);
                    //        //ssccRangeLength = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode);
                    //        newFirstSSCC = LastSSCC + 1;
                    //        newLastSSCC = LastSSCC + sscctoprint;

                    //        if (SSCCExt == "9" || SSCCExt == "8" || SSCCExt == "7")
                    //        {
                    //            SSCCExt = "0";
                    //        }
                    //        else
                    //        {
                    //            SSCCExt = (Convert.ToInt32(SSCCExt) + 1).ToString();
                    //        }

                    //        LastSSCC = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);
                    //        //ssccRangeLength = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode);
                    //        newFirstSSCC = LastSSCC + 1;
                    //        newLastSSCC = LastSSCC + sscctoprint;
                    //    }
                    //} while (newLastSSCC.ToString().Length >= totalWidth);

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC", SSCCExt, CustomerId))
                    {
                        //M_Customer obj = db.M_Customer.FirstOrDefault(x => x.Id == CustomerId);
                        //if (isLoose)
                        //{
                        //    obj.LoosExt = SSCCExt;
                        //}
                        //else
                        //{
                        //    obj.SSCCExt = SSCCExt;
                        //}
                        //db.Entry(obj).State = EntityState.Modified;
                        //db.SaveChanges();

                        SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(sscctoprint, CompanyCode, PlantCode, newFirstSSCC, SSCCExt);
                        
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
                //SSCC = SSCC.Concat(SSCC1).ToList();
                return SSCC;
            }
            else
            {
                return null;
            }
        }

        private List<string> complianceCommonGenrtrTLink(int JobID, int requestedUidsQty, string SSCCExt, int CustomerId, string pkglvl)
        {
            string CompanyCode, PlantCode;
            int LastSSCC, sscc, sscctoprint;
            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);


            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                if (pkglvl != "DSCSA")
                {
                    sscctoprint = requestedUidsQty;
                }
                else
                {
                    sscctoprint = requestedUidsQty - looseshiper;
                }

                //Commented by Kiran
                //if (looseshiper>0)
                //{
                //    sscc = ssccHelperObj.getLastSSCC("Loos");
                //    LastSSCC = (sscc == 0) ? 1 : sscc;
                //    int newFirstSSCC = LastSSCC + 1;
                //    int newLastSSCC = LastSSCC + looseshiper;

                //    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "Loos"))
                //    {
                //        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                //        SSCC1= TracelinkService.SSCCGeneration.GenerateSSCC(looseshiper, CompanyCode, PlantCode, LastSSCC,"Loos");

                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}
                //else
                //{
                //    return null;
                //}

                if (sscctoprint > 0)
                {
                    sscc = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);
                    LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode, sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + sscctoprint;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC", SSCCExt, CustomerId))
                    {
                        SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(sscctoprint, CompanyCode, PlantCode, newFirstSSCC, SSCCExt);

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
                //SSCC = SSCC.Concat(SSCC1).ToList();
                return SSCC;
            }
            else
            {
                return null;
            }
        }

        private List<string> compliancePPN(int requestedUidsQty, string IAC_CIN,string selectedJobType)
        {

            PTPLUidGen obj = new PTPLUidGen();
            Uids = new List<string>();
            string uid;
            for (int i = 0; i < requestedUidsQty; i++)
            {
                uid = obj.GenerateUID(PPNTransportNumberUID, selectedJobType);
                Uids.Add(IAC_CIN + uid);
            }
            return Uids;
        }

        private List<string> complianceCommonDSCSA(int JobID, int requestedUidsQty)
        {
            string CompanyCode, PlantCode;
            int LastSSCC, sscc;
            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);


            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                int sscctoprint = requestedUidsQty - looseshiper;



                if (sscctoprint > 0)
                {
                    sscc = ssccHelperObj.getLastSSCC("SSCC");
                    LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode,sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + sscctoprint;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC"))
                    {
                        SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(sscctoprint, CompanyCode, PlantCode, newFirstSSCC, "SSCC");

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

                return SSCC;
            }
            else
            {
                return null;
            }
        }
        private List<string> complianceCommonDSCSA(int JobID, int requestedUidsQty, string loosExt, string SSCCExt, int CustomerId)
        {
            string CompanyCode, PlantCode;
            int LastSSCC, sscc;

            ssccHelperObj = new SSCCHelper();
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            //Quantity = ssccHelperObj.getQuantity(JobID);


            if (requestedUidsQty > 0)
            {
                //int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                int sscctoprint = requestedUidsQty - looseshiper;


                //if (looseshiper > 0)
                //{
                //    sscc = ssccHelperObj.getLastSSCC("DLoos");
                //    LastSSCC = (sscc == 0) ? 1 : sscc;
                //    int newFirstSSCC = LastSSCC + 1;
                //    int newLastSSCC = LastSSCC + looseshiper;

                //    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "DLoos"))
                //    {
                //        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                //        SSCC1 = TracelinkService.SSCCGeneration.GenerateSSCC(looseshiper, CompanyCode, PlantCode, newFirstSSCC, "Loos");

                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}
                //else
                //{
                //    return null;
                //}

                if (requestedUidsQty > 0)
                {
                    sscc = ssccHelperObj.getLastSSCC("SSCC", CustomerId, SSCCExt);
                  
                    LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode,sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + requestedUidsQty;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "SSCC", SSCCExt, CustomerId))
                    {
                        SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, newFirstSSCC, SSCCExt);

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
               
                return SSCC;
            }
            else
            {
                return null;
            }
        }
        public List<string> generateSSCC(int JobID, int requestedUidsQty, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId)
        {
            try
            {

                BatchComplianceTypeHelper ctHelpr = new BatchComplianceTypeHelper();
                BatchComplianceType ctype = BatchComplianceTypeHelper.convertToComplianceType(selectedJobType);

                switch (ctype)
                {
                    case BatchComplianceType.India_DGFT:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    //case BatchComplianceType.Germany_PPN:
                    //    return compliancePPN(requestedUidsQty, IAC_CIN);
                    case BatchComplianceType.Germany_PPN:
                        return compliancePPN(requestedUidsQty, IAC_CIN,selectedJobType);

                    case BatchComplianceType.USA_DSCSA:
                        return complianceCommonDSCSA(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.France_CIP:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.Non_GS1:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.RUSSIA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);
                    case BatchComplianceType.SOUTHKOREA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.SAUDIARABIA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.CHINACODE:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.TURKEY:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    case BatchComplianceType.EU:
                        return complianceCommonGenrtr(JobID, requestedUidsQty, LoosExt, SSCCExt, CustomerId);

                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> generateSSCC(int JobID, int requestedUidsQty, string selectedJobType, string IAC_CIN)
        {
            try
            {

                BatchComplianceTypeHelper ctHelpr = new BatchComplianceTypeHelper();
                BatchComplianceType ctype = BatchComplianceTypeHelper.convertToComplianceType(selectedJobType);

                switch (ctype)
                {
                    case BatchComplianceType.India_DGFT:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.Germany_PPN:
                        return compliancePPN(requestedUidsQty, IAC_CIN, selectedJobType);

                    case BatchComplianceType.USA_DSCSA:
                        return complianceCommonDSCSA(JobID, requestedUidsQty);

                    case BatchComplianceType.France_CIP:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.Non_GS1:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.RUSSIA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);
                    case BatchComplianceType.SOUTHKOREA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.SAUDIARABIA:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.CHINACODE:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);

                    case BatchComplianceType.TURKEY:
                        return complianceCommonGenrtr(JobID, requestedUidsQty);


                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> generateSSCCTlink(int JobID, int requestedUidsQty, string selectedJobType, string IAC_CIN, bool IsLoos, string SSCCExt, int CustomerId)
        {
            try
            {
                if (string.IsNullOrEmpty(SSCCExt))
                {
                    string cur_yr_ext = DateTime.Now.ToString("yyyy");
                    SSCCExt = cur_yr_ext.Substring(3, 1);
                }
                //if (string.IsNullOrEmpty(LoosExt))
                //{
                //    string cur_yr_ext = DateTime.Now.ToString("yyyy");
                //    LoosExt = cur_yr_ext.Substring(3, 1);
                //}

                BatchComplianceTypeHelper ctHelpr = new BatchComplianceTypeHelper();
                BatchComplianceType ctype = BatchComplianceTypeHelper.convertToComplianceType(selectedJobType);


                return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, selectedJobType.ToString(), IsLoos);

                //switch (ctype)
                //{
                //    case BatchComplianceType.India_DGFT:
                //return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId,"DGFT");

                //    case BatchComplianceType.Germany_PPN:
                //        return compliancePPN(requestedUidsQty, IAC_CIN, selectedJobType);

                //    case BatchComplianceType.USA_DSCSA:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId,"DSCSA");

                //    case BatchComplianceType.France_CIP:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId,"CIP");

                //    case BatchComplianceType.Non_GS1:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "Non_GS1");

                //    case BatchComplianceType.RUSSIA:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "RUSSIA");

                //    case BatchComplianceType.SAUDIARABIA:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "SAUDIARABIA");

                //    case BatchComplianceType.TURKEY:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "TURKEY");
                //    case BatchComplianceType.EU:
                //        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "EU");
                //    default:
                //        return null;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> generateSSCCTlink(int JobID, int requestedUidsQty, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId)
        {
            try
            {

                BatchComplianceTypeHelper ctHelpr = new BatchComplianceTypeHelper();
                BatchComplianceType ctype = BatchComplianceTypeHelper.convertToComplianceType(selectedJobType);

                switch (ctype)
                {
                    case BatchComplianceType.India_DGFT:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "DGFT");

                    case BatchComplianceType.Germany_PPN:
                        return compliancePPN(requestedUidsQty, IAC_CIN, selectedJobType);

                    case BatchComplianceType.USA_DSCSA:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "DSCSA");

                    case BatchComplianceType.France_CIP:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "CIP");

                    case BatchComplianceType.Non_GS1:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "Non_GS1");

                    case BatchComplianceType.RUSSIA:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "RUSSIA");

                    case BatchComplianceType.SAUDIARABIA:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "SAUDIARABIA");

                    case BatchComplianceType.TURKEY:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "TURKEY");
                    case BatchComplianceType.EU:
                        return complianceCommonGenrtrTLink(JobID, requestedUidsQty, SSCCExt, CustomerId, "EU");
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<string> generateSSCC(int JobID, int requestedUidsQty)
        //    {
        //        try
        //        {
        //            string CompanyCode, PlantCode;
        //            int LastSSCC;
        //            ssccHelperObj = new SSCCHelper();
        //            DataRow dr = SSCCHelper.getCompanyDetails();
        //            CompanyCode = dr[0].ToString();
        //            PlantCode = dr[1].ToString();
        //            Quantity = ssccHelperObj.getQuantity(JobID);

        //            int sscc = ssccHelperObj.getLastSSCC();
        //            LastSSCC = (sscc == 0) ? 1 : sscc;

        //            if (requestedUidsQty > 0)
        //            {
        //                int CountSSCC = (int)Math.Ceiling(((double)requestedUidsQty + (double)Quantity - 1) / (double)Quantity);


        //                int newFirstSSCC = LastSSCC + 1;
        //                int newLastSSCC = newFirstSSCC + Quantity;

        //                if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC))
        //                {
        //                    SSCC = SSCCGeneration.GenerateSSCC(CountSSCC, CompanyCode, PlantCode, LastSSCC);
        //                    return SSCC;
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }


    }

    class SSCCHelper
        {
        public bool insertLineHolder(int jobId, int FirstSSCC, int LastSSCC, string Type)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                string qry = @"INSERT INTO [dbo].[SSCCLineHolder]  ([LastSSCC] ,[JobID] ,[FirstSSCC],[Type])  VALUES
                                                                (" + FirstSSCC + "," + jobId + "," + LastSSCC + ",'" + Type + "')";
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);

                if (rowAffected == 1)
                {
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

        public bool insertLineHolder(int jobId, int FirstSSCC, int LastSSCC,string Type, string Extension, int Customerid)
        {
                try
                {
                    string connectionString, qry;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                if (Extension != null)
                {
                    qry = @"INSERT INTO [dbo].[SSCCLineHolder]  ([LastSSCC] ,[JobID] ,[FirstSSCC],[Type],[PackageIndicator],[Customer])  VALUES
                                                                (" + FirstSSCC + "," + jobId + "," + LastSSCC + ",'" + Type + "','" + Extension + "'," + Customerid + ")";
                }
                else
                {
                    qry = @"INSERT INTO [dbo].[SSCCLineHolder]  ([LastSSCC] ,[JobID] ,[FirstSSCC],[Type],[Customer])  VALUES
                                                                (" + FirstSSCC + "," + jobId + "," + LastSSCC + ",'" + Type + "'," + Customerid + ")";
                }
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);

                    if (rowAffected == 1)
                    {
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



      

        public static DataRow getCompanyDetails()
            {
                try
                {
                    string connectionString;
                    connectionString = Utilities.getConnectionString("DefaultConnection");
                    string qry = "SELECT   TOP (1)  CompanyCode, PlantCode FROM dbo.Settings";
                    DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, qry);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }



            public int getLastSSCC()
            {
                try
                {
                    string qry = @"SELECT        FirstSSCC FROM dbo.SSCCLineHolder WHERE(ID =
                             (SELECT        IDENT_CURRENT('dbo.SSCCLineHolder') AS Expr1 ))";
                    return getNumberUtility(qry);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        public int getLastSSCC(string type)
        {
            try
            {
                string qry = string.Empty;
                if (type == "SSCC")
                {
                    qry = @"select  top 1 firstsscc from SSCCLineHolder where type='" + type + "' or type is null order by ID desc";
                }

                else
                {
                    qry = @"select  top 1 firstsscc from SSCCLineHolder where type='" + type + "' order by ID desc";
                }
                return getNumberUtility(qry);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int getLastSSCC(string type, int CustomerId, string Extension)
        {
            try
            {
                string qry = string.Empty;
                if (Extension != null )
                {
                    qry = @"select  top 1 firstsscc from SSCCLineHolder where type='" + type + "' and PackageIndicator='" + Extension + "'  order by ID desc";
                }
                else
                {
                    if (type == "Loos")
                    {
                        qry = @"select  top 1 firstsscc from SSCCLineHolder  order by ID desc";
                    }
                    else
                    {
                        qry = @"select  top 1 firstsscc from SSCCLineHolder where type='" + type + "' order by ID  desc";
                    }
                }

                return getNumberUtility(qry);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int getNumberUtility(string Query)
            {
                try
                {
                    string connectionString;
                    connectionString = Utilities.getConnectionString("DefaultConnection");
                    DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, Query);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        public int getLastSSCCCompLengthWise(string CompanyCode, int sscc)
        {
            int RequiredSSCLength = 0;
            string sscclength = sscc.ToString();

            switch (CompanyCode.Length.ToString())
            {
                case "7":
                    RequiredSSCLength = 7;
                    break;
                case "8":
                    RequiredSSCLength = 6;
                    break;
                case "9":
                    RequiredSSCLength = 5;
                    break;
                case "10":
                    RequiredSSCLength = 4;
                    break;
            }
            if (sscclength.Length > RequiredSSCLength)
            {
                sscc = 0;
            }
            return sscc;
        }


    }

        class DemoSSCC
        {
            int PPBCode = 7;
            int MOCCode = 8;
            int OBXCode = 9;
            int ISHCode = 10;
            int OSHCode = 11;
            int PALCode = 12;
            int SSCCCode = 20;
            private int MOCtoPrint, OBXtoPrint, ISHtoPrint, OSHtoPrint, PPBtoPrint, PALtoPrint, SSCCtoPrint;
            private int MOC, OBX, ISH, PAL, PPB, OSH;
            private string TertiaryLevel;


            public int getSSCCToPrint(Job job)
            {
                setProductData((int)job.PAID);
                CalculateIds(job.Quantity + job.SurPlusQty);
                CalculateSSCCToPrint();
                return SSCCtoPrint;
            }

            public string getTertDeck()
            {
                return TertiaryLevel;
            }


            private void setProductData(int PAID)
            {
                try
                {
                    string connectionString;
                    connectionString = Utilities.getConnectionString("DefaultConnection");
                    string query = @"SELECT DISTINCT TOP(6) PackageTypeCode, Size, GTIN, LastUpdatedDate
                            FROM            dbo.PackagingAssoDetails
                            WHERE(PAID = " + PAID + ")  ORDER BY LastUpdatedDate DESC ";
                    DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, query);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var dr = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "MOC").FirstOrDefault();

                        if (dr != null)
                        {
                            MOC = Convert.ToInt32(dr[1].ToString());
                        }
                        else
                        {
                            MOC = -1;
                        }

                        var dr1 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "PPB").FirstOrDefault();
                        if (dr1 != null)
                        {
                            PPB = Convert.ToInt32(dr1[1].ToString());
                        }
                        else
                        {
                            PPB = -1;
                        }

                        var dr2 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "OBX").FirstOrDefault();
                        if (dr2 != null)
                        {
                            OBX = Convert.ToInt32(dr2[1].ToString());
                        }
                        else
                        {
                            OBX = -1;
                        }

                        var dr3 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "ISH").FirstOrDefault();
                        if (dr3 != null)
                        {
                            ISH = Convert.ToInt32(dr3[1].ToString());
                        }
                        else
                        {
                            ISH = -1;
                        }

                        var dr4 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "OSH").FirstOrDefault();
                        if (dr4 != null)
                        {
                            OSH = Convert.ToInt32(dr4[1].ToString());
                        }
                        else
                        {
                            OSH = -1;
                        }

                        var dr5 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "PAL").FirstOrDefault();
                        if (dr5 != null)
                        {
                            PAL = Convert.ToInt32(dr5[1].ToString());
                        }
                        else
                        {
                            PAL = -1;
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }



            private int GetLastLevel()
            {
                try
                {

                    if (PAL != -1)
                    {
                        return PALCode;
                    }
                    else if (OSH != -1)
                    {
                        return OSHCode;
                    }

                    else if (ISH != -1)
                    {
                        return ISHCode;
                    }

                    else if (OBX != -1)
                    {
                        return OBXCode;
                    }

                    else if (MOC != -1)
                    {
                        return MOCCode;
                    }
                    else if (PPB != -1)
                    {
                        return PPBCode;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

            private void CalculateSSCCToPrint()
            {
                try
                {


                    int LastLevel;
                    LastLevel = GetLastLevel();
                    if (LastLevel == PALCode)
                    {
                        SSCCtoPrint = PALtoPrint;
                        TertiaryLevel = "PAL";
                    }
                    else if (LastLevel == OSHCode)
                    {
                        SSCCtoPrint = OSHtoPrint;
                        TertiaryLevel = "OSH";
                    }
                    else if (LastLevel == ISHCode)
                    {
                        SSCCtoPrint = ISHtoPrint;
                        TertiaryLevel = "ISH";
                    }
                    else if (LastLevel == OBXCode)
                    {
                        SSCCtoPrint = OBXtoPrint;
                        TertiaryLevel = "OBX";
                    }
                    else if (LastLevel == MOCCode)
                    {
                        SSCCtoPrint = MOCtoPrint;
                        TertiaryLevel = "MOC";
                    }

                    else if (LastLevel == PPBCode)
                    {
                        SSCCtoPrint = PPB;
                        TertiaryLevel = "PPB";
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }


            private bool IsLevelExisting(int level)
            {
                if (level == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            private void CalculateIds(int TotalUIds)
            {
                int UIDQuantity = TotalUIds;
                try
                {

                    if (UIDQuantity == 0) return;

                    if (IsLevelExisting(PPB))
                    {
                        PPBtoPrint = UIDQuantity;
                        if (PPBtoPrint != 0)
                        {

                        }
                    }

                    if (IsLevelExisting(MOC))
                    {
                        if (!IsLevelExisting(PPB))
                        {
                            MOCtoPrint = UIDQuantity;

                        }
                        else
                        {
                            double res = (float)PPBtoPrint / (float)MOC;
                            MOCtoPrint = (int)Math.Ceiling(res);

                        }
                    }

                    if (IsLevelExisting(OBX))
                    {
                        if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                        {
                            OBXtoPrint = UIDQuantity;

                        }
                        else
                        {
                            if (!IsLevelExisting(MOC))
                            {
                                if (IsLevelExisting(PPB))
                                {
                                    double res = (float)PPBtoPrint / (float)OBX;
                                    OBXtoPrint = (int)Math.Ceiling(res);

                                }
                            }
                            else
                            {
                                double res = (float)MOCtoPrint / (float)OBX;
                                OBXtoPrint = (int)Math.Ceiling(res);
                            }

                        }
                    }


                    if (IsLevelExisting(ISH))
                    {
                        if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                        {
                            ISHtoPrint = UIDQuantity;
                        }
                        else
                        {
                            if (!IsLevelExisting(OBX))
                            {
                                if (!IsLevelExisting(MOC))
                                {
                                    if (IsLevelExisting(PPB))
                                    {
                                        double res = (float)PPBtoPrint / (float)ISH;
                                        ISHtoPrint = (int)Math.Ceiling(res);
                                    }
                                }
                                else
                                {
                                    double res = (float)MOCtoPrint / (float)ISH;
                                    ISHtoPrint = (int)Math.Ceiling(res);
                                }

                            }
                            else
                            {
                                double res = (float)OBXtoPrint / (float)ISH;
                                ISHtoPrint = (int)Math.Ceiling(res);

                            }
                        }
                    }

                    if (IsLevelExisting(OSH))
                    {
                        if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                        {
                            OSHtoPrint = UIDQuantity;


                        }
                        else
                        {
                            if (!IsLevelExisting(ISH))
                            {
                                if (!IsLevelExisting(OBX))
                                {
                                    if (!IsLevelExisting(MOC))
                                    {
                                        if (IsLevelExisting(PPB))
                                        {
                                            double res = (float)PPBtoPrint / (float)OSH;
                                            OSHtoPrint = (int)Math.Ceiling(res);
                                        }
                                    }
                                    else
                                    {
                                        double res = (float)MOCtoPrint / (float)OSH;
                                        OSHtoPrint = (int)Math.Ceiling(res);
                                    }
                                }
                                else
                                {
                                    double res = (float)OBXtoPrint / (float)OSH;
                                    OSHtoPrint = (int)Math.Ceiling(res);
                                }
                            }
                            else
                            {
                                double res = (float)ISHtoPrint / (float)OSH;
                                OSHtoPrint = (int)Math.Ceiling(res);
                            }
                        }
                    }

                    if (IsLevelExisting(PAL))
                    {
                        if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                        {
                            PALtoPrint = UIDQuantity;
                        }
                        else
                        {
                            if (!IsLevelExisting(OSH))
                            {
                                if (!IsLevelExisting(ISH))
                                {
                                    if (!IsLevelExisting(OBX))
                                    {
                                        if (!IsLevelExisting(MOC))
                                        {
                                            if (IsLevelExisting(PPB))
                                            {
                                                double res = (float)PPBtoPrint / (float)PAL;
                                                PALtoPrint = (int)Math.Ceiling(res);
                                            }
                                        }
                                        else
                                        {
                                            double res = (float)MOCtoPrint / (float)PAL;
                                            PALtoPrint = (int)Math.Ceiling(res);
                                        }
                                    }
                                    else
                                    {
                                        double res = (float)OBXtoPrint / (float)PAL;
                                        PALtoPrint = (int)Math.Ceiling(res);
                                    }
                                }
                                else
                                {
                                    double res = (float)ISHtoPrint / (float)PAL;
                                    PALtoPrint = (int)Math.Ceiling(res);
                                }
                            }
                            else
                            {
                                double res = (float)OSHtoPrint / (float)PAL;
                                PALtoPrint = (int)Math.Ceiling(res);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    
}