using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.JobService
{
    public class GlobalServerDataTransferHelper
    {
        
        public bool Tranfer(decimal JobId,string JbNm,int LoginId)
        {
            string con = Utilities.getConnectionString("GlobalServerConnection").ToString();
            if (string.IsNullOrEmpty(con)){ return false;}

            TransferJobHelper tjh = new TransferJobHelper();
            //show progress when completing.
            tjh.JobName = JbNm;
            tjh.OverallProgress = 1;
            return tjh.TransferJobToGlobalServer(JobId, con,LoginId);
            
        }
        public bool TranferToGLobal(decimal JobId, string JbNm,int LoginId)
        {
            string con = Utilities.getConnectionString("GlobalServerConnection").ToString();
            if (string.IsNullOrEmpty(con)) { return false; }

            TransferJobHelper tjh = new TransferJobHelper();
            //show progress when completing.
            tjh.JobName = JbNm;
            tjh.OverallProgress = 1;
            return tjh.TransferJobToGlobalServer(JobId, con,LoginId);

        }
    }
}