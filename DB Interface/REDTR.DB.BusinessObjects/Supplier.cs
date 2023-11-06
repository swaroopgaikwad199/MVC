using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]
    public class Supplier
    {
        private int ID;

        public int ID1
        {
            get { return ID; }
            set { ID = value; }
        }
        private string UIDs;

        public string UIDs1
        {
            get { return UIDs; }
            set { UIDs = value; }
        }

        private string SDate;

        public string SDate1
        {
            get { return SDate; }
            set { SDate = value; }
        }

        private bool SFlag;

        public bool SFlag1
        {
            get { return SFlag; }
            set { SFlag = value; }
        }

        private int RecordCount;

        public int RecordCount1
        {
            get { return RecordCount; }
            set { RecordCount = value; }
        }

        private int FirstID;

        public int FirstID1
        {
            get { return FirstID; }
            set { FirstID = value; }
        }
        private string BatchNo;

        public string BatchNo1
        {
            get { return BatchNo; }
            set { BatchNo = value; }
        }

        private Int32 BatchId;

        public Int32 BatchId1
        {
            get { return BatchId; }
            set { BatchId = value; }
        }

        private string TransNo;

        public string TransNo1
        {
            get { return TransNo; }
            set { TransNo = value; }
        }

        private string SupplierName;

        public string SupplierName1
        {
            get { return SupplierName; }
            set { SupplierName = value; }
        }

        private Int32 SupplierId;

        public Int32 SupplierId1
        {
            get { return SupplierId; }
            set { SupplierId = value; }
        }


        private string ProductName;

        public string ProductName1
        {
            get { return ProductName; }
            set { ProductName = value; }
        }

        private Int32 ProdId;

        public Int32 ProdId1
        {
            get { return ProdId; }
            set { ProdId = value; }
        }

        private string Status;

        public string Status1
        {
            get { return Status; }
            set { Status = value; }
        }

        private string _PackageTypeCode;

        public string PackageTypeCode
        {
            get { return _PackageTypeCode; }
            set { _PackageTypeCode = value; }
        }
        private int FirstId;

        public int FirstId1
        {
            get { return FirstId; }
            set { FirstId = value; }
        }

        private int CountToCompare;

        public int CountToCompare1
        {
            get { return CountToCompare; }
            set { CountToCompare = value; }
        }

        private Int32 BatchQuantity;

        public Int32 BatchQuantity1
        {
            get { return BatchQuantity; }
            set { BatchQuantity = value; }
        }

        private Int32 SurPlusQuantity;

        public Int32 SurPlusQuantity1
        {
            get { return SurPlusQuantity; }
            set { SurPlusQuantity = value; }
        }

        public Supplier()
        {}

        public Supplier(int ID, string UIDs, string SDate, bool SFlag, string BatchNo, string TransNo, string SupplierName, string ProductName, int FirstId, int CountToCompare, string Status,Int32 ProdId,Int32 SupplierId,Int32 BatchId,Int32 BatchQuantity,Int32 SurPlusQuantity)
        {
            this.ID = ID;
            this.UIDs = UIDs;
            this.SDate = SDate;
            this.SFlag = SFlag;
            this.BatchNo = BatchNo;
            this.TransNo = TransNo;
            this.SupplierName = SupplierName;
            this.ProductName = ProductName;
            this.FirstId = FirstId;
            this.CountToCompare = CountToCompare;
            this.Status = Status;
            this.BatchId = BatchId;
            this.SupplierId = SupplierId;
            this.ProdId = ProdId;
            this.BatchQuantity = BatchQuantity;
            this.SurPlusQuantity = SurPlusQuantity;
        }
        
    }
}
