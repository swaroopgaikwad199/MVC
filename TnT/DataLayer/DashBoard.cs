using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;
using static TnT.Models.Home.DashboardViewModel;

namespace TnT.DataLayer
{
    public class DashBoard
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static object[] getDashboardData(string month,string year)
        {


            int mon = Convert.ToInt32(month);
            int yr = Convert.ToInt32(year);
            DbHelper m_dbhelper = new DbHelper();
            //Monthly batch statistics
            //string query4 = "select count(*) from job where JobStatus=3 union select count(*) from job where JobStatus=1 union select count(*) from job where JobStatus=4";
            //DataSet ds4 = m_dbhelper.GetDataSet(query4);
           
            List<GrapDataBatchStatus> lstbatchstatus = new List<GrapDataBatchStatus>();

            //if (ds4.Tables[0].Rows.Count > 1)
            //{
            var closebatch = db.Job.Where(x => x.JobStatus == 3 && x.CreatedDate.Month==mon && x.CreatedDate.Year==yr).Count();
                GrapDataBatchStatus lstbatch = new GrapDataBatchStatus();
                lstbatch.x = TnT.LangResource.GlobalRes.cmnMenuItemBatchMasterSubItemCloseBatch;
                lstbatch.y = Convert.ToString(closebatch);
                lstbatchstatus.Add(lstbatch);

            var verifybatch = db.Job.Where(x => x.JobStatus == 1 && x.CreatedDate.Month == mon && x.CreatedDate.Year == yr).Count();
                GrapDataBatchStatus lstbatch1 = new GrapDataBatchStatus();
                lstbatch1.x = TnT.LangResource.GlobalRes.cmnMenuItemBatchMasterSubItemVerifyBatch;
                lstbatch1.y = Convert.ToString(verifybatch);
                lstbatchstatus.Add(lstbatch1);

            var decombatch = db.Job.Where(x => x.JobStatus == 4 && x.CreatedDate.Month == mon && x.CreatedDate.Year == yr).Count();
                GrapDataBatchStatus lstbatch2 = new GrapDataBatchStatus();
                lstbatch2.x =TnT.LangResource.GlobalRes.HomeLiveExeDecommBatch;
                lstbatch2.y = Convert.ToString(decombatch);
                lstbatchstatus.Add(lstbatch2);
           // }
            object[] jobstatus = { lstbatchstatus };
            List<GraphData> dataList = new List<GraphData>();
            //Operator Statistics
            string query = "select cast(COUNT(*) as nvarchar(50)) jobcount,u.UserName from job j inner join users u on j.CreatedBy=u.ID where CreatedBy in ( select distinct(createdby) from job ) and MONTH(j.CreatedDate)="+month+" and YEAR(j.CreatedDate)="+year+" group by u.UserName";
            DataSet ds = m_dbhelper.GetDataSet(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dataList = ds.Tables[0].AsEnumerable().Select(dataRow => new GraphData { y = dataRow.Field<string>("jobcount"), x = dataRow.Field<string>("UserName").ToUpper() }).ToList();
            }
            object[] optrStat = { dataList };

            //Product and batch wise summary
            string query1 = "select cast(COUNT(*) as nvarchar(50)) jobcount,LEFT(DATENAME(month,CreatedDate),3) createdmonth,MONTH(CreatedDate)  from job where YEAR(CreatedDate)="+year+" and MONTH(CreatedDate)="+month+" group by DATENAME(month,CreatedDate),MONTH(CreatedDate) order by month(CreatedDate)";
            DataSet ds1 = m_dbhelper.GetDataSet(query1);
            
            List<GraphDataBatchProd> dataList1 = new List<GraphDataBatchProd>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    GraphDataBatchProd lst = new GraphDataBatchProd();
                    lst.batch = ds1.Tables[0].Rows[i]["jobcount"].ToString();
                    lst.month = ds1.Tables[0].Rows[i]["createdmonth"].ToString();


                    string query2 = "select cast(COUNT(*) as nvarchar(50)) prodcount,DATENAME(month,CreatedDate) createdmonth from PackagingAsso where LEFT(DATENAME(month,CreatedDate),3)='" + ds1.Tables[0].Rows[i]["createdmonth"].ToString() + "' and YEAR(CreatedDate)="+year+" group by DATENAME(month,CreatedDate)";
                    DataSet ds2 = m_dbhelper.GetDataSet(query2);
                    if (ds2.Tables[0].Rows.Count > 0)
                        lst.prod = ds2.Tables[0].Rows[0]["prodcount"].ToString();
                    else
                        lst.prod = "0";
                    dataList1.Add(lst);
                }
            }
            object[] batchProd = { dataList1 };


            //Line wise batch Summary
            List<GraphDataLinewise> dataList2 = new List<GraphDataLinewise>();
            string query3 = "select distinct(j.LineCode),cast(count(*) as nvarchar(50)) batchcount,l.LineCode line from job j inner join LineLocation l on j.LineCode=l.ID where j.JobStatus=5 and l.IsActive=1 and MONTH(j.CreatedDate)="+month+" and YEAR(j.CreatedDate)="+year+"  group by l.LineCode,j.LineCode having count(*)>0";
            DataSet ds3 = m_dbhelper.GetDataSet(query3);
            if (ds3.Tables[0].Rows.Count > 0)
            {
                dataList2 = ds3.Tables[0].AsEnumerable().Select(dataRow => new GraphDataLinewise { line = dataRow.Field<string>("line"), batchCount = dataRow.Field<string>("batchcount").ToUpper() }).ToList();
            }
            object[] linewise = { dataList2 };


            object[] response = { jobstatus, optrStat, batchProd, linewise };
            return response;
        }

    }
}