using System.Collections.Generic;
using System.IO;
using RedXML;

namespace REDTR.UTILS
{
    public class DatamanSysOpCollection
    {
        private int _LineNo;

        public int LineNo
        {
            get { return _LineNo; }
            set { _LineNo = value; }
        }
        private bool _ResForExtIllumntn;
        public bool ResForExtIllumntn
        {
            get { return _ResForExtIllumntn; }
            set { _ResForExtIllumntn = value; }
        }

        private bool _Read;
        public bool Read
        {
            get { return _Read; }
            set { _Read = value; }
        }

        private bool _NoRead;
        public bool NoRead
        {
            get { return _NoRead; }
            set { _NoRead = value; }
        }

        private bool _ValidatnFailure;
        public bool ValidatnFailure
        {
            get { return _ValidatnFailure; }
            set { _ValidatnFailure = value; }
        }

        private bool _TriggerOverrn;
        public bool TriggerOverrn
        {
            get { return _TriggerOverrn; }
            set { _TriggerOverrn = value; }
        }

        private bool _BufferOverflow;
        public bool BufferOverflow
        {
            get { return _BufferOverflow; }
            set { _BufferOverflow = value; }
        }

        private bool _Open;
        public bool Open
        {
            get { return _Open; }
            set { _Open = value; }
        }

        private bool _Closed;
        public bool Closed
        {
            get { return _Closed; }
            set { _Closed = value; }
        }

        private int _PulseWidth;
        public int PulseWidth
        {
            get { return _PulseWidth; }
            set { _PulseWidth = value; }
        }

    }

    public class DatamanSysOp
    {
        private List<DatamanSysOpCollection> LstDatamanSysOpCollection = new List<DatamanSysOpCollection>();
        //public List<DatamanSysOuput> MainLst;

        public List<DatamanSysOpCollection> LoadDatamanSysOp() //For Current Activated File
        {
            string FilePath = REDTR.UTILS.SettingsPath.DMSysOpSetting;

            if (File.Exists(FilePath))   //SettingsPath.LabelDataSetup))
            {
                LstDatamanSysOpCollection = GenericXmlSerializer<List<DatamanSysOpCollection>>.Deserialize(REDTR.UTILS.SettingsPath.DMSysOpSetting); //SettingsPath.LabelDataSetup
            }
            else
            {
                SaveInfo(); //SettingsPath.LabelDataSetup
            }
            return LstDatamanSysOpCollection;
        }

        private void SaveInfo()
        {
            DatamanSysOpCollection dmset = new DatamanSysOpCollection();
            dmset.LineNo = 0;
            dmset.ResForExtIllumntn = false;
            dmset.Read = false;
            dmset.NoRead = true;
            dmset.ValidatnFailure = true;
            dmset.TriggerOverrn = false;
            dmset.BufferOverflow = false;
            dmset.Closed = true;
            dmset.Open = false;
            dmset.PulseWidth = 100;
            LstDatamanSysOpCollection.Add(dmset);


            dmset = new DatamanSysOpCollection();
            dmset.LineNo = 1;
            dmset.ResForExtIllumntn = false;
            dmset.Read = true;
            dmset.NoRead = false;
            dmset.ValidatnFailure = false;
            dmset.TriggerOverrn = false;
            dmset.BufferOverflow = false;
            dmset.Closed = true;
            dmset.Open = false;
            dmset.PulseWidth = 10;
            LstDatamanSysOpCollection.Add(dmset);

            GenericXmlSerializer<List<DatamanSysOpCollection>>.Serialize(LstDatamanSysOpCollection, REDTR.UTILS.SettingsPath.DMSysOpSetting);
        }
    }
}
