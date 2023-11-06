using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{
    public class ReceiverValidator
    {
        HashSet<string> hsUids, hsSSCC, hsGTIN;
        List<string> lstUids, lstSSCC, lstGTIN;
        string GTIN, SSCC, Uids;


        public ReceiverValidator(int RequestId)
        {
            try
            {
                Response res = new Response();
                GTIN = res.getGTINCodeforVldtn(RequestId);
                SSCC = res.getSSCCCodeforVldtn(RequestId);
                Uids = res.getUIDsforVldtn(RequestId);

                lstGTIN = GTIN.Split(',').ToList();
                lstSSCC = SSCC.Split(',').ToList();
                lstUids = Uids.Split(',').ToList();

                hsGTIN = new HashSet<string>(lstGTIN);
                hsSSCC = new HashSet<string>(lstSSCC);
                hsUids = new HashSet<string>(lstUids);


                //////////////
                lstGTIN = null;
                lstSSCC = null;
                lstUids = null;
            }
            catch (Exception)
            {

                throw;
            }
         
        }

        public bool IsUIdValid(string uId)
        {
            try
            {
                return hsUids.Contains(uId);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public bool IsSSCCdValid(string sscc)
        {
            try
            {
                return hsSSCC.Contains(sscc);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}