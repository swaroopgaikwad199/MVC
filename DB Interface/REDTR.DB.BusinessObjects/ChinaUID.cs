using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]

    public class ChinaUID
    {
        private int _SendId;

        public int SendId
        {
            get { return _SendId; }
            set { _SendId = value; }
        }

        private Nullable<int> _JobId;

        public Nullable<int> JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }

        private Nullable<int> _TransId;

        public Nullable<int> TransId
        {
            get { return _TransId; }
            set { _TransId = value; }
        }

        private string _UID;

        public string UID
        {
            get { return _UID; }
            set { _UID = value; }
        }

        private Nullable<bool> _Result;

        public Nullable<bool> Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        private Nullable<bool> _IsUsed;

        public Nullable<bool> IsUsed
        {
            get { return _IsUsed; }
            set { _IsUsed = value; }
        }

        public ChinaUID()
        { }

        public ChinaUID(int SendId, Nullable<int> JobId, Nullable<int> TransId, string UID, Nullable<bool> Result)
        {
            this.SendId = SendId;
            this.JobId = JobId;
            this.TransId = TransId;
            this.UID = UID;
            this.Result = Result;
            this.IsUsed = IsUsed;
        }

        public override string ToString()
        {
            return "SendId = " + SendId.ToString() + ",JobId = " + JobId.ToString() + ",TransId = " + TransId.ToString() + ",UID = " + UID + ",Result = " + Result.ToString();
        }

        public class SendIdComparer : System.Collections.Generic.IComparer<ChinaUID>
        {
            public SorterMode SorterMode;
            public SendIdComparer()
            { }
            public SendIdComparer(SorterMode SorterMode)
            {
                this.SorterMode = SorterMode;
            }
            #region IComparer<ChinaUID> Membres
            int System.Collections.Generic.IComparer<ChinaUID>.Compare(ChinaUID x, ChinaUID y)
            {
                if (SorterMode == SorterMode.Ascending)
                {
                    return y.SendId.CompareTo(x.SendId);
                }
                else
                {
                    return x.SendId.CompareTo(y.SendId);
                }
            }
            #endregion
        }
        public class UIDComparer : System.Collections.Generic.IComparer<ChinaUID>
        {
            public SorterMode SorterMode;
            public UIDComparer()
            { }
            public UIDComparer(SorterMode SorterMode)
            {
                this.SorterMode = SorterMode;
            }
            #region IComparer<ChinaUID> Membres
            int System.Collections.Generic.IComparer<ChinaUID>.Compare(ChinaUID x, ChinaUID y)
            {
                if (SorterMode == SorterMode.Ascending)
                {
                    return y.UID.CompareTo(x.UID);
                }
                else
                {
                    return x.UID.CompareTo(y.UID);
                }
            }
            #endregion
        }
    }
}
