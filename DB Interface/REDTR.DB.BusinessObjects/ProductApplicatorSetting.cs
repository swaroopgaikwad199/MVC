using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]
    public class ProductApplicatorSetting
    {
        private decimal _ServerPAID;

        public decimal ServerPAID
        {
            get { return _ServerPAID; }
            set { _ServerPAID = value; }
        }

        private Nullable<float> _S1;

        public Nullable<float> S1
        {
            get { return _S1; }
            set { _S1 = value; }
        }

        private Nullable<float> _S2;

        public Nullable<float> S2
        {
            get { return _S2; }
            set { _S2 = value; }
        }

        private Nullable<float> _S3;

        public Nullable<float> S3
        {
            get { return _S3; }
            set { _S3 = value; }
        }

        private Nullable<float> _S4;

        public Nullable<float> S4
        {
            get { return _S4; }
            set { _S4 = value; }
        }

        private Nullable<float> _S5;

        public Nullable<float> S5
        {
            get { return _S5; }
            set { _S5 = value; }
        }

        private Nullable<float> _FrontLabelOffset;

        public Nullable<float> FrontLabelOffset
        {
            get { return _FrontLabelOffset; }
            set { _FrontLabelOffset = value; }
        }

        private Nullable<float> _BackLabelOffset;

        public Nullable<float> BackLabelOffset
        {
            get { return _BackLabelOffset; }
            set { _BackLabelOffset = value; }
        }

        private Nullable<float> _CartonLength;

        public Nullable<float> CartonLength
        {
            get { return _CartonLength; }
            set { _CartonLength = value; }
        }

        public ProductApplicatorSetting(decimal ServerPAID,Nullable<float> S1,Nullable<float> S2,Nullable<float> S3,Nullable<float> S4,Nullable<float> S5,Nullable<float> FrontLabelOffset,Nullable<float> BackLabelOffset,Nullable<float> CartonLength)
        {
            this.ServerPAID = ServerPAID;
            this.S1 = S1;
            this.S2 = S2;
            this.S3 = S3;
            this.S4 = S4;
            this.S5 = S5;
            this.FrontLabelOffset = FrontLabelOffset;
            this.BackLabelOffset = BackLabelOffset;
            this.CartonLength = CartonLength;
        }

        public ProductApplicatorSetting()
        { }

    }
}
