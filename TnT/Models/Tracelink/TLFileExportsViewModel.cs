using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TnT.DataLayer.TracelinkService;

namespace TnT.Models.Tracelink
{
    public class TLFileExportsViewModel
    {


        [Display(Name = "TracelinkExportFilesBatchNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int JobId { get; set; }


        [Display(Name = "TracelinkExportFilesSOMDetails", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int SOMId { get; set; }

        public bool IsMoc { get; set; }


        [Display(Name = "TracelinkExportFilesFileType", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string FileType { get; set; }


        public TLFileTypes TLFileType { get; set; }

    }
}