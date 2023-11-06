using EPCIS_XMLs_Generation;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using TnT.DataLayer.RFXCELService;
using TnT.DataLayer.TracelinkService;
using TnT.DataLayer.UIDService;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.ImportXml;
using TnT.Models.Job;
using TnT.Models.TraceLinkImporter;

namespace TnT.DataLayer.JobService
{
    public class IDDataGeneratorUtility
    {

        ApplicationDbContext db = new ApplicationDbContext();


        public bool GenerateTracelinkData(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            try
            {
                string type = job.CompType;
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id);
                pacAssoDet = pacAssoDet.Distinct().OrderBy(x => x.Id);
                string tertiaryGTIN = "";
                List<X_Identities> masterList = new List<X_Identities>();
                var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN, x.NTIN });
                TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
                List<M_TracelinkRequest> lstTLRequests = new List<M_TracelinkRequest>();
                M_TracelinkRequest TLdata = new M_TracelinkRequest();
                var selectedCustomer = db.M_Customer.FirstOrDefault(s => s.Id == job.CustomerId);

                foreach (var item in comparableData)
                {
                    if (selectedJobType == "RUSSIA" && !Convert.ToBoolean(selectedCustomer.IsProvideCodeForMiddleDeck))
                    {
                        if (item.PackageTypeCode == "MOC")
                        {
                            if (item.GTIN != "")
                            {
                                TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN &&
                                                                          x.CustomerId == selectedCustomer.Id &&
                                                                          x.ProviderId == job.ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();
                                type = "GTIN";
                            }
                            else
                            {
                                TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.NTIN &&
                                                                          x.CustomerId == selectedCustomer.Id &&
                                                                          x.ProviderId == job.ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();
                            }
                            if (TLdata != null)                                                                                                                            // lstTLRequests.AddRange(TLdata);
                                lstTLRequests.Add(TLdata);
                        }
                    }
                    else
                    {
                        if (item.GTIN != "")
                        {
                            TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN &&
                                                                      x.CustomerId == selectedCustomer.Id &&
                                                                      x.ProviderId == job.ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();
                            type = "GTIN";
                        }
                        else
                        {
                            TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.NTIN &&
                                                                      x.CustomerId == selectedCustomer.Id &&
                                                                      x.ProviderId == job.ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();
                        }
                        if (TLdata != null)                                                                                                                            // lstTLRequests.AddRange(TLdata);
                            lstTLRequests.Add(TLdata);
                    }
                }
                if (lstTLRequests.Count() > 0 || comparableData.Count() == 1)
                {
                    //Request  Id, GTIN, PackTypeCode
                    List<Tuple<int, string, string>> actualData = new List<Tuple<int, string, string>>();

                    //if (comparableData.Count() ==  lstTLRequests.Count())
                    //{
                    foreach (var item in comparableData)
                    {
                        foreach (var req in lstTLRequests)
                        {
                            if (item.GTIN != "")
                            {
                                if (item.GTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.GTIN, item.PackageTypeCode));
                                }
                            }
                            else
                            {
                                if (item.NTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.NTIN, item.PackageTypeCode));
                                }
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}

                    var UIDsToGenrate = job.Quantity + job.SurPlusQty;

                    //Dictionary<string, string> lsttluids = new Dictionary<string, string>();
                    List<Tuple<string, string, string>> lsttluids = new List<Tuple<string, string, string>>();

                    BulkDataHelper bulkHlpr = new BulkDataHelper();

                    tldHlpr.CalculateUIDsToGenerate(UIDsToGenrate, (int)job.PAID);


                    foreach (var item in actualData)
                    {
                        int qtyToGet = tldHlpr.getQtyToGenerate(item.Item3);
                        if (selectedJobType != "DSCSA")
                        {
                            if (item.Item3 != "MOC")
                            {
                                qtyToGet = qtyToGet + looseshiper;
                            }
                        }
                        if (qtyToGet > 0)
                        {
                            List<X_TracelinkUIDStore> tluids;
                            if (item.Item3 == lastLvl && item.Item3 != "MOC")
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false && x.Type == "SSCC").Take(qtyToGet).ToList();
                                tertiaryGTIN = item.Item2;
                            }
                            else
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false).Take(qtyToGet).ToList();
                            }

                            if (tluids.Count > 0)
                            {
                                foreach (var id in tluids)
                                {
                                    //lsttluids.Add(id.SerialNo, item.Item3);
                                    lsttluids.Add(new Tuple<string, string, string>(item.Item3, id.SerialNo, id.CryptoCode));

                                    bulkHlpr.setFlagToTransferdUID(id.Id);
                                }
                            }

                        }
                    }



                    if (lsttluids.Count > 0 || comparableData.Count() == 1)
                    {


                        foreach (var item in pacAssoDet)
                        {
                            M_Identities ids = new M_Identities();
                            ids.CreatedOn = DateTime.Now;
                            ids.CustomerId = (int)job.CustomerId;
                            ids.GTIN = item.GTIN;
                            ids.PPN = item.PPN;
                            ids.JID = job.JID;
                            ids.PackageTypeCode = item.PackageTypeCode;
                            ids.IsTransfered = false;
                            ids.NTIN = item.NTIN;
                            ids.IsExtra = false;
                            db.M_Identities.Add(ids);
                        }
                        db.SaveChanges();
                        var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
                        //   var gtin = db.M_Identities.Where(x => x.JID == job.JID && x.PackageTypeCode == lastLvl).FirstOrDefault();


                        //  tldHlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, (int)job.CustomerId, gtin.GTIN);
                        if (lastLvl != "MOC")
                        {
                            tldHlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                        }

                        if (lastLvl != "MOC")
                        {
                            if (selectedJobType == "DSCSA")
                            {
                                if (cust.ProviderId != 2)
                                {
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));
                                }
                                else
                                {
                                    int looseqty = getLSSCCQty(job.PAID);
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));

                                    tldHlpr.CalculateLoosSSCCForTLink(tertiaryGTIN, Convert.ToInt32(Utilities.getAppSettings("LoosShipper")));
                                }
                            }
                        }
                        var lstssccs = tldHlpr.getMasterData();
                        var tertLvl = tldHlpr.getTertiaryLvl();
                        //var data = lsttluids;
                        //var data = lsttluids.Union(lstssccs);
                        var data = lsttluids.Union(lstssccs.Select(s => new Tuple<string, string, string>(s.Value, s.Key, null)));

                        M_Identities masterGTIN = new M_Identities();
                        //data.Union(lsttluids);
                        if (data != null)
                        {
                            foreach (var item in comparableData)
                            {
                                if (item.GTIN != "")
                                {
                                    masterGTIN = db.M_Identities.Where(x => x.GTIN == item.GTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode).FirstOrDefault();
                                }
                                else
                                {
                                    masterGTIN = db.M_Identities.Where(x => x.NTIN == item.NTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode).FirstOrDefault();
                                }
                                var tempdata = data.Where(x => x.Item1 == item.PackageTypeCode);
                                var temp = data.Where(x => x.Item1 == item.PackageTypeCode + "Loos");
                                tempdata = tempdata.Union(temp);
                                var loos = data.Where(x => x.Item1 == item.PackageTypeCode + "LSSCC");
                                tempdata = tempdata.Union(loos);
                                if (item.PackageTypeCode == tertLvl)
                                {
                                    var ssccdata = data.Where(x => x.Item1 == "SSC");
                                    tempdata = tempdata.Union(ssccdata);
                                }
                                var ConvertedData = converUIDDataWithCrypto(tempdata, masterGTIN.Id, tertLvl);
                                masterList.AddRange(ConvertedData);
                            }

                            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);

                            #region For Russia other than MOC = SSCC
                            if (selectedJobType == "RUSSIA" && !Convert.ToBoolean(selectedCustomer.IsProvideCodeForMiddleDeck))
                            {
                                Dictionary<string, int> dicUidToPrint = new Dictionary<string, int>();
                                if (pacAssoDet.Count() > 2)
                                {
                                    var lstPack = pacAssoDet.OrderBy(s => s.Id).ToList();
                                    int parentCnt = UIDsToGenrate;

                                    for (int i = 0; i < lstPack.Count(); i++)
                                    {
                                        if (i == 0)
                                        {
                                            dicUidToPrint.Add(lstPack[i].PackageTypeCode, parentCnt);
                                        }
                                        else
                                        {
                                            double map = (parentCnt / lstPack[i].Size);
                                            parentCnt = (int)Math.Ceiling(map);
                                            dicUidToPrint.Add(lstPack[i].PackageTypeCode, parentCnt);
                                        }
                                    }
                                    string firstdeck = pacAssoDet.FirstOrDefault(s => s.Id == pacAssoDet.Min(x => x.Id)).PackageTypeCode;
                                    string lastdeck = pacAssoDet.FirstOrDefault(s => s.Id == pacAssoDet.Max(x => x.Id)).PackageTypeCode;
                                    foreach (var item in dicUidToPrint)
                                    {
                                        if (item.Key != firstdeck && item.Key != lastdeck)
                                        {
                                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                                            IDGenrationFactory obj = new IDGenrationFactory();

                                            //var lstPropixSerialNo = obj.generateIDs(item.Value, 13, selectedJobType);
                                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId), item.Value);

                                            var lstPropixSerialNo = ssccForMiddleDek.getMasterData();

                                            masterGTIN = db.M_Identities.FirstOrDefault(x => (x.JID == job.JID) && (x.PackageTypeCode == item.Key));

                                            foreach (var sscc in lstPropixSerialNo)
                                            {
                                                DtconvertedData.Rows.Add(new object[] { 0, masterGTIN.Id, sscc.Key, false, item.Key, false, string.Empty, null });
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region V1
        public bool GenerateCodeNonPropix(Job selectedJob, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN, bool isExtra)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var selectedProduct = db.PackagingAssoDetails.Where(x => x.PAID == selectedJob.PAID).OrderBy(s => s.Id).ToList();
                DataTable dt_X_Identities = new DataTable();
                dt_X_Identities.Columns.Add("MasterId", typeof(int)); dt_X_Identities.Columns.Add("SerialNo", typeof(string));
                dt_X_Identities.Columns.Add("CodeType", typeof(string)); dt_X_Identities.Columns.Add("IsTransfered", typeof(bool));
                dt_X_Identities.Columns.Add("PackTypeCode", typeof(string)); dt_X_Identities.Columns.Add("CryptoCode", typeof(string));
                dt_X_Identities.Columns.Add("PublicKey", typeof(string));

                var firstDek = selectedProduct.ToList().First();
                var lastDek = selectedProduct.ToList().Last();
                int UIDsToGenrate = 0;
                var selectedCustomer = db.M_Customer.Find(selectedJob.CustomerId);
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                if (isExtra == true)
                {
                    UIDsToGenrate = selectedJob.Quantity;
                }
                else
                {
                    UIDsToGenrate = selectedJob.Quantity + selectedJob.SurPlusQty;
                }
                Dictionary<string, int> dicUidToPrint = new Dictionary<string, int>();
                BulkDataHelper bulkHlpr = new BulkDataHelper();

                #region UID TO PRINT

                int parentCnt = UIDsToGenrate;
                for (int i = 0; i < selectedProduct.Count(); i++)
                {
                    if (i == 0)
                    {
                        dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                    }
                    else
                    {
                        int loosExtra = (parentCnt % selectedProduct[i].Size);
                        if (loosExtra > 0 && parentCnt < selectedProduct[i].Size)
                        {
                            loosExtra = 0;
                        }

                        float map = (parentCnt / selectedProduct[i].Size);
                        parentCnt = (map == 0) ? 1 : (int)Math.Ceiling(map);

                        parentCnt = parentCnt + ((loosExtra > 0) ? 1 : 0);

                        dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                    }
                }

                #endregion

                foreach (var dek in selectedProduct)
                {
                    List<X_TracelinkUIDStore> lstCodeFromStore = new List<X_TracelinkUIDStore>();
                    var dekUIDToPrint = dicUidToPrint.FirstOrDefault(s => s.Key == dek.PackageTypeCode);

                    string GTINVal = dek.GTIN, lastdeckGTIN = lastDek.GTIN;
                    bool isNTIN = false;
                    if (string.IsNullOrEmpty(dek.GTIN))
                    {
                        isNTIN = true;
                        GTINVal = dek.NTIN;

                        lastdeckGTIN = lastDek.NTIN;
                    }
                    M_Identities m_Identities = new M_Identities();
                    m_Identities.CreatedOn = DateTime.Now;
                    m_Identities.CustomerId = (int)selectedJob.CustomerId;
                    m_Identities.GTIN = dek.GTIN;
                    m_Identities.PPN = dek.PPN;
                    m_Identities.JID = selectedJob.JID;
                    m_Identities.PackageTypeCode = dek.PackageTypeCode;
                    m_Identities.IsTransfered = false;
                    m_Identities.NTIN = dek.NTIN;
                    m_Identities.IsExtra = isExtra;
                    db.M_Identities.Add(m_Identities);
                    db.SaveChanges();

                    if (dek.GTIN == firstDek.GTIN && dek.NTIN == firstDek.NTIN)
                    {
                        if (selectedJobType == "RUSSIA")
                        {
                            lstCodeFromStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTINVal && x.IsUsed == false && !string.IsNullOrEmpty(x.CryptoCode)).Take(dekUIDToPrint.Value).ToList();
                        }
                        else
                        {
                            lstCodeFromStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTINVal && x.IsUsed == false).Take(dekUIDToPrint.Value).ToList();
                        }
                    }
                    else if ((isNTIN == false && dek.GTIN != firstDek.GTIN && dek.GTIN != lastDek.GTIN) ||
                             (isNTIN == true && dek.NTIN != firstDek.NTIN && dek.NTIN != lastDek.NTIN))// MIDDLE DECK
                    {
                        if (Convert.ToBoolean(selectedCustomer.IsProvideCodeForMiddleDeck)) // CODE FROM STORE
                        {
                            lstCodeFromStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTINVal && x.IsUsed == false).Take(dekUIDToPrint.Value).ToList();
                        }
                        else // CODE FROM PROPIX
                        {
                            if (selectedJobType == "RUSSIA") // RUSSIA MIDDLE DECK == SSCC
                            {
                                TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();

                                //ADD COMPULSARY LOOSE FOR OTHER THAN MOC DECK MAIL RECEIVED BY SAGAR
                                int uidcount = dekUIDToPrint.Value + looseshiper;

                                ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, false, selectedCustomer.SSCCExt,
                                                                        Convert.ToInt32(selectedJob.CustomerId), uidcount);

                                var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                                foreach (var sscc in lstSSCCFromPropix)
                                {
                                    dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode, string.Empty, string.Empty });
                                }
                            }
                            else
                            {
                                string SerialNumberLength = Utilities.getAppSettings("SerialNumberLength");
                                int uidlen = 13;
                                int.TryParse(SerialNumberLength, out uidlen);

                                IDGenrationFactory codeForMiddleDeck = new IDGenrationFactory();
                                //ADD COMPULSARY LOOSE FOR OTHER THAN MOC DECK MAIL RECEIVED BY SAGAR
                                int uidcount = dekUIDToPrint.Value + looseshiper;

                                var lstSerialNoFromPropix = codeForMiddleDeck.generateIDs(uidcount, uidlen, selectedJobType);

                                foreach (var code in lstSerialNoFromPropix)
                                {
                                    dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, code, false, false, dek.PackageTypeCode, string.Empty, string.Empty });
                                }
                            }
                        }
                        if (selectedJobType == "DSCSA") // DSCSA MIDDLE DECK == SSCC
                        {
                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, true, selectedCustomer.LoosExt,
                                                                    Convert.ToInt32(selectedJob.CustomerId), looseshiper);

                            var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                            foreach (var sscc in lstSSCCFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode + "Loos", string.Empty, string.Empty });
                            }
                        }
                    }
                    else if (dek.GTIN == lastDek.GTIN && dek.NTIN == lastDek.NTIN) // LAST DECK
                    {
                        if (Convert.ToBoolean(selectedCustomer.IsSSCC)) // SSCC FROM STORE
                        {
                            lstCodeFromStore = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTINVal && x.IsUsed == false).Take(dekUIDToPrint.Value).ToList();
                        }
                        else // SSCC FROM PROPIX
                        {
                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, false, selectedCustomer.SSCCExt,
                                                                    Convert.ToInt32(selectedJob.CustomerId), dekUIDToPrint.Value);

                            var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                            foreach (var sscc in lstSSCCFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, "SSC", string.Empty, string.Empty });
                            }
                        }
                    }

                    DataTable dtIdsToUpdate = new DataTable();
                    dtIdsToUpdate.Columns.Add("Id", typeof(int));
                    dtIdsToUpdate.Clear();

                    for (int i = 0; i < lstCodeFromStore.Count(); i++)
                    {
                        dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, lstCodeFromStore[i].SerialNo, (lstCodeFromStore[i].GTIN== lastdeckGTIN),
                            false, ((dek.PackageTypeCode ==lastDek.PackageTypeCode)?"SSC":dek.PackageTypeCode),  lstCodeFromStore[i].CryptoCode, lstCodeFromStore[i].PublicKey});

                        dtIdsToUpdate.Rows.Add(new object[] { lstCodeFromStore[i].Id });

                        //bulkHlpr.setFlagToTransferdUID(lstCodeFromStore[i].Id);
                    }

                    #region Update IsUsed Status
                    if (dtIdsToUpdate.Rows.Count > 0)
                    {
                        Models.Crypto.CryptoGenerator crypto = new Models.Crypto.CryptoGenerator();
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("@IDs", dtIdsToUpdate);
                        List<Models.Crypto.ExecutionStatus> lstExecutionStatus = new List<Models.Crypto.ExecutionStatus>();

                        object affected_row = crypto.ServerBulkUpdate("sp_UPDATE_XTL_ISUSE_STATUS", dic, lstExecutionStatus);

                        if (Convert.ToInt32(affected_row) != dtIdsToUpdate.Rows.Count)
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region ADD LOOSE SSCC
                    if (dek.GTIN == lastDek.GTIN && dek.NTIN == lastDek.NTIN && dek.PackageTypeCode == "PAL" || (selectedProduct.Count() > 1 && dek.GTIN == lastDek.GTIN))
                    {
                        //LOOSE SSCC IF SSCC PROVIDED BY CUSTOMER
                        if (Convert.ToBoolean(selectedCustomer.IsSSCC))
                        {
                            var lstLooseSSCC = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTINVal && x.IsUsed == false).Take(looseshiper).ToList();

                            dtIdsToUpdate.Clear();
                            for (int i = 0; i < lstLooseSSCC.Count(); i++)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, lstLooseSSCC[i].SerialNo, (lstLooseSSCC[i].GTIN== lastdeckGTIN),
                                false, dek.PackageTypeCode+ "Loos",  lstLooseSSCC[i].CryptoCode,lstLooseSSCC[i].PublicKey});

                                dtIdsToUpdate.Rows.Add(new object[] { lstLooseSSCC[i].Id });
                                //bulkHlpr.setFlagToTransferdUID(lstLooseSSCC[i].Id);
                            }

                            #region Update IsUsed Status
                            if (dtIdsToUpdate.Rows.Count > 0)
                            {
                                Models.Crypto.CryptoGenerator crypto = new Models.Crypto.CryptoGenerator();
                                Dictionary<string, object> dic = new Dictionary<string, object>();
                                dic.Add("@IDs", dtIdsToUpdate);
                                List<Models.Crypto.ExecutionStatus> lstExecutionStatus = new List<Models.Crypto.ExecutionStatus>();

                                object affected_row = crypto.ServerBulkUpdate("sp_UPDATE_XTL_ISUSE_STATUS", dic, lstExecutionStatus);

                                if (Convert.ToInt32(affected_row) != dtIdsToUpdate.Rows.Count)
                                {
                                    return false;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            //LOOSE SSCC IF SSCC PROVIDED BY PROPIX
                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, true, selectedCustomer.LoosExt,
                                                                    Convert.ToInt32(selectedJob.CustomerId), looseshiper);

                            var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                            foreach (var sscc in lstSSCCFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode + "Loos", string.Empty, string.Empty });
                            }
                        }
                    }
                    #endregion
                }

                return bulkHlpr.InsertUIDIdenties(dt_X_Identities);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GenerateCodePropix(Job selectedJob, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN, bool isExtra)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var selectedProduct = db.PackagingAssoDetails.Where(x => x.PAID == selectedJob.PAID).OrderBy(s => s.Id).ToList();
                DataTable dt_X_Identities = new DataTable();
                dt_X_Identities.Columns.Add("MasterId", typeof(int)); dt_X_Identities.Columns.Add("SerialNo", typeof(string));
                dt_X_Identities.Columns.Add("CodeType", typeof(string)); dt_X_Identities.Columns.Add("IsTransfered", typeof(bool));
                dt_X_Identities.Columns.Add("PackTypeCode", typeof(string)); dt_X_Identities.Columns.Add("CryptoCode", typeof(string));
                dt_X_Identities.Columns.Add("PublicKey", typeof(string));

                var firstDek = selectedProduct.ToList().First();
                var lastDek = selectedProduct.ToList().Last();
                int UIDsToGenrate = 0;
                var selectedCustomer = db.M_Customer.Find(selectedJob.CustomerId);
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                if (isExtra == true)
                {
                    UIDsToGenrate = selectedJob.Quantity;
                }
                else
                {
                    UIDsToGenrate = selectedJob.Quantity + selectedJob.SurPlusQty;
                }
                Dictionary<string, int> dicUidToPrint = new Dictionary<string, int>();
                BulkDataHelper bulkHlpr = new BulkDataHelper();

                #region UID TO PRINT

                int parentCnt = UIDsToGenrate;
                for (int i = 0; i < selectedProduct.Count(); i++)
                {
                    if (i == 0)
                    {
                        dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                    }
                    else
                    {
                        int loosExtra = (parentCnt % selectedProduct[i].Size);
                        if (loosExtra > 0 && parentCnt < selectedProduct[i].Size)
                        {
                            loosExtra = 0;
                        }

                        float map = (parentCnt / selectedProduct[i].Size);
                        parentCnt = (map == 0) ? 1 : (int)Math.Ceiling(map);

                        parentCnt = parentCnt + ((loosExtra > 0) ? 1 : 0);

                        dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                    }
                }

                #endregion

                foreach (var dek in selectedProduct)
                {
                    var dekUIDToPrint = dicUidToPrint.FirstOrDefault(s => s.Key == dek.PackageTypeCode);

                    bool isNTIN = false;
                    string GTINVal = dek.GTIN, lastdeckGTIN = lastDek.GTIN;
                    if (string.IsNullOrEmpty(dek.GTIN))
                    {
                        isNTIN = true;
                        GTINVal = dek.NTIN;

                        lastdeckGTIN = lastDek.NTIN;
                    }

                    M_Identities m_Identities = new M_Identities();
                    m_Identities.CreatedOn = DateTime.Now;
                    m_Identities.CustomerId = (int)selectedJob.CustomerId;
                    m_Identities.GTIN = dek.GTIN;
                    m_Identities.PPN = dek.PPN;
                    m_Identities.JID = selectedJob.JID;
                    m_Identities.PackageTypeCode = dek.PackageTypeCode;
                    m_Identities.IsTransfered = false;
                    m_Identities.NTIN = dek.NTIN;
                    m_Identities.IsExtra = isExtra;
                    db.M_Identities.Add(m_Identities);
                    db.SaveChanges();

                    if (dek.GTIN == firstDek.GTIN && dek.NTIN == firstDek.NTIN && dek.PackageTypeCode != "PAL")
                    {
                        string SerialNumberLength = Utilities.getAppSettings("SerialNumberLength");
                        int uidlen = 13;
                        int.TryParse(SerialNumberLength, out uidlen);

                        IDGenrationFactory codeForFirstDeck = new IDGenrationFactory();
                        var lstSerialNoFromPropix = codeForFirstDeck.generateIDs(dekUIDToPrint.Value, uidlen, selectedJobType);

                        string CryptoCode = string.Empty;
                        string publicKey = string.Empty;

                        for (int i = 0; i < lstSerialNoFromPropix.Count; i++)
                        {
                            if (selectedJobType == "RUSSIA")
                            {
                                CryptoCode = "propixCR" + Guid.NewGuid().ToString().Replace("-", "/");
                                publicKey = "PROP1";
                            }

                            dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, lstSerialNoFromPropix[i], false, false, dek.PackageTypeCode, CryptoCode, publicKey });
                        }
                    }
                    //else if (dek.GTIN != firstDek.GTIN && dek.GTIN != lastDek.GTIN)// MIDDLE DECK
                    else if ((isNTIN == false && dek.GTIN != firstDek.GTIN && dek.GTIN != lastDek.GTIN) ||
                             (isNTIN == true && dek.NTIN != firstDek.NTIN && dek.NTIN != lastDek.NTIN))// MIDDLE DECK

                    {
                        if (selectedJobType == "RUSSIA") // RUSSIA MIDDLE DECK == SSCC
                        {
                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, false, selectedCustomer.SSCCExt,
                                                                    Convert.ToInt32(selectedJob.CustomerId), dekUIDToPrint.Value);

                            var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                            foreach (var sscc in lstSSCCFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode, string.Empty, string.Empty });
                            }
                        }
                        else
                        {
                            string SerialNumberLength = Utilities.getAppSettings("SerialNumberLength");
                            int uidlen = 13;
                            int.TryParse(SerialNumberLength, out uidlen);

                            IDGenrationFactory codeForMiddleDeck = new IDGenrationFactory();

                            //ADD COMPULSARY LOOSE FOR OTHER THAN MOC DECK MAIL RECEIVED BY SAGAR
                            int uidcount = dekUIDToPrint.Value + looseshiper;

                            var lstSerialNoFromPropix = codeForMiddleDeck.generateIDs(uidcount, uidlen, selectedJobType);

                            foreach (var code in lstSerialNoFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, code, false, false, dek.PackageTypeCode, string.Empty, string.Empty });
                            }
                        }

                        if (selectedJobType == "DSCSA") // DSCSA MIDDLE DECK == SSCC
                        {
                            TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                            ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, true, selectedCustomer.LoosExt,
                                                                    Convert.ToInt32(selectedJob.CustomerId), looseshiper);

                            var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                            foreach (var sscc in lstSSCCFromPropix)
                            {
                                dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode + "Loos", string.Empty, string.Empty });
                            }
                        }
                    }
                    //else if (dek.GTIN == lastDek.GTIN) // LAST DECK
                    else if (dek.GTIN == lastDek.GTIN && dek.NTIN == lastDek.NTIN) // LAST DECK
                    {
                        TracelinkUIDDataHelper ssccForMiddleDek = new TracelinkUIDDataHelper();
                        ssccForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, false, selectedCustomer.SSCCExt,
                                                                Convert.ToInt32(selectedJob.CustomerId), dekUIDToPrint.Value);

                        var lstSSCCFromPropix = ssccForMiddleDek.getMasterData();

                        foreach (var sscc in lstSSCCFromPropix)
                        {
                            dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, "SSC", string.Empty, string.Empty });
                        }

                        #region ADD LAST DECK LOOSE SSCC

                        //LOOSE SSCC IF SSCC PROVIDED BY PROPIX
                        TracelinkUIDDataHelper loosSSCCForMiddleDek = new TracelinkUIDDataHelper();
                        loosSSCCForMiddleDek.GenerateMiddleDekSSCCs((int)selectedJob.JID, selectedJobType, IAC_CIN, true, selectedCustomer.LoosExt,
                                                                Convert.ToInt32(selectedJob.CustomerId), looseshiper);

                        var lstLoosSSCCFromPropix = loosSSCCForMiddleDek.getMasterData();

                        foreach (var sscc in lstLoosSSCCFromPropix)
                        {
                            dt_X_Identities.Rows.Add(new object[] { m_Identities.Id, sscc.Key, false, false, dek.PackageTypeCode + "Loos", string.Empty, string.Empty });
                        }
                        #endregion
                    }
                }

                return bulkHlpr.InsertUIDIdenties(dt_X_Identities);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        public int getLSSCCQty(decimal PAID)
        {
            int LSSCCQty = 0;
            var pkgAssoDtls = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).ToList();
            if (pkgAssoDtls.Count == 3 || pkgAssoDtls.Count == 2)
            {
                LSSCCQty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            }
            else if (pkgAssoDtls.Count == 4 || pkgAssoDtls.Count == 5)
            {
                LSSCCQty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper")) * 2;
            }
            else if (pkgAssoDtls.Count == 1 && pkgAssoDtls[0].PackageTypeCode != "MOC")
            {
                LSSCCQty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            }



            return LSSCCQty;
        }
        public bool GenerateTracelinkDataAdditionalBatchQty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            try
            {
                string type = job.CompType;
                string tertiaryGTIN = "";
                int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id);
                pacAssoDet = pacAssoDet.Distinct().OrderBy(x => x.Id);

                List<X_Identities> masterList = new List<X_Identities>();
                var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN, x.NTIN });

                List<M_TracelinkRequest> lstTLRequests = new List<M_TracelinkRequest>();
                M_TracelinkRequest TLdata = new M_TracelinkRequest();
                foreach (var item in comparableData)
                {

                    if (item.GTIN != "")
                    {
                        TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();
                        type = "GTIN";
                    }
                    else
                    {
                        TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == item.NTIN).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();

                    }
                    if (TLdata != null)                                                                                                                            // lstTLRequests.AddRange(TLdata);
                        lstTLRequests.Add(TLdata);
                }
                if (lstTLRequests.Count() > 0)
                {
                    //Request  Id, GTIN, PackTypeCode
                    List<Tuple<int, string, string>> actualData = new List<Tuple<int, string, string>>();

                    //if (comparableData.Count() ==  lstTLRequests.Count())
                    //{
                    foreach (var item in comparableData)
                    {
                        foreach (var req in lstTLRequests)
                        {
                            if (item.GTIN != "")
                            {
                                if (item.GTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.GTIN, item.PackageTypeCode));
                                }
                            }
                            else
                            {
                                if (item.NTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.NTIN, item.PackageTypeCode));
                                }
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}


                    var UIDsToGenrate = job.Quantity + job.SurPlusQty;

                    Dictionary<string, string> lsttluids = new Dictionary<string, string>();
                    TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
                    BulkDataHelper bulkHlpr = new BulkDataHelper();
                    tldHlpr.CalculateUIDsToGenerate(UIDsToGenrate, (int)job.PAID);


                    foreach (var item in actualData)
                    {
                        int qtyToGet = tldHlpr.getQtyToGenerate(item.Item3);
                        if (selectedJobType != "DSCSA")
                        {
                            if (item.Item3 != "MOC")
                            {
                                qtyToGet = qtyToGet + looseshiper;
                            }
                        }
                        if (qtyToGet > 0)
                        {
                            List<X_TracelinkUIDStore> tluids;
                            if (item.Item3 == lastLvl && item.Item3 != "MOC")
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false && x.Type == "SSCC").Take(qtyToGet).ToList();
                                tertiaryGTIN = item.Item2;
                            }
                            else
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false).Take(qtyToGet).ToList();
                            }
                            if (tluids.Count > 0)
                            {
                                foreach (var id in tluids)
                                {
                                    lsttluids.Add(id.SerialNo, item.Item3);

                                    bulkHlpr.setFlagToTransferdUID(id.Id);
                                }
                            }
                        }
                    }



                    if (lsttluids.Count > 0)
                    {


                        foreach (var item in pacAssoDet)
                        {
                            M_Identities ids = new M_Identities();
                            ids.CreatedOn = DateTime.Now;
                            ids.CustomerId = (int)job.CustomerId;
                            ids.GTIN = item.GTIN;
                            ids.PPN = item.PPN;
                            ids.JID = job.JID;
                            ids.PackageTypeCode = item.PackageTypeCode;
                            ids.IsTransfered = false;
                            ids.IsExtra = true;
                            db.M_Identities.Add(ids);
                        }
                        db.SaveChanges();
                        var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
                        //   var gtin = db.M_Identities.Where(x => x.JID == job.JID && x.PackageTypeCode == lastLvl).FirstOrDefault();


                        //  tldHlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, (int)job.CustomerId, gtin.GTIN);
                        if (lastLvl != "MOC")
                        {
                            tldHlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                        }

                        if (lastLvl != "MOC")
                        {
                            if (selectedJobType == "DSCSA")
                            {
                                if (cust.ProviderId != 2)
                                {
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));
                                }
                                else
                                {
                                    int looseqty = getLSSCCQty(job.PAID);
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));

                                    tldHlpr.CalculateLoosSSCCForTLink(tertiaryGTIN, Convert.ToInt32(Utilities.getAppSettings("LoosShipper")));
                                }
                            }
                        }

                        var lstssccs = tldHlpr.getMasterData();
                        var tertLvl = tldHlpr.getTertiaryLvl();
                        //var data = lsttluids;
                        var data = lsttluids.Union(lstssccs);
                        M_Identities masterGTIN = new M_Identities();
                        //data.Union(lsttluids);
                        foreach (var item in comparableData)
                        {
                            if (item.GTIN != "")
                            {
                                masterGTIN = db.M_Identities.Where(x => x.GTIN == item.GTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode).FirstOrDefault();
                            }
                            else
                            {
                                masterGTIN = db.M_Identities.Where(x => x.NTIN == item.NTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode).FirstOrDefault();
                            }
                            var tempdata = data.Where(x => x.Value == item.PackageTypeCode);
                            var temp = data.Where(x => x.Value == item.PackageTypeCode + "Loos");
                            tempdata = tempdata.Union(temp);
                            if (item.PackageTypeCode == tertLvl)
                            {
                                var ssccdata = data.Where(x => x.Value == "SSC");
                                tempdata = tempdata.Union(ssccdata);
                            }
                            var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                            masterList.AddRange(ConvertedData);
                        }


                        DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);
                        return bulkHlpr.InsertUIDIdenties(DtconvertedData);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool GenerateTraceKeyCodeAdditionalBatchQty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            try
            {
                string type = job.CompType;
               // int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id);
                pacAssoDet = pacAssoDet.Distinct().OrderBy(x => x.Id);
                string tertiaryGTIN = "";
                List<X_Identities> masterList = new List<X_Identities>();
                var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN, x.NTIN });
                TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
                List<M_TracelinkRequest> lstTLRequests = new List<M_TracelinkRequest>();
                M_TracelinkRequest TLdata = new M_TracelinkRequest();
                #region "RUSSIA CODE GET FROM X_TracelinkUIDStore"

                #endregion

                foreach (var item in comparableData)
                {
                    TLdata = db.M_TracelinkRequest.Where(x => x.GTIN == (string.IsNullOrEmpty(item.GTIN) ? item.NTIN : item.GTIN)).OrderByDescending(x => x.RequestedOn).FirstOrDefault(); //db.M_TracelinkRequest.Where(x => x.GTIN == item.GTIN).ToList();

                    if (TLdata != null) lstTLRequests.Add(TLdata);
                }

                if (lstTLRequests.Count() > 0 || comparableData.Count() == 1)
                {
                    //Request  Id, GTIN, PackTypeCode
                    List<Tuple<int, string, string>> actualData = new List<Tuple<int, string, string>>();


                    foreach (var item in comparableData)
                    {
                        foreach (var req in lstTLRequests)
                        {
                            if (item.GTIN != "")
                            {
                                if (item.GTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.GTIN, item.PackageTypeCode));
                                }
                            }
                            else
                            {
                                if (item.NTIN == req.GTIN)
                                {
                                    actualData.Add(new Tuple<int, string, string>(req.Id, item.NTIN, item.PackageTypeCode));
                                }
                            }
                        }
                    }



                    var UIDsToGenrate = job.Quantity /*+ job.SurPlusQty*/;

                    //Dictionary<string, string> lsttluids = new Dictionary<string, string>();
                    List<Tuple<string, string, string>> lsttluids = new List<Tuple<string, string, string>>();
                    DataTable dt_X_Identities = new DataTable();
                    dt_X_Identities.Columns.Add("MasterId", typeof(int)); dt_X_Identities.Columns.Add("SerialNo", typeof(string));
                    dt_X_Identities.Columns.Add("CodeType", typeof(string)); dt_X_Identities.Columns.Add("IsTransfered", typeof(bool));
                    dt_X_Identities.Columns.Add("PackTypeCode", typeof(string)); dt_X_Identities.Columns.Add("CryptoCode", typeof(string));

                    BulkDataHelper bulkHlpr = new BulkDataHelper();

                    tldHlpr.CalculateUIDsToGenerate(UIDsToGenrate, (int)job.PAID);
                    foreach (var item in actualData)
                    {
                        int qtyToGet = tldHlpr.getQtyToGenerate(item.Item3);
                        if (selectedJobType != "DSCSA")
                        {
                            if (item.Item3 != "MOC")
                            {
                                qtyToGet = qtyToGet;// + looseshiper;
                            }
                        }
                        if (qtyToGet > 0)
                        {
                            List<X_TracelinkUIDStore> tluids;
                            if (item.Item3 == lastLvl && item.Item3 != "MOC")
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false && x.Type == "SSCC").Take(qtyToGet).ToList();
                                tertiaryGTIN = item.Item2;
                            }
                            else
                            {
                                tluids = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.Item2 && x.IsUsed == false).Take(qtyToGet).ToList();
                            }

                            if (tluids.Count > 0)
                            {
                                foreach (var id in tluids)
                                {
                                    lsttluids.Add(new Tuple<string, string, string>(item.Item3, id.SerialNo, id.CryptoCode));
                                    bulkHlpr.setFlagToTransferdUID(id.Id);
                                }
                            }
                        }
                    }

                    if (lsttluids.Count > 0 || comparableData.Count() == 1)
                    {
                        foreach (var item in pacAssoDet)
                        {
                            M_Identities ids = new M_Identities();
                            ids.CreatedOn = DateTime.Now;
                            ids.CustomerId = (int)job.CustomerId;
                            ids.GTIN = item.GTIN;
                            ids.PPN = item.PPN;
                            ids.JID = job.JID;
                            ids.PackageTypeCode = item.PackageTypeCode;
                            ids.IsTransfered = false;
                            ids.NTIN = item.NTIN;
                            ids.IsExtra = true;
                            db.M_Identities.Add(ids);
                        }
                        db.SaveChanges();
                        var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();

                        if (!cust.IsSSCC && lastLvl != "MOC")
                        {
                            tldHlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                        }

                        if (lastLvl != "MOC")
                        {
                            if (selectedJobType == "DSCSA")
                            {
                                if (cust.ProviderId != 2)
                                {
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));
                                }
                                else
                                {
                                    int looseqty = getLSSCCQty(job.PAID);
                                    tldHlpr.CalculateLoosSSCC(firstLvl, lastLvl, Convert.ToInt32(job.JID), cust.LoosExt, Convert.ToInt32(cust.Id));

                                    tldHlpr.CalculateLoosSSCCForTLink(tertiaryGTIN, Convert.ToInt32(Utilities.getAppSettings("LoosShipper")));
                                }
                            }
                        }
                        var lstssccs = tldHlpr.getMasterData();
                        var tertLvl = tldHlpr.getTertiaryLvl();

                        var data = lsttluids.Union(lstssccs.Select(s => new Tuple<string, string, string>(s.Value, s.Key, string.Empty)));

                        M_Identities masterGTIN = new M_Identities();

                        if (data != null)
                        {
                            foreach (var item in comparableData)
                            {
                                masterGTIN = db.M_Identities.OrderByDescending(x => x.Id).FirstOrDefault(x => (x.JID == job.JID) && (x.PackageTypeCode == item.PackageTypeCode) &&
                                               (!string.IsNullOrEmpty(item.GTIN) && x.GTIN == item.GTIN) || (!string.IsNullOrEmpty(item.NTIN) && x.NTIN == item.NTIN));

                                var tempdata = data.Where(x => x.Item1 == item.PackageTypeCode ||
                                                               x.Item1 == item.PackageTypeCode + "Loos" ||
                                                               x.Item1 == item.PackageTypeCode + "LSSCC").ToList();

                                if (item.PackageTypeCode == tertLvl)
                                {
                                    var ssccdata = data.Where(x => x.Item1 == "SSC");
                                    tempdata = tempdata.Union(ssccdata).ToList();
                                }

                                for (int i = 0; i < tempdata.Count(); i++)
                                {
                                    dt_X_Identities.Rows.Add(new object[] { masterGTIN.Id, tempdata[i].Item2, (tempdata[i].Item1 == "SSC"), false, tempdata[i].Item1, tempdata[i].Item3 });
                                }

                            }


                            return bulkHlpr.InsertUIDIdenties(dt_X_Identities);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool GenerateData(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            string numberFrom = string.Empty;
            string numberTo = string.Empty;
            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id);
            pacAssoDet = pacAssoDet.Distinct().OrderBy(x => x.Id);
            List<string> lvlforCode = new List<string>();
            TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
            List<X_Identities> masterList = new List<X_Identities>();
            var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN, x.NTIN });
            var UIDsToGenrate = job.Quantity + job.SurPlusQty;


            foreach (var item in pacAssoDet)
            {
                M_Identities ids = new M_Identities();
                ids.CreatedOn = DateTime.Now;
                ids.CustomerId = (int)job.CustomerId;
                ids.GTIN = item.GTIN;
                ids.PPN = item.PPN;
                ids.JID = job.JID;
                ids.PackageTypeCode = item.PackageTypeCode;
                ids.IsTransfered = false;
                ids.IsExtra = false;
                ids.NTIN = item.NTIN;
                db.M_Identities.Add(ids);
            }
            db.SaveChanges();

            UIDDataHlper hlpr = new UIDDataHlper();
            hlpr.setProductData((int)job.PAID);
            if (comparableData.Count() != 1 || lastLvl == "MOC" || selectedJobType != "DSCSA")
            {
                hlpr.CalculateAndGenerateIds(UIDsToGenrate, selectedJobType, firstLvl, lastLvl);
            }
            if (selectedJobType == "SOUTH_KOREA")
            {
                string name = "GTIN";
                var pkdata = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).FirstOrDefault();
                var odata = db.SK_ObjectKey.Where(x => x.Name == name && x.Value == pkdata.GTIN && x.IsUsed == false).FirstOrDefault();
                var mdata = db.M_SKMaster.Where(x => x.MID == odata.MID && x.IsUsed == false).FirstOrDefault();
                numberFrom = mdata.NumberFrom;
                numberTo = mdata.NumberTo;
                string[] nfrom = mdata.NumberFrom.Split(')');
                string[] nto = mdata.NumberTo.Split(')');
                hlpr.CalculateGenerateUIDForSKorea(job.Quantity, nfrom[2], firstLvl, lastLvl, Convert.ToInt32(job.JID));
            }
            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            if (selectedJobType == "DSCSA" && lastLvl != "MOC")
            {
                var lvl = pacAssoDet.Select(x => x.PackageTypeCode).ToList();
                List<string> availablelevels = new List<string>();
                availablelevels = lvlforSSCC(lvl);
                hlpr.CalculateLoosSSCC(availablelevels, firstLvl, lastLvl, Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                availablelevels.Clear();

                if (comparableData.Count() != 1 || lastLvl == "MOC")
                {
                    availablelevels = lvlforLoosUID(lvl);
                    hlpr.CalculateLoosCode(availablelevels, selectedJobType);
                }

            }


            if (lastLvl != "MOC")
            {
                if (comparableData.Count() == 1)
                {
                    hlpr.CalculateUIDsToGenerate(UIDsToGenrate, Convert.ToInt32(job.PAID));
                }

                hlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
            }

            var tertLvl = hlpr.getTertiaryLvl();
            var data = hlpr.getMasterData();

            foreach (var item in comparableData)
            {
                M_Identities masterGTIN = new M_Identities();
                if (item.NTIN != "")
                {
                    masterGTIN = db.M_Identities.Where(x => x.NTIN == item.NTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode && x.IsTransfered == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                else
                {
                    masterGTIN = db.M_Identities.Where(x => x.GTIN == item.GTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode && x.IsTransfered == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                var tempdata = data.Where(x => x.Value == item.PackageTypeCode);
                var temp = data.Where(x => x.Value == item.PackageTypeCode + "Loos");
                tempdata = tempdata.Union(temp);
                if (item.PackageTypeCode == tertLvl)
                {
                    var ssccdata = data.Where(x => x.Value == "SSC");
                    tempdata = tempdata.Union(ssccdata);
                }
                if (selectedJobType == "RUSSIA")
                {
                    var ConvertedData = converUIDDataCryptoCode(tempdata, masterGTIN.Id, tertLvl);
                    masterList.AddRange(ConvertedData);
                }
                else
                {
                    var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                    masterList.AddRange(ConvertedData);
                }
            }


            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);
            if (selectedJobType == "SOUTH_KOREA")
            {
                var M_Skdata = db.M_SKMaster.Where(x => x.NumberFrom == numberFrom && x.NumberTo == numberTo).FirstOrDefault();
                M_SKMaster m = new M_SKMaster();
                m = db.M_SKMaster.Where(x => x.MID == M_Skdata.MID).FirstOrDefault();
                m.IsUsed = true;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();

                List<SK_ObjectKey> lst = new List<SK_ObjectKey>();
                lst = db.SK_ObjectKey.Where(x => x.MID == M_Skdata.MID).ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    SK_ObjectKey s = new SK_ObjectKey();
                    int id = lst[i].OID;
                    s = db.SK_ObjectKey.Where(x => x.OID == id).FirstOrDefault();
                    s.IsUsed = true;
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            BulkDataHelper bulkHlpr = new BulkDataHelper();
            return bulkHlpr.InsertUIDIdenties(DtconvertedData);


        }

        public bool GenerateDataAdditionalBatchQty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            string numberFrom = string.Empty;
            string numberTo = string.Empty;
            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id);
            pacAssoDet = pacAssoDet.Distinct().OrderBy(x => x.Id);
            List<string> lvlforCode = new List<string>();

            List<X_Identities> masterList = new List<X_Identities>();
            var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN });
            var UIDsToGenrate = job.Quantity + job.SurPlusQty;


            foreach (var item in pacAssoDet)
            {
                M_Identities ids = new M_Identities();
                ids.CreatedOn = DateTime.Now;
                ids.CustomerId = (int)job.CustomerId;
                ids.GTIN = item.GTIN;
                ids.PPN = item.PPN;
                ids.JID = job.JID;
                ids.PackageTypeCode = item.PackageTypeCode;
                ids.IsTransfered = false;
                ids.IsExtra = true;
                db.M_Identities.Add(ids);
            }
            db.SaveChanges();

            UIDDataHlper hlpr = new UIDDataHlper();
            hlpr.setProductData((int)job.PAID);
            //if (selectedJobType != "South Korea")
            {
                hlpr.CalculateAndGenerateIds(UIDsToGenrate, selectedJobType, firstLvl, lastLvl, "AdditionalBatch");
            }
            if (selectedJobType == "SOUTH_KOREA")
            {
                string name = "GTIN";
                var pkdata = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).FirstOrDefault();
                var odata = db.SK_ObjectKey.Where(x => x.Name == name && x.Value == pkdata.GTIN && x.IsUsed == false).FirstOrDefault();
                var mdata = db.M_SKMaster.Where(x => x.MID == odata.MID && x.IsUsed == false).FirstOrDefault();
                numberFrom = mdata.NumberFrom;
                numberTo = mdata.NumberTo;
                string[] nfrom = mdata.NumberFrom.Split(')');
                string[] nto = mdata.NumberTo.Split(')');
                hlpr.CalculateGenerateUIDForSKorea(job.Quantity, nfrom[2], firstLvl, lastLvl, Convert.ToInt32(job.JID));
            }
            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            if (selectedJobType == "DSCSA")
            {
                var lvl = pacAssoDet.Select(x => x.PackageTypeCode).ToList();
                List<string> availablelevels = new List<string>();
                availablelevels = lvlforSSCC(lvl);
                //if (lvl.Count > 1)
                //{ 
                //availablelevels = lvlforSSCC(lvl);
                //    hlpr.CalculateLoosSSCC(availablelevels, firstLvl, lastLvl, Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                //    availablelevels.Clear();

                //    availablelevels = lvlforLoosUID(lvl);
                //    hlpr.CalculateLoosCode(availablelevels, selectedJobType);
                //    hlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));


                //}
                //else
                //{
                //   availablelevels= lvl;
                //}


                hlpr.CalculateLoosSSCC(availablelevels, firstLvl, lastLvl, Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                availablelevels.Clear();

                availablelevels = lvlforLoosUID(lvl);
                hlpr.CalculateLoosCode(availablelevels, selectedJobType);

            }

            //if (selectedJobType != "South Korea")
            //if (selectedJobType != "DSCSA")
            //{
            //    hlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
            //}
            if (firstLvl != lastLvl && lastLvl != "MOC")
            {
                hlpr.GenerateSSCCs((int)job.JID, selectedJobType, IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
            }
            
            var tertLvl = hlpr.getTertiaryLvl();
            var data = hlpr.getMasterData();

            foreach (var item in comparableData)
            {
                var masterGTIN = db.M_Identities.Where(x => x.GTIN == item.GTIN && x.JID == job.JID && x.PackageTypeCode == item.PackageTypeCode && x.IsTransfered == false && x.IsExtra == true).FirstOrDefault();
                var tempdata = data.Where(x => x.Value == item.PackageTypeCode);
                var temp = data.Where(x => x.Value == item.PackageTypeCode + "Loos");
                tempdata = tempdata.Union(temp);
                if (item.PackageTypeCode == tertLvl)
                {
                    var ssccdata = data.Where(x => x.Value == "SSC");
                    tempdata = tempdata.Union(ssccdata);
                }
                //if (selectedJobType == "RUSSIA")
                //{
                //   var ConvertedData = converUIDDataCryptoCode(tempdata, masterGTIN.Id, tertLvl);
                //    masterList.AddRange(ConvertedData);
                //}
                //else
                //{
                //    var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                //    masterList.AddRange(ConvertedData);
                //}
                var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                masterList.AddRange(ConvertedData);
            }


            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);
            if (selectedJobType == "SOUTH_KOREA")
            {
                var M_Skdata = db.M_SKMaster.Where(x => x.NumberFrom == numberFrom && x.NumberTo == numberTo).FirstOrDefault();
                M_SKMaster m = new M_SKMaster();
                m = db.M_SKMaster.Where(x => x.MID == M_Skdata.MID).FirstOrDefault();
                m.IsUsed = true;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();

                List<SK_ObjectKey> lst = new List<SK_ObjectKey>();
                lst = db.SK_ObjectKey.Where(x => x.MID == M_Skdata.MID).ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    SK_ObjectKey s = new SK_ObjectKey();
                    int id = lst[i].OID;
                    s = db.SK_ObjectKey.Where(x => x.OID == id).FirstOrDefault();
                    s.IsUsed = true;
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }


            BulkDataHelper bulkHlpr = new BulkDataHelper();
            return bulkHlpr.InsertUIDIdenties(DtconvertedData);


        }

        private static List<string> lvlforSSCC(List<string> lvl)
        {
            List<string> lvlSorted = sorttheLevels(lvl);
            int cnt = lvl.Count();
            List<string> lvlSSCC = new List<string>();
            switch (cnt)
            {
                case 1:
                    if (cnt == 1 && lvlSorted[0] != "MOC")         //if (cnt == 1 || lvlSorted[0] != "MOC")  &&
                    {
                        lvlSSCC.Add(lvlSorted[0]);
                        return lvlSSCC;
                    }
                    else
                    {
                        return null;
                    }
                case 2:
                    for (int i = 1; i < 2; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;
                case 3:
                    for (int i = 1; i < 3; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 4:
                    for (int i = 2; i < 4; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 5:
                    for (int i = 3; i < 5; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 6:
                    for (int i = 4; i <= 6; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                default:
                    return null;
            }
        }

        private static List<string> lvlforLoosUID(List<string> lvl)
        {
            List<string> lvlSorted = sorttheLevels(lvl);
            int cnt = lvl.Count();
            List<string> lvlSSCC = new List<string>();
            switch (cnt)
            {
                case 1:
                    if (cnt == 1 && lvlSorted[0] != "MOC")  // if (cnt == 1 || lvlSorted[0] != "MOC")&&
                    {
                        lvlSSCC.Add(lvlSorted[0]);
                        return lvlSSCC;
                    }
                    else
                    {
                        return null;
                    }
                case 2:
                    for (int i = 0; i < 1; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;
                case 3:
                    for (int i = 0; i < 1; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 4:
                    for (int i = 0; i < 2; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 5:
                    for (int i = 0; i < 3; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                case 6:
                    for (int i = 0; i < 4; i++)
                    {
                        lvlSSCC.Add(lvlSorted[i]);
                    }
                    return lvlSSCC;

                default:
                    return null;
            }
        }
        private static List<string> sorttheLevels(List<string> levels)
        {
            try
            {
                // string code;
                List<string> output = new List<string>(6);

                for (int i = 0; i < 6; i++)
                {
                    output.Add("");
                    //= "";
                }

                foreach (string code in levels)
                {
                    if (code == "PPB")
                    {
                        output[0] = code;
                    }

                    if (code == "MOC")
                    {
                        output[1] = code;
                    }

                    if (code == "OBX")
                    {
                        output[2] = code;
                    }
                    if (code == "ISH")
                    {
                        output[3] = code;
                    }
                    if (code == "OSH")
                    {
                        output[4] = code;
                    }
                    if (code == "PAL")
                    {
                        output[5] = code;
                    }
                }





                output.RemoveAll(x => x.Equals(""));
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool GenerateExtraUIDData(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose)
        {

            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID && x.PackageTypeCode == PackagingLevel);
            var firstPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).FirstOrDefault();
            var lastPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderByDescending(p => p.Id).FirstOrDefault().PackageTypeCode;

            pacAssoDet = pacAssoDet.Distinct();

            List<X_Identities> masterList = new List<X_Identities>();
            var comparableData = pacAssoDet.Select(x => new { x.GTIN, x.PackageTypeCode, x.PPN });
            var UIDsToGenrate = Quantity;
            var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();



            foreach (var item in pacAssoDet)
            {
                M_Identities ids = new M_Identities();
                ids.CreatedOn = DateTime.Now;
                ids.CustomerId = (int)job.CustomerId;
                ids.GTIN = item.GTIN;
                ids.PPN = item.PPN;
                ids.JID = job.JID;
                ids.PackageTypeCode = item.PackageTypeCode;
                ids.IsTransfered = false;
                ids.IsExtra = true;
                db.M_Identities.Add(ids);
            }
            db.SaveChanges();

            UIDDataHlper hlpr = new UIDDataHlper();
            hlpr.setProductData((int)job.PAID);
            var firstLvl = firstPacRec.PackageTypeCode;
            var tertLvl = lastPacRec;
            var IAC_CIN = db.Settings.FirstOrDefault();

            #region Commented
            if (IsLoose == true)
            {
                if (PackagingLevel != "MOC")
                {
                    if (selectedJobType.Job_Type != "DSCSA")
                    {
                        int loosQuantity = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                        UIDsToGenrate = UIDsToGenrate + loosQuantity;
                        hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);
                    }
                    else
                    {
                        hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);
                    }
                }
                else
                {
                    hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);
                }
            }
            else
            {
                hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);
            }
            // hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);


            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            if (IsTertiary == true)
            {
                hlpr.GenerateExtraSSCCs((int)job.JID, Quantity, IsLoose, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                if (IsLoose == true)
                {
                    if (selectedJobType.Job_Type == "DSCSA")
                    {
                        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                    }
                }
            }
            else
            {
                if (selectedJobType.Job_Type == "DSCSA")
                {
                    if (IsLoose == true)
                    {
                        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                    }
                }
            }

            #endregion

            #region Mustafa Implementation
            //var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            //if (IsTertiary == true)
            //{
            //    hlpr.GenerateExtraSSCCs((int)job.JID, Quantity, IsLoose, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));

            //    if (IsLoose == true)
            //    {
            //        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
            //    }
            //}
            //else
            //{
            //    if (selectedJobType.Job_Type != "DSCSA" && selectedJobType.Job_Type != "RUSSAI")
            //    {
            //        hlpr.CalculateAndGenerateExtraIds(Convert.ToInt32(job.JID), UIDsToGenrate, selectedJobType.Job_Type, PackagingLevel, tertLvl);

            //    }
            //    else
            //    {

            //    }
            //}
            #endregion


            var data = hlpr.getMasterData();


            foreach (var item in comparableData)
            {
                var masterGTIN = db.M_Identities.Where(x => x.GTIN == item.GTIN && x.JID == job.JID && x.IsExtra == true && x.PackageTypeCode == item.PackageTypeCode).OrderByDescending(p => p.CreatedOn).FirstOrDefault();
                if (IsTertiary == true)
                {
                    if (IsLoose == false)
                    {

                        var tempdata = data.Where(x => x.Value == item.PackageTypeCode);
                        if (item.PackageTypeCode == tertLvl)
                        {
                            var ssccdata = data.Where(x => x.Value == "SSC");
                            tempdata = tempdata.Union(ssccdata);
                        }

                        var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                        masterList.AddRange(ConvertedData);
                    }
                    else
                    {
                        var ConvertedData = converUIDData(data, masterGTIN.Id, tertLvl);
                        masterList.AddRange(ConvertedData);
                    }
                }
                else
                {
                    var ConvertedData = converUIDData(data, masterGTIN.Id, tertLvl);
                    masterList.AddRange(ConvertedData);
                }
            }


            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);


            BulkDataHelper bulkHlpr = new BulkDataHelper();
            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
        }

        public bool GenerateTLExtraUIDData(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {

            List<X_Identities> masterList = new List<X_Identities>();
            int loosqty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID && x.PackageTypeCode == PackagingLevel).FirstOrDefault();
            var firstPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).FirstOrDefault();
            var lastPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderByDescending(p => p.Id).FirstOrDefault().PackageTypeCode;
            if (IsLoose == true)
            {
                if (PackagingLevel != "MOC")
                {
                    if (selectedJobType.Job_Type != "DSCSA")
                    {
                        if (job.ProviderId != 2)
                        {
                            Quantity = Quantity + loosqty;
                        }

                    }

                }
            }
            //if (PackagingLevel != "PAL")
            //{
            if (IsTertiary == false)
            {
                bool result = TLExtraUIDs(pacAssoDet.GTIN, Quantity, ProviderId, job.CompType);
            }

            //if (result == true)
            //{
            M_Identities ids = new M_Identities();
            ids.CreatedOn = DateTime.Now;
            ids.CustomerId = (int)job.CustomerId;
            ids.GTIN = pacAssoDet.GTIN;
            ids.PPN = pacAssoDet.PPN;
            ids.JID = job.JID;
            ids.PackageTypeCode = pacAssoDet.PackageTypeCode;
            ids.IsTransfered = false;
            ids.IsExtra = true;
            db.M_Identities.Add(ids);
            db.SaveChanges();

            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            UIDDataHlper hlpr = new UIDDataHlper();
            hlpr.setProductData((int)job.PAID);
            var firstLvl = firstPacRec.PackageTypeCode;
            var tertLvl = lastPacRec;
            var IAC_CIN = db.Settings.FirstOrDefault();
            if (IsLoose == true)
            {
                if (selectedJobType.Job_Type == "DSCSA")
                {
                    if (!IsTertiary)
                    {
                        hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
                    }
                    if (job.ProviderId != 2)
                    {
                        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                    }
                    else
                    {
                        bool loosSSCC = TLExtraUIDs(pacAssoDet.GTIN, Quantity, ProviderId, "LSSCC");
                        hlpr.CalculateTlinkLoosSSCC(PackagingLevel, pacAssoDet.GTIN, Convert.ToInt32(Utilities.getAppSettings("LoosShipper")));
                        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                    }


                }
                else
                {
                    hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
                }
            }
            else
            {
                hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
            }

            if (IsTertiary == true)
            {

                hlpr.GenerateExtraSSCCs((int)job.JID, Quantity, IsLoose, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                if (job.ProviderId == 2)
                {
                    bool SSCC = TLExtraUIDs(pacAssoDet.GTIN, Quantity, ProviderId, "SSCC");
                    hlpr.GenerateTlinkSSCC(PackagingLevel, pacAssoDet.GTIN, Quantity);

                }
            }
            var data = hlpr.getMasterData();
            var masterGTIN = db.M_Identities.Where(x => x.GTIN == pacAssoDet.GTIN && x.JID == job.JID && x.IsExtra == true && x.PackageTypeCode == pacAssoDet.PackageTypeCode).OrderByDescending(p => p.CreatedOn).FirstOrDefault();
            if (IsTertiary == true)
            {
                var tempdata = data.Where(x => x.Value == pacAssoDet.PackageTypeCode);
                if (pacAssoDet.PackageTypeCode == tertLvl)
                {
                    var ssccdata = data.Where(x => x.Value == "SSC");
                    tempdata = tempdata.Union(ssccdata);
                    if (ProviderId == 2)
                    {
                        var Tssccdata = data.Where(x => x.Value.Contains("SSCC"));
                        tempdata = tempdata.Union(Tssccdata);
                    }
                }

                if (IsLoose == true)
                {
                    var loos = data.Where(x => x.Value.Contains("Loos"));
                    tempdata = tempdata.Union(loos);

                    //var Lsscc = data.Where(x => x.Value.Contains("LSSCC"));
                    //tempdata = tempdata.Union(Lsscc);
                }



                var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                masterList.AddRange(ConvertedData);
            }
            else
            {
                var ConvertedData = converUIDData(data, masterGTIN.Id, tertLvl);
                masterList.AddRange(ConvertedData);
            }

            // }
            //}
            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);


            BulkDataHelper bulkHlpr = new BulkDataHelper();
            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
        }


        public bool GenerateRFXCELExtraUIDData(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            PackagingLevel = PackagingLevel.Trim();
            List<X_Identities> masterList = new List<X_Identities>();
            int loosqty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID && x.PackageTypeCode == PackagingLevel).FirstOrDefault();
            var firstPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).FirstOrDefault();
            var lastPacRec = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderByDescending(p => p.Id).FirstOrDefault().PackageTypeCode;
            if (IsLoose == true)
            {
                if (PackagingLevel != "MOC")
                {
                    if (selectedJobType.Job_Type != "DSCSA")
                    {
                        Quantity = Quantity + loosqty;
                    }

                }
            }
            if (PackagingLevel != "PAL")
            {
                bool result = RFXCELExtraUIDs(pacAssoDet.GTIN, Quantity, ProviderId);

                if (result == true)
                {
                    M_Identities ids = new M_Identities();
                    ids.CreatedOn = DateTime.Now;
                    ids.CustomerId = (int)job.CustomerId;
                    ids.GTIN = pacAssoDet.GTIN;
                    ids.PPN = pacAssoDet.PPN;
                    ids.JID = job.JID;
                    ids.PackageTypeCode = pacAssoDet.PackageTypeCode;
                    ids.IsTransfered = false;
                    ids.IsExtra = true;
                    db.M_Identities.Add(ids);
                    db.SaveChanges();

                    var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
                    UIDDataHlper hlpr = new UIDDataHlper();
                    hlpr.setProductData((int)job.PAID);
                    var firstLvl = firstPacRec.PackageTypeCode;
                    var tertLvl = lastPacRec;
                    var IAC_CIN = db.Settings.FirstOrDefault();
                    if (IsLoose == true)
                    {
                        if (selectedJobType.Job_Type == "DSCSA")
                        {
                            hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
                            if (PackagingLevel != "MOC")
                            {
                                hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                            }

                        }
                        else
                        {
                            hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
                        }
                    }
                    else
                    {
                        hlpr.CalculateTLExtraUID(pacAssoDet.GTIN, Quantity, ProviderId, PackagingLevel);
                    }

                    if (IsTertiary == true)
                    {
                        hlpr.GenerateExtraSSCCs((int)job.JID, Quantity, IsLoose, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
                    }
                    var data = hlpr.getMasterData();
                    var masterGTIN = db.M_Identities.Where(x => x.GTIN == pacAssoDet.GTIN && x.JID == job.JID && x.IsExtra == true && x.PackageTypeCode == pacAssoDet.PackageTypeCode).OrderByDescending(p => p.CreatedOn).FirstOrDefault();
                    if (IsTertiary == true)
                    {
                        var tempdata = data.Where(x => x.Value == pacAssoDet.PackageTypeCode);
                        if (pacAssoDet.PackageTypeCode == tertLvl)
                        {
                            var ssccdata = data.Where(x => x.Value == "SSC");
                            tempdata = tempdata.Union(ssccdata);
                        }

                        if (IsLoose == true)
                        {
                            var loos = data.Where(x => x.Value.Contains("Loos"));
                            tempdata = tempdata.Union(loos);
                        }

                        var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                        masterList.AddRange(ConvertedData);
                    }
                    else
                    {
                        var ConvertedData = converUIDData(data, masterGTIN.Id, tertLvl);
                        masterList.AddRange(ConvertedData);
                    }

                }
            }
            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);


            BulkDataHelper bulkHlpr = new BulkDataHelper();
            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
        }

        public bool GenerateTraceKeyExtraUIDData(Job job, int Quantity, string PackagingLevel, string LineCode, bool IsTertiary, bool IsLoose, int ProviderId)
        {
            BulkDataHelper bulkHlpr = new BulkDataHelper();
            List<X_Identities> masterList = new List<X_Identities>();
            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(x => x.Id).ToList();
            var pckdtls = pacAssoDet.Where(x => x.PackageTypeCode == PackagingLevel).FirstOrDefault();
            if (!IsTertiary)
            {

                var tempCustomer = db.M_TracelinkRequest.Where(x => x.GTIN == pckdtls.GTIN && x.ProviderId == ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                if (tempCustomer == null)
                {
                    ExceptionHandler.ExceptionLogger.LogError("GTIN=" + pckdtls.GTIN + " does not belong to M_TracelinkRequest table");
                    return false;
                }
            }



            if (!IsTertiary)
            {
                var uid = db.X_TracelinkUIDStore.Where(x => x.GTIN == pckdtls.GTIN && x.IsUsed == false).Take(Quantity).ToList();
                if (uid.Count < Quantity)
                {
                    ExceptionHandler.ExceptionLogger.LogError("Uid Exist=" + uid.Count + " Uid Requested" + Quantity);
                    return false;
                }


            }
            M_Identities ids = new M_Identities();
            ids.CreatedOn = DateTime.Now;
            ids.CustomerId = (int)job.CustomerId;
            ids.GTIN = pckdtls.GTIN;
            ids.PPN = pckdtls.PPN;
            ids.JID = job.JID;
            ids.PackageTypeCode = pckdtls.PackageTypeCode;
            ids.IsTransfered = false;
            ids.IsExtra = true;
            db.M_Identities.Add(ids);
            db.SaveChanges();
            var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            UIDDataHlper hlpr = new UIDDataHlper();
            hlpr.setProductData((int)job.PAID);
            var lvls = pacAssoDet.Select(x => x.PackageTypeCode).ToList();
            List<string> sortedlvls = ProductPackageHelper.sorttheLevels(lvls);
            var firstLvl = sortedlvls[0];
            var tertLvl = sortedlvls[sortedlvls.Count - 1];
            var IAC_CIN = db.Settings.FirstOrDefault();
            if (IsLoose == true)
            {
                if (selectedJobType.Job_Type == "DSCSA")
                {
                    hlpr.CalculateTLExtraUID(pckdtls.GTIN, Quantity, ProviderId, PackagingLevel);
                    if (PackagingLevel != "MOC")
                    {
                        hlpr.CalculateExtraLoosSSCC(PackagingLevel, "MOC", "", Convert.ToInt32(job.JID), Convert.ToInt32(job.CustomerId), cust.LoosExt);
                    }

                }
                else
                {
                    hlpr.CalculateTLExtraUID(pckdtls.GTIN, Quantity, ProviderId, PackagingLevel);
                }
            }
            else
            {
                hlpr.CalculateTLExtraUID(pckdtls.GTIN, Quantity, ProviderId, PackagingLevel);
            }

            if (IsTertiary == true && PackagingLevel != "MOC")
            {
                hlpr.GenerateExtraSSCCs((int)job.JID, Quantity, IsLoose, selectedJobType.Job_Type, IAC_CIN.IAC_CIN, cust.LoosExt, cust.SSCCExt, Convert.ToInt32(job.CustomerId));
            }

            var data = hlpr.getMasterData();
            var masterGTIN = db.M_Identities.Where(x => x.GTIN == pckdtls.GTIN && x.JID == job.JID && x.IsExtra == true && x.PackageTypeCode == pckdtls.PackageTypeCode).OrderByDescending(p => p.CreatedOn).FirstOrDefault();
            if (IsTertiary == true)
            {
                var tempdata = data.Where(x => x.Value == pckdtls.PackageTypeCode);
                if (pckdtls.PackageTypeCode == tertLvl && tertLvl != "MOC")
                {
                    var ssccdata = data.Where(x => x.Value == "SSC");
                    tempdata = tempdata.Union(ssccdata);
                }

                if (IsLoose == true)
                {
                    var loos = data.Where(x => x.Value.Contains("Loos"));
                    tempdata = tempdata.Union(loos);
                }

                var ConvertedData = converUIDData(tempdata, masterGTIN.Id, tertLvl);
                masterList.AddRange(ConvertedData);
            }
            else
            {
                if (selectedJobType.Job_Type == "RUSSIA")
                {
                    var ConvertedData = converUIDDataCryptoCode(data, masterGTIN.Id, tertLvl);
                    masterList.AddRange(ConvertedData);
                }
                else
                {
                    var ConvertedData = converUIDData(data, masterGTIN.Id, tertLvl);
                    masterList.AddRange(ConvertedData);
                }
            }
            DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(masterList);

            return bulkHlpr.InsertUIDIdenties(DtconvertedData);
        }
        #region TraceLink UID Request for GTIN
        [System.Web.Http.HttpPost]
        public bool TLExtraUIDs(string GTIN, int UidQuantity, int ProviderId, string Srno)
        {
            string type = "";
            if (Srno != "")
            {
                type = Srno;
            }
            else
            {
                type = "GTIN";
            }
            bool msg = false;
            int threshhold, uidqty = 0;
            if (UidQuantity <= 0) { return false; }


            var tempCustomer = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId && x.SrnoType == type).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
            if (tempCustomer == null) { return false; }

            var customer = db.M_Customer.Find(tempCustomer.CustomerId);
            if (customer == null) { return false; }
            Tracelink tl = new Tracelink();
            if (UidQuantity > 0)
            {
                var isGTINRegistered = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId && x.SrnoType == type).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                if (isGTINRegistered != null)
                {
                    threshhold = isGTINRegistered.Threshold;

                    var tlUnusedUidLst = db.X_TracelinkUIDStore.Where(x => x.TLRequestId == isGTINRegistered.Id && x.IsUsed == false).ToList();
                    if (tlUnusedUidLst.Count < (threshhold + UidQuantity))
                    {
                        int uidreq = threshhold + UidQuantity;
                        if (uidreq > tlUnusedUidLst.Count)
                        {
                            int uidqtyrequired = uidreq - tlUnusedUidLst.Count();
                            uidqty = uidqtyrequired;
                        }

                    }
                    else
                    {
                        return true;//tlUnusedUidLst.Count + " .UID ALREADY EXIST AND THE COUNT REQUIRED IS. " + (threshhold + UidQuantity);
                    }


                }
                else
                {
                    uidqty = UidQuantity;
                }
                if (isGTINRegistered.SrnoType == "NTIN")
                {
                    if (Srno == "")
                    {
                        type = "NTIN";
                    }
                }
                if (Srno == "LSSCC")
                {
                    type = "LSSCC";
                    uidqty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                }

                if (Srno == "SSCC")
                {
                    type = "SSCC";
                }
                var data = tl.getDataFromTracelink(customer.APIUrl, customer.SenderId, customer.ReceiverId, uidqty, GTIN, type, customer.CompanyCode, isGTINRegistered.FilterValue);
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        var errs = tl.getErrors();
                        if (errs.Count() == 0)
                        {

                            foreach (var item in data)
                            {
                                item.TLRequestId = isGTINRegistered.Id;
                            }


                            var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                            BulkDataHelper dataHlpr = new BulkDataHelper();
                            if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                            {

                                msg = true;// + " " + convertedData.Rows.Count + " serial numbers imported !";
                            }
                            else
                            {
                                msg = false;
                            }
                        }
                    }
                }
            }

            return msg;
        }
        #endregion

        #region RFXCEL UID Request for GTIN
        [System.Web.Http.HttpPost]
        public bool RFXCELExtraUIDs(string GTIN, int UidQuantity, int ProviderId)
        {
            bool msg = false;
            int threshhold, uidqty = 0;
            if (UidQuantity <= 0) { return false; }


            var tempCustomer = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
            if (tempCustomer == null) { return false; }

            var customer = db.M_Customer.Find(tempCustomer.CustomerId);
            if (customer == null) { return false; }

            if (UidQuantity > 0)
            {
                var isGTINRegistered = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                if (isGTINRegistered != null)
                {
                    threshhold = isGTINRegistered.Threshold;

                    var tlUnusedUidLst = db.X_TracelinkUIDStore.Where(x => x.TLRequestId == isGTINRegistered.Id && x.IsUsed == false).ToList();
                    if (tlUnusedUidLst.Count < (threshhold + UidQuantity))
                    {
                        int uidreq = threshhold + UidQuantity;
                        if (uidreq > tlUnusedUidLst.Count)
                        {
                            int uidqtyrequired = uidreq - tlUnusedUidLst.Count();
                            uidqty = uidqtyrequired;
                        }

                    }
                    else
                    {
                        return true;//tlUnusedUidLst.Count + " .UID ALREADY EXIST AND THE COUNT REQUIRED IS. " + (threshhold + UidQuantity);
                    }


                }
                else
                {
                    uidqty = UidQuantity;
                }
                RfxcelServices r1 = new RfxcelServices();
                EPCISConfig gln = new EPCISConfig();
                var data = r1.getSerialNumbersFromRfxcel(customer.APIUrl, customer.SenderId, customer.ReceiverId, uidqty, GTIN, gln.GetEPCGLN(customer.Id));
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        //var errs = tl.getErrors();
                        //if (errs.Count() == 0)
                        //{

                        foreach (var item in data)
                        {
                            item.TLRequestId = isGTINRegistered.Id;
                        }


                        var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                        BulkDataHelper dataHlpr = new BulkDataHelper();
                        if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                        {

                            msg = true;// + " " + convertedData.Rows.Count + " serial numbers imported !";
                        }
                        else
                        {
                            msg = false;
                        }
                        // }
                    }
                }
            }

            return msg;
        }
        #endregion

        public List<X_Identities> converUIDData(IEnumerable<KeyValuePair<string, string>> data, int MasterId, string TertiaryDec)
        {
            List<X_Identities> lst = new List<X_Identities>();
            foreach (var item in data)
            {
                X_Identities id = new X_Identities();

                //SSCC = true  // UID = false 
                if (item.Value == "SSC")// || item.Value == TertiaryDec)
                {
                    id.CodeType = true;
                }
                else
                {
                    id.CodeType = false;
                }
                id.IsTransfered = false;
                id.MasterId = MasterId;
                id.SerialNo = item.Key;
                id.PackTypeCode = item.Value;
                lst.Add(id);
            }
            return lst;
        }

        public List<X_Identities> converUIDDataWithCrypto(IEnumerable<Tuple<string, string, string>> data, int MasterId, string TertiaryDec)
        {
            List<X_Identities> lst = new List<X_Identities>();
            foreach (var item in data)
            {
                X_Identities id = new X_Identities();

                //SSCC = true  // UID = false 
                if (item.Item1 == "SSC")// || item.Value == TertiaryDec)
                {
                    id.CodeType = true;
                }
                else
                {
                    id.CodeType = false;
                }
                id.IsTransfered = false;
                id.MasterId = MasterId;
                id.SerialNo = item.Item2;
                id.CryptoCode = item.Item3;
                id.PackTypeCode = item.Item1;
                lst.Add(id);
            }
            return lst;
        }

        PTPLUidGen obj = new PTPLUidGen();
        public List<X_Identities> converUIDDataCryptoCode(IEnumerable<KeyValuePair<string, string>> data, int MasterId, string TertiaryDec)
        {
            List<X_Identities> lst = new List<X_Identities>();
            foreach (var item in data)
            {
                X_Identities id = new X_Identities();

                //SSCC = true  // UID = false 
                if (item.Value == "SSC")// || item.Value == TertiaryDec)
                {
                    id.CodeType = true;
                }
                else
                {
                    id.CodeType = false;
                    id.CryptoCode = obj.GenerateCryptoCode(44);
                }
                id.IsTransfered = false;
                id.MasterId = MasterId;
                id.SerialNo = item.Key;
                id.PackTypeCode = item.Value;

                lst.Add(id);
            }
            return lst;
        }
    }

    public class UIDDataHlper
    {
        public int DummyUidno;
        public int DummyIMno;
        public int DummyTerNo;

        #region SSCC
        int PPBCode = 7;
        int MOCCode = 8;
        int OBXCode = 9;
        int ISHCode = 10;
        int OSHCode = 11;
        int PALCode = 12;
        int SSCCCode = 20;
        #endregion

        private int MOC, OBX, ISH, PAL, PPB, OSH;
        private int MOCtoPrint, OBXtoPrint, ISHtoPrint, OSHtoPrint, PPBtoPrint, PALtoPrint, SSCCtoPrint;
        private int lengthOfUID = Convert.ToInt32(Utilities.getAppSettings("SerialNumberLength"));
        private string TertiaryLevel;
        private Dictionary<string, string> masterList = new Dictionary<string, string>();
        private int PAID;


        int UIDQuantity = 0;
        public void setProductData(int PAID)
        {
            try
            {
                this.PAID = PAID;
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
        public void CalculateAndGenerateIds(int TotalUIds, string TID, string firstLvl, string lastLvl,  string requesttype="")
        {
            UIDQuantity = TotalUIds;
            //try
            //{

            if (UIDQuantity == 0) return;

            if (IsLevelExisting(PPB))
            {
                PPBtoPrint = UIDQuantity;
                if (PPBtoPrint != 0)
                {
                    setUIDList("PPB", PPBtoPrint, TID, firstLvl, lastLvl, requesttype);
                }
            }

            if (IsLevelExisting(MOC))
            {

                if (!IsLevelExisting(PPB))
                {
                    MOCtoPrint = UIDQuantity;
                    setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl, requesttype);
                }
                else
                {
                    double res = (float)PPBtoPrint / (float)MOC;
                    MOCtoPrint = (int)Math.Ceiling(res);
                    setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl, requesttype);
                }
            }

            if (IsLevelExisting(OBX))
            {
                if (TID != "PPN")
                {
                    if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        OBXtoPrint = UIDQuantity;
                        setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);
                    }
                    else
                    {
                        if (!IsLevelExisting(MOC))
                        {
                            if (IsLevelExisting(PPB))
                            {
                                double res = (float)PPBtoPrint / (float)OBX;
                                OBXtoPrint = (int)Math.Ceiling(res);
                                setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)OBX;
                            OBXtoPrint = (int)Math.Ceiling(res);
                            setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);

                        }

                    }
                }
                else
                {

                    if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        OBXtoPrint = UIDQuantity;
                        setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);
                    }
                    else
                    {
                        if (!IsLevelExisting(MOC))
                        {
                            if (IsLevelExisting(PPB))
                            {
                                double res = (float)PPBtoPrint / (float)OBX;
                                OBXtoPrint = (int)Math.Ceiling(res);
                                setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)OBX;
                            OBXtoPrint = (int)Math.Ceiling(res);
                            setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl, requesttype);

                        }

                    }

                }
            }


            if (IsLevelExisting(ISH))
            {
                if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    ISHtoPrint = UIDQuantity;
                    setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl, requesttype);

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
                                setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl, requesttype);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)ISH;
                            ISHtoPrint = (int)Math.Ceiling(res);
                            setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl, requesttype);
                        }

                    }
                    else
                    {
                        double res = (float)OBXtoPrint / (float)ISH;
                        ISHtoPrint = (int)Math.Ceiling(res);
                        setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl, requesttype);
                    }
                }
            }

            if (IsLevelExisting(OSH))
            {
                if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    OSHtoPrint = UIDQuantity;
                    setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl, requesttype);

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
                                    setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl, requesttype);
                                }
                            }
                            else
                            {
                                double res = (float)MOCtoPrint / (float)OSH;
                                OSHtoPrint = (int)Math.Ceiling(res);
                                setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl, requesttype);
                            }
                        }
                        else
                        {
                            double res = (float)OBXtoPrint / (float)OSH;
                            OSHtoPrint = (int)Math.Ceiling(res);
                            setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl, requesttype);
                        }
                    }
                    else
                    {
                        double res = (float)ISHtoPrint / (float)OSH;
                        OSHtoPrint = (int)Math.Ceiling(res);
                        setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl, requesttype);
                    }
                }
            }

            if (IsLevelExisting(PAL))
            {
                if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    PALtoPrint = UIDQuantity;
                    setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
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
                                        setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
                                    }
                                }
                                else
                                {
                                    double res = (float)MOCtoPrint / (float)PAL;
                                    PALtoPrint = (int)Math.Ceiling(res);
                                    setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
                                }
                            }
                            else
                            {
                                double res = (float)OBXtoPrint / (float)PAL;
                                PALtoPrint = (int)Math.Ceiling(res);
                                setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
                            }
                        }
                        else
                        {
                            double res = (float)ISHtoPrint / (float)PAL;
                            PALtoPrint = (int)Math.Ceiling(res);
                            setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
                        }
                    }
                    else
                    {
                        double res = (float)OSHtoPrint / (float)PAL;
                        PALtoPrint = (int)Math.Ceiling(res);
                        setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl, requesttype);
                    }

                }
            }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        }

        public void CalculateUIDsToGenerate(int TotalUIds, int PAID)
        {
            setProductData(PAID);
            UIDQuantity = TotalUIds;
            if (UIDQuantity == 0) return;
            if (IsLevelExisting(PPB))
            {
                PPBtoPrint = UIDQuantity;

            }

            if (IsLevelExisting(MOC))
            {
                if (!IsLevelExisting(PPB))
                {
                    MOCtoPrint = UIDQuantity;
                    //setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl);
                }
                else
                {
                    double res = (float)PPBtoPrint / (float)MOC;
                    MOCtoPrint = (int)Math.Ceiling(res);
                    //setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl);
                }
            }

            if (IsLevelExisting(OBX))
            {
                if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    OBXtoPrint = UIDQuantity;
                    //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);
                }
                else
                {
                    if (!IsLevelExisting(MOC))
                    {
                        if (IsLevelExisting(PPB))
                        {
                            double res = (float)PPBtoPrint / (float)OBX;
                            OBXtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)MOCtoPrint / (float)OBX;
                        OBXtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);

                    }

                }
            }


            if (IsLevelExisting(ISH))
            {
                if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    ISHtoPrint = UIDQuantity;
                    //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);

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
                                //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)ISH;
                            ISHtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                        }

                    }
                    else
                    {
                        double res = (float)OBXtoPrint / (float)ISH;
                        ISHtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                    }
                }
            }

            if (IsLevelExisting(OSH))
            {
                if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    OSHtoPrint = UIDQuantity;
                    //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);

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
                                    //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                                }
                            }
                            else
                            {
                                double res = (float)MOCtoPrint / (float)OSH;
                                OSHtoPrint = (int)Math.Ceiling(res);
                                //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)OBXtoPrint / (float)OSH;
                            OSHtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)ISHtoPrint / (float)OSH;
                        OSHtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                    }
                }
            }

            if (IsLevelExisting(PAL))
            {
                if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    PALtoPrint = UIDQuantity;
                    //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
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
                                        //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                                    }
                                }
                                else
                                {
                                    double res = (float)MOCtoPrint / (float)PAL;
                                    PALtoPrint = (int)Math.Ceiling(res);
                                    //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                                }
                            }
                            else
                            {
                                double res = (float)OBXtoPrint / (float)PAL;
                                PALtoPrint = (int)Math.Ceiling(res);
                                //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)ISHtoPrint / (float)PAL;
                            PALtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)OSHtoPrint / (float)PAL;
                        PALtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                    }

                }
            }


        }

        public void CalculateGenerateUIDForSKorea(int TotalId, string numfrom, string firstLvl, string lastLvl, int jobid)
        {
            long nfrom = Convert.ToInt64(numfrom);

            Dictionary<string, string> ids = new Dictionary<string, string>();
            for (int i = 0; i < TotalId; i++)
            {
                long n = nfrom + i;
                ids.Add(n.ToString(), firstLvl);
            }
            masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
        }

        public void CalculateLoosSSCC(List<string> lvl, string firstLvl, string lastLvl, int jobid, int customerId, string Extension)
        {
            if (lvl.Contains("OBX"))
            {
                setSSCC("OBX", firstLvl, lastLvl, jobid, customerId, Extension);
            }
            if (lvl.Contains("ISH"))
            {
                setSSCC("ISH", firstLvl, lastLvl, jobid, customerId, Extension);
            }
            if (lvl.Contains("OSH"))
            {
                setSSCC("OSH", firstLvl, lastLvl, jobid, customerId, Extension);
            }
            if (lvl.Contains("PAL"))
            {
                setSSCC("PAL", firstLvl, lastLvl, jobid, customerId, Extension);
            }
        }

        public void CalculateLoosCode(List<string> lvl, string selectedJobType)
        {
            if (lvl.Contains("OBX"))
            {
                setLoosUID("OBX", selectedJobType);
            }

            if (lvl.Contains("ISH"))
            {
                setLoosUID("ISH", selectedJobType);
            }

            if (lvl.Contains("OSH"))
            {
                setLoosUID("OSH", selectedJobType);
            }

            if (lvl.Contains("PAL"))
            {
                setLoosUID("PAL", selectedJobType);
            }
        }
        public void CalculateExtraLoosSSCC(string PAckageType, string firstLvl, string lastLvl, int jobid, int customerId, string Extension)
        {

            setSSCC(PAckageType, firstLvl, lastLvl, jobid, customerId, Extension);

        }

        public void CalculateTlinkLoosSSCC(string PackageType, string GTIN, int Qty)
        {
            setTLLSSCC(GTIN, PackageType, Qty);
        }


        public void GenerateTlinkSSCC(string PackageType, string GTIN, int Qty)
        {
            setTlinkSSCC(GTIN, PackageType, Qty);
        }
        //public void CalculateAndGenerateExtraIds(int JID, int TotalUIds, string TID, string firstLvl, string lastLvl)
        //{
        //    UIDQuantity = TotalUIds;
        //    //try
        //    //{

        //    if (UIDQuantity == 0) return;

        //    if (IsLevelExisting(PPB))
        //    {
        //        PPBtoPrint = UIDQuantity;
        //        if (PPBtoPrint != 0)
        //        {
        //            setExtraUIDList(JID, "PPB", PPBtoPrint, TID, firstLvl, lastLvl);
        //        }
        //    }

        //    if (IsLevelExisting(MOC))
        //    {
        //        if (!IsLevelExisting(PPB))
        //        {
        //            MOCtoPrint = UIDQuantity;
        //            setExtraUIDList(JID, "MOC", MOCtoPrint, TID, firstLvl, lastLvl);
        //        }
        //        else
        //        {
        //            double res = (float)PPBtoPrint / (float)MOC;
        //            MOCtoPrint = (int)Math.Ceiling(res);
        //            setExtraUIDList(JID, "MOC", MOCtoPrint, TID, firstLvl, lastLvl);
        //        }
        //    }

        //    if (IsLevelExisting(OBX))
        //    {
        //        if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
        //        {
        //            OBXtoPrint = UIDQuantity;
        //            setExtraUIDList(JID, "OBX", OBXtoPrint, TID, firstLvl, lastLvl);
        //        }
        //        else
        //        {
        //            if (!IsLevelExisting(MOC))
        //            {
        //                if (IsLevelExisting(PPB))
        //                {
        //                    double res = (float)PPBtoPrint / (float)OBX;
        //                    OBXtoPrint = (int)Math.Ceiling(res);
        //                    setExtraUIDList(JID, "OBX", OBXtoPrint, TID, firstLvl, lastLvl);
        //                }
        //            }
        //            else
        //            {
        //                double res = (float)MOCtoPrint / (float)OBX;
        //                OBXtoPrint = (int)Math.Ceiling(res);
        //                setExtraUIDList(JID, "OBX", OBXtoPrint, TID, firstLvl, lastLvl);

        //            }

        //        }
        //    }


        //    if (IsLevelExisting(ISH))
        //    {
        //        if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
        //        {
        //            ISHtoPrint = UIDQuantity;
        //            setExtraUIDList(JID, "ISH", ISHtoPrint, TID, firstLvl, lastLvl);

        //        }
        //        else
        //        {
        //            if (!IsLevelExisting(OBX))
        //            {
        //                if (!IsLevelExisting(MOC))
        //                {
        //                    if (IsLevelExisting(PPB))
        //                    {
        //                        double res = (float)PPBtoPrint / (float)ISH;
        //                        ISHtoPrint = (int)Math.Ceiling(res);
        //                        setExtraUIDList(JID, "ISH", ISHtoPrint, TID, firstLvl, lastLvl);
        //                    }
        //                }
        //                else
        //                {
        //                    double res = (float)MOCtoPrint / (float)ISH;
        //                    ISHtoPrint = (int)Math.Ceiling(res);
        //                    setExtraUIDList(JID, "ISH", ISHtoPrint, TID, firstLvl, lastLvl);
        //                }

        //            }
        //            else
        //            {
        //                double res = (float)OBXtoPrint / (float)ISH;
        //                ISHtoPrint = (int)Math.Ceiling(res);
        //                setExtraUIDList(JID, "ISH", ISHtoPrint, TID, firstLvl, lastLvl);
        //            }
        //        }
        //    }

        //    if (IsLevelExisting(OSH))
        //    {
        //        if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
        //        {
        //            OSHtoPrint = UIDQuantity;
        //            setExtraUIDList(JID, "OSH", OSHtoPrint, TID, firstLvl, lastLvl);

        //        }
        //        else
        //        {
        //            if (!IsLevelExisting(ISH))
        //            {
        //                if (!IsLevelExisting(OBX))
        //                {
        //                    if (!IsLevelExisting(MOC))
        //                    {
        //                        if (IsLevelExisting(PPB))
        //                        {
        //                            double res = (float)PPBtoPrint / (float)OSH;
        //                            OSHtoPrint = (int)Math.Ceiling(res);
        //                            setExtraUIDList(JID, "OSH", OSHtoPrint, TID, firstLvl, lastLvl);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        double res = (float)MOCtoPrint / (float)OSH;
        //                        OSHtoPrint = (int)Math.Ceiling(res);
        //                        setExtraUIDList(JID, "OSH", OSHtoPrint, TID, firstLvl, lastLvl);
        //                    }
        //                }
        //                else
        //                {
        //                    double res = (float)OBXtoPrint / (float)OSH;
        //                    OSHtoPrint = (int)Math.Ceiling(res);
        //                    setExtraUIDList(JID, "OSH", OSHtoPrint, TID, firstLvl, lastLvl);
        //                }
        //            }
        //            else
        //            {
        //                double res = (float)ISHtoPrint / (float)OSH;
        //                OSHtoPrint = (int)Math.Ceiling(res);
        //                setExtraUIDList(JID, "OSH", OSHtoPrint, TID, firstLvl, lastLvl);
        //            }
        //        }
        //    }

        //    if (IsLevelExisting(PAL))
        //    {
        //        if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
        //        {
        //            PALtoPrint = UIDQuantity;
        //            setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //        }
        //        else
        //        {
        //            if (!IsLevelExisting(OSH))
        //            {
        //                if (!IsLevelExisting(ISH))
        //                {
        //                    if (!IsLevelExisting(OBX))
        //                    {
        //                        if (!IsLevelExisting(MOC))
        //                        {
        //                            if (IsLevelExisting(PPB))
        //                            {
        //                                double res = (float)PPBtoPrint / (float)PAL;
        //                                PALtoPrint = (int)Math.Ceiling(res);
        //                                setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            double res = (float)MOCtoPrint / (float)PAL;
        //                            PALtoPrint = (int)Math.Ceiling(res);
        //                            setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        double res = (float)OBXtoPrint / (float)PAL;
        //                        PALtoPrint = (int)Math.Ceiling(res);
        //                        setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //                    }
        //                }
        //                else
        //                {
        //                    double res = (float)ISHtoPrint / (float)PAL;
        //                    PALtoPrint = (int)Math.Ceiling(res);
        //                    setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //                }
        //            }
        //            else
        //            {
        //                double res = (float)OSHtoPrint / (float)PAL;
        //                PALtoPrint = (int)Math.Ceiling(res);
        //                setExtraUIDList(JID, "PAL", PALtoPrint, TID, firstLvl, lastLvl);
        //            }

        //        }
        //    }
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    throw;
        //    //}

        //}

        public void CalculateAndGenerateExtraIds(int JID, int TotalUIds, string TID, string firstLvl, string lastLvl)
        {
            try
            {
                setExtraUIDList(JID, firstLvl, TotalUIds, TID, firstLvl, lastLvl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CalculateTLExtraUID(string GTIN, int Quantity, int ProviderId, string PackageTypeCode)
        {
            setTLExtraUIDList(GTIN, Quantity, ProviderId, PackageTypeCode);
        }

        public Dictionary<string, string> getMasterData()
        {
            return masterList;
        }

        #region UID UTILS
        private List<string> generateUIDs(int Quantity, int LengthOfUid, string selectedJobType)
        {
            try
            {
                IDGenrationFactory obj = new IDGenrationFactory();
                return obj.generateIDs(Quantity, LengthOfUid, selectedJobType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void setUIDList(string PackageTypeCode, int QtyToGenerate, string TID, string firstLvl, string lastLvl, string requesttype)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string uidlen = Utilities.getAppSettings("SerialNumberLength");
            //if(!string.IsNullOrEmpty(uidlen))
            //{
            //    lengthOfUID = Convert.ToInt32(uidlen);
            //}
            //string startdigit = Utilities.getAppSettings("SerialNumberStartDigit"];
            var loosQuantity = 0;
            if (requesttype!="AdditionalBatch")
            {
                loosQuantity =Convert.ToInt32( Utilities.getAppSettings("LoosShipper"));
            }
           
            if (PackageTypeCode != firstLvl || PackageTypeCode != "MOC")
            {
                if (TID != "DSCSA")
                {
                    QtyToGenerate = QtyToGenerate+ Convert.ToInt32(loosQuantity);
                }
            }

            List<string> LstToSave = new List<string>();
            try
            {
                if (TID == "DGFT" || TID == "DSCSA" || TID == "RUSSIA" || TID == "SAUDI_ARABIA" || TID == "TURKEY" || TID == "EU")
                {
                    UIDVerifier verifier = new UIDVerifier();
                    int i = 1;

                    while (i <= QtyToGenerate)
                    {
                        //if (string.IsNullOrEmpty(startdigit))
                        //{
                        //    lengthOfUID = 12;
                        string Uid = generateUIDs(1, lengthOfUID, TID).FirstOrDefault();
                        if (verifier.AddCodeGen(Uid))
                        {
                            i++;
                        }
                        //}
                        //else
                        //{
                        //    string Uid = generateUIDs(1, lengthOfUID).First();
                        //    Uid = startdigit + Uid;
                        //    if (verifier.AddCodeGen(Uid))
                        //    {
                        //        i++;
                        //    }
                        //}
                    }
                    LstToSave = verifier.getUniqueIds();

                    Dictionary<string, string> ids = new Dictionary<string, string>();

                    foreach (var item in LstToSave)
                    {
                        ids.Add(item, PackageTypeCode);
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }
                else if (TID == "CIP")
                {
                    //if (PackageTypeCode == firstLvl)
                    //{
                    UIDVerifier verifier = new UIDVerifier();
                    int i = 1;

                    while (i <= QtyToGenerate)
                    {
                        string Uid = GEtDummyUID(i, PackageTypeCode);
                        if (verifier.AddCodeGen(Uid))
                        {
                            i++;
                        }
                    }
                    LstToSave = verifier.getUniqueIds();

                    Dictionary<string, string> ids = new Dictionary<string, string>();

                    foreach (var item in LstToSave)
                    {
                        ids.Add(item, PackageTypeCode);
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

                }
                else if (TID == "CHINACODE")
                {
                    UIDVerifier verifier = new UIDVerifier();
                    int i = 1;

                    while (i <= QtyToGenerate)
                    {
                        string Uid = getChinaUID(PAID, PackageTypeCode);
                        if (verifier.AddCodeGen(Uid))
                        {
                            i++;
                        }
                    }
                    LstToSave = verifier.getUniqueIds();

                    Dictionary<string, string> ids = new Dictionary<string, string>();

                    foreach (var item in LstToSave)
                    {
                        ids.Add(item, PackageTypeCode);
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }
                else if (TID == "PPN")
                {
                    if (PackageTypeCode != lastLvl)
                    {
                        UIDVerifier verifier = new UIDVerifier();
                        int i = 1;

                        while (i <= QtyToGenerate)
                        {
                            //if (string.IsNullOrEmpty(startdigit))
                            //{
                            //    lengthOfUID = 12;
                            string Uid = generateUIDs(1, lengthOfUID, TID).FirstOrDefault();
                            if (verifier.AddCodeGen(Uid))
                            {
                                i++;
                            }
                            //}
                            //else
                            //{
                            //    string Uid = generateUIDs(1, lengthOfUID).First();
                            //    Uid = startdigit + Uid;
                            //    if (verifier.AddCodeGen(Uid))
                            //    {
                            //        i++;
                            //    }
                            //}
                        }
                        LstToSave = verifier.getUniqueIds();

                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in LstToSave)
                        {
                            ids.Add(item, PackageTypeCode);
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }
                    else
                    {
                        PTPLUidGen obj = new PTPLUidGen();
                        List<string> Uids = new List<string>();
                        var IAC_CIN = db.Settings.FirstOrDefault();
                        string uid;
                        for (int i = 0; i < QtyToGenerate; i++)
                        {
                            uid = obj.GenerateUID(13, TID);
                            Uids.Add(IAC_CIN.IAC_CIN + uid);
                        }

                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in Uids)
                        {
                            ids.Add(item, PackageTypeCode);
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private ApplicationDbContext db = new ApplicationDbContext();
        private string getChinaUID(int pAID, string packageTypeCode)
        {
            X_ChinaUIDs uid = db.X_ChinaUIDs.Where(x => x.PAID == PAID && x.PackageTypeCode == packageTypeCode && x.IsUsed == false).FirstOrDefault();
            try
            {
                uid.IsUsed = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return uid.Code;
        }

        private void setSSCCOld(string PackageTypeCode, string firstLvl, string lastLvl, int JobID, int CustomerId, string Extension, string requestType="", int Provider = 2)
        {
            int LastSSCC, sscc;

            string CompanyCode, PlantCode;
            SSCCHelper ssccHelperObj = new SSCCHelper();
            int loosQuantity = 0;
            if (requestType!="AdditionBatch")
            {
                loosQuantity= Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));

            }
           //
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            if (PackageTypeCode != firstLvl || firstLvl != "MOC")
            {
                if (loosQuantity > 0)
                {
                    //if (Provider == 2) //Added by Kiran
                    //    sscc = ssccHelperObj.getLastSSCC("DLoos");
                    //else
                    sscc = ssccHelperObj.getLastSSCC("Loos", CustomerId, Extension);

                    LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode, sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + loosQuantity;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "Loos", Extension, CustomerId))
                    {
                        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                        List<string> SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(loosQuantity, CompanyCode, PlantCode, newFirstSSCC, Extension);
                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in SSCC)
                        {
                            ids.Add(item, PackageTypeCode + "Loos");
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }

                }

            }

        }

        private void setSSCC(string PackageTypeCode, string firstLvl, string lastLvl, int JobID, int CustomerId, string Extension, int Provider = 2)
        {
            int LastSSCC, sscc;

            string CompanyCode, PlantCode;
            SSCCHelper ssccHelperObj = new SSCCHelper();
            int loosQuantity = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            if (PackageTypeCode != firstLvl || firstLvl != "MOC")
            {
                if (loosQuantity > 0)
                {
                    //if (Provider == 2) //Added by Kiran
                    //    sscc = ssccHelperObj.getLastSSCC("DLoos");
                    //else
                    sscc = ssccHelperObj.getLastSSCC("Loos", CustomerId, Extension);

                    LastSSCC = sscc;//ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode, sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + loosQuantity;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "Loos", Extension, CustomerId))
                    {
                        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                        List<string> SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(loosQuantity, CompanyCode, PlantCode, newFirstSSCC, Extension);
                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in SSCC)
                        {
                            ids.Add(item, PackageTypeCode + "Loos");
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }

                }

            }

        }



        private void setTLLSSCC(string GTIN, string PackageTypeCode, int qty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var uid = db.X_TracelinkUIDStore.Where(x => x.IsUsed == false && x.Type == "LSSCC" && x.GTIN == GTIN).Take(qty).ToList();
            BulkDataHelper bulkHlpr = new BulkDataHelper();
            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in uid)
            {

                ids.Add(item.SerialNo, PackageTypeCode + "LSSCC");
                bulkHlpr.setFlagToTransferdUID(item.Id);

            }


            masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

        }

        private void setTlinkSSCC(string GTIN, string PackageTypeCode, int qty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var uid = db.X_TracelinkUIDStore.Where(x => x.IsUsed == false && x.Type == "SSCC" && x.GTIN == GTIN).Take(qty).ToList();
            BulkDataHelper bulkHlpr = new BulkDataHelper();
            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in uid)
            {

                ids.Add(item.SerialNo, PackageTypeCode + "SSCC");
                bulkHlpr.setFlagToTransferdUID(item.Id);

            }


            masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

        }

        private void setLoosUID(string lvl, string selectedJobType)
        {
            List<string> LstToSave = new List<string>();
            try
            {
                //int QtyToGenerate = 0;
                //if (requestType!="AdditionalBatch")
                //{
                //    QtyToGenerate=Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                //}

                int QtyToGenerate = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                UIDVerifier verifier = new UIDVerifier();
                int i = 1;

                while (i <= QtyToGenerate)
                {
                    string Uid = generateUIDs(1, lengthOfUID, selectedJobType).FirstOrDefault();
                    if (verifier.AddCodeGen(Uid))
                    {
                        i++;
                    }
                }
                LstToSave = verifier.getUniqueIds();

                Dictionary<string, string> ids = new Dictionary<string, string>();

                foreach (var item in LstToSave)
                {
                    ids.Add(item, lvl);
                }
                masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setExtraUIDList(int JID, string PackageTypeCode, int QtyToGenerate, string TID, string firstLvl, string lastLvl)
        {
            List<string> LstToSave = new List<string>();
            try
            {
                if (TID == "DGFT" || TID == "DSCSA" || TID == "RUSSIA" || TID == "SAUDI_ARABIA" || TID == "TURKEY" || TID == "EU")
                {
                    UIDVerifier verifier = new UIDVerifier();
                    int i = 1;

                    while (i <= QtyToGenerate)
                    {
                        string Uid = generateUIDs(1, lengthOfUID, TID).FirstOrDefault();
                        if (verifier.AddCodeGen(Uid))
                        {
                            i++;
                        }
                    }
                    LstToSave = verifier.getUniqueIds();

                    Dictionary<string, string> ids = new Dictionary<string, string>();

                    foreach (var item in LstToSave)
                    {
                        ids.Add(item, PackageTypeCode);
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }
                else if (TID == "CIP")
                {


                    //if (PackageTypeCode == firstLvl)
                    //{

                    UIDDataHelper Dhlpr = new UIDDataHelper();
                    var lastRec = Dhlpr.getLastUID(JID, PackageTypeCode);
                    var PlainID = lastRec.Remove(0, 3);

                    UIDVerifier verifier = new UIDVerifier();
                    int i = Convert.ToInt32(PlainID) + 1;
                    int totalQty = QtyToGenerate + Convert.ToInt32(PlainID);
                    while (i <= totalQty)
                    {
                        string Uid = GEtDummyUID(i, PackageTypeCode);
                        if (verifier.AddCodeGen(Uid))
                        {
                            i++;
                        }
                    }
                    LstToSave = verifier.getUniqueIds();

                    Dictionary<string, string> ids = new Dictionary<string, string>();

                    foreach (var item in LstToSave)
                    {
                        ids.Add(item, PackageTypeCode);
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    //}
                    //else if (PackageTypeCode == lastLvl)
                    //{
                    //    UIDDataHelper Dhlpr = new UIDDataHelper();
                    //    var lastRec = Dhlpr.getLastUID(JID, PackageTypeCode);
                    //    var PlainID = lastRec.Remove(0, 3);

                    //    UIDVerifier verifier = new UIDVerifier();
                    //    int i = Convert.ToInt32(PlainID);
                    //    int totalQty = QtyToGenerate + Convert.ToInt32(PlainID);
                    //    while (i <= totalQty)
                    //    {
                    //        string Uid = GEtDummyTerCode(i);
                    //        if (verifier.AddCodeGen(Uid))
                    //        {
                    //            i++;
                    //        }
                    //    }
                    //    LstToSave = verifier.getUniqueIds();

                    //    Dictionary<string, string> ids = new Dictionary<string, string>();

                    //    foreach (var item in LstToSave)
                    //    {
                    //        ids.Add(item, PackageTypeCode);
                    //    }
                    //    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    //}
                    //else
                    //{
                    //    UIDDataHelper Dhlpr = new UIDDataHelper();
                    //    var lastRec = Dhlpr.getLastUID(JID, PackageTypeCode);
                    //    var PlainID = lastRec.Remove(0, 3);

                    //    UIDVerifier verifier = new UIDVerifier();
                    //    int i = Convert.ToInt32(PlainID);
                    //    int totalQty = QtyToGenerate + Convert.ToInt32(PlainID);
                    //    while (i <= totalQty)
                    //    {
                    //        string Uid = GEtDummyIMCode(i);
                    //        if (verifier.AddCodeGen(Uid))
                    //        {
                    //            i++;
                    //        }
                    //    }
                    //    LstToSave = verifier.getUniqueIds();

                    //    Dictionary<string, string> ids = new Dictionary<string, string>();

                    //    foreach (var item in LstToSave)
                    //    {
                    //        ids.Add(item, PackageTypeCode);
                    //    }
                    //    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    //}
                }
                else if (TID == "PPN")
                {
                    if (PackageTypeCode != lastLvl)
                    {
                        UIDVerifier verifier = new UIDVerifier();
                        int i = 1;

                        while (i <= QtyToGenerate)
                        {
                            //if (string.IsNullOrEmpty(startdigit))
                            //{
                            //    lengthOfUID = 12;
                            string Uid = generateUIDs(1, lengthOfUID, TID).FirstOrDefault();
                            if (verifier.AddCodeGen(Uid))
                            {
                                i++;
                            }
                            //}
                            //else
                            //{
                            //    string Uid = generateUIDs(1, lengthOfUID).First();
                            //    Uid = startdigit + Uid;
                            //    if (verifier.AddCodeGen(Uid))
                            //    {
                            //        i++;
                            //    }
                            //}
                        }
                        LstToSave = verifier.getUniqueIds();

                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in LstToSave)
                        {
                            ids.Add(item, PackageTypeCode);
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }
                    else
                    {
                        PTPLUidGen obj = new PTPLUidGen();
                        List<string> Uids = new List<string>();
                        var IAC_CIN = db.Settings.FirstOrDefault();
                        string uid;
                        for (int i = 0; i < QtyToGenerate; i++)
                        {
                            uid = obj.GenerateUID(13, TID);
                            Uids.Add(IAC_CIN.IAC_CIN + uid);
                        }

                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in Uids)
                        {
                            ids.Add(item, PackageTypeCode);
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void setTLExtraUIDList(string GTIN, int Quantity, int Provider, string PackageTypeCode)
        {
            BulkDataHelper bulkHlpr = new BulkDataHelper();
            var isGTINRegistered = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == Provider).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
            // var uid = db.X_TracelinkUIDStore.Where(x => x.TLRequestId == isGTINRegistered.Id && x.IsUsed==false).Select(x => x.SerialNo).Take(Quantity).ToList();
            var uid = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTIN && x.IsUsed == false).Take(Quantity).ToList();
            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in uid)
            {
                ids.Add(item.SerialNo, PackageTypeCode);
                bulkHlpr.setFlagToTransferdUID(item.Id);
            }
            masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
        }
        public string GEtDummyUID(int i, string PackageTypeCode)
        {
            if (PackageTypeCode == "OBX")
            {
                PackageTypeCode = "0BX";
            }

            if (PackageTypeCode == "OSH")
            {
                PackageTypeCode = "ASH";
            }

            string UId = PackageTypeCode;

            UId = UId + i.ToString().PadLeft(9, '0');
            return UId;
        }


        public string GEtDummyIMCode(int i)
        {
            string IMNO = "IML";
            IMNO = IMNO + i.ToString().PadLeft(9, '0');
            return IMNO;
        }

        public string GEtDummyTerCode(int i)
        {
            string TERNo = "TER";
            TERNo = TERNo + i.ToString().PadLeft(9, '0');
            return TERNo;
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
        #endregion


        ////////////////////

        #region SSCCsutils



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




        public void GenerateSSCCs(int JobId, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId)
        {
            try
            {
                CalculateSSCCToPrint();
                List<string> UIDsSSCC;

                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();
                var loosQuantity = Utilities.getAppSettings("LoosShipper");


                SSCCtoPrint = SSCCtoPrint + Convert.ToInt32(loosQuantity);

                UIDsSSCC = obj.generateSSCC(JobId, SSCCtoPrint, selectedJobType, IAC_CIN, LoosExt, SSCCExt, CustomerId);

                if (UIDsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in UIDsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GenerateExtraSSCCs(int JobId, int Qty, bool IsLoose, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId)
        {
            try
            {
                var loosQuantity = Utilities.getAppSettings("LoosShipper");
                SSCCtoPrint = Qty;
                //if (IsLoose == true)
                //{
                // SSCCtoPrint = SSCCtoPrint;//+ Convert.ToInt32(loosQuantity);
                // }

                List<string> UIDsSSCC;

                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();

                UIDsSSCC = obj.generateSSCC(JobId, SSCCtoPrint, selectedJobType, IAC_CIN, LoosExt, SSCCExt, CustomerId);

                if (UIDsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in UIDsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GenerateSSCCsForTraceLink(List<string> LstsSSCC)
        {
            try
            {
                if (LstsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in LstsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string getTertiaryLvl()
        {
            return TertiaryLevel;
        }

        #endregion



    }

    public class TracelinkUIDDataHelper
    {
        int UIDQuantity = 0;
        private int MOC, OBX, ISH, PAL, PPB, OSH;
        private int MOCtoPrint, OBXtoPrint, ISHtoPrint, OSHtoPrint, PPBtoPrint, PALtoPrint, SSCCtoPrint;
        private string TertiaryLevel;

        #region SSCC
        int PPBCode = 7;
        int MOCCode = 8;
        int OBXCode = 9;
        int ISHCode = 10;
        int OSHCode = 11;
        int PALCode = 12;
        int SSCCCode = 20;

        private Dictionary<string, string> masterList = new Dictionary<string, string>();
        #endregion

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

        public void CalculateUIDsToGenerate(int TotalUIds, int PAID)
        {
            setProductData(PAID);
            UIDQuantity = TotalUIds;
            if (UIDQuantity == 0) return;
            if (IsLevelExisting(PPB))
            {
                PPBtoPrint = UIDQuantity;

            }

            if (IsLevelExisting(MOC))
            {
                if (!IsLevelExisting(PPB))
                {
                    MOCtoPrint = UIDQuantity;
                    //setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl);
                }
                else
                {
                    double res = (float)PPBtoPrint / (float)MOC;
                    MOCtoPrint = (int)Math.Ceiling(res);
                    //setUIDList("MOC", MOCtoPrint, TID, firstLvl, lastLvl);
                }
            }

            if (IsLevelExisting(OBX))
            {
                if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    OBXtoPrint = UIDQuantity;
                    //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);
                }
                else
                {
                    if (!IsLevelExisting(MOC))
                    {
                        if (IsLevelExisting(PPB))
                        {
                            double res = (float)PPBtoPrint / (float)OBX;
                            OBXtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)MOCtoPrint / (float)OBX;
                        OBXtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("OBX", OBXtoPrint, TID, firstLvl, lastLvl);

                    }

                }
            }


            if (IsLevelExisting(ISH))
            {
                if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    ISHtoPrint = UIDQuantity;
                    //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);

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
                                //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)ISH;
                            ISHtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                        }

                    }
                    else
                    {
                        double res = (float)OBXtoPrint / (float)ISH;
                        ISHtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("ISH", ISHtoPrint, TID, firstLvl, lastLvl);
                    }
                }
            }

            if (IsLevelExisting(OSH))
            {
                if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    OSHtoPrint = UIDQuantity;
                    //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);

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
                                    //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                                }
                            }
                            else
                            {
                                double res = (float)MOCtoPrint / (float)OSH;
                                OSHtoPrint = (int)Math.Ceiling(res);
                                //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)OBXtoPrint / (float)OSH;
                            OSHtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)ISHtoPrint / (float)OSH;
                        OSHtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("OSH", OSHtoPrint, TID, firstLvl, lastLvl);
                    }
                }
            }

            if (IsLevelExisting(PAL))
            {
                if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                {
                    PALtoPrint = UIDQuantity;
                    //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
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
                                        //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                                    }
                                }
                                else
                                {
                                    double res = (float)MOCtoPrint / (float)PAL;
                                    PALtoPrint = (int)Math.Ceiling(res);
                                    //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                                }
                            }
                            else
                            {
                                double res = (float)OBXtoPrint / (float)PAL;
                                PALtoPrint = (int)Math.Ceiling(res);
                                //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                            }
                        }
                        else
                        {
                            double res = (float)ISHtoPrint / (float)PAL;
                            PALtoPrint = (int)Math.Ceiling(res);
                            //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                        }
                    }
                    else
                    {
                        double res = (float)OSHtoPrint / (float)PAL;
                        PALtoPrint = (int)Math.Ceiling(res);
                        //setUIDList("PAL", PALtoPrint, TID, firstLvl, lastLvl);
                    }

                }
            }


        }


        public int getQtyToGenerate(string Lvl)
        {
            if (Lvl == "PAL")
            {
                return PALtoPrint;
            }
            else if (Lvl == "OSH")
            {
                return OSHtoPrint;
            }
            else if (Lvl == "ISH")
            {
                return ISHtoPrint;
            }
            else if (Lvl == "OBX")
            {
                return OBXtoPrint;
            }
            else if (Lvl == "MOC")
            {
                return MOCtoPrint;
            }
            else if (Lvl == "PPB")
            {
                return PPBtoPrint;
            }
            else
            {
                return 0;
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

        public void GenerateMiddleDekSSCCs(int JobId, string selectedJobType, string IAC_CIN, bool IsLoos, string SSCCExt, int CustomerId, int Quantity)
        {
            try
            {
                //   ApplicationDbContext db = new ApplicationDbContext();
                CalculateSSCCToPrint();
                List<string> UIDsSSCC;
                List<string> SSCCIDs;
                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();
               // var loosQuantity = Utilities.getAppSettings("LoosShipper");
                SSCCtoPrint = Quantity;
                //SSCCtoPrint = SSCCtoPrint + Convert.ToInt32(loosQuantity);

                UIDsSSCC = obj.generateSSCCTlink(JobId, SSCCtoPrint, selectedJobType, IAC_CIN, IsLoos, SSCCExt, CustomerId);
                //SSCCIDs = obj.getSSCCTlink(JobId, SSCCtoPrint, CustomerId);
                if (UIDsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in UIDsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GenerateMiddleDekSSCCs(int JobId, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId, int Quantity)
        {
            try
            {
                //   ApplicationDbContext db = new ApplicationDbContext();
                CalculateSSCCToPrint();
                List<string> UIDsSSCC;
                List<string> SSCCIDs;
                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();
                var loosQuantity = Utilities.getAppSettings("LoosShipper");
                SSCCtoPrint = Quantity;
                //SSCCtoPrint = SSCCtoPrint + Convert.ToInt32(loosQuantity);

                UIDsSSCC = obj.generateSSCCTlink(JobId, SSCCtoPrint, selectedJobType, IAC_CIN, LoosExt, SSCCExt, CustomerId);
                //SSCCIDs = obj.getSSCCTlink(JobId, SSCCtoPrint, CustomerId);
                if (UIDsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in UIDsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        //  public void GenerateSSCCs(int JobId, string selectedJobType, string IAC_CIN,int custid,string GTIN)
        public void GenerateSSCCs(int JobId, string selectedJobType, string IAC_CIN, string LoosExt, string SSCCExt, int CustomerId)
        {
            try
            {
                //   ApplicationDbContext db = new ApplicationDbContext();
                CalculateSSCCToPrint();
                List<string> UIDsSSCC;
                List<string> SSCCIDs;
                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();
                var loosQuantity = Utilities.getAppSettings("LoosShipper");

                SSCCtoPrint = SSCCtoPrint + Convert.ToInt32(loosQuantity);


                UIDsSSCC = obj.generateSSCCTlink(JobId, SSCCtoPrint, selectedJobType, IAC_CIN, LoosExt, SSCCExt, CustomerId);
                //SSCCIDs = obj.getSSCCTlink(JobId, SSCCtoPrint, CustomerId);
                if (UIDsSSCC.Count > 0)
                {
                    Dictionary<string, string> ids = new Dictionary<string, string>();
                    foreach (var item in UIDsSSCC)
                    {
                        ids.Add(item, "SSC");
                    }
                    masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<string, string> getMasterData()
        {
            return masterList;
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

        public string getTertiaryLvl()
        {
            return TertiaryLevel;
        }

        public void CalculateLoosSSCC(string firstLvl, string lastLvl, int jobid, string Extension, int CustomerId)
        {
            if (IsLevelExisting(OBX))
            {
                setSSCC("OBX", firstLvl, lastLvl, jobid, Extension, CustomerId);
            }
            if (IsLevelExisting(ISH))
            {
                setSSCC("ISH", firstLvl, lastLvl, jobid, Extension, CustomerId);
            }
            if (IsLevelExisting(OSH))
            {
                setSSCC("OSH", firstLvl, lastLvl, jobid, Extension, CustomerId);
            }
            if (IsLevelExisting(PAL))
            {
                setSSCC("PAL", firstLvl, lastLvl, jobid, Extension, CustomerId);
            }
        }

        public void CalculateLoosSSCCForTLink(string GTIN, int qty)
        {
            if (IsLevelExisting(OBX))
            {
                setTLLSSCC(GTIN, "OBX", qty);
            }

            if (IsLevelExisting(ISH))
            {
                setTLLSSCC(GTIN, "ISH", qty);
            }
            if (IsLevelExisting(OSH))
            {
                setTLLSSCC(GTIN, "OSH", qty);
            }
            if (IsLevelExisting(PAL))
            {
                setTLLSSCC(GTIN, "PAL", qty);
            }

        }

        private void setTLLSSCC(string GTIN, string PackageTypeCode, int qty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var uid = db.X_TracelinkUIDStore.Where(x => x.IsUsed == false && x.Type == "LSSCC" && x.GTIN == GTIN).Take(qty).ToList();
            BulkDataHelper bulkHlpr = new BulkDataHelper();
            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in uid)
            {

                ids.Add(item.SerialNo, PackageTypeCode + "LSSCC");
                bulkHlpr.setFlagToTransferdUID(item.Id);

            }


            masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

        }

        private void setSSCC(string PackageTypeCode, string firstLvl, string lastLvl, int JobID, string Extension, int CustomerId)
        {
            int LastSSCC, sscc;

            string CompanyCode, PlantCode;
            SSCCHelper ssccHelperObj = new SSCCHelper();
            int loosQuantity = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            DataRow dr = SSCCHelper.getCompanyDetails();
            CompanyCode = dr[0].ToString();
            PlantCode = dr[1].ToString();
            if (PackageTypeCode != firstLvl || PackageTypeCode != "MOC")
            {
                if (loosQuantity > 0)
                {
                    sscc = ssccHelperObj.getLastSSCC("Loos", CustomerId, Extension);

                    LastSSCC = ssccHelperObj.getLastSSCCCompLengthWise(CompanyCode, sscc);
                    int newFirstSSCC = LastSSCC + 1;
                    int newLastSSCC = LastSSCC + loosQuantity;

                    if (ssccHelperObj.insertLineHolder(JobID, newFirstSSCC, newLastSSCC, "Loos", Extension, CustomerId))
                    {
                        //SSCC = SSCCGeneration.GenerateSSCC(requestedUidsQty, CompanyCode, PlantCode, LastSSCC);
                        List<string> SSCC = TracelinkService.SSCCGeneration.GenerateSSCC(loosQuantity, CompanyCode, PlantCode, newFirstSSCC, Extension);
                        Dictionary<string, string> ids = new Dictionary<string, string>();

                        foreach (var item in SSCC)
                        {
                            ids.Add(item, PackageTypeCode + "Loos");
                        }
                        masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
                    }

                }

            }

        }
    }


}