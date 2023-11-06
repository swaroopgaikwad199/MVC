using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.Job;

namespace TnT.DataLayer.JobService
{

    public class UIDTransferHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<PackagingDetails> convertToPackagingDetailsold(List<X_Identities> LstOfIdsToSave, Job job, string TeritiaryDeck, decimal PAID, decimal JID, string secondLastDeck)
        {
            List<PackagingDetails> lstPackDetails = new List<PackagingDetails>();
            var UIds = LstOfIdsToSave.Where(x => x.CodeType == false);

            var ssccsCodes = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck);
            var SSCCNos = LstOfIdsToSave.Where(x => x.PackTypeCode == "SSC");

            var SSCCdictionary = ssccsCodes.Zip(SSCCNos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);

            foreach (var uid in UIds)
            {
                if (uid.PackTypeCode != TeritiaryDeck || TeritiaryDeck == "MOC")
                {
                    if (uid.PackTypeCode != TeritiaryDeck + "Loos")
                    {
                        if (job.ProviderId == 2)
                        {
                            if (!uid.PackTypeCode.Contains("LSSCC") && !uid.PackTypeCode.Contains("Loos"))
                            {
                                PackagingDetails pd = new PackagingDetails();
                                pd.PAID = PAID;
                                pd.JobID = JID;
                                pd.PackageTypeCode = uid.PackTypeCode;
                                pd.MfgPackDate = job.MfgDate;
                                pd.ExpPackDate = job.ExpDate;
                                pd.NextLevelCode = "FFFFF";
                                pd.CreatedDate = DateTime.Now;
                                pd.LastUpdatedDate = DateTime.Now;
                                pd.LineCode = job.LineCode;
                                pd.IsUsed = false;
                                pd.CryptoCode = uid.CryptoCode;
                                if (uid.SerialNo.Length==18)
                                {
                                    pd.SSCC = uid.SerialNo;
                                }

                                if (uid.PackTypeCode != "MOC")
                                {
                                    //string sub = uid.SerialNo.Substring(0, 1);
                                    //if (uid.PackTypeCode.Contains("Loos"))
                                    //{
                                    //    pd.SSCC = uid.SerialNo;
                                    //    pd.Code = uid.SerialNo;
                                    //    pd.IsLoose = true;
                                    //    pd.PackageTypeCode = uid.PackTypeCode.Replace("Loos", "");
                                    //    if (pd.PackageTypeCode == TeritiaryDeck)
                                    //        pd.NextLevelCode = null;
                                    //}
                                    //else
                                    //{

                                    pd.Code = uid.SerialNo;
                                    // }
                                }
                                else
                                {
                                    pd.Code = uid.SerialNo;
                                }
                                pd.RCResult = 2;
                                lstPackDetails.Add(pd);
                            }
                        }
                        else
                        {

                            PackagingDetails pd = new PackagingDetails();
                            pd.PAID = PAID;
                            pd.JobID = JID;
                            pd.PackageTypeCode = uid.PackTypeCode;
                            pd.MfgPackDate = job.MfgDate;
                            pd.ExpPackDate = job.ExpDate;
                            pd.NextLevelCode = "FFFFF";
                            pd.CreatedDate = DateTime.Now;
                            pd.LastUpdatedDate = DateTime.Now;
                            pd.LineCode = job.LineCode;
                            pd.IsUsed = false;
                            pd.CryptoCode = uid.CryptoCode;
                            if (uid.PackTypeCode != "MOC")
                            {
                                string sub = uid.SerialNo.Substring(0, 1);
                                if (uid.PackTypeCode.Contains("Loos"))
                                {
                                    pd.SSCC = uid.SerialNo;
                                    pd.Code = uid.SerialNo;
                                    pd.IsLoose = true;
                                    pd.PackageTypeCode = uid.PackTypeCode.Replace("Loos", "");
                                    if (pd.PackageTypeCode == TeritiaryDeck)
                                        pd.NextLevelCode = null;
                                }
                                else
                                {

                                    pd.Code = uid.SerialNo;
                                }
                            }
                            else
                            {
                                pd.Code = uid.SerialNo;
                            }
                            if (uid.SerialNo.Length == 18)
                            {
                                pd.SSCC = uid.SerialNo;
                            }
                            pd.RCResult = 2;
                            lstPackDetails.Add(pd);

                        }
                    }
                }

            }

            if (job.ProviderId == 2)
            {
                var secondlvl = LstOfIdsToSave.Where(x => x.PackTypeCode == secondLastDeck + "Loos");
                var secondloos = LstOfIdsToSave.Where(x => x.PackTypeCode == secondLastDeck + "LSSCC");

                var SSCCdictionarySecondLvl = secondlvl.Zip(secondloos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);

                if (SSCCdictionarySecondLvl.Count > 0)
                {

                    foreach (var uid in SSCCdictionarySecondLvl)
                    {

                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = secondLastDeck;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsLoose = true;
                        pd.IsUsed = false;
                        pd.Code = uid.Key;
                        pd.SSCC = uid.Key;
                        pd.Remarks = uid.Value;
                        lstPackDetails.Add(pd);


                    }
                }

                else
                {
                    if (secondlvl.Count() > 0)
                    {
                        foreach (var uid in secondlvl)
                        {

                            PackagingDetails pd = new PackagingDetails();
                            pd.PAID = PAID;
                            pd.JobID = JID;
                            pd.PackageTypeCode = secondLastDeck;
                            pd.MfgPackDate = job.MfgDate;
                            pd.ExpPackDate = job.ExpDate;
                            pd.CreatedDate = DateTime.Now;
                            pd.LastUpdatedDate = DateTime.Now;
                            pd.LineCode = job.LineCode;
                            pd.IsLoose = true;
                            pd.IsUsed = false;
                            pd.CryptoCode = uid.CryptoCode;
                            pd.Code = uid.SerialNo;
                            pd.SSCC = uid.SerialNo;
                            lstPackDetails.Add(pd);


                        }
                    }
                }

            }
            if (SSCCdictionary.Count() > 0)
            {
                if (TeritiaryDeck != "MOC")
                {
                    foreach (var uid in SSCCdictionary)
                    {
                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = TeritiaryDeck;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        if (job.ProviderId == 2)
                        {
                            pd.Code = uid.Value;
                            pd.SSCC = uid.Value;
                            pd.Remarks = uid.Key;
                        }
                        else
                        {
                            pd.Code = uid.Key;
                            pd.SSCC = uid.Value;
                        }
                        lstPackDetails.Add(pd);
                    }
                }
            }
            else
            {
                foreach (var sscc in SSCCNos)
                {
                    PackagingDetails pd = new PackagingDetails();
                    pd.PAID = PAID;
                    pd.JobID = JID;
                    pd.PackageTypeCode = TeritiaryDeck;
                    pd.MfgPackDate = job.MfgDate;
                    pd.ExpPackDate = job.ExpDate;
                    pd.CreatedDate = DateTime.Now;
                    pd.LastUpdatedDate = DateTime.Now;
                    pd.LineCode = job.LineCode;
                    pd.IsUsed = false;
                    pd.Code = sscc.SerialNo;
                    pd.SSCC = sscc.SerialNo;
                    lstPackDetails.Add(pd);
                }
            }

            if (job.ProviderId != 2)
            {
                var uids = UIds.Where(x => x.PackTypeCode == TeritiaryDeck + "Loos");
                foreach (var uid in uids)
                {
                    if (uid.PackTypeCode.Contains("Loos"))
                    {
                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = uid.PackTypeCode.Replace("Loos", "");

                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.IsLoose = true;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        pd.CryptoCode = uid.CryptoCode;
                        pd.SSCC = uid.SerialNo;
                        pd.Code = uid.SerialNo;
                        pd.RCResult = 2;

                        lstPackDetails.Add(pd);
                    }
                }
            }

            if (job.ProviderId == 2)
            {
                var lvl = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck + "Loos");
                var loos = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck + "LSSCC");

                var SSCCdictionary1 = lvl.Zip(loos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);
                if (SSCCdictionary1.Count > 0)
                {
                    foreach (var uid in SSCCdictionary1)
                    {

                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = TeritiaryDeck;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        pd.Code = uid.Key;
                        pd.SSCC = uid.Key;
                        pd.Remarks = uid.Value;
                        pd.IsLoose = true;
                        lstPackDetails.Add(pd);



                    }
                }
                else
                {
                    if (lvl.Count() > 0)
                    {
                        foreach (var uid in lvl)
                        {

                            PackagingDetails pd = new PackagingDetails();
                            pd.PAID = PAID;
                            pd.JobID = JID;
                            pd.PackageTypeCode = TeritiaryDeck;
                            pd.MfgPackDate = job.MfgDate;
                            pd.ExpPackDate = job.ExpDate;
                            pd.CreatedDate = DateTime.Now;
                            pd.LastUpdatedDate = DateTime.Now;
                            pd.LineCode = job.LineCode;
                            pd.IsUsed = false;
                            pd.Code = uid.SerialNo;
                            pd.SSCC = uid.SerialNo;
                            pd.CryptoCode = uid.CryptoCode;
                            pd.IsLoose = true;
                            lstPackDetails.Add(pd);



                        }
                    }
                }

            }
            return lstPackDetails;

        }
        public List<PackagingDetails> convertToPackagingDetails(List<X_Identities> LstOfIdsToSave, Job job, string TeritiaryDeck, decimal PAID, decimal JID, string secondLastDeck)
        {
            List<PackagingDetails> lstPackDetails = new List<PackagingDetails>();

            try
            {
                var providerOfJob = db.M_Providers.FirstOrDefault(p => p.Id == job.Customer.ProviderId);
                var jobDetails = db.JobDetails.Where(p => p.JD_JobID == job.JID).OrderBy(s => s.Id).ToList();
                var selectedJobType = db.JOBTypes.FirstOrDefault(s => s.TID == job.TID);

                foreach (var jdeck in jobDetails)
                {
                    List<X_Identities> ssccOfDeck = new List<X_Identities>();
                    List<X_Identities> codeOfDeck = new List<X_Identities>();
                    List<X_Identities> loosessccOfDeck = new List<X_Identities>();

                    #region AddCode

                    if (jdeck.JD_Deckcode != jobDetails.OrderByDescending(s => s.Id).FirstOrDefault().JD_Deckcode || jdeck.JD_Deckcode == jobDetails.OrderByDescending(s => s.Id).FirstOrDefault().JD_Deckcode)
                    {
                        codeOfDeck = LstOfIdsToSave.Where(s => s.PackTypeCode == jdeck.JD_Deckcode).ToList();
                    }
                    foreach (var uid in codeOfDeck)
                    {
                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = jdeck.JD_Deckcode;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        if (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)
                        {
                            pd.NextLevelCode = null;
                        }
                        else
                        {
                            pd.NextLevelCode = "FFFFF";
                        }
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        pd.RCResult = 2;
                        pd.CryptoCode = string.IsNullOrWhiteSpace(uid.CryptoCode) ? null : uid.CryptoCode;
                        pd.Code = uid.SerialNo;
                        pd.PublicKey = string.IsNullOrWhiteSpace(uid.PublicKey) ? null : uid.PublicKey;
                        //RUSSIA OTHER THAN MOC == SSCC || LAST DEKC ALWAYS SSCC
                        if ((jobDetails.First().JD_Deckcode != jdeck.JD_Deckcode && selectedJobType.Job_Type == "RUSSIA") ||
                            (jobDetails.Last().JD_Deckcode == jdeck.JD_Deckcode))
                        {
                            if (uid.SerialNo.Length == 18)
                            {
                                pd.SSCC = uid.SerialNo;
                            }
                        }

                        lstPackDetails.Add(pd);
                    }
                    #endregion

                    #region AddSSCC and Loose SSCC
                    if (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)
                    {
                        ssccOfDeck = LstOfIdsToSave.Where(s => s.PackTypeCode == "SSC" ||
                                                               s.PackTypeCode == jdeck.JD_Deckcode + "Loos" ||
                                                               s.PackTypeCode == jdeck.JD_Deckcode + "LSSCC").ToList();
                    }
                    else if (jdeck.JD_Deckcode != jobDetails.First().JD_Deckcode && selectedJobType.Job_Type == "DSCSA")
                    {
                        ssccOfDeck = LstOfIdsToSave.Where(s => s.PackTypeCode == jdeck.JD_Deckcode + "Loos" ||
                                                               s.PackTypeCode == jdeck.JD_Deckcode + "LSSCC").ToList();
                    }

                    foreach (var sscc in ssccOfDeck)
                    {
                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = jdeck.JD_Deckcode;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.NextLevelCode = null;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        pd.RCResult = 2;
                        pd.CryptoCode = string.IsNullOrWhiteSpace(sscc.CryptoCode) ? null : sscc.CryptoCode;
                        pd.Code = sscc.SerialNo;
                        pd.PublicKey = string.IsNullOrWhiteSpace(sscc.PublicKey) ? null : sscc.PublicKey;
                        if (sscc.SerialNo.Length == 18)
                        {
                            pd.SSCC = sscc.SerialNo;
                        }

                        pd.IsLoose = (sscc.PackTypeCode.Contains("Loos") || sscc.PackTypeCode.Contains("LSSCC"));

                        lstPackDetails.Add(pd);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lstPackDetails;
        }

        public List<PackagingDetails> convertToPackagingDetailsForExtraUIDForTlink(List<X_Identities> LstOfIdsToSave, Job job, string TeritiaryDeck, decimal PAID, decimal JID, string secondLastDeck)
        {
            List<PackagingDetails> lstPackDetails = new List<PackagingDetails>();
            var UIds = LstOfIdsToSave.Where(x => x.CodeType == false);

            var ssccsCodes = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck + "SSCC");
            var SSCCNos = LstOfIdsToSave.Where(x => x.PackTypeCode == "SSC");

            var SSCCdictionary = ssccsCodes.Zip(SSCCNos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);

            foreach (var uid in UIds)
            {
                if (uid.PackTypeCode != TeritiaryDeck || TeritiaryDeck == "MOC")
                {
                    if (uid.PackTypeCode != TeritiaryDeck + "Loos")
                    {
                        if (!uid.PackTypeCode.Contains("LSSCC") && !uid.PackTypeCode.Contains("Loos") && !uid.PackTypeCode.Contains("SSCC"))
                        {
                            PackagingDetails pd = new PackagingDetails();
                            pd.PAID = PAID;
                            pd.JobID = JID;
                            pd.PackageTypeCode = uid.PackTypeCode;
                            pd.MfgPackDate = job.MfgDate;
                            pd.ExpPackDate = job.ExpDate;
                            pd.NextLevelCode = "FFFFF";
                            pd.CreatedDate = DateTime.Now;
                            pd.LastUpdatedDate = DateTime.Now;
                            pd.LineCode = job.LineCode;
                            pd.IsUsed = false;
                            if (uid.PackTypeCode != "MOC")
                            {
                                //string sub = uid.SerialNo.Substring(0, 1);
                                //if (uid.PackTypeCode.Contains("Loos"))
                                //{
                                //    pd.SSCC = uid.SerialNo;
                                //    pd.Code = uid.SerialNo;
                                //    pd.IsLoose = true;
                                //    pd.PackageTypeCode = uid.PackTypeCode.Replace("Loos", "");
                                //    if (pd.PackageTypeCode == TeritiaryDeck)
                                //        pd.NextLevelCode = null;
                                //}
                                //else
                                //{

                                pd.Code = uid.SerialNo;
                                // }
                            }
                            else
                            {
                                pd.Code = uid.SerialNo;
                            }
                            pd.RCResult = 2;
                            lstPackDetails.Add(pd);
                        }
                    }
                }

            }

            var secondlvl = LstOfIdsToSave.Where(x => x.PackTypeCode == secondLastDeck + "Loos");
            var secondloos = LstOfIdsToSave.Where(x => x.PackTypeCode == secondLastDeck + "LSSCC");

            var SSCCdictionarySecondLvl = secondlvl.Zip(secondloos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);
            if (SSCCdictionarySecondLvl.Count > 0)
            {

                foreach (var uid in SSCCdictionarySecondLvl)
                {

                    PackagingDetails pd = new PackagingDetails();
                    pd.PAID = PAID;
                    pd.JobID = JID;
                    pd.PackageTypeCode = secondLastDeck;
                    pd.MfgPackDate = job.MfgDate;
                    pd.ExpPackDate = job.ExpDate;
                    pd.CreatedDate = DateTime.Now;
                    pd.LastUpdatedDate = DateTime.Now;
                    pd.LineCode = job.LineCode;
                    pd.IsUsed = false;
                    pd.Code = uid.Value;
                    pd.SSCC = uid.Key;
                    lstPackDetails.Add(pd);


                }
            }


            if (SSCCdictionary.Count() > 0)
            {
                if (TeritiaryDeck != "MOC")
                {
                    foreach (var uid in SSCCdictionary)
                    {
                        PackagingDetails pd = new PackagingDetails();
                        pd.PAID = PAID;
                        pd.JobID = JID;
                        pd.PackageTypeCode = TeritiaryDeck;
                        pd.MfgPackDate = job.MfgDate;
                        pd.ExpPackDate = job.ExpDate;
                        pd.CreatedDate = DateTime.Now;
                        pd.LastUpdatedDate = DateTime.Now;
                        pd.LineCode = job.LineCode;
                        pd.IsUsed = false;
                        pd.Code = uid.Key;
                        pd.SSCC = uid.Value;
                        lstPackDetails.Add(pd);
                    }
                }
            }
            else
            {
                foreach (var sscc in SSCCNos)
                {
                    PackagingDetails pd = new PackagingDetails();
                    pd.PAID = PAID;
                    pd.JobID = JID;
                    pd.PackageTypeCode = TeritiaryDeck;
                    pd.MfgPackDate = job.MfgDate;
                    pd.ExpPackDate = job.ExpDate;
                    pd.CreatedDate = DateTime.Now;
                    pd.LastUpdatedDate = DateTime.Now;
                    pd.LineCode = job.LineCode;
                    pd.IsUsed = false;
                    pd.Code = sscc.SerialNo;
                    pd.SSCC = sscc.SerialNo;
                    lstPackDetails.Add(pd);
                }
            }



            var lvl = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck + "Loos");
            var loos = LstOfIdsToSave.Where(x => x.PackTypeCode == TeritiaryDeck + "LSSCC");

            var SSCCdictionary1 = lvl.Zip(loos, (k, v) => new { Key = k.SerialNo, Value = v.SerialNo }).ToDictionary(x => x.Key, x => x.Value);
            if (SSCCdictionary1.Count > 0)
            {
                foreach (var uid in SSCCdictionary1)
                {

                    PackagingDetails pd = new PackagingDetails();
                    pd.PAID = PAID;
                    pd.JobID = JID;
                    pd.PackageTypeCode = TeritiaryDeck;
                    pd.MfgPackDate = job.MfgDate;
                    pd.ExpPackDate = job.ExpDate;
                    pd.CreatedDate = DateTime.Now;
                    pd.LastUpdatedDate = DateTime.Now;
                    pd.LineCode = job.LineCode;
                    pd.IsUsed = false;
                    pd.Code = uid.Value;
                    pd.SSCC = uid.Key;
                    lstPackDetails.Add(pd);



                }
            }


            return lstPackDetails;

        }

    }
}