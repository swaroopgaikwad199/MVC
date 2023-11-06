namespace TnT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class as2moduleAdded : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.BizStepMaster",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            BizStep = c.String(maxLength: 50),
            //            CommonIsactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Country",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CountryName = c.String(),
            //            TwoLetterAbbreviation = c.String(),
            //            ThreeLetterAbbreviation = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Dispositions",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            BizStepId = c.Int(nullable: false),
            //            Disposition = c.String(maxLength: 50),
            //            IsReused = c.Boolean(nullable: false),
            //            Action = c.String(maxLength: 20),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Dosage",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DosageName = c.String(maxLength: 100),
            //            UseRestrictions = c.String(maxLength: 50),
            //            ShortName = c.String(maxLength: 50),
            //            FDACode = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            NCIConceptID = c.String(maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.EpcisEventDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            JobId = c.Int(nullable: false),
            //            UUID = c.String(maxLength: 50),
            //            EventType = c.String(maxLength: 50),
            //            BizStepId = c.Int(nullable: false),
            //            CreationDate = c.DateTime(),
            //            CreatedBy = c.Int(),
            //            EventTime = c.DateTime(),
            //            RecordTime = c.DateTime(),
            //            EventTimeZoneOffset = c.String(maxLength: 50),
            //            ParentID = c.String(maxLength: 100),
            //            ChildEPC = c.String(unicode: false, storeType: "text"),
            //            EpcList = c.String(unicode: false, storeType: "text"),
            //            Action = c.String(maxLength: 50),
            //            BizStep = c.String(maxLength: 50),
            //            Disposition = c.String(maxLength: 50),
            //            ReadPoint = c.String(maxLength: 100),
            //            BizLocation = c.String(maxLength: 100),
            //            BizTransactionList = c.String(unicode: false, storeType: "text"),
            //            ExtensionData1 = c.String(unicode: false, storeType: "text"),
            //            ExtensionData2 = c.String(unicode: false, storeType: "text"),
            //            UserData1 = c.String(unicode: false, storeType: "text"),
            //            UserData2 = c.String(unicode: false, storeType: "text"),
            //            UserData3 = c.String(unicode: false, storeType: "text"),
            //            EpcisVersion = c.Double(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ExporterModelView",
            //    c => new
            //        {
            //            JobName = c.String(nullable: false, maxLength: 128),
            //            PackagingType = c.String(nullable: false),
            //            Username = c.String(nullable: false),
            //            Password = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.JobName);
            
            //CreateTable(
            //    "dbo.S_State",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            StateName = c.String(),
            //            CountryID = c.Int(nullable: false),
            //            displayorder = c.Int(nullable: false),
            //            TwoLetterAbbreviation = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.M_BizTransactionList",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Value = c.String(maxLength: 50),
            //            Type = c.String(maxLength: 50),
            //            Definition = c.String(maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.M_EPCISReceiver",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false),
            //            CountryId = c.Int(nullable: false),
            //            StateId = c.Int(nullable: false),
            //            City = c.String(nullable: false),
            //            Street1 = c.String(nullable: false),
            //            Street2 = c.String(nullable: false),
            //            PostalCode = c.String(nullable: false),
            //            GLN = c.String(nullable: false),
            //            site = c.String(),
            //            street3 = c.String(),
            //            countryCode = c.String(),
            //            latitude = c.Decimal(precision: 18, scale: 2),
            //            logitude = c.Decimal(precision: 18, scale: 2),
            //            CreatedBy = c.Int(),
            //            IsActive = c.Boolean(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastModified = c.DateTime(nullable: false),
            //            ModifiedBy = c.Int(),
            //            Extension = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
            //    .ForeignKey("dbo.S_State", t => t.StateId, cascadeDelete: true)
            //    .Index(t => t.CountryId)
            //    .Index(t => t.StateId);
            
            CreateTable(
                "dbo.M_ServersAS2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        HostAddress = c.String(nullable: false, maxLength: 200),
                        HostPort = c.Int(nullable: false),
                        FromName = c.String(nullable: false, maxLength: 200),
                        ToName = c.String(nullable: false, maxLength: 200),
                        HostPublicKeyPath = c.String(nullable: false, maxLength: 300),
                        SelfPublicKeyPath = c.String(nullable: false, maxLength: 300),
                        SelfPrivateKeyPath = c.String(nullable: false, maxLength: 300),
                        SelfPrivateKeyPassword = c.String(nullable: false, maxLength: 300),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.M_SKMaster",
            //    c => new
            //        {
            //            MID = c.Int(nullable: false, identity: true),
            //            ReceivingSystem = c.String(),
            //            ActionCode = c.String(),
            //            IDType = c.String(),
            //            NumberFrom = c.String(),
            //            NumberTo = c.String(),
            //            EncodingType = c.String(),
            //            IsUsed = c.Boolean(nullable: false),
            //            NFrom = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Nto = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.MID);
            
            //CreateTable(
            //    "dbo.M_Transporter",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false),
            //            ContactNo = c.String(nullable: false),
            //            EmailId = c.String(nullable: false),
            //            Address = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.ModelViewXmlUIDList",
            //    c => new
            //        {
            //            JobID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Deck = c.String(),
            //            iNoofRecPerFile = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            StrReaderId = c.String(),
            //            StrCommand = c.String(),
            //            StrDeviceId = c.String(),
            //            StrUOM = c.String(),
            //        })
            //    .PrimaryKey(t => t.JobID);
            
            //CreateTable(
            //    "dbo.ProductImportViewModel",
            //    c => new
            //        {
            //            Name = c.String(nullable: false, maxLength: 128),
            //            ProductCode = c.String(),
            //            Description = c.String(),
            //            Remarks = c.String(),
            //            ScheduledDrug = c.String(),
            //            DoseUsage = c.String(),
            //            GenericName = c.String(),
            //            Composition = c.String(),
            //            FGCode = c.String(),
            //            UseExpDay = c.String(),
            //            ExpDateFormat = c.String(),
            //            SAPProductCode = c.String(),
            //            InternalMaterialCode = c.String(),
            //            CountryDrugCode = c.String(),
            //            PrimaryPackBox = c.String(),
            //            PPBSize = c.String(),
            //            PPBBundleQty = c.String(),
            //            MonoCarton = c.String(),
            //            MOCSize = c.String(),
            //            MOCBundleQty = c.String(),
            //            OuterBox = c.String(),
            //            OBXSize = c.String(),
            //            OBXBundleQty = c.String(),
            //            InnerShipper = c.String(),
            //            ISHSize = c.String(),
            //            ISHBundleQty = c.String(),
            //            OuterShipper = c.String(),
            //            OSHSize = c.String(),
            //            OSHBundleQty = c.String(),
            //            Pallet = c.String(),
            //            PALSize = c.String(),
            //            PALBundleQty = c.String(),
            //        })
            //    .PrimaryKey(t => t.Name);
            
            //CreateTable(
            //    "dbo.RFXLImportViewModel",
            //    c => new
            //        {
            //            CustomerId = c.Int(nullable: false, identity: true),
            //            Quantity = c.Int(nullable: false),
            //            GTIN = c.String(),
            //        })
            //    .PrimaryKey(t => t.CustomerId);
            
            //CreateTable(
            //    "dbo.RptAvailableSerialNo",
            //    c => new
            //        {
            //            CompanyName = c.String(nullable: false, maxLength: 128),
            //            Address = c.String(),
            //            UserName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CompanyName);
            
            //CreateTable(
            //    "dbo.SerailNo",
            //    c => new
            //        {
            //            SerailNoCount = c.String(nullable: false, maxLength: 128),
            //            GTIN = c.String(),
            //            RptAvailableSerialNo_CompanyName = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.SerailNoCount)
            //    .ForeignKey("dbo.RptAvailableSerialNo", t => t.RptAvailableSerialNo_CompanyName)
            //    .Index(t => t.RptAvailableSerialNo_CompanyName);
            
            //CreateTable(
            //    "dbo.RptDavaViewModel",
            //    c => new
            //        {
            //            CompanyName = c.String(nullable: false, maxLength: 128),
            //            Address = c.String(),
            //            UserNAme = c.String(),
            //            ReportType = c.String(),
            //            stat = c.String(),
            //        })
            //    .PrimaryKey(t => t.CompanyName);
            
            //CreateTable(
            //    "dbo.DavaData",
            //    c => new
            //        {
            //            DavaType = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(),
            //            GenerationDate = c.DateTime(nullable: false),
            //            Status = c.String(),
            //            Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SSCCDone = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            RptDavaViewModel_CompanyName = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.DavaType)
            //    .ForeignKey("dbo.RptDavaViewModel", t => t.RptDavaViewModel_CompanyName)
            //    .Index(t => t.RptDavaViewModel_CompanyName);
            
            //CreateTable(
            //    "dbo.RptTlinkRequest",
            //    c => new
            //        {
            //            CompanyName = c.String(nullable: false, maxLength: 128),
            //            Address = c.String(),
            //            UserName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CompanyName);
            
            //CreateTable(
            //    "dbo.RequestedGtin",
            //    c => new
            //        {
            //            Quantity = c.Int(nullable: false, identity: true),
            //            GTIN = c.String(),
            //            Customer = c.String(),
            //            RequestedOn = c.DateTime(nullable: false),
            //            RptTlinkRequest_CompanyName = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Quantity)
            //    .ForeignKey("dbo.RptTlinkRequest", t => t.RptTlinkRequest_CompanyName)
            //    .Index(t => t.RptTlinkRequest_CompanyName);
            
            //CreateTable(
            //    "dbo.RptUIDDetailViewModel",
            //    c => new
            //        {
            //            JID = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(),
            //            Address = c.String(),
            //            UserName = c.String(),
            //            Type = c.String(),
            //            JobType = c.String(),
            //            TID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            BPR = c.String(),
            //            BatchNo = c.String(),
            //            ProductName = c.String(),
            //            ProductCode = c.String(),
            //            GTIN = c.String(),
            //            MfgDate = c.DateTime(nullable: false),
            //            ExpDate = c.DateTime(nullable: false),
            //            BatchQty = c.Int(nullable: false),
            //            JobWithUid = c.String(),
            //            LocationCode = c.String(),
            //            DivCode = c.String(),
            //            PlantCode = c.String(),
            //            LineCode = c.String(),
            //            InspectionSet = c.String(),
            //            PackagingType = c.String(),
            //            PackagingDate = c.DateTime(nullable: false),
            //            Operator = c.String(),
            //            Status = c.String(),
            //            SSCC = c.String(),
            //            CaseNo = c.String(),
            //            SSCCVerifiedStatus = c.String(),
            //            parentCode = c.String(),
            //            UIDCode = c.String(),
            //            FailureReason = c.String(),
            //        })
            //    .PrimaryKey(t => t.JID);
            
            //CreateTable(
            //    "dbo.ChildCode",
            //    c => new
            //        {
            //            SrNo = c.String(nullable: false, maxLength: 128),
            //            RptUIDDetailViewModel_JID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.SrNo)
            //    .ForeignKey("dbo.RptUIDDetailViewModel", t => t.RptUIDDetailViewModel_JID)
            //    .Index(t => t.RptUIDDetailViewModel_JID);
            
            //CreateTable(
            //    "dbo.RptUnusedSrNo",
            //    c => new
            //        {
            //            CompanyName = c.String(nullable: false, maxLength: 128),
            //            Address = c.String(),
            //            UserName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CompanyName);
            
            //CreateTable(
            //    "dbo.UnusedSrno",
            //    c => new
            //        {
            //            SrNo = c.String(nullable: false, maxLength: 128),
            //            GTIN = c.String(),
            //            RptUnusedSrNo_CompanyName = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.SrNo)
            //    .ForeignKey("dbo.RptUnusedSrNo", t => t.RptUnusedSrNo_CompanyName)
            //    .Index(t => t.RptUnusedSrNo_CompanyName);
            
            //CreateTable(
            //    "dbo.RptUserViewModel",
            //    c => new
            //        {
            //            CompanyName = c.String(nullable: false, maxLength: 128),
            //            Address = c.String(),
            //            UserName = c.String(),
            //            Status = c.String(),
            //            ReportType = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.CompanyName);
            
            //CreateTable(
            //    "dbo.UDetail",
            //    c => new
            //        {
            //            RoleName = c.String(nullable: false, maxLength: 128),
            //            UserName = c.String(),
            //            Status = c.String(),
            //            CreatedDate = c.DateTime(nullable: false),
            //            LastUpdated = c.DateTime(nullable: false),
            //            RptUserViewModel_CompanyName = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.RoleName)
            //    .ForeignKey("dbo.RptUserViewModel", t => t.RptUserViewModel_CompanyName)
            //    .Index(t => t.RptUserViewModel_CompanyName);
            
            //CreateTable(
            //    "dbo.S_Activity",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Activity = c.String(),
            //            Type = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.SK_ObjectKey",
            //    c => new
            //        {
            //            OID = c.Int(nullable: false, identity: true),
            //            MID = c.Int(nullable: false),
            //            Name = c.String(),
            //            Value = c.String(),
            //            IsUsed = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.OID);
            
            //CreateTable(
            //    "dbo.X_ChinaUIDs",
            //    c => new
            //        {
            //            Id = c.Decimal(nullable: false, precision: 18, scale: 2, identity: true),
            //            Code = c.String(),
            //            PAID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PackageTypeCode = c.String(),
            //            IsUsed = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Job", "TransferToGlobal", c => c.Boolean());
            //AddColumn("dbo.M_Customer", "CompanyCode", c => c.String());
            //AddColumn("dbo.M_Customer", "BizLocGLN", c => c.String());
            //AddColumn("dbo.M_Customer", "BizLocGLN_Ext", c => c.String());
            //AddColumn("dbo.M_Customer", "stateOrRegion", c => c.Int(nullable: false));
            //AddColumn("dbo.M_Customer", "city", c => c.String());
            //AddColumn("dbo.M_Customer", "postalCode", c => c.String());
            //AddColumn("dbo.M_Customer", "License", c => c.String());
            //AddColumn("dbo.M_Customer", "LicenseState", c => c.String());
            //AddColumn("dbo.M_Customer", "LicenseAgency", c => c.String());
            //AddColumn("dbo.M_Customer", "street1", c => c.String());
            //AddColumn("dbo.M_Customer", "street2", c => c.String());
            //AddColumn("dbo.M_Customer", "Host", c => c.String());
            //AddColumn("dbo.M_Customer", "HostUser", c => c.String());
            //AddColumn("dbo.M_Customer", "HostPswd", c => c.String());
            //AddColumn("dbo.M_Customer", "HostPort", c => c.Int(nullable: false));
            //AddColumn("dbo.M_Customer", "ToCorpID", c => c.String());
            //AddColumn("dbo.M_Customer", "SSCCExt", c => c.String());
            //AddColumn("dbo.M_Customer", "LoosExt", c => c.String());
            //AddColumn("dbo.M_Providers", "Code", c => c.String());
            //AddColumn("dbo.LineLocation", "LineName", c => c.String());
            //AddColumn("dbo.LineLocation", "ReadGLN", c => c.String());
            //AddColumn("dbo.LineLocation", "GLNExtension", c => c.String());
            //AddColumn("dbo.M_SOM", "DeliveryNumber", c => c.String());
            //AddColumn("dbo.M_SOM", "DeliveryCompleteFlag", c => c.Boolean(nullable: false));
            //AddColumn("dbo.M_SOM", "TransactionIdentifier", c => c.String());
            //AddColumn("dbo.M_SOM", "TransactionDate", c => c.DateTime(nullable: false));
            //AddColumn("dbo.M_SOM", "SalesDistributionType", c => c.String());
            //AddColumn("dbo.M_SOM", "IsSerialized", c => c.Boolean(nullable: false));
            //AddColumn("dbo.M_SOM", "FromBusinessPartyLookupId", c => c.String());
            //AddColumn("dbo.M_SOM", "ShipFromLocationLookupId", c => c.String());
            //AddColumn("dbo.M_SOM", "ToBusinessPartyLookupId", c => c.String());
            //AddColumn("dbo.M_TracelinkRequest", "Threshold", c => c.Int(nullable: false));
            //AddColumn("dbo.M_TracelinkRequest", "ProviderId", c => c.Int());
            //AddColumn("dbo.M_TracelinkRequest", "SrnoType", c => c.String());
            //AddColumn("dbo.PackagingAsso", "DosageForm", c => c.String());
            //AddColumn("dbo.PackagingAsso", "FEACN", c => c.String());
            //AddColumn("dbo.PackagingAsso", "ProviderId", c => c.Int());
            //AddColumn("dbo.PackagingAsso", "SubTypeNo", c => c.String());
            //AddColumn("dbo.PackagingAsso", "PackageSpec", c => c.String());
            //AddColumn("dbo.PackagingAsso", "AuthorizedNo", c => c.String());
            //AddColumn("dbo.PackagingAsso", "SubType", c => c.String());
            //AddColumn("dbo.PackagingAsso", "SubTypeSpec", c => c.String());
            //AddColumn("dbo.PackagingAsso", "PackUnit", c => c.String());
            //AddColumn("dbo.PackagingAsso", "ResProdCode", c => c.String());
            //AddColumn("dbo.PackagingAsso", "Workshop", c => c.String());
            //AddColumn("dbo.Permissions", "IsActive", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ServerSideTrails", "Reason", c => c.String(nullable: false));
            //AddColumn("dbo.ServerSideTrails", "Activity", c => c.String(nullable: false));
            //AddColumn("dbo.ServerSideTrails", "RoleId", c => c.Int(nullable: false));
            //AddColumn("dbo.Settings", "GLN", c => c.String());
            //AddColumn("dbo.Settings", "Street", c => c.String());
            //AddColumn("dbo.Settings", "StateOrRegion", c => c.Int(nullable: false));
            //AddColumn("dbo.Settings", "City", c => c.String());
            //AddColumn("dbo.Settings", "PostalCode", c => c.String());
            //AddColumn("dbo.Settings", "Country", c => c.Int(nullable: false));
            //AddColumn("dbo.Settings", "License", c => c.String());
            //AddColumn("dbo.Settings", "LicenseState", c => c.String());
            //AddColumn("dbo.Settings", "LicenseAgency", c => c.String());
            //AddColumn("dbo.SSCCLineHolder", "Type", c => c.String());
            //AddColumn("dbo.SSCCLineHolder", "Customer", c => c.Int(nullable: false));
            //AddColumn("dbo.Users", "UserType", c => c.Int());
            //AddColumn("dbo.X_TracelinkUIDStore", "GTIN", c => c.String());
            //AlterColumn("dbo.M_Customer", "Country", c => c.Int());
            //AlterColumn("dbo.M_Customer", "IsSSCC", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.Settings", "PlantCode", c => c.String());
            //AlterColumn("dbo.SSCCLineHolder", "PackageIndicator", c => c.Int());
            //AlterColumn("dbo.SSCCLineHolder", "LastSSCC", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AlterColumn("dbo.SSCCLineHolder", "JobID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AlterColumn("dbo.SSCCLineHolder", "FirstSSCC", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AlterColumn("dbo.SSCCLineHolder", "RequestId", c => c.Int());
            //CreateIndex("dbo.M_Customer", "Country");
            //CreateIndex("dbo.M_Customer", "stateOrRegion");
            //CreateIndex("dbo.PackagingAsso", "ProviderId");
            //CreateIndex("dbo.ROLESPermission", "Roles_Id");
            //CreateIndex("dbo.Settings", "StateOrRegion");
            //CreateIndex("dbo.Settings", "Country");
            //AddForeignKey("dbo.M_Customer", "Country", "dbo.Country", "Id");
            //AddForeignKey("dbo.M_Customer", "stateOrRegion", "dbo.S_State", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.PackagingAsso", "ProviderId", "dbo.M_Providers", "Id");
            //AddForeignKey("dbo.ROLESPermission", "Roles_Id", "dbo.Roles", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.Settings", "Country", "dbo.Country", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Settings", "StateOrRegion", "dbo.S_State", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Settings", "StateOrRegion", "dbo.S_State");
            DropForeignKey("dbo.Settings", "Country", "dbo.Country");
            DropForeignKey("dbo.UDetail", "RptUserViewModel_CompanyName", "dbo.RptUserViewModel");
            DropForeignKey("dbo.UnusedSrno", "RptUnusedSrNo_CompanyName", "dbo.RptUnusedSrNo");
            DropForeignKey("dbo.ChildCode", "RptUIDDetailViewModel_JID", "dbo.RptUIDDetailViewModel");
            DropForeignKey("dbo.RequestedGtin", "RptTlinkRequest_CompanyName", "dbo.RptTlinkRequest");
            DropForeignKey("dbo.DavaData", "RptDavaViewModel_CompanyName", "dbo.RptDavaViewModel");
            DropForeignKey("dbo.SerailNo", "RptAvailableSerialNo_CompanyName", "dbo.RptAvailableSerialNo");
            DropForeignKey("dbo.ROLESPermission", "Roles_Id", "dbo.Roles");
            DropForeignKey("dbo.PackagingAsso", "ProviderId", "dbo.M_Providers");
            DropForeignKey("dbo.M_EPCISReceiver", "StateId", "dbo.S_State");
            DropForeignKey("dbo.M_EPCISReceiver", "CountryId", "dbo.Country");
            DropForeignKey("dbo.M_Customer", "stateOrRegion", "dbo.S_State");
            DropForeignKey("dbo.M_Customer", "Country", "dbo.Country");
            DropIndex("dbo.Settings", new[] { "Country" });
            DropIndex("dbo.Settings", new[] { "StateOrRegion" });
            DropIndex("dbo.UDetail", new[] { "RptUserViewModel_CompanyName" });
            DropIndex("dbo.UnusedSrno", new[] { "RptUnusedSrNo_CompanyName" });
            DropIndex("dbo.ChildCode", new[] { "RptUIDDetailViewModel_JID" });
            DropIndex("dbo.RequestedGtin", new[] { "RptTlinkRequest_CompanyName" });
            DropIndex("dbo.DavaData", new[] { "RptDavaViewModel_CompanyName" });
            DropIndex("dbo.SerailNo", new[] { "RptAvailableSerialNo_CompanyName" });
            DropIndex("dbo.ROLESPermission", new[] { "Roles_Id" });
            DropIndex("dbo.PackagingAsso", new[] { "ProviderId" });
            DropIndex("dbo.M_EPCISReceiver", new[] { "StateId" });
            DropIndex("dbo.M_EPCISReceiver", new[] { "CountryId" });
            DropIndex("dbo.M_Customer", new[] { "stateOrRegion" });
            DropIndex("dbo.M_Customer", new[] { "Country" });
            AlterColumn("dbo.SSCCLineHolder", "RequestId", c => c.Int(nullable: false));
            AlterColumn("dbo.SSCCLineHolder", "FirstSSCC", c => c.Int(nullable: false));
            AlterColumn("dbo.SSCCLineHolder", "JobID", c => c.Int(nullable: false));
            AlterColumn("dbo.SSCCLineHolder", "LastSSCC", c => c.Int(nullable: false));
            AlterColumn("dbo.SSCCLineHolder", "PackageIndicator", c => c.Int(nullable: false));
            AlterColumn("dbo.Settings", "PlantCode", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.M_Customer", "IsSSCC", c => c.Boolean());
            AlterColumn("dbo.M_Customer", "Country", c => c.String(nullable: false));
            DropColumn("dbo.X_TracelinkUIDStore", "GTIN");
            DropColumn("dbo.Users", "UserType");
            DropColumn("dbo.SSCCLineHolder", "Customer");
            DropColumn("dbo.SSCCLineHolder", "Type");
            DropColumn("dbo.Settings", "LicenseAgency");
            DropColumn("dbo.Settings", "LicenseState");
            DropColumn("dbo.Settings", "License");
            DropColumn("dbo.Settings", "Country");
            DropColumn("dbo.Settings", "PostalCode");
            DropColumn("dbo.Settings", "City");
            DropColumn("dbo.Settings", "StateOrRegion");
            DropColumn("dbo.Settings", "Street");
            DropColumn("dbo.Settings", "GLN");
            DropColumn("dbo.ServerSideTrails", "RoleId");
            DropColumn("dbo.ServerSideTrails", "Activity");
            DropColumn("dbo.ServerSideTrails", "Reason");
            DropColumn("dbo.Permissions", "IsActive");
            DropColumn("dbo.PackagingAsso", "Workshop");
            DropColumn("dbo.PackagingAsso", "ResProdCode");
            DropColumn("dbo.PackagingAsso", "PackUnit");
            DropColumn("dbo.PackagingAsso", "SubTypeSpec");
            DropColumn("dbo.PackagingAsso", "SubType");
            DropColumn("dbo.PackagingAsso", "AuthorizedNo");
            DropColumn("dbo.PackagingAsso", "PackageSpec");
            DropColumn("dbo.PackagingAsso", "SubTypeNo");
            DropColumn("dbo.PackagingAsso", "ProviderId");
            DropColumn("dbo.PackagingAsso", "FEACN");
            DropColumn("dbo.PackagingAsso", "DosageForm");
            DropColumn("dbo.M_TracelinkRequest", "SrnoType");
            DropColumn("dbo.M_TracelinkRequest", "ProviderId");
            DropColumn("dbo.M_TracelinkRequest", "Threshold");
            DropColumn("dbo.M_SOM", "ToBusinessPartyLookupId");
            DropColumn("dbo.M_SOM", "ShipFromLocationLookupId");
            DropColumn("dbo.M_SOM", "FromBusinessPartyLookupId");
            DropColumn("dbo.M_SOM", "IsSerialized");
            DropColumn("dbo.M_SOM", "SalesDistributionType");
            DropColumn("dbo.M_SOM", "TransactionDate");
            DropColumn("dbo.M_SOM", "TransactionIdentifier");
            DropColumn("dbo.M_SOM", "DeliveryCompleteFlag");
            DropColumn("dbo.M_SOM", "DeliveryNumber");
            DropColumn("dbo.LineLocation", "GLNExtension");
            DropColumn("dbo.LineLocation", "ReadGLN");
            DropColumn("dbo.LineLocation", "LineName");
            DropColumn("dbo.M_Providers", "Code");
            DropColumn("dbo.M_Customer", "LoosExt");
            DropColumn("dbo.M_Customer", "SSCCExt");
            DropColumn("dbo.M_Customer", "ToCorpID");
            DropColumn("dbo.M_Customer", "HostPort");
            DropColumn("dbo.M_Customer", "HostPswd");
            DropColumn("dbo.M_Customer", "HostUser");
            DropColumn("dbo.M_Customer", "Host");
            DropColumn("dbo.M_Customer", "street2");
            DropColumn("dbo.M_Customer", "street1");
            DropColumn("dbo.M_Customer", "LicenseAgency");
            DropColumn("dbo.M_Customer", "LicenseState");
            DropColumn("dbo.M_Customer", "License");
            DropColumn("dbo.M_Customer", "postalCode");
            DropColumn("dbo.M_Customer", "city");
            DropColumn("dbo.M_Customer", "stateOrRegion");
            DropColumn("dbo.M_Customer", "BizLocGLN_Ext");
            DropColumn("dbo.M_Customer", "BizLocGLN");
            DropColumn("dbo.M_Customer", "CompanyCode");
            DropColumn("dbo.Job", "TransferToGlobal");
            DropTable("dbo.X_ChinaUIDs");
            DropTable("dbo.SK_ObjectKey");
            DropTable("dbo.S_Activity");
            DropTable("dbo.UDetail");
            DropTable("dbo.RptUserViewModel");
            DropTable("dbo.UnusedSrno");
            DropTable("dbo.RptUnusedSrNo");
            DropTable("dbo.ChildCode");
            DropTable("dbo.RptUIDDetailViewModel");
            DropTable("dbo.RequestedGtin");
            DropTable("dbo.RptTlinkRequest");
            DropTable("dbo.DavaData");
            DropTable("dbo.RptDavaViewModel");
            DropTable("dbo.SerailNo");
            DropTable("dbo.RptAvailableSerialNo");
            DropTable("dbo.RFXLImportViewModel");
            DropTable("dbo.ProductImportViewModel");
            DropTable("dbo.ModelViewXmlUIDList");
            DropTable("dbo.M_Transporter");
            DropTable("dbo.M_SKMaster");
            DropTable("dbo.M_ServersAS2");
            DropTable("dbo.M_EPCISReceiver");
            DropTable("dbo.M_BizTransactionList");
            DropTable("dbo.S_State");
            DropTable("dbo.ExporterModelView");
            DropTable("dbo.EpcisEventDetails");
            DropTable("dbo.Dosage");
            DropTable("dbo.Dispositions");
            DropTable("dbo.Country");
            DropTable("dbo.BizStepMaster");
        }
    }
}
