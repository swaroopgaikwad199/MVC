using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]
    public class ProductGlueSetting
    {
        private decimal _ServerPAID;

        public decimal ServerPAID
        {
            get { return _ServerPAID; }
            set { _ServerPAID = value; }
        }

        private Nullable<float> _HotGlueStartDistance;

        public Nullable<float> HotGlueStartDistance
        {
            get { return _HotGlueStartDistance; }
            set { _HotGlueStartDistance = value; }
        }

        private Nullable<float> _HotGlueGapDistance;

        public Nullable<float> HotGlueGapDistance
        {
            get { return _HotGlueGapDistance; }
            set { _HotGlueGapDistance = value; }
        }

        private Nullable<float> _HotGlueDotSize;

        public Nullable<float> HotGlueDotSize
        {
            get { return _HotGlueDotSize; }
            set { _HotGlueDotSize = value; }
        }

        private Nullable<float> _ColdGlueStartDistance;

        public Nullable<float> ColdGlueStartDistance
        {
            get { return _ColdGlueStartDistance; }
            set { _ColdGlueStartDistance = value; }
        }

        private Nullable<float> _ColdGlueGapDistance;

        public Nullable<float> ColdGlueGapDistance
        {
            get { return _ColdGlueGapDistance; }
            set { _ColdGlueGapDistance = value; }
        }

        private Nullable<float> _ColdGlueDotSize;

        public Nullable<float> ColdGlueDotSize
        {
            get { return _ColdGlueDotSize; }
            set { _ColdGlueDotSize = value; }
        }

        public ProductGlueSetting(decimal ServerPAID, Nullable<float> HotGlueStartDistance, Nullable<float> HotGlueGapDistance, Nullable<float> HotGlueDotSize, Nullable<float> ColdGlueStartDistance, Nullable<float> ColdGlueGapDistance, Nullable<float> ColdGlueDotSize)
        {
            this.ServerPAID = ServerPAID;
            this.HotGlueStartDistance = HotGlueStartDistance;
            this.HotGlueGapDistance = HotGlueGapDistance;
            this.HotGlueDotSize = HotGlueDotSize;
            this.ColdGlueStartDistance = ColdGlueStartDistance;
            this.ColdGlueGapDistance = ColdGlueGapDistance;
            this.ColdGlueDotSize = ColdGlueDotSize;
        }

        public ProductGlueSetting()
        { }
    }
}
