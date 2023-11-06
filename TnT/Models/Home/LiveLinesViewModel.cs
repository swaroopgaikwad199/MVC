using TnT.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Home
{
    public class LiveLinesViewModel
    {
        public string LineId { get; set; }
        public string LineName { get; set; }

        [Display(Name = "JobsBatchManagerProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ProductName { get; set; }

        [Display(Name = "JobsIndexPONumber", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string OrderName { get; set; }

        [Display(Name = "JobsIndexlLotNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BatchNo { get; set; }

        [Display(Name = "ReportsJobsBatchesBatchQty", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int Size { get; set; }

        public int Status { get; set; }
        public List<PrintedPackDetails> printingDetails { get; set; }
        public string Message { get; set; }


    }

    public class PrintedPackDetails
    {
        public string PackagingTypeCode { get; set; }

        public int TotalPrintedQty { get; set; }

    }
    public class PrintStatusInfo
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime printDT { get; set; }
        public List<PrintedPackDetails> packDetails { get; set; }
    }

}