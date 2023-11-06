using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.EPCIS;
using TnT.Models.Providers;

namespace TnT.Models.Product
{
    public class ProductImportViewModel
    {
        [Key]
        public string Name { get; set; }
        public string ProductCode { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }

        public string ScheduledDrug { get; set; }

        public string DoseUsage { get; set; }

        public string GenericName { get; set; }
        public string Composition { get; set; }
        public string FGCode { get; set; }

        public string UseExpDay { get; set; }

        public string ExpDateFormat { get; set; }
        public string PlantCode { get; set; }
        public string SAPProductCode { get; set; }

        public string InternalMaterialCode { get; set; }
        public string CountryDrugCode { get; set; }

        public string Manufacturer { get; set; }
        public string DosageForm { get; set; }

        public string Strength { get; set; }

        public string ContainerSize { get; set; }

        public string FEACN { get; set; }


        public string SubTypeNo { get; set; }

        public string PackageSpec { get; set; }

        public string AuthorizedNo { get; set; }

        public string SubType { get; set; }

        public string SubTypeSpec { get; set; }

        public string PackUnit { get; set; }

        public string ResProdCode { get; set; }

        public string Workshop { get; set; }

    
        public string ProviderId { get; set; }

        public string SaudiDrugCode { get; set; }

        public string NHRN { get; set; }

        //public string PrimaryPackBox { get; set; }

        //public string PPBSize { get; set; }
        //public string PPBBundleQty { get; set; }
        public string MOCPPN { get; set; }
        public string MonoCarton { get; set; }
        public string MOCNTIN { get; set; }
        public string MOCSize { get; set; }
        public string MOCBundleQty { get; set; }

        public string OBXPPN { get; set; }
        public string OuterBox { get; set; }
        public string OBXSize { get; set; }

        public string OBXNTIN { get; set; }
        public string OBXBundleQty { get; set; }

        public string ISHPPN { get; set; }
        public string InnerShipper { get; set; }

        public string ISHNTIN { get; set; }
        public string ISHSize { get; set; }
        public string ISHBundleQty { get; set; }

        public string OSHPPN { get; set; }
        public string OuterShipper { get; set; }
        public string OSHNTIN { get; set; }
        public string OSHSize { get; set; }
        public string OSHBundleQty { get; set; }

        public string PALPPN { get; set; }
        public string Pallet { get; set; }
        public string PALNTIN { get; set; }
        public string PALSize { get; set; }
        public string PALBundleQty { get; set; }


    }
}