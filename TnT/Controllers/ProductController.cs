using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Product;
using TnT.DataLayer.Trailings;
using REDTR.DB.BLL;
using System.Diagnostics;
using System.Resources;
using System.Globalization;
using TnT.DataLayer.Security;
using System.IO;
using TnT.DataLayer.LabelDesigner;
using TnT.DataLayer.ImportService;
using TnT.DataLayer.ProductService;
using System.Web;
using System.Data.OleDb;
using System.ComponentModel;
using System.Reflection;
using TnT.DataLayer.JobService;
using System.Configuration;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ProductController : BaseController
    {
        BatchNotificationHelper batchNotificationHlpr = new BatchNotificationHelper();
        static string baseDir = AppDomain.CurrentDomain.BaseDirectory + "ResourceSet";
        ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("18_Common", baseDir, null);
        CultureInfo cul = CultureInfo.CreateSpecificCulture("de");
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        bool duallabel = Convert.ToBoolean(Utilities.getAppSettings("DualLabel"));
        private List<PackagingAssoDetails> LstPackDetails = new List<PackagingAssoDetails>();
        private List<PackageLabelMaster> LstPackLabelMstr = new List<PackageLabelMaster>();
        private REDTR.HELPER.DbHelper dbhelper = new REDTR.HELPER.DbHelper();
        // GET: Product
        public ActionResult Index()
        {
            try
            {
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewProduct, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewProduct,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                return View(db.PackagingAsso.ToList());
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }


        public ActionResult Verification()
        {
            try
            {

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductVerofiedProduct, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductVerofiedProduct,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                return View(db.PackagingAsso.Where(m => m.VerifyProd == false && m.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        public ActionResult Verify(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == id).OrderBy(x => x.Id).ToList();
                var datapackageLableMaster = db.PackageLabelMaster.Where(m => m.PAID == id).OrderBy(x => x.Id).ToList();

                List<string> availablelevels = new List<string>();


                //bool isPPB, isMOC, isOBX, isISH, isOSH, isPAL ;
                //retrive the level mainained

                foreach (var item in datapackageLableMaster)
                {
                    string code = item.Code;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }

                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }

                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }

                if (packagingAsso == null)
                {
                    return HttpNotFound();
                }
                decimal dose = Convert.ToDecimal(packagingAsso.DosageForm);
                ViewBag.DosageForm1 = db.Dosage.Where(x => x.ID == dose).FirstOrDefault();
                ViewBag.PackagingLevel = availablelevels;
                ViewBag.PackagingAssoDetails = dataPackagingAssoDetails;
                ViewBag.PackagingLabelMaster = datapackageLableMaster;
                //var JobTypeID = (int)datapackageLableMaster.Select(x => x.JobTypeID).FirstOrDefault();
                //ViewBag.JobType = db.JOBTypes.Where(m => m.TID == JobTypeID).FirstOrDefault();
                Bind();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductRequestViewProduct + " " + packagingAsso.Name + " " + TnT.LangResource.GlobalRes.TrailProductforverification, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductRequestViewProduct + " " + packagingAsso.Name + " " + TnT.LangResource.GlobalRes.TrailProductforverification,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                return View("VerifyProduct", packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        public ActionResult NotVerifyProduct(string PAID, string Remark)
        {
            //var job = db.Job.Find(jb.JID);
            REDTR.DB.BusinessObjects.PackagingAsso PA = new REDTR.DB.BusinessObjects.PackagingAsso();
 
            PA=dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(PackagingAssoBLL.PackagingAssoOp.GetProduct, Convert.ToString(PAID));
            var userData = User;
            batchNotificationHlpr.notifyPerimissionHoldersProduct(ref PA, BatchEventType.productNotVerifyEvent, userData);
            TempData["Success"] = PA.Name + "  "+ TnT.LangResource.GlobalRes.TempDataProductNotVerified;
            trail.AddTrail(PA.Name + TnT.LangResource.GlobalRes.TempDataProductNotVerified, Convert.ToInt32(User.ID), PA.Name + TnT.LangResource.GlobalRes.TempDataProductNotVerified, TnT.LangResource.GlobalRes.TrailActionProductActivity);
            return RedirectToAction("Verification");
        }

        public ActionResult VerifyProduct(int id)
        {
            try
            {
                PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                if (ModelState.IsValid)
                {
                    REDTR.DB.BusinessObjects.PackagingAsso PA = new REDTR.DB.BusinessObjects.PackagingAsso();
                    PA.VerifyProd = true;   //verification status         

                    PA.PAID = id;
                    PA.Remarks = db.PackagingAsso.Find(id).Remarks;
                    dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.updateRemark, PA);

                    //ctrlProductPackInfo.InsertOrUpdatePackagingAssDetails(PA.PAID, PackagingAssoDetailsBLL.PckAssoDtlsOp.UpdatePckAssoDtls);

                    //packagingAsso.VerifyProd = true;
                    //db.Entry(packagingAsso).State = EntityState.Modified;
                    //db.SaveChanges();


                    decimal dose = Convert.ToDecimal(packagingAsso.DosageForm);
                    ViewBag.DosageForm = db.Dosage.Where(x => x.ID == dose).FirstOrDefault();
                    REDTR.DB.BusinessObjects.PackagingAsso pkg = new REDTR.DB.BusinessObjects.PackagingAsso();
                    pkg = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(PackagingAssoBLL.PackagingAssoOp.GetProduct,Convert.ToString(id));
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductverifiedproduct + " " + packagingAsso.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductverifiedproduct + " " + packagingAsso.Name,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                    var userData = User;
                    batchNotificationHlpr.notifyPerimissionHoldersProduct(ref pkg,BatchEventType.productVerifyEvent, userData);
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductverifiedsuccessfully;
                    return RedirectToAction("Verification");
                }
                return View(packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }

        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == id).OrderBy(x => x.Id).ToList();
                var datapackageLableMaster = db.PackageLabelMaster.Where(m => m.PAID == id).ToList();

                List<string> availablelevels = new List<string>();


                //bool isPPB, isMOC, isOBX, isISH, isOSH, isPAL ;
                //retrive the level mainained

                foreach (var item in datapackageLableMaster)
                {
                    string code = item.Code;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }

                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }

                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }

                if (packagingAsso == null)
                {
                    return HttpNotFound();
                }
                decimal dose = Convert.ToDecimal(packagingAsso.DosageForm);
                ViewBag.DosageForm = db.Dosage.Where(x => x.ID == dose).FirstOrDefault();
                ViewBag.PackagingLevel = availablelevels;
                ViewBag.PackagingAssoDetails = dataPackagingAssoDetails;
                ViewBag.PackagingLabelMaster = datapackageLableMaster;

                //var JobTypeID = (int)datapackageLableMaster.Select(x => x.JobTypeID).FirstOrDefault(); // Commented to remove compliance dependency at Product
                //ViewBag.JobType = db.JOBTypes.Where(m => m.TID == JobTypeID).FirstOrDefault(); // Commented to remove compliance dependency at Product

                //Bind();


                //if (id == null)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}
                //PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                //if (packagingAsso == null)
                //{
                //    return HttpNotFound();
                //}
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewDetail + " " + packagingAsso.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewDetail + " " + packagingAsso.Name,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                return View(packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        private void Bind()
        {
            try
            {
                ViewBag.DosageForm = db.Dosage;
                ViewBag.Types = db.JOBTypes;
                ViewBag.DateFormats = db.S_DateFormats;
                ViewBag.Provider = db.M_Providers;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]

        public ActionResult GetLabelDesignerFiles()
        {
            try
            {
                LabelDesignHelper lblHelper = new LabelDesignHelper();
                var data = lblHelper.getLabelDesignerFiles();
                
                return Json(data);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult loadFileContent(string FileName)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                LabelDesignHelper lblHelper = new LabelDesignHelper();
                var data = lblHelper.GetFieldSetPreview(FileName);
                return Json(data);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.toastrLblLytDsgNoData);
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            try
            {
                Bind();
                return View();
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }



        private void AddToListOfPAckagingDetails(string[] values, string PackageTypeCode, decimal PAID, int JobTypeCode)
        {

            PackageLabelMaster objLableMaster = new PackageLabelMaster();
            PackagingAssoDetails obj = new PackagingAssoDetails();
            obj.PAID = PAID;
            obj.PackageTypeCode = PackageTypeCode;
            
           

            //For PPN
            //if ((values[0].Length < 13) || (values[0].Length > 14))
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid PPN !  Kindly correct it.");
            //    return;
            //}
            obj.PPN = values[0];


            //For GTIN
            //if ((values[1].Length < 13) || (values[1].Length > 14))
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid GTIN !  Kindly correct it.");
            //    return;
            //}
            obj.GTIN = values[1];
            obj.NTIN = values[2];

            //FOR MRP 
            try
            {
                obj.MRP = Convert.ToDecimal(values[3]);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorProctuctInvalidMRP);
                return;
            }


            // FOR Size
            try
            {
                if (Convert.ToInt32(values[4]) >= 0)
                {
                    obj.Size = Convert.ToInt32(values[4]);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModelErroJobInvalidPCmap);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModelErroJobInvalidPCmap);
            }


            //For Bundle Qty
            try
            {
                if (Convert.ToInt32(values[5]) >= 0)
                {
                    obj.BundleQty = Convert.ToInt32(values[5]);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorJobInvalidBundelQty);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorJobInvalidBundelQty);
            }

            obj.LastUpdatedDate = DateTime.Now;

            LstPackDetails.Add(obj);

            objLableMaster.PAID = PAID;
            objLableMaster.Code = PackageTypeCode;
            objLableMaster.JobTypeID = JobTypeCode;
            objLableMaster.LabelName = values[6];
            objLableMaster.Filter = values[7];
            objLableMaster.LastUpdatedDate = DateTime.Now;

            LstPackLabelMstr.Add(objLableMaster);

        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // removed "Type" parameter to save product as per new Product layout. -- Vikrant, 15-05-2018
        public ActionResult Create([Bind(Include = "PAID,Name,ProductCode,Description,Remarks,CreatedDate,IsActive,LastUpdatedDate,LineCode,SYNC,ScheduledDrug,DoseUsage,GenericName,Composition,ProductImage,DAVAPortalUpload,FGCode,VerifyProd,UseExpDay,ExpDateFormat,PlantCode,SAPProductCode,InternalMaterialCode,CountryDrugCode,DosageForm,FEACN,SubTypeNo,PackageSpec,AuthorizedNo,SubType,SubTypeSpec,PackUnit,ResProdCode,Workshop,ProviderId,SaudiDrugCode,NHRN,PublicKey")] PackagingAsso packagingAsso)
        {
            try
            {
                
                bool IsTemperEvidence =Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
                var isPPB = Request["chkPPB"];
                var isMOC = Request["chkMOC"];
                var isOBX = Request["chkOBX"];
                var isISH = Request["chkISH"];
                var isOSH = Request["chkOSH"];
                var isPAL = Request["chkPAL"];

                if ((isPPB == null) && (isMOC == null) && (isOBX == null) && (isISH == null) && (isOSH == null) && (isPAL == null))
                {
                    Bind();
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorProductSelectPackaginglevels);
                    return View(packagingAsso);
                }

                string PPBValues, MOCValues, OBXValues, ISHValues, OSHValues, PALValues,MOCPartial,OBXPartial,ISHPartial,OSHPartial,PALPartial;
                string[] values;

                if (isPPB == "on")
                {
                    PPBValues = Request["PPB[]"];
                    values = PPBValues.Split(',');
                    // As we are removing product and compliance relation. Replace parameter "packagingAsso.Type" to 0
                    AddToListOfPAckagingDetails(values, "PPB", 0, 0);
                }
                if (isMOC == "on")
                {
                    MOCValues = Request["MOC[]"];
                    values = MOCValues.Split(',');
                    if (duallabel)
                    {
                        MOCPartial = Request["PartialMOC[]"];
                        values[6] = values[6] + "," + MOCPartial;
                    }
                    AddToListOfPAckagingDetails(values, "MOC", 0, 0);
                }
                if (isOBX == "on")
                {
                    OBXValues = Request["OBX[]"];
                    values = OBXValues.Split(',');
                    if (duallabel)
                    {
                        OBXPartial = Request["PartialOBX[]"];
                        values[6] = values[6] + "," + OBXPartial;
                    }
                    AddToListOfPAckagingDetails(values, "OBX", 0, 0);
                }
                if (isISH == "on")
                {
                    ISHValues = Request["ISH[]"];
                    values = ISHValues.Split(',');
                    if (duallabel)
                    {
                        ISHPartial = Request["PartialISH[]"];
                        values[6] = values[6] + "," + ISHPartial;
                    }
                    AddToListOfPAckagingDetails(values, "ISH", 0, 0);
                }
                if (isOSH == "on")
                {
                    OSHValues = Request["OSH[]"];
                    values = OSHValues.Split(',');
                    if (duallabel)
                    {
                        OSHPartial = Request["PartialOSH[]"];
                        values[6] = values[6] + "," + OSHPartial;
                    }
                    AddToListOfPAckagingDetails(values, "OSH", 0, 0);
                }
                if (isPAL == "on")
                {
                    PALValues = Request["PAL[]"];
                    values = PALValues.Split(',');
                    if (duallabel)
                    {
                        PALPartial = Request["PartialPAL[]"];
                        values[6] = values[6] + "," + PALPartial;
                    }
                    AddToListOfPAckagingDetails(values, "PAL", 0, 0);
                }



                //var demo = Convert.ToInt32(Request["txtRate"].ToString());
                packagingAsso.CreatedDate = DateTime.Now;
                packagingAsso.LastUpdatedDate = DateTime.Now;
                packagingAsso.DAVAPortalUpload = false;
                packagingAsso.VerifyProd = false;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    REDTR.DB.BusinessObjects.PackagingAsso PA = new REDTR.DB.BusinessObjects.PackagingAsso();
                    PA.Name = packagingAsso.Name;
                    PA.ProductCode = packagingAsso.ProductCode;
                    PA.FGCode = packagingAsso.FGCode;
                    PA.Description = packagingAsso.Description;
                    PA.IsActive = true;
                    PA.Composition = packagingAsso.Composition;
                    PA.DoseUsage = packagingAsso.DoseUsage;
                    PA.ProductImage = packagingAsso.ProductImage;
                    PA.GenericName = packagingAsso.GenericName;
                    PA.Remarks = packagingAsso.Remarks;
                    PA.ScheduledDrug = packagingAsso.ScheduledDrug;
                    PA.DAVAPortalUpload = false;
                    PA.UseExpDay = packagingAsso.UseExpDay;
                    PA.ExpDateFormat = packagingAsso.ExpDateFormat;
                    PA.VerifyProd = false;
                    PA.InternalMaterialCode = packagingAsso.InternalMaterialCode;
                    PA.CountryDrugCode = packagingAsso.CountryDrugCode;
                    PA.SaudiDrugCode = packagingAsso.SaudiDrugCode;
                    PA.DosageForm = packagingAsso.DosageForm;
                    PA.FEACN = packagingAsso.FEACN;
                    PA.SubType = packagingAsso.SubType;
                    PA.SubTypeNo = packagingAsso.SubTypeNo;
                    PA.PackageSpec = packagingAsso.PackageSpec;
                    PA.AuthorizedNo = packagingAsso.AuthorizedNo;
                    PA.SubTypeSpec = packagingAsso.SubTypeSpec;
                    PA.PackUnit = packagingAsso.PackUnit;
                    PA.ResProdCode = packagingAsso.ResProdCode;
                    PA.Workshop = packagingAsso.Workshop;
                    PA.NHRN = packagingAsso.NHRN;
                   // PA.PublicKey = packagingAsso.PublicKey;
                    //PA.CompType = packagingAsso.CompType;
                    PA.ProviderId =Convert.ToInt32(packagingAsso.ProviderId);
                    PA.PAID = dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.AddPackagingAsso, PA);
                    //SubTypeNo,PackageSpec,AuthorizedNo,SubType,SubTypeSpec,PackUnit,ResProdCode,Workshop
                    //db.PackagingAsso.Add(packagingAsso);
                    //db.SaveChanges();

                    decimal PAID = PA.PAID;

                    foreach (var item in LstPackDetails)
                    {
                        REDTR.DB.BusinessObjects.JOBType Jtype = dbhelper.DBManager.JOBTypeBLL.GetJOBType(JOBTypeBLL.JOBTypeOp.GetJOBTypeByName, "DGFT"); // For Product wise label allocation [Sunil] only for DGFT job type.

                        REDTR.DB.BusinessObjects.PackagingAssoDetails PADtls = new REDTR.DB.BusinessObjects.PackagingAssoDetails();
                        PADtls.PAID = PAID;
                        PADtls.PackageTypeCode = item.PackageTypeCode;
                        PADtls.PPN = item.PPN;
                        PADtls.GTIN = item.GTIN;
                        PADtls.GTINCTI = null;// DGPacks[i, (int)DGPacksEnum.GTINCTIRw].Value.ToString();                        
                        PADtls.MRP = item.MRP;
                        PADtls.Size = item.Size;
                        PADtls.BundleQty = item.BundleQty;
                        PADtls.TerCaseIndex = 0;
                        PADtls.NTIN = item.NTIN;
                        dbhelper.DBManager.PackagingAssoDetailsBLL.InsertOrUpdatePckAssoDtls(PackagingAssoDetailsBLL.PckAssoDtlsOp.AddPckAssoDtls, PADtls);
                    }


                    foreach (var item in LstPackLabelMstr)
                    {

                        REDTR.DB.BusinessObjects.PackageLabelAsso PckLabel = new REDTR.DB.BusinessObjects.PackageLabelAsso();
                        PckLabel.PAID = PAID;
                        PckLabel.Code = item.Code;
                        PckLabel.LabelName = item.LabelName;
                        PckLabel.Filter = item.Filter;
                        PckLabel.JobTypeID = item.JobTypeID;


                        dbhelper.DBManager.PackageLabelBLL.InsertOrUpdatePackagingLabel(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.AddNewPackageLabel), PckLabel);

                        //item.PAID = PAID;
                        //db.PackageLabelMaster.Add(item);
                        //db.SaveChanges();
                    }
                    if (IsTemperEvidence == true)
                    {
                        REDTR.DB.BusinessObjects.ProductApplicatorSetting pro = new REDTR.DB.BusinessObjects.ProductApplicatorSetting();
                        pro.ServerPAID = PAID;
                        pro.S1 = 0;
                        pro.S2 = 0;
                        pro.S3 = 0;
                        pro.S4 = 0;
                        pro.S5 = 0;
                        pro.BackLabelOffset = 0;
                        pro.FrontLabelOffset = 0;
                        pro.CartonLength = 0;
                        dbhelper.DBManager.ProductApplicatorSettingBLL.AddProductApplicatorSetting(pro);
                    }
                    var pkgasso = db.PackagingAsso.Where(x => x.PAID == PAID).FirstOrDefault();
                    PA.CreatedDate = pkgasso.CreatedDate;
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductsavedsuccessfully;
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProducycreatedproduct + " " + packagingAsso.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProducycreatedproduct + " " + packagingAsso.Name,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                    var userData = User;
                    batchNotificationHlpr.notifyPerimissionHoldersProduct(ref PA, BatchEventType.productCreationEvent, userData);

                    return RedirectToAction("Index");
                }

                Bind();
                return View(packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }

        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == id).OrderBy(x => x.Id).ToList();
                var datapackageLableMaster = db.PackageLabelMaster.Where(m => m.PAID == id).ToList();

                List<string> availablelevels = new List<string>();


                //bool isPPB, isMOC, isOBX, isISH, isOSH, isPAL ;
                //retrive the level mainained

                foreach (var item in datapackageLableMaster)
                {
                    string code = item.Code;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }

                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }

                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }

                if (packagingAsso == null)
                {
                    return HttpNotFound();
                }

                ViewBag.DosageForm = db.Dosage;
                ViewBag.PackagingLevel = availablelevels;
                ViewBag.PackagingAssoDetails = dataPackagingAssoDetails;
                ViewBag.PackagingLabelMaster = datapackageLableMaster;
                ViewBag.Provider = db.M_Providers;
                packagingAsso.Type = (int)datapackageLableMaster.Select(x => x.JobTypeID).FirstOrDefault();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductEditProduct + " " + packagingAsso.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductEditProduct + " " + packagingAsso.Name,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                Bind();

                return View("Edit", packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
            //try
            //{
            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            //    PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
            //    if (packagingAsso == null)
            //    {
            //        return HttpNotFound();
            //    }
            //    return View(packagingAsso);
            //}
            //catch (Exception ex)
            //{
            //     return View("HError",ex);

            //}
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PAID,Name,ProductCode,Description,Remarks,CreatedDate,IsActive,LastUpdatedDate,LineCode,SYNC,ScheduledDrug,DoseUsage,GenericName,Composition,ProductImage,DAVAPortalUpload,FGCode,VerifyProd,UseExpDay,ExpDateFormat,PlantCode,SAPProductCode,InternalMaterialCode,CountryDrugCode,DosageForm,SubTypeNo,PackageSpec,AuthorizedNo,SubType,SubTypeSpec,PackUnit,ResProdCode,Workshop,ProviderId,SaudiDrugCode,FEACN,NHRN,PublicKey")] PackagingAsso packagingAsso)
        {
            try
            {
                List<string> compare = new List<string>();
                compare.Add("CreatedDate");
                compare.Add("LastUpdatedDate");
                compare.Add("LineCode");
                compare.Add("SYNC");
                compare.Add("PlantCode");
                compare.Add("DAVAPortalUpload");
                compare.Add("ProductImage");
                compare.Add("VerifyProd");
                compare.Add("SAPProductCode");
                compare.Add("Proivder");
                var oldprod = db.PackagingAsso.Where(x => x.PAID == packagingAsso.PAID).FirstOrDefault();
                System.Reflection.PropertyInfo[] properties = oldprod.GetType().GetProperties();
                string msg = "";
                foreach (var oProperty in properties)
                {
                    if (oProperty.Name != "Proivder")
                    {
                        if (!compare.Contains(oProperty.Name))
                        {
                            var oOldValue = oProperty.GetValue(oldprod, null);
                            var oNewValue = oProperty.GetValue(packagingAsso, null);
                            // this will handle the scenario where either value is null

                            if (!object.Equals(oOldValue, oNewValue))
                            {
                                // Handle the display values when the underlying value is null
                                var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                                var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();

                                //msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " ,";
                                msg += oProperty.Name + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + sOldValue + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + " ,";

                            }
                        }
                    }
                }
                var isPPB = Request["chkPPB"];
                var isMOC = Request["chkMOC"];
                var isOBX = Request["chkOBX"];
                var isISH = Request["chkISH"];
                var isOSH = Request["chkOSH"];
                var isPAL = Request["chkPAL"];

                if ((isPPB == null) && (isMOC == null) && (isOBX == null) && (isISH == null) && (isOSH == null) && (isPAL == null))
                {
                    Bind();
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorProductSelectPackaginglevels);
                    return View(packagingAsso);
                }

                string PPBValues, MOCValues, OBXValues, ISHValues, OSHValues, PALValues, MOCPartial,OBXPartial, ISHPartial, OSHPartial, PALPartial;
                string[] values;

                if (isPPB == "on")
                {
                    PPBValues = Request["PPB[]"];
                    values = PPBValues.Split(',');

                    AddToListOfPAckagingDetails(values, "PPB", 0, 0);
                }
                if (isMOC == "on")
                {
                    MOCValues = Request["MOC[]"];
                    values = MOCValues.Split(',');
                    if (duallabel)
                    {
                        MOCPartial = Request["PartialMOC[]"];
                        values[6] = values[6] + "," + MOCPartial;
                    }
                    AddToListOfPAckagingDetails(values, "MOC", 0, 0);
                }
                if (isOBX == "on")
                {
                    OBXValues = Request["OBX[]"];
                    values = OBXValues.Split(',');
                    if (duallabel)
                    {
                        OBXPartial = Request["PartialOBX[]"];
                        values[6] = values[6] + "," + OBXPartial;
                    }
                    AddToListOfPAckagingDetails(values, "OBX", 0, 0);
                }
                if (isISH == "on")
                {
                    ISHValues = Request["ISH[]"];
                    values = ISHValues.Split(',');
                    if (duallabel)
                    {
                        ISHPartial = Request["PartialISH[]"];
                        values[6] = values[6] + "," + ISHPartial;
                    }
                    AddToListOfPAckagingDetails(values, "ISH", 0, 0);
                }
                if (isOSH == "on")
                {
                    OSHValues = Request["OSH[]"];
                    values = OSHValues.Split(',');
                    if (duallabel)
                    {
                        OSHPartial = Request["PartialOSH[]"];
                        values[6] = values[6] + "," + OSHPartial;
                    }
                    AddToListOfPAckagingDetails(values, "OSH", 0, 0);
                }
                if (isPAL == "on")
                {
                    PALValues = Request["PAL[]"];
                    values = PALValues.Split(',');
                    if (duallabel)
                    {
                        PALPartial = Request["PartialPAL[]"];
                        values[6] = values[6] + "," + PALPartial;
                    }
                    AddToListOfPAckagingDetails(values, "PAL", 0, 0);
                }
                PackagingAsso OldProductDetails = db.PackagingAsso.Find(packagingAsso.PAID);
                //var demo = Convert.ToInt32(Request["txtRate"].ToString());
                packagingAsso.CreatedDate = DateTime.Now;
                packagingAsso.LastUpdatedDate = DateTime.Now;
                packagingAsso.DAVAPortalUpload = false;
                packagingAsso.VerifyProd = false;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    REDTR.DB.BusinessObjects.PackagingAsso PA = new REDTR.DB.BusinessObjects.PackagingAsso();
                    PA.Name = packagingAsso.Name;
                    PA.ProductCode = packagingAsso.ProductCode;
                    PA.FGCode = packagingAsso.FGCode;
                    PA.Description = packagingAsso.Description;
                    PA.IsActive = packagingAsso.IsActive;
                    PA.Composition = packagingAsso.Composition;
                    PA.DoseUsage = packagingAsso.DoseUsage;
                    PA.ProductImage = packagingAsso.ProductImage;
                    PA.GenericName = packagingAsso.GenericName;
                    PA.Remarks = packagingAsso.Remarks;
                    PA.ScheduledDrug = packagingAsso.ScheduledDrug;
                    PA.DAVAPortalUpload = false;
                    PA.UseExpDay = packagingAsso.UseExpDay;
                    PA.ExpDateFormat = packagingAsso.ExpDateFormat;
                    PA.VerifyProd = false;
                    PA.PAID = packagingAsso.PAID;
                    PA.InternalMaterialCode = packagingAsso.InternalMaterialCode;
                    PA.CountryDrugCode = packagingAsso.CountryDrugCode;
                    PA.SaudiDrugCode = packagingAsso.SaudiDrugCode;
                    PA.DosageForm = packagingAsso.DosageForm;
                    PA.FEACN = packagingAsso.FEACN;
                    PA.SubType = packagingAsso.SubType;
                    PA.SubTypeNo = packagingAsso.SubTypeNo;
                    PA.PackageSpec = packagingAsso.PackageSpec;
                    PA.AuthorizedNo = packagingAsso.AuthorizedNo;
                    PA.SubTypeSpec = packagingAsso.SubTypeSpec;
                    PA.PackUnit = packagingAsso.PackUnit;
                    PA.ResProdCode = packagingAsso.ResProdCode;
                    PA.Workshop = packagingAsso.Workshop;
                    PA.ProviderId = Convert.ToInt32(packagingAsso.ProviderId);
                    PA.NHRN = packagingAsso.NHRN;
                    //PA.PublicKey = packagingAsso.PublicKey;
                    //PA.CompType = packagingAsso.CompType;
                    dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.UpdatePackagingAsso, PA);
                    //db.PackagingAsso.Add(packagingAsso);
                    //db.SaveChanges();
                    decimal PAID = PA.PAID;
                    List<REDTR.DB.BusinessObjects.PackagingAssoDetails> getOldPkdtls = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(packagingAsso.PAID);
                    foreach (var item in LstPackDetails)
                    {
                        REDTR.DB.BusinessObjects.JOBType Jtype = dbhelper.DBManager.JOBTypeBLL.GetJOBType(JOBTypeBLL.JOBTypeOp.GetJOBTypeByName, "DGFT"); // For Product wise label allocation [Sunil] only for DGFT job type.

                        REDTR.DB.BusinessObjects.PackagingAssoDetails PADtls = new REDTR.DB.BusinessObjects.PackagingAssoDetails();
                        PADtls.PAID = PAID;
                        PADtls.PackageTypeCode = item.PackageTypeCode;
                        PADtls.PPN = item.PPN;
                        PADtls.GTIN = item.GTIN;
                        PADtls.GTINCTI = null;// DGPacks[i, (int)DGPacksEnum.GTINCTIRw].Value.ToString();
                        PADtls.MRP = item.MRP;
                        PADtls.Size = item.Size;
                        PADtls.BundleQty = item.BundleQty;
                        PADtls.TerCaseIndex = 0;
                        PADtls.NTIN = item.NTIN;

                        REDTR.DB.BusinessObjects.PackagingAssoDetails PADtlsOld = getOldPkdtls.Where(x => x.PackageTypeCode == item.PackageTypeCode).FirstOrDefault();
                        System.Reflection.PropertyInfo[] propertiesProd = PADtlsOld.GetType().GetProperties();
                        foreach (var oProperty in propertiesProd)
                        {
                            if (!compare.Contains(oProperty.Name))
                            {
                                var oOldValue = oProperty.GetValue(PADtlsOld, null);
                                var oNewValue = oProperty.GetValue(PADtls, null);
                                // this will handle the scenario where either value is null

                                if (!object.Equals(oOldValue, oNewValue))
                                {
                                    // Handle the display values when the underlying value is null
                                    var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                                    var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();

                                    // msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " ,";
                                    msg += oProperty.Name + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + sOldValue + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + " ,";
                                }
                            }
                        }



                        dbhelper.DBManager.PackagingAssoDetailsBLL.InsertOrUpdatePckAssoDtls(PackagingAssoDetailsBLL.PckAssoDtlsOp.UpdatePckAssoDtls, PADtls);

                        Trace.TraceInformation("Product Details=>{0},{1},{2},{3},{4}", PADtls.PackageTypeCode, PADtls.Size, PADtls.GTIN, PADtls.MRP, PADtls.TerCaseIndex);


                    }

                    var packlbl = db.PackageLabelMaster.Where(x => x.PAID == packagingAsso.PAID);
                    foreach (var item in LstPackLabelMstr)
                    {

                        REDTR.DB.BusinessObjects.PackageLabelAsso PckLabel = new REDTR.DB.BusinessObjects.PackageLabelAsso();
                        PckLabel.PAID = PAID;
                        PckLabel.Code = item.Code;
                        PckLabel.LabelName = item.LabelName;
                        PckLabel.Filter = item.Filter;
                        PckLabel.JobTypeID = item.JobTypeID;

                        var oldPacklbl = packlbl.Where(x => x.Code == item.Code).FirstOrDefault();
                        if(oldPacklbl.LabelName!= PckLabel.LabelName)
                        {
                            //msg += "Label Name was:" + oldPacklbl.LabelName + " ;is changed to :" + PckLabel.LabelName + ",";
                            msg += TnT.LangResource.GlobalRes.ProductLabelName + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + oldPacklbl.LabelName + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + PckLabel.LabelName + ",";
                        }

                        if(oldPacklbl.Filter!=PckLabel.Filter)
                        {
                            // msg += "Filter was:" + oldPacklbl.Filter + "; is changed to :" + PckLabel.Filter + ",";
                            msg += TnT.LangResource.GlobalRes.RptAuditTrailFilter + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + oldPacklbl.Filter + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + PckLabel.Filter + ",";

                            
                        }

                        dbhelper.DBManager.PackageLabelBLL.InsertOrUpdatePackagingLabel(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.AddNewPackageLabel), PckLabel);

                    }

                    msg = msg.TrimEnd(',');
                    if(msg=="")
                    {
                        msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductUpdatedproduct;
                    }
                    else
                    {
                        msg += TnT.LangResource.GlobalRes.RptAuditTrailUsersFor + " :";
                    }
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductUpdateSuccessfully;
                    //trail.AddTrail(User.FirstName + " Updated a product as " + packagingAsso.Name + ". Old Name was " + OldProductDetails.Name, Convert.ToInt32(User.ID));
                    trail.AddTrail(msg+ " "+packagingAsso.Name, Convert.ToInt32(User.ID),msg,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                    return RedirectToAction("Index");
                }

                Bind();
                return View(packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }

        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PackagingAsso packagingAsso = db.PackagingAsso.Find(id);
                if (packagingAsso == null)
                {
                    return HttpNotFound();
                }
                return View(packagingAsso);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                List<REDTR.DB.BusinessObjects.Job> JobLst = dbhelper.DBManager.JobBLL.GetJobs(REDTR.DB.BLL.JobBLL.JobOp.GetJobsOfProducts, -1, id.ToString(), null);
                if (JobLst.Count > 0)
                {
                    var mdl = db.PackagingAsso.Find(id);
                    ModelState.AddModelError(string.Empty, rm.GetString("FrmProductPackMaster.Msg_Prod5", cul));
                    //MessageBoxEx.Show(rm.GetString("FrmProductPackMaster.Msg_Prod5", cul), APPNAME, MessageBoxEx.MessageBoxButtonsEx.OK, 0);
                    return View(mdl);
                }
                else
                {
                    dbhelper.DBManager.PackagingAssoBLL.RemovePackagingAsso(id);
                    //ViewBag.DeleteSuccess =  rm.GetString("FrmProductPackMaster.Msg_SuccDelete", cul);
                    //trail
                    //return  View("Index");
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductDeleted;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        public ActionResult IsProductExisting(string Name, string FGCode, string ProductCode)
        {
            try
            {
                var data = db.PackagingAsso.Where(x => x.Name == Name && x.FGCode == FGCode && x.ProductCode == ProductCode).FirstOrDefault();
                if (data != null)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region ImportProducts

        public ActionResult Import()
        {
            ViewBag.MyList = null;
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewImport, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProductViewImport,TnT.LangResource.GlobalRes.TrailActionProductActivity);
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Import(FormCollection frm)
        //{
        //    try
        //    {
        //        int imprtCnt = 0, failCnt = 0;
        //        string repeatedProds = "";
        //        var httpRequest = System.Web.HttpContext.Current.Request;
        //        if (httpRequest.Files.Count > 0)
        //        {

        //            var docfiles = new List<string>();
        //            foreach (string file in httpRequest.Files)
        //            {
        //                string filePath = "";
        //                var postedFile = httpRequest.Files[file];
        //                FileInfo fi = new FileInfo(postedFile.FileName);
        //                if (fi.Extension == ".xlsx" || fi.Extension == ".xls")
        //                {
        //                    string fNm = postedFile.FileName;
        //                    string tempPath = Server.MapPath("~/Content/ImportedProducts/");
        //                    if (!System.IO.Directory.Exists(tempPath))
        //                    {
        //                        System.IO.Directory.CreateDirectory(tempPath);
        //                    }
        //                    filePath = Server.MapPath("~/Content/ImportedProducts/" + fNm);
        //                    postedFile.SaveAs(filePath);
        //                    ProductImporter imptr = new DataLayer.ImportService.ProductImporter();
        //                    var prds = imptr.getProductsToUpload(filePath);
        //                    var err = imptr.getFailureDetails();
        //                    var errDtls = imptr.getFailureReason();
        //                    System.IO.File.Delete(filePath);
        //                    //docfiles.Add(filePath);
        //                    if (err == FileErrors.none && errDtls.Count == 0)
        //                    {
        //                        if (prds.Count > 0)
        //                        {
        //                            foreach (var pro in prds)
        //                            {

        //                                ProductHelper phlp = new ProductHelper();
        //                                if (!phlp.IsProductExisting(pro.product.Name, pro.product.FGCode, pro.product.ProductCode))
        //                                {
        //                                    ImportedProductDBHelper db = new ImportedProductDBHelper();
        //                                    db.saveProduct(pro);
        //                                    imprtCnt++;
        //                                }
        //                                else
        //                                {
        //                                    repeatedProds += pro.product.Name + " " + TnT.LangResource.GlobalRes.TrailProductImportAlreadyExist;
        //                                    failCnt++;
        //                                }
        //                            }
        //                            if (imprtCnt == prds.Count)
        //                            {
        //                                TempData["Success"] = imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully;
        //                                trail.AddTrail(imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully, Convert.ToInt32(User.ID));
        //                            }
        //                            else
        //                            {
        //                                TempData["Success"] = imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedoutof + " " + prds.Count + ".";
        //                                trail.AddTrail(imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully, Convert.ToInt32(User.ID));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport;
        //                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport, Convert.ToInt32(User.ID));
        //                        }
        //                        //var prds = imptr.

        //                    }
        //                    else
        //                    {
        //                        string errs = "";

        //                        switch (err)
        //                        {
        //                            case FileErrors.InvalidGTIN:
        //                                errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidGTIN;
        //                                break;
        //                            case FileErrors.InvalidColumns:
        //                                errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidcolumns;
        //                                break;
        //                            case FileErrors.ExistingProduct:
        //                                errs = TnT.LangResource.GlobalRes.MsgErrorProductalreadyexists;
        //                                break;
        //                            case FileErrors.ProductDetailsNotProvided:
        //                                errs = TnT.LangResource.GlobalRes.MsgErrorProductdetailsnotprovided;
        //                                break;
        //                            case FileErrors.InvalidProductData:
        //                                errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidProduct;
        //                                break;
        //                            case FileErrors.none:
        //                                break;
        //                            default:
        //                                break;
        //                        }


        //                        foreach (var er in errDtls)
        //                        {
        //                            errs += "\\r\\n";
        //                            errs += er + "\\r\\n";
        //                        }
        //                        TempData["Success"] = errs;
        //                        string msg = errs.Replace("\\r\\n", "   ");
        //                        trail.AddTrail(msg, Convert.ToInt32(User.ID));
        //                    }

        //                }
        //                else
        //                {
        //                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductInvalidfileformat;
        //                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataProductInvalidfileformat, Convert.ToInt32(User.ID));
        //                }

        //            }

        //        }
        //        else
        //        {
        //            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataNofilesfound;
        //            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataNofilesfound, Convert.ToInt32(User.ID));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Success"] = TnT.LangResource.GlobalRes.TempdataProductError + " " + ex.Message;
        //        //return View();

        //        return View("HError", ex);
        //    }
        //    return View();

        //}


        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(FormCollection frm)
        {

            var httpRequest = System.Web.HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {

                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    string filePath = "";
                    var postedFile = httpRequest.Files[file];
                    FileInfo fi = new FileInfo(postedFile.FileName);
                    if (fi.Extension == ".xlsx" || fi.Extension == ".xls")
                    {
                        string fNm = postedFile.FileName;
                        string tempPath = Server.MapPath("~/Content/ImportedProducts/");
                        if (!System.IO.Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }
                        filePath = Server.MapPath("~/Content/ImportedProducts/" + fNm);
                        DataTable DtProds = new DataTable();
                        postedFile.SaveAs(filePath);
                        ProductImporter imptr = new DataLayer.ImportService.ProductImporter();
                        //var prds = imptr.getProductsToUpload(filePath);
                        //ViewBag.MyList = prds;


                        string myexceldataquery = "select * from [Sheet1$]";
                        //create our connection strings
                        string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
                        OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                        OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                        oledbconn.Open();
                        OleDbDataReader dr = oledbcmd.ExecuteReader();
                        DtProds.Load(dr);
                        DtProds.Columns.Add("Present", typeof(string));
                        for (int i = 0; i < DtProds.Rows.Count; i++)
                        {
                            string name = DtProds.Rows[i][0].ToString().TrimStart();
                            string pname = name.TrimEnd();
                            string prodcode = DtProds.Rows[i][1].ToString().Trim();
                            string FGcode = DtProds.Rows[i][8].ToString().Trim();
                            var prod = db.PackagingAsso.Where(x => x.Name == name && x.FGCode == FGcode && x.ProductCode == prodcode).FirstOrDefault();
                            if (prod != null)
                            {
                                DtProds.Rows[i][50] = "Exist";
                            }
                            else
                            {
                                DtProds.Rows[i][50] = "NotExist";
                            }
                        }

                        oledbconn.Close();
                        ViewData.Model = DtProds.AsEnumerable();
                        System.IO.File.Delete(filePath);
                        return View();
                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductInvalidfileformat;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataProductInvalidfileformat, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataProductInvalidfileformat,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                    }
                }
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataNofilesfound;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataNofilesfound, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataNofilesfound,TnT.LangResource.GlobalRes.TrailActionProductActivity);

            }
            return View();
        }

        public ActionResult GetData(List<ProductImportViewModel> itemlist)
        {
            int imprtCnt = 0, failCnt = 0;
            string repeatedProds = "";
            
            DataTable dt = ListToDataTable(itemlist);
            ProductImporter imptr = new DataLayer.ImportService.ProductImporter();
            var prds = imptr.getProductsToUpload(dt);
            var err = imptr.getFailureDetails();
            var errDtls = imptr.getFailureReason();
            if (err == FileErrors.none && errDtls.Count == 0)
            {
                if (prds.Count > 0)
                {
                    foreach (var pro in prds)
                    {

                        ProductHelper phlp = new ProductHelper();
                        if (!phlp.IsProductExisting(pro.product.Name, pro.product.FGCode, pro.product.ProductCode))
                        {
                            ImportedProductDBHelper db = new ImportedProductDBHelper();
                            db.saveProduct(pro);
                            imprtCnt++;
                        }
                        else
                        {
                            repeatedProds += pro.product.Name + " " + TnT.LangResource.GlobalRes.TrailProductImportAlreadyExist;
                            failCnt++;
                        }
                    }
                    if (imprtCnt == prds.Count)
                    {
                      //  TempData["Success"] = imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully;
                        trail.AddTrail(imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully, Convert.ToInt32(User.ID), imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                        return Json(TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully);
                    }
                    else
                    {
                       // TempData["Success"] = imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedoutof + " " + prds.Count + ".";
                        trail.AddTrail(imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully, Convert.ToInt32(User.ID), imprtCnt + " " + TnT.LangResource.GlobalRes.TempDataProductimportedSuccessfully,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                        return Json(TnT.LangResource.GlobalRes.TempDataProductimportedoutof);
                    }
                }
                else
                {
                  //  TempData["Success"] = TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                    return Json(TnT.LangResource.GlobalRes.TempDataProductNotavailabletoimport);
                }
            }
            else
            {
                string errs = "";

                switch (err)
                {
                    case FileErrors.InvalidGTIN:
                        errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidGTIN;
                        break;
                    case FileErrors.InvalidColumns:
                        errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidcolumns;
                        break;
                    case FileErrors.ExistingProduct:
                        errs = TnT.LangResource.GlobalRes.MsgErrorProductalreadyexists;
                        break;
                    case FileErrors.ProductDetailsNotProvided:
                        errs = TnT.LangResource.GlobalRes.MsgErrorProductdetailsnotprovided;
                        break;
                    case FileErrors.InvalidProductData:
                        errs = TnT.LangResource.GlobalRes.MsgErrorProductInvalidProduct;
                        break;
                    case FileErrors.none:
                        break;
                    default:
                        break;
                }


                foreach (var er in errDtls)
                {
                    errs += "\\r\\n";
                    errs += er + "\\r\\n";
                }
               // TempData["Success"] = errs;
                string msg = errs.Replace("\\r\\n", "   ");
                
                trail.AddTrail(msg, Convert.ToInt32(User.ID),msg,TnT.LangResource.GlobalRes.TrailActionProductActivity);
                return Json(errs);
            }
            return View();
        }

        public static DataTable ListToDataTable<ProductImportViewModel>(IList<ProductImportViewModel> thisList)
        {
            DataTable dt = new DataTable();
            if (typeof(ProductImportViewModel).IsValueType || typeof(ProductImportViewModel).Equals(typeof(string)))
            {
                DataColumn dc = new DataColumn("CountryList");
                dt.Columns.Add(dc);

                foreach (ProductImportViewModel item in thisList)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                PropertyInfo[] propertyInfo = typeof(ProductImportViewModel).GetProperties();
                foreach (PropertyInfo pi in propertyInfo)
                {
                    DataColumn dc = new DataColumn(pi.Name, pi.PropertyType);
                    dt.Columns.Add(dc);
                }

                for (int item = 0; item < thisList.Count(); item++)
                {
                    DataRow dr = dt.NewRow();
                    for (int property = 0; property < propertyInfo.Length; property++)
                    {
                        dr[property] = propertyInfo[property].GetValue(thisList[item], null);
                    }
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;

        }

    }
}
