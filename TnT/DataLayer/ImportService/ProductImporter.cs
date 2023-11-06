using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using TnT.DataLayer.ProductService;
using TnT.Models;

namespace TnT.DataLayer.ImportService
{

    public enum FileErrors
    {
        InvalidGTIN,
        InvalidColumns,
        ExistingProduct,
        ProductDetailsNotProvided,
        InvalidProductData,
        none
    }
    public class ProductImporter
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ExcelColumnHelper hlpr = new ExcelColumnHelper();


        FileErrors errDtls = FileErrors.none;
        private DataTable DtProducts;
        List<string> errors = new List<string>();

        private bool IsFileValid(DataTable dt)
        {
            DataColumnCollection columns = dt.Columns;
            // 32 columns in excel sheet 

            if (columns.Count > 56)
            {
                return false;
            }
            else
            {
                if ((!columns.Contains(hlpr.exlProdName)) && (!columns.Contains(hlpr.exlProdCode)) && (!columns.Contains(hlpr.exlDescription)) &&
                    (!columns.Contains(hlpr.exlRemark)) && (!columns.Contains(hlpr.exlIsSceduledDrug))
                    && (!columns.Contains(hlpr.exlDoseUsage)) && (!columns.Contains(hlpr.exlGenericName)) && (!columns.Contains(hlpr.exlComposition))
                    && (!columns.Contains(hlpr.exlFGCode)) && (!columns.Contains(hlpr.exlUseExpDay)) && (!columns.Contains(hlpr.exlExpDateFormat)) && (!columns.Contains(hlpr.exlPlantCode)) && (!columns.Contains(hlpr.exlSAPProductCode)) && (!columns.Contains(hlpr.exlInternalMaterialCode))
                    && (!columns.Contains(hlpr.exlCountryDrugCode)) && (!columns.Contains(hlpr.exlManufacturer)) && (!columns.Contains(hlpr.exlDosageForm)) && (!columns.Contains(hlpr.exlStrength)) && (!columns.Contains(hlpr.exlContainerSize))
                    && (!columns.Contains(hlpr.exlFEACN)) && (!columns.Contains(hlpr.exlSubTypeNo)) && (!columns.Contains(hlpr.exlPackageSpec)) && (!columns.Contains(hlpr.exlAuthorizedNo)) && (!columns.Contains(hlpr.exlSubType)) && (!columns.Contains(hlpr.exlSubTypeSpec)) && (!columns.Contains(hlpr.exlPackUnit))
                    && (!columns.Contains(hlpr.exlResProdCode)) && (!columns.Contains(hlpr.exlWorkshop)) && (!columns.Contains(hlpr.exlProviderId)) && (!columns.Contains(hlpr.exlSaudiDrugCode)) && (!columns.Contains(hlpr.exlNHRN)) && (!columns.Contains(hlpr.exlMOCPPN)) &&
                    (!columns.Contains(hlpr.exlMOCGTIN)) && (!columns.Contains(hlpr.exlMOCNTIN)) && (!columns.Contains(hlpr.exlMOCSize)) && (!columns.Contains(hlpr.exlMOCBundleQty)) && (!columns.Contains(hlpr.exlOBXPPN)) && (!columns.Contains(hlpr.exlOBXGTIN)) && (!columns.Contains(hlpr.exlOBXNTIN)) &&
                    (!columns.Contains(hlpr.exlOBXSize)) && (!columns.Contains(hlpr.exlOBXBundleQty)) && (!columns.Contains(hlpr.exlISHPPN)) && (!columns.Contains(hlpr.exlISHGTIN)) && (!columns.Contains(hlpr.exlOSHNTIN)) && (!columns.Contains(hlpr.exlISHSize)) &&
                    (!columns.Contains(hlpr.exlISHBundleQty)) && (!columns.Contains(hlpr.exlOSHPPN)) && (!columns.Contains(hlpr.exlOSHGTIN)) && (!columns.Contains(hlpr.exlOSHNTIN)) && (!columns.Contains(hlpr.exlOSHSize)) && (!columns.Contains(hlpr.exlOSHBundleQty)) && (!columns.Contains(hlpr.exlPALPPN)) &&
                    (!columns.Contains(hlpr.exlPALGTIN)) && (!columns.Contains(hlpr.exlPALNTIN)) && (!columns.Contains(hlpr.exlPALSize)) && (!columns.Contains(hlpr.exlPALBundleQty)))
                {
                    return false;
                }
            }

            return true;
        }

        private DataTable getProductsDataset(string exlfileLocation)
        {
            try
            {
                DataTable DtProds = new DataTable();
                string myexceldataquery = "select * from [Sheet1$]";
                //create our connection strings
                string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exlfileLocation + ";Extended Properties=Excel 12.0;";
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                DtProds.Load(dr);
                oledbconn.Close();
                System.IO.File.Delete(exlfileLocation);
                return DtProds;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public FileErrors getFailureDetails()
        {
            return errDtls;
        }
        public List<string> getFailureReason()
        {
            return errors;
        }

        public List<ProdData> getProductsToUpload(DataTable dt)
        {
            List<PackagingAsso> lstProductsToSave = new List<PackagingAsso>();
            List<ProdData> productsData = new List<ProdData>();
            // var dt = getProductsDataset(exlfileLocation);
            if (IsFileValid(dt))
            {



                foreach (DataRow prod in dt.Rows)
                {
                    string ProductName = prod[hlpr.exlProdName].ToString();
                    string ProductCode = prod[hlpr.exlProdCode].ToString();
                    string FGCode = prod[hlpr.exlFGCode].ToString();

                    //check product name and fg code exist
                    if (string.IsNullOrEmpty(ProductName) || string.IsNullOrEmpty(ProductCode) || string.IsNullOrEmpty(FGCode))
                    {
                        errDtls = FileErrors.ProductDetailsNotProvided;
                    }
                    else
                    {
                        ProductHelper phlp = new ProductHelper();
                        if (!phlp.IsProductExisting(ProductName, FGCode, ProductCode))
                        {
                            ProdData pp = new ProdData();
                            ImportedProductHelper hlpr = new ImportedProductHelper(prod);

                            var data = hlpr.setProductData();
                            if (data != null)
                            {
                                if (data.product != null)
                                {
                                    if (data.productDetails.Count() == data.productLabels.Count())
                                    {
                                        productsData.Add(data);
                                    }
                                    else
                                    {
                                        errors.Add(TnT.LangResource.GlobalRes.ErrorMsgProductImporterInvalidProduct + " " + ProductName);
                                    }
                                }
                            }
                            else
                            {
                                errDtls = FileErrors.InvalidGTIN;
                            }
                        }
                        else
                        {
                            errors.Add(TnT.LangResource.GlobalRes.ErrorMsgProductImporterProductAlreadyExist + " " + ProductName + ".");
                            errDtls = FileErrors.ExistingProduct;
                        }
                    }
                }

            }
            else
            {
                errDtls = FileErrors.InvalidColumns;

            }
            return productsData;

        }
    }

    public class ProdData
    {
        public PackagingAsso product { get; set; }

        public List<PackagingAssoDetails> productDetails { get; set; }

        public List<PackageLabelAsso> productLabels { get; set; }
    }

    class ImportedProductHelper
    {

        FileErrors errDtls;
        DataRow productData;
        List<string> availableDecks = new List<string>();
        List<string> invalidDecks = new List<string>();
        ExcelColumnHelper hlpr = new ExcelColumnHelper();
        public ImportedProductHelper(DataRow data)
        {
            productData = data;
            getDecks();
        }

        public ProdData setProductData()
        {
            if (invalidDecks.Count > 0)
            {
                errDtls = FileErrors.InvalidGTIN;
                return null;
            }
            ProdData data = new ProdData();
            data.product = setPackagingAsso();
            data.productDetails = setPackagingAssoDetails();
            data.productLabels = setPackageLabelMaster();
            return data;
        }

        private void getDecks()
        {
            int lnth = 14;

            if (!string.IsNullOrEmpty(productData[hlpr.exlMOCGTIN].ToString()))
            {
                string[] MOC = productData[hlpr.exlMOCGTIN].ToString().Split('\n');
                if (MOC[0].Length == lnth)
                {
                    availableDecks.Add("MOC");
                }
                else
                {
                    invalidDecks.Add("MOC");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(productData[hlpr.exlMOCPPN].ToString()))
                {
                    string[] MOC = productData[hlpr.exlMOCPPN].ToString().Split('\n');
                    if (MOC[0].Length == lnth)
                    {
                        availableDecks.Add("MOC");
                    }
                    else
                    {
                        invalidDecks.Add("MOC");
                    }
                }
            }
            if (!string.IsNullOrEmpty(productData[hlpr.exlOBXGTIN].ToString()))
            {
                string[] OBX = productData[hlpr.exlOBXGTIN].ToString().Split('\n');
                if (OBX[0].Length == lnth)
                {
                    availableDecks.Add("OBX");
                }
                else
                {
                    invalidDecks.Add("OBX");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(productData[hlpr.exlOBXPPN].ToString()))
                {
                    string[] OBX = productData[hlpr.exlOBXPPN].ToString().Split('\n');
                    if (OBX[0].Length == lnth)
                    {
                        availableDecks.Add("OBX");
                    }
                    else
                    {
                        invalidDecks.Add("OBX");
                    }
                }
            }
            if (!string.IsNullOrEmpty(productData[hlpr.exlISHGTIN].ToString()))
            {
                string[] ISH = productData[hlpr.exlISHGTIN].ToString().Split('\n');
                if (ISH[0].Length == lnth)
                {
                    availableDecks.Add("ISH");
                }
                else
                {
                    invalidDecks.Add("ISH");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(productData[hlpr.exlISHPPN].ToString()))
                {
                    string[] ISH = productData[hlpr.exlISHPPN].ToString().Split('\n');
                    if (ISH[0].Length == lnth)
                    {
                        availableDecks.Add("ISH");
                    }
                    else
                    {
                        invalidDecks.Add("ISH");
                    }
                }
            }
            if (!string.IsNullOrEmpty(productData[hlpr.exlOSHGTIN].ToString()))
            {
                string[] OSH = productData[hlpr.exlOSHGTIN].ToString().Split('\n');
                if (OSH[0].Length == lnth)
                {
                    availableDecks.Add("OSH");
                }
                else
                {
                    invalidDecks.Add("OSH");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(productData[hlpr.exlOSHPPN].ToString()))
                {
                    string[] OSH = productData[hlpr.exlOSHPPN].ToString().Split('\n');
                    if (OSH[0].Length == lnth)
                    {
                        availableDecks.Add("OSH");
                    }
                    else
                    {
                        invalidDecks.Add("OSH");
                    }
                }
            }

            if (!string.IsNullOrEmpty(productData[hlpr.exlPALGTIN].ToString()))
            {
                string[] PAL = productData[hlpr.exlPALGTIN].ToString().Split('\n');
                if (PAL[0].Length == lnth)
                {
                    availableDecks.Add("PAL");
                }
                else
                {
                    invalidDecks.Add("PAL");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(productData[hlpr.exlPALPPN].ToString()))
                {
                    string[] PAL = productData[hlpr.exlPALPPN].ToString().Split('\n');
                    if (PAL[0].Length == lnth)
                    {
                        availableDecks.Add("PAL");
                    }
                    else
                    {
                        invalidDecks.Add("PAL");
                    }
                }
            }
        }
        private PackagingAsso setPackagingAsso()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            PackagingAsso PA = new PackagingAsso();
            PA.Name = productData[hlpr.exlProdName].ToString();
            PA.ProductCode = productData[hlpr.exlProdCode].ToString();
            PA.FGCode = productData[hlpr.exlFGCode].ToString();
            PA.Description = productData[hlpr.exlDescription].ToString();
            PA.Composition = productData[hlpr.exlComposition].ToString();
            PA.DoseUsage = productData[hlpr.exlDoseUsage].ToString();
            PA.GenericName = productData[hlpr.exlGenericName].ToString();
            PA.Remarks = productData[hlpr.exlRemark].ToString();
            PA.ScheduledDrug = Convert.ToBoolean((productData[hlpr.exlIsSceduledDrug].ToString() == "1"));
            PA.UseExpDay = Convert.ToBoolean((productData[hlpr.exlUseExpDay].ToString() == "1"));
            PA.ExpDateFormat = productData[hlpr.exlExpDateFormat].ToString();
            PA.InternalMaterialCode = productData[hlpr.exlInternalMaterialCode].ToString();
            PA.CountryDrugCode = productData[hlpr.exlCountryDrugCode].ToString();
            PA.PlantCode = productData[hlpr.exlPlantCode].ToString();
            PA.SAPProductCode = productData[hlpr.exlSAPProductCode].ToString();
            PA.Manufacturer = productData[hlpr.exlManufacturer].ToString();
            string DosageForm = productData[hlpr.exlDosageForm].ToString();
            var dos = db.Dosage.Where(x => x.DosageName == DosageForm).FirstOrDefault();
            if(dos!=null)
            {
                PA.DosageForm =Convert.ToString(dos.ID);
            }
            PA.Strength= productData[hlpr.exlStrength].ToString();
            PA.ContainerSize=productData[hlpr.exlContainerSize].ToString();
            PA.FEACN= productData[hlpr.exlFEACN].ToString();
            PA.SubTypeNo= productData[hlpr.exlSubTypeNo].ToString();
            PA.PackageSpec= productData[hlpr.exlPackageSpec].ToString();
            PA.AuthorizedNo= productData[hlpr.exlAuthorizedNo].ToString();
            PA.SubType= productData[hlpr.exlSubType].ToString();
            PA.SubTypeSpec= productData[hlpr.exlSubTypeSpec].ToString();
            PA.PackUnit = productData[hlpr.exlPackUnit].ToString();
            PA.ResProdCode= productData[hlpr.exlResProdCode].ToString();
            PA.Workshop= productData[hlpr.exlWorkshop].ToString();
            string pname= productData[hlpr.exlProviderId].ToString();
            var pd = db.M_Providers.Where(x => x.Name == pname).FirstOrDefault();
            if(pd!=null)
            {
                PA.ProviderId = Convert.ToInt32(pd.Id);
            }
            PA.SaudiDrugCode= productData[hlpr.exlSaudiDrugCode].ToString();

            //PA.PAID = dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.AddPackagingAsso, PA);
            return PA;

        }
        private List<PackagingAssoDetails> setPackagingAssoDetails()
        {
            List<PackagingAssoDetails> productDetails = new List<PackagingAssoDetails>();
            foreach (var deck in availableDecks)
            {
                PackagingAssoDetails pad = new PackagingAssoDetails();
                pad.PackageTypeCode = deck;

                if (deck == "MOC")
                {
                    pad.GTIN = productData[hlpr.exlMOCGTIN].ToString();
                    pad.Size = Convert.ToInt32(productData[hlpr.exlMOCSize].ToString());
                    pad.BundleQty = Convert.ToInt32(productData[hlpr.exlMOCBundleQty].ToString());
                }
                else if (deck == "OBX")
                {
                    pad.GTIN = productData[hlpr.exlOBXGTIN].ToString();
                    pad.Size = Convert.ToInt32(productData[hlpr.exlOBXSize].ToString());
                    pad.BundleQty = Convert.ToInt32(productData[hlpr.exlOBXBundleQty].ToString());
                }
                else if (deck == "ISH")
                {
                    pad.GTIN = productData[hlpr.exlISHGTIN].ToString();
                    pad.Size = Convert.ToInt32(productData[hlpr.exlISHSize].ToString());
                    pad.BundleQty = Convert.ToInt32(productData[hlpr.exlISHBundleQty].ToString());
                }
                else if (deck == "OSH")
                {
                    pad.GTIN = productData[hlpr.exlOSHGTIN].ToString();
                    pad.Size = Convert.ToInt32(productData[hlpr.exlOSHSize].ToString());
                    pad.BundleQty = Convert.ToInt32(productData[hlpr.exlOSHBundleQty].ToString());
                }
                else if (deck == "PAL")
                {
                    pad.GTIN = productData[hlpr.exlPALGTIN].ToString();
                    pad.Size = Convert.ToInt32(productData[hlpr.exlPALSize].ToString());
                    pad.BundleQty = Convert.ToInt32(productData[hlpr.exlPALBundleQty].ToString());
                }
                productDetails.Add(pad);
            }
            return productDetails;
        }
        private List<PackageLabelAsso> setPackageLabelMaster()
        {
            List<PackageLabelAsso> productLabels = new List<PackageLabelAsso>();

            foreach (var deck in availableDecks)
            {
                PackageLabelAsso pad = new PackageLabelAsso();
                pad.Code = deck;

                pad.JobTypeID = 4;
                pad.LabelName = deck + ".job";
                pad.Filter = "GS12D1-01-17-10-21|GTIN|EXP|LOT|UID";
                pad.LastUpdatedDate = DateTime.Now;
                productLabels.Add(pad);
            }

            return productLabels;
        }

    }

    class ImportedProductDBHelper
    {
        private REDTR.HELPER.DbHelper dbhelper = new REDTR.HELPER.DbHelper();
        public void saveProduct(ProdData productData)
        {
            PackagingAsso PA = new PackagingAsso();
            PA.Name = productData.product.Name;
            PA.ProductCode = productData.product.ProductCode;
            PA.FGCode = productData.product.FGCode;
            PA.Description = productData.product.Description;
            PA.IsActive = true;
            PA.Composition = productData.product.Composition;
            PA.DoseUsage = productData.product.DoseUsage;
            PA.ProductImage = productData.product.ProductImage;
            PA.GenericName = productData.product.GenericName;
            PA.Remarks = productData.product.Remarks;
            PA.ScheduledDrug = productData.product.ScheduledDrug;
            PA.DAVAPortalUpload = false;
            PA.UseExpDay = productData.product.UseExpDay;
            PA.ExpDateFormat = productData.product.ExpDateFormat;
            PA.VerifyProd = false;
            PA.InternalMaterialCode = productData.product.InternalMaterialCode;
            PA.CountryDrugCode = productData.product.CountryDrugCode;
            PA.CreatedDate = DateTime.Now;
            PA.LastUpdatedDate = DateTime.Now;
            PA.PlantCode = productData.product.PlantCode;
            PA.SYNC = false;
            PA.ProviderId = productData.product.ProviderId;
            PA.SAPProductCode = productData.product.SAPProductCode;
            PA.Manufacturer = productData.product.Manufacturer;
            PA.DosageForm = productData.product.DosageForm;
            PA.Strength = productData.product.Strength;
            PA.ContainerSize = productData.product.ContainerSize;
            PA.FEACN = productData.product.FEACN;
            PA.SubTypeNo = productData.product.SubTypeNo;
            PA.PackageSpec = productData.product.PackageSpec;
            PA.AuthorizedNo = productData.product.AuthorizedNo;
            PA.SubType = productData.product.SubType;
            PA.SubTypeSpec = productData.product.SubTypeSpec;
            PA.PackUnit = productData.product.PackUnit;
            PA.ResProdCode = productData.product.ResProdCode;
            PA.Workshop = productData.product.Workshop;
            PA.SaudiDrugCode = productData.product.SaudiDrugCode;
            PA.NHRN = productData.product.NHRN;
            PA.PAID = dbhelper.DBManager.PackagingAssoBLL.InsertOrUpdatePackagingAsso(PackagingAssoBLL.PackagingAssoOp.AddPackagingAsso, PA);
         

            decimal PAID = PA.PAID;

            foreach (var item in productData.productDetails)
            {
                REDTR.DB.BusinessObjects.JOBType Jtype = dbhelper.DBManager.JOBTypeBLL.GetJOBType(JOBTypeBLL.JOBTypeOp.GetJOBTypeByName, "DGFT"); // For Product wise label allocation [Sunil] only for DGFT job type.

                REDTR.DB.BusinessObjects.PackagingAssoDetails PADtls = new REDTR.DB.BusinessObjects.PackagingAssoDetails();
                PADtls.PAID = PAID;
                PADtls.PackageTypeCode = item.PackageTypeCode;
                PADtls.PPN = item.PPN;
                PADtls.GTIN = item.GTIN;
                PADtls.NTIN = item.NTIN;
                PADtls.GTINCTI = null;// DGPacks[i, (int)DGPacksEnum.GTINCTIRw].Value.ToString();                        
                PADtls.MRP = item.MRP;
                PADtls.Size = item.Size;
                PADtls.BundleQty = item.BundleQty;
                PADtls.TerCaseIndex = 0;
                dbhelper.DBManager.PackagingAssoDetailsBLL.InsertOrUpdatePckAssoDtls(PackagingAssoDetailsBLL.PckAssoDtlsOp.AddPckAssoDtls, PADtls);
            }


            foreach (var item in productData.productLabels)
            {
                REDTR.DB.BusinessObjects.PackageLabelAsso PckLabel = new REDTR.DB.BusinessObjects.PackageLabelAsso();
                PckLabel.PAID = PAID;
                PckLabel.Code = item.Code;
                PckLabel.LabelName = item.LabelName;
                PckLabel.Filter = item.Filter;
                PckLabel.JobTypeID = item.JobTypeID;
                dbhelper.DBManager.PackageLabelBLL.InsertOrUpdatePackagingLabel(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.AddNewPackageLabel), PckLabel);
            }
        }

    }

    class ExcelColumnHelper
    {
        public string exlProdName = "";
        public string exlProdCode = "";
        public string exlDescription = "";
        public string exlRemark = "";
        public string exlIsSceduledDrug = "";
        public string exlDoseUsage = "";
        public string exlGenericName = "";
        public string exlComposition = "";
        public string exlFGCode = "";
        public string exlUseExpDay = "";
        public string exlExpDateFormat = "";
        public string exlPlantCode = "";
        public string exlSAPProductCode = "";
        public string exlInternalMaterialCode = "";
        public string exlCountryDrugCode = "";
        public string exlManufacturer = "";
        public string exlDosageForm = "";
        public string exlStrength = "";
        public string exlContainerSize = "";
        public string exlFEACN = "";
        public string exlSubTypeNo = "";
        public string exlPackageSpec = "";
        public string exlAuthorizedNo = "";
        public string exlSubType = "";
        public string exlSubTypeSpec = "";
        public string exlPackUnit = "";
        public string exlResProdCode = "";
        public string exlWorkshop = "";
        public string exlProviderId = "";
        public string exlSaudiDrugCode = "";
        public string exlNHRN = "";

        public string exlMOCPPN = "";
        public string exlMOCGTIN = "";
        public string exlMOCNTIN = "";
        public string exlMOCSize = "";
        public string exlMOCBundleQty = "";

        public string exlOBXPPN = "";
        public string exlOBXGTIN = "";
        public string exlOBXNTIN = "";
        public string exlOBXSize = "";
        public string exlOBXBundleQty = "";

        public string exlISHPPN = "";
        public string exlISHGTIN = "";
        public string exlISHNTIN = "";
        public string exlISHSize = "";
        public string exlISHBundleQty = "";

        public string exlOSHPPN = "";
        public string exlOSHGTIN = "";
        public string exlOSHNTIN = "";
        public string exlOSHSize = "";
        public string exlOSHBundleQty = "";

        public string exlPALPPN = "";
        public string exlPALGTIN = "";
        public string exlPALNTIN = "";
        public string exlPALSize = "";
        public string exlPALBundleQty = "";

        public ExcelColumnHelper()
        {
            exlProdName = "Name";
            exlProdCode = "ProductCode";
            exlDescription = "Description";
            exlRemark = "Remarks";
            exlIsSceduledDrug = "ScheduledDrug";
            exlDoseUsage = "DoseUsage";
            exlGenericName = "GenericName";
            exlComposition = "Composition";
            exlFGCode = "FGCode";
            exlUseExpDay = "UseExpDay";
            exlExpDateFormat = "ExpDateFormat";
            exlPlantCode = "PlantCode";
            exlSAPProductCode = "SAPProductCode";
            exlInternalMaterialCode = "InternalMaterialCode";
            exlCountryDrugCode = "CountryDrugCode";
            exlManufacturer = "Manufacturer";
            exlDosageForm = "DosageForm";
            exlStrength = "Strength";
            exlContainerSize = "ContainerSize";
            exlFEACN = "FEACN";
            exlSubTypeNo = "SubTypeNo";
            exlPackageSpec = "PackageSpec";
            exlAuthorizedNo = "AuthorizedNo";
            exlSubType = "SubType";
            exlSubTypeSpec = "SubTypeSpec";
            exlPackUnit = "PackUnit";
            exlResProdCode = "ResProdCode";
            exlWorkshop = "Workshop";
            exlProviderId = "ProviderId";
            exlSaudiDrugCode = "SaudiDrugCode";
            exlNHRN = "NHRN";

            exlMOCPPN = "MOCPPN";
            exlMOCGTIN = "MonoCarton";
            exlMOCNTIN = "MOCNTIN";
            exlMOCSize = "MOCSize";
            exlMOCBundleQty = "MOCBundleQty";
            exlOBXPPN = "OBXPPN";
            exlOBXGTIN = "OuterBox";
            exlOBXNTIN = "OBXNTIN";
            exlOBXSize = "OBXSize";
            exlOBXBundleQty = "OBXBundleQty";
            exlISHPPN = "ISHPPN";
            exlISHGTIN = "InnerShipper";
            exlISHNTIN = "ISHNTIN";
            exlISHSize = "ISHSize";
            exlISHBundleQty = "ISHBundleQty";
            exlOSHPPN = "OSHPPN";
            exlOSHGTIN = "OuterShipper";
            exlOSHNTIN = "OSHNTIN";
            exlOSHSize = "OSHSize";
            exlOSHBundleQty = "OSHBundleQty";
            exlPALPPN = "PALPPN";
            exlPALGTIN = "Pallet";
            exlPALNTIN = "PALNTIN";
            exlPALSize = "PALSize";
            exlPALBundleQty = "PALBundleQty";
        }

    }

}