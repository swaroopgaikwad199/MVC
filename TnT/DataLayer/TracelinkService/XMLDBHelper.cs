using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TnT.Models.Job;

namespace TnT.DataLayer.TracelinkService
{
    public class XMLDBHelper
    {
        SqlConnection con = new SqlConnection(Utilities.getConnectionString("DefaultConnection"));
        SqlCommand cmd = null;
        SqlDataAdapter da = null;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        public DataSet AssignedData(int jid,bool IsMOC)
        {
            try
            {
                DataTable dtLineCOde = new DataTable();
                DataTable dttype = new DataTable();
                con.Open();
                if (IsMOC)
                {
                    cmd = new SqlCommand("select p.PackageTypeCode,p.Code,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.LastUpdatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose,p.Remarks from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and p.PackageTypeCode='MOC' and JobID=" + jid + "", con);
                }
                else
                {
                    cmd = new SqlCommand("select p.PackageTypeCode,p.Code,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.LastUpdatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose,p.Remarks from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and JobID=" + jid + "", con);
                }
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dt.TableName = "Packtype";
                ds.Tables.Add(dt);

                
                dttype.Columns.Add("PackageTypeCode");
                dttype.TableName = "TypeName";
                var lvls = ProductPackageHelper.getAllDeck(jid.ToString());
                lvls = ProductPackageHelper.sorttheLevelsDesc(lvls).ToList();
                
                for(int i=0;i<lvls.Count();i++)
                {
                    dttype.Rows.Add(lvls[i]);
                }
                ds.Tables.Add(dttype);

                if(IsMOC)
                {
                    cmd = new SqlCommand("select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid + "and jb.JD_Deckcode='MOC' ", con);
                }
                else
                {
                    cmd = new SqlCommand("select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid + " ", con);
                }
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtLineCOde);
                dtLineCOde.TableName = "LineCode";
                ds.Tables.Add(dtLineCOde);


                DataTable dtLevelCount = new DataTable();
                DataTable dtSSCC = new DataTable();
                if (IsMOC)
                {
                    cmd = new SqlCommand("select distinct(PackageTypeCode),COUNT(*) as totalpkgtype from PackagingDetails where JobID=" + jid + " and PackageTypeCode='MOC' group by PackageTypeCode order by totalpkgtype asc", con);
                }
                else
                {
                    cmd = new SqlCommand("select distinct(PackageTypeCode),COUNT(*) as totalpkgtype from PackagingDetails where JobID=" + jid + " group by PackageTypeCode order by totalpkgtype asc", con);
                }
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtLevelCount);
                dtLevelCount.TableName = "LastLevel";
                ds.Tables.Add(dtLevelCount);

                //cmd = new SqlCommand("select p.Code,p.SSCC,p.MfgPackDate,j.Quantity,jb.JD_GTIN,jb.JD_Deckcode,p.NextLevelCode from PackagingDetails p inner join Job j on p.JobID=j.JID inner join JobDetails jb on j.JID=jb.JD_JobID where p.IsRejected=0 and p.IsUsed = 1 and JobID=" + jid + "", con);
                if(IsMOC)
                {
                    cmd = new SqlCommand("select p.Code,p.SSCC,p.LastUpdatedDate,j.Quantity,p.NextLevelCode,p.PackageTypeCode,p.Remarks from PackagingDetails p inner join Job j on p.JobID=j.JID  where p.IsRejected=0 and p.IsUsed = 1 and p.IsDecomission=0 and p.PackageTypeCode='MOC' and JobID=" + jid, con);
                }
                else
                {
                    cmd = new SqlCommand("select p.Code,p.SSCC,p.LastUpdatedDate,j.Quantity,p.NextLevelCode,p.PackageTypeCode,p.Remarks from PackagingDetails p inner join Job j on p.JobID=j.JID  where p.IsRejected=0 and p.IsUsed = 1 and p.IsDecomission=0 and JobID=" + jid, con);
                }
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtSSCC);
                dtSSCC.TableName = "LastLeveSSCC";
                ds.Tables.Add(dtSSCC);


                return ds;
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region Disposition Assign
        public DataTable DispAssCommEvent(int jid, bool IsMOC)
        {
            try
            {
                con.Open();
                string query = string.Empty;



                if (IsMOC)
                {
                    query = "select p.PackageTypeCode,p.Code,p.NextLevelCode,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.CreatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose,p.remarks from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and p.PackageTypeCode='MOC' and (NextLevelCode!='FFFFF' or NextLevelCode is NULL) and JobID=" + jid;
                    // cmd = new SqlCommand("select p.PackageTypeCode,p.Code,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.LastUpdatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and p.PackageTypeCode='MOC' and JobID=" + jid + "", con);
                }
                else
                {
                    query = "select p.PackageTypeCode,p.Code,p.NextLevelCode,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.CreatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose,p.remarks from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and (NextLevelCode!='FFFFF' or NextLevelCode is NULL) and JobID=" + jid+ "   ";
                    // cmd = new SqlCommand("select p.PackageTypeCode,p.Code,p.NextLevelCode,j.JD_GTIN,p.ExpPackDate,jb.BatchNo,jb.JobStartTime,p.LastUpdatedDate,p.SSCC,j.JD_Deckcode,p.IsLoose from PackagingDetails p inner join JobDetails j on p.JobID=j.JD_JobID and p.PackageTypeCode=j.JD_Deckcode inner join Job jb on jb.JID=j.JD_JobID  where p.IsRejected=0 and p.IsDecomission=0 and JobID=" + jid + "", con);
                }
                using (cmd = new SqlCommand(query, con))
                {
                    cmd.CommandTimeout = 0;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt.TableName = "Packtype";
                }
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DispAssCommAttr(int jid, bool IsMOC)
        {
            string query = "";
            DataTable dtLine = new DataTable();
            if (IsMOC)
            {
                // cmd = new SqlCommand("select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid + "and jb.JD_Deckcode='MOC' ", con);
                query = "select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid + "and jb.JD_Deckcode='MOC'";
            }
            else
            {
                //  cmd = new SqlCommand("select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid + " ", con);
                query = "select p.LineCode,j.BatchNo,j.MfgDate,j.ExpDate,j.JobName,u.UserName,jb.JD_GTIN,jb.JD_Deckcode,p.InternalMaterialCode,p.CountryDrugCode from job j inner join PackagingAsso p on j.PAID=p.PAID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on jb.JD_JobID=j.JID where j.JID=" + jid;
            }
            using (cmd = new SqlCommand(query, con))
            {
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtLine);
                dtLine.TableName = "LineCode";

            }
            return dtLine;
        }
        #endregion

        public DataSet SOMData(Job job)
        {
            try
            {
                
                string TertiaryDec = ProductPackageHelper.getBottomDeck((int)job.PAID);
                //  cmd = new SqlCommand("select jb.JD_GTIN,p.SSCC,j.BatchNo,u.UserName,j.Quantity,pkgAss.InternalMaterialCode,pkgAss.CountryDrugCode from JobDetails jb inner join PackagingDetails p on p.JobID=jb.JD_JobID inner join job j on j.JID=p.JobID inner join PackagingAsso pkgAss on pkgAss.PAID=p.PAID inner join  Users u on u.ID=j.CreatedBy where p.JobID=" + jid + " and p.IsLoose=0 or p.IsLoose is null  and jb.JD_Deckcode in (select top(1) PackageTypeCode from PackagingDetails where JobID=" + jid + " group by PackageTypeCode order by count(*) asc ) and p.PackageTypeCode in (select top(1) PackageTypeCode from PackagingDetails where JobID=" + jid + " group by PackageTypeCode order by count(*) asc )", con);
                cmd = new SqlCommand("select jb.JD_GTIN,p.SSCC,j.BatchNo,u.UserName,j.Quantity,pkgAss.InternalMaterialCode,pkgAss.CountryDrugCode from JobDetails jb inner join PackagingDetails p on p.JobID=jb.JD_JobID inner join job j on j.JID=p.JobID inner join PackagingAsso pkgAss on pkgAss.PAID=p.PAID inner join  Users u on u.ID=j.CreatedBy where p.JobID=" + job.JID + " and p.IsLoose=0 or p.IsLoose is null  and jb.JD_Deckcode='" + TertiaryDec + "' and p.PackageTypeCode='" + TertiaryDec + "'  and jb.JD_JobID="+job.JID+" and p.JobID="+job.JID+ " and p.IsUsed=1", con);
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DispositionData(int jid,bool IsMOC)
        {
            try
            {
                DataTable dt1 = new DataTable();

                //    cmd = new SqlCommand("select j.BatchNo,j.MfgDate,u.UserName,p.LineCode,jb.JD_GTIN,p.Code from Job j inner join PackagingDetails p on j.JID=p.JobID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on j.JID=jb.JD_JobID  where j.JID=" + jid + " and p.PackageTypeCode='MOC' and p.IsRejected=1 and jb.JD_Deckcode='MOC' ", con);
                cmd = new SqlCommand("select top 1 j.BatchNo,j.LastUpdatedDate,u.UserName,p.LineCode,jb.JD_GTIN from Job j inner join PackagingDetails p on j.JID=p.JobID inner join Users u on u.ID=j.CreatedBy inner join JobDetails jb on j.JID=jb.JD_JobID  where j.JID=" + jid + " and p.PackageTypeCode='MOC' and jb.JD_Deckcode='MOC' ", con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                ds.Tables.Add(dt);
                //cross apply ManualUpdateDesc.nodes('/Action') as M(MUpdate) where M.MUpdate.value('@Type','varchar(max)') like '%QASAMPLING%' and -- remove the qa sampling condition as the detail job data and disposition update count should be same
                if (IsMOC)
                {
                    cmd = new SqlCommand("select code,IsDecomission,IsRejected,IsManualUpdated,IsUsed from PackagingDetails where  IsDecomission=1 and jobid=" + jid + " and sscc is  null and PackageTypeCode='MOC' "
+ "union select code, IsDecomission, IsRejected, IsManualUpdated, IsUsed from PackagingDetails where IsRejected = 1 and jobid = " + jid + " and sscc is  null and PackageTypeCode='MOC' "
+ "union select code, IsDecomission, IsRejected, IsManualUpdated, IsUsed from PackagingDetails p inner join job j on p.JobID = j.JID  where IsUsed = 0 or IsUsed is null and jobid = " + jid + " and j.JobStatus = 3 and sscc is  null and PackageTypeCode='MOC'", con);
                }
                else
                {
                    //cross apply ManualUpdateDesc.nodes('/Action') as M(MUpdate) where M.MUpdate.value('@Type','varchar(max)') like '%QASAMPLING%'
                    cmd = new SqlCommand("select code,IsDecomission,IsRejected,IsManualUpdated,IsUsed from PackagingDetails where  IsDecomission=1 and jobid="+jid+ " and sscc is  null "
+ "union select code, IsDecomission, IsRejected, IsManualUpdated, IsUsed from PackagingDetails where IsRejected = 1 and jobid = "+jid+ " and sscc is  null "
+ "union select code, IsDecomission, IsRejected, IsManualUpdated, IsUsed from PackagingDetails p inner join job j on p.JobID = j.JID  where IsUsed = 0 or IsUsed is null and jobid = "+jid+ " and j.JobStatus = 3 and sscc is  null", con);

                }
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                ds.Tables.Add(dt1);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}