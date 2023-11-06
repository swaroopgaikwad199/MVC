using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.DAVAService
{
    public enum XMLFILEType
    {
        PRODUCT,
        BATCH,
        PRODUCTION,
        TEREXEMPTED,
        TNS,
        TER,
        NONE
    }

    public enum DAVAStatus
    {
        NoProductsAvailable,
        NoBatchesAvailable,
        NoProductionDataAvailable,
        NoSSCCsAvailable,
        AlreadyBatchExported,
        Success,
        Failure
    }
    public class DAVAGenerator
    {
        DAVAUtility util = new DAVAUtility();


        public DAVAStatus generateDAVAProduct() 
        {
            return util.CreateProductDAVA();
        }
        public DAVAStatus generateDAVABatch(bool IsAllBatches, string BatchNo, bool IsExemptBarcode, string Country, string CountryCode,string EXEMPTION_NOTIFICATION_AND_DATE)
        {
            return util.CreateBatchDAVA(IsAllBatches, BatchNo, IsExemptBarcode, Country, CountryCode, EXEMPTION_NOTIFICATION_AND_DATE);
        }
        public DAVAStatus generateDAVAProduction(decimal JObId, string BatchNo, bool IsWholeBatch, List<string> cods)
        {
            return util.CreateProductionDAVA(JObId, BatchNo, IsWholeBatch, cods);
        }

        public DAVAStatus generateDAVAProductionTNS(decimal JObId, string BatchNo, bool IsWholeBatch, List<string> cods)
        {
            return util.CreateProductionDAVATNS(JObId, BatchNo, IsWholeBatch, cods);
        }

    

        public DAVAStatus generateDAVATertPackExemp(decimal JObId, string BatchNo, bool IsWholeBatch, bool IsExempBarcode,string ExempCode, string CountryCode, List<string> cods)
        {
            return util.CreateTertiaryExemption(JObId, BatchNo, IsWholeBatch,IsExempBarcode,ExempCode,CountryCode, cods);
        }
    }
}