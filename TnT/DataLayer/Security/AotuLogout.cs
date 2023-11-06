using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.Security
{
    public class AotuLogout
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public  bool getLastentry(decimal id)
        {
            if (id>0)
            {
                var trailentry = db.ServerSideTrails.Where(x => x.UserId == id).OrderByDescending(x => x.ActitvityTime).FirstOrDefault();
                if (trailentry != null)
                {
                    int i = trailentry.Message.IndexOf(" ") + 1;
                    string strW = trailentry.Message.Substring(i);
                    if (strW != " Logged Out.")
                    {
                        return true;
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
    }
}