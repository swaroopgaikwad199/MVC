using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]
    public class PackageLabelAsso
    {
        private Decimal _PAID;

        public Decimal PAID
        {
            get { return _PAID; }
            set { _PAID = value; }
        }

        private Decimal _JobTypeID;

        public Decimal JobTypeID
        {
            get { return _JobTypeID; }
            set { _JobTypeID = value; }
        }

        string _Code;

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        string _LabelName;

        public string LabelName
        {
            get { return _LabelName; }
            set { _LabelName = value; }
        }

        string _Filter;

        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        private DateTime _LastUpdatedDate;

        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }

        public PackageLabelAsso()
        { }

        public PackageLabelAsso(Decimal PAID, Decimal JobTypeID,string Code,string LabelName,string Filter,DateTime LastUpdatedDate)
        {
            this.PAID = PAID;
            this.JobTypeID = JobTypeID;
            this.Code = Code;
            this.LabelName = LabelName;
            this.Filter = Filter;
            this.LastUpdatedDate = LastUpdatedDate;
        }

    }
}
