using System;
using System.Collections.Generic;
using RedXML;
using REDTR.UTILS.SystemIntegrity;
namespace REDTR.UTILS
{
    public class JobPrintLabelAsso
    {
        public String JobType;
        public String Label2Use;
        public bool HasUID;
        public String GS1Filter;

        public static List<JobPrintLabelAsso> Read(string FilePath)
        {
            List<JobPrintLabelAsso> lstJobsLblAsso = null;

            if (System.IO.File.Exists(FilePath))
            {
                lstJobsLblAsso = GenericXmlSerializer<List<JobPrintLabelAsso>>.Deserialize(FilePath);
            }

            return lstJobsLblAsso;
        }
        public static void Write(string FilePath, List<JobPrintLabelAsso> lstJobsLblAsso)
        {
            try
            {
                GenericXmlSerializer<List<JobPrintLabelAsso>>.Serialize(lstJobsLblAsso, FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<JobPrintLabelAsso> CreateDefault(List<string> LstTypes)
        {   
            String msgNo = "Not_Set";
            List<JobPrintLabelAsso> lstJobsLblAsso = new List<JobPrintLabelAsso>();
            JobPrintLabelAsso obj = null;
            foreach (string jT in LstTypes)
            {
                obj = new JobPrintLabelAsso();
                obj.JobType = jT;
                obj.Label2Use = msgNo;
                obj.HasUID = true;
                obj.GS1Filter = Globals.AppSettings.GS1FilterWithUID;// "GS12D1-01-11-17-10-21|GTIN|MFG|EXP|LOT|UID";
               
                lstJobsLblAsso.Add(obj);

                obj = new JobPrintLabelAsso();
                obj.JobType = jT;
                obj.Label2Use = msgNo;
                obj.HasUID = false;
                obj.GS1Filter = Globals.AppSettings.GS1FilterWithOutUID;// "GS12D1-01-11-17-10|GTIN|MFG|EXP|LOT";
                lstJobsLblAsso.Add(obj);
            }
            return lstJobsLblAsso;
        }
    }
}
