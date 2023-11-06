using System;
using System.Collections.Generic;
using System.Diagnostics;
using REDTR.UTILS.SystemIntegrity;
using RedXML;

namespace REDTR.UTILS
{
    //The "JobTypeCodeFormat" Class has been included in the UTILS NameSpace according to the Instructions in Phase IV Review(Tuesday,20-11-2012). It has been inherited in the JobTypeCodeFormat_Comm_UI in the CommonUIs NameSpace
    [Serializable]
    public class JobTypeCodeFormat
    {
        public enum DataField
        {
            MFD,
            MRP,
            MLNO,
            CustomData1,
            CustomData2
        }

        public enum JobType
        {
            mDomestic = 1,
            mExport = 2,
            mTender = 3,
            mExportChina = 4,
            mDuelMRP = 5,
            mDGFT = 6,
            mDGFT_Ex = 7,
        }
        private DataField _DATAField;
        public DataField DATAField
        {
            get { return _DATAField; }
            set { _DATAField = value; }
        }

        private string _Position;
        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        private int _Size;
        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        private string _Prelable;
        public string PreLable
        {
            get { return _Prelable; }
            set { _Prelable = value; }
        }

        private string _Data;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        private string _PostLable;
        public string PostLable
        {
            get { return _PostLable; }
            set { _PostLable = value; }
        }

        public List<JobTypeCodeFormat> LstJbTypeFormat;
        public List<string> JbTypeDatatoPrint = new List<string>(2);
        public int MaxLines = 2;
        public const string mSeparatorTender = "#";



        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }
        public static string GetDefaultXml(JobType JType)
        {
            string Xml = string.Empty;
            JobTypeCodeFormat Jb;
            List<JobTypeCodeFormat> Lst = new List<JobTypeCodeFormat>();
            switch (JType)
            {
                case JobType.mDGFT:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "1-1";
                    Jb.Size = 15;
                    Jb.PreLable = "MFD:";
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MRP;
                    Jb.Position = "1-2";
                    Jb.Size = 45;
                    Jb.PreLable = " MRP: ";
                    Jb.PostLable = "(INCL. OF ALL TAXES)";
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                case JobType.mDGFT_Ex:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "1-1";
                    Jb.Size = 15;
                    Jb.PreLable = "MFD:";
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MRP;
                    Jb.Position = "1-2";
                    Jb.Size = 45;
                    Jb.PreLable = " MRP: ";
                    Jb.PostLable = "(INCL. OF ALL TAXES)";
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                case JobType.mDomestic:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "1-1";
                    Jb.Size = 15;
                    Jb.PreLable = "MFD:";
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MRP;
                    Jb.Position = "1-2";
                    Jb.Size = 45;
                    Jb.PreLable = " MRP: ";
                    Jb.PostLable = "(INCL. OF ALL TAXES)";
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                case JobType.mExport:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "1-1";
                    Jb.Size = 15;
                    Jb.PreLable = "MFD:";
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MLNO;
                    Jb.Position = "1-2";
                    Jb.Size = 45;
                    Jb.PreLable = " MLNO:  ";
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                case JobType.mTender:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "1-1";
                    Jb.Size = 15;
                    Jb.PreLable = "MFD:";
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.CustomData1;
                    Jb.Position = "1-2";
                    Jb.Size = 45;
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.CustomData2;
                    Jb.Position = "2-1";
                    Jb.Size = 60;
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                case JobType.mExportChina:
                    Xml = string.Empty;
                    break;

                case JobType.mDuelMRP:
                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.CustomData1;
                    Jb.Position = "1-1";
                    Jb.Size = 60;
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MFD;
                    Jb.Position = "2-1";
                    Jb.PreLable = "MFD:";
                    Jb.Size = 15;
                    Lst.Add(Jb);

                    Jb = new JobTypeCodeFormat();
                    Jb.DATAField = DataField.MRP;
                    Jb.Position = "2-2";
                    Jb.PreLable = "MRP:";
                    Jb.PostLable = "(INCL. OF ALL TAXES)";
                    Jb.Size = 45;
                    Lst.Add(Jb);
                    Xml = GenericXmlSerializer<List<JobTypeCodeFormat>>.Serialize(Lst);
                    break;
                default:
                    Xml = string.Empty;
                    break;
            }
            return Xml;
        }

        public static JobType GetJobType(string Type)
        {
            string[] ArrJbType = Enum.GetNames(typeof(JobType));
            string JbType = string.Empty;
            JobType RetVal;
            foreach (string item in ArrJbType)
            {
                if (item.Contains(Type))
                {
                    JbType = item;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(JbType))
            {
                RetVal = (JobType)Enum.Parse(typeof(JobType), JbType);
                return RetVal;
            }
            else
                return JobType.mDGFT;
        }
    }
}
