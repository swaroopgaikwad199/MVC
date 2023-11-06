using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.UIDService
{
    public class UIDDataHelper
    {

        ApplicationDbContext db = new ApplicationDbContext();
         
        public string getLastUID(int JobId, string DeckCode)
        {
            var MasterID = 0;
            var ExtraCount = db.M_Identities.Where(mi => mi.JID == JobId && mi.IsExtra == true && mi.PackageTypeCode == DeckCode).Count();
            if (ExtraCount > 1)
            {
                 MasterID = db.M_Identities.Where(mi => mi.JID == JobId && mi.IsExtra == true && mi.IsTransfered == true && mi.PackageTypeCode == DeckCode).OrderByDescending(k=> k.CreatedOn).FirstOrDefault().Id;
            }
            else {
                 MasterID = db.M_Identities.Where(mi => mi.JID == JobId && mi.IsExtra == false && mi.PackageTypeCode == DeckCode).FirstOrDefault().Id;
            }
            var code = db.X_Identities.Where(x => x.MasterId== MasterID && x.PackTypeCode == DeckCode).OrderByDescending(x => x.Id).FirstOrDefault();
            return code.SerialNo.ToString();
        }

    }
}