using TnT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace TnT.DataLayer.Trailings
{
    public class Trails
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void AddTrail(string Message, int userId,string Reason,string Activity)
        {
            try
            {
                var usr = db.Users.Where(x => x.ID == userId).FirstOrDefault();
                ServerSideTrails trail = new ServerSideTrails();
                trail.Message = Message;
                trail.UserId = userId;
                trail.ActitvityTime = DateTime.Now;
                trail.Reason = Reason;
                trail.Activity = Activity;
                trail.RoleId = usr.RoleID;
                db.ServerSideTrails.Add(trail);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}