using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;
using TnT.Models.Account;
using TnT.Models.Code;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.DataLayer.Tracer
{
    public class DataHelper
    {
     
        private ApplicationDbContext db = new ApplicationDbContext();

        public DataSet getBatchData(decimal JobId)
        {
            
            var job = getJob(JobId);
            var jobDetails = getJobDetails(JobId);
            var packagingAsso = getPackagingAsso(job.FirstOrDefault().PAID);
            var packagingAssoDetails = getPackagingAssoDetails(job.FirstOrDefault().PAID);
            var packageLabelMaster = getPackageLabelMaster(job.FirstOrDefault().PAID);
            var packagingDetails = getPackagingDetails(JobId);
            //var users = getUsers();

            //convert to datatable and then in DS
            DataSet DSmaster = new DataSet();

            var DTjob = GeneralDataHelper.convertToDataTable(job);DTjob.TableName = "job";
            DSmaster.Tables.Add(DTjob);
            var DTjobDetails = GeneralDataHelper.convertToDataTable(jobDetails);DTjobDetails.TableName = "jobDetails";
            DSmaster.Tables.Add(DTjobDetails);
            var DTpackagingAsso = GeneralDataHelper.convertToDataTable(packagingAsso);DTpackagingAsso.TableName = "packagingAsso";
            DSmaster.Tables.Add(DTpackagingAsso);
            var DTpackagingAssoDetails = GeneralDataHelper.convertToDataTable(packagingAssoDetails);DTpackagingAssoDetails.TableName = "packagingAssoDetails";
            DSmaster.Tables.Add(DTpackagingAssoDetails);
            var DTpackageLabelMaster = GeneralDataHelper.convertToDataTable(packageLabelMaster);DTpackageLabelMaster.TableName = "packageLabelMaster";
            DSmaster.Tables.Add(DTpackageLabelMaster);
            var DTpackagingDetails = GeneralDataHelper.convertToDataTable(packagingDetails);DTpackagingDetails.TableName = "packagingDetails";
            DSmaster.Tables.Add(DTpackagingDetails);

            return DSmaster;
        }


        #region Utils
        private List<PackagingAsso> getPackagingAsso(decimal PAID)
        {
            var data = db.PackagingAsso.Where(x=>x.PAID == PAID).ToList();
            return data;
        }
        private List<PackageLabelMaster> getPackageLabelMaster(decimal PAID)
        {
            var data = db.PackageLabelMaster.Where(x => x.PAID == PAID).ToList();
            return data;
        }
        private List<PackagingAssoDetails> getPackagingAssoDetails(decimal PAID)
        {
            var data = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).ToList();
            return data;
        }

        private List<Job> getJob(decimal JobID)
        {
            var data = db.Job.Where(x=>x.JID==JobID).ToList();
            return data;          
        }

        private List<JobDetails> getJobDetails(decimal JobID)
        {
            var data = db.JobDetails.Where(x=>x.JD_JobID ==JobID).ToList();
            return data;
        }

        private List<PackagingDetails> getPackagingDetails(decimal JobID)
        {
            var data = db.PackagingDetails.Where(x => x.JobID == JobID).ToList();
            return data;
        }

        private List<Users> getUsers(decimal[] UserIds)
        {
            List<Users> lstUsers = new List<Users>();
            foreach (var item in UserIds)
            {
                lstUsers.Add(db.Users.Find(item));
            }
            return lstUsers;
        }

        #endregion

    }
}