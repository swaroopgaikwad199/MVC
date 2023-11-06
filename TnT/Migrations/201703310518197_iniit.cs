namespace TnT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iniit : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.AppSettings",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Key = c.String(nullable: false),
            //            Value = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ExecutionData",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            LineCode = c.String(),
            //            ProductName = c.String(),
            //            JobName = c.String(),
            //            Quantity = c.Int(nullable: false),
            //            PackagingLevel = c.String(),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            BatchNo = c.String(),
            //            JobStatus = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.Job",
            //    c => new
            //        {
            //            JID = c.Decimal(nullable: false, precision: 18, scale: 2, identity: true),
            //            JobName = c.String(nullable: false),
            //            PackagingLvlId = c.Int(nullable: false),
            //            PAID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            BatchNo = c.String(nullable: false),
            //            MfgDate = c.DateTime(nullable: false),
            //            ExpDate = c.DateTime(nullable: false),
            //            Quantity = c.Int(nullable: false),
            //            SurPlusQty = c.Int(nullable: false),
            //            JobStatus = c.Byte(nullable: false),
            //            DetailInfo = c.String(),
            //            JobStartTime = c.DateTime(nullable: false),
            //            JobEndTime = c.DateTime(),
            //            LabelStartIndex = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AutomaticBatchCloser = c.Boolean(nullable: false),
            //            TID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            MLNO = c.String(),
            //            TenderText = c.String(),
            //            JobWithUID = c.Boolean(nullable: false),
            //            Remarks = c.String(),
            //            CreatedBy = c.Decimal(precision: 18, scale: 2),
            //            VerifiedBy = c.Decimal(precision: 18, scale: 2),
            //            VerifiedDate = c.DateTime(),
            //            CreatedDate = c.DateTime(nullable: false),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            AppId = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            LineCode = c.String(),
            //            SYNC = c.Boolean(nullable: false),
            //            ForExport = c.Boolean(nullable: false),
            //            PrimaryPCMapCount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DavaPortalUpload = c.Boolean(nullable: false),
            //            PlantCode = c.String(),
            //            NoReadCount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ExpDateFormat = c.String(),
            //            UseExpDay = c.Boolean(nullable: false),
            //            CustomerId = c.Int(),
            //            ProviderId = c.Int(),
            //            PPNCountryCode = c.String(),
            //            PPNPostalCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.JID)
            //    .ForeignKey("dbo.M_Customer", t => t.CustomerId)
            //    .ForeignKey("dbo.PackagingLevels", t => t.PackagingLvlId, cascadeDelete: true)
            //    .ForeignKey("dbo.M_Providers", t => t.ProviderId)
            //    .Index(t => t.PackagingLvlId)
            //    .Index(t => t.CustomerId)
            //    .Index(t => t.ProviderId);
            
            //CreateTable(
            //    "dbo.M_Customer",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false),
            //            ContactPerson = c.String(nullable: false),
            //            ContactNo = c.String(nullable: false),
            //            Email = c.String(nullable: false),
            //            Address = c.String(nullable: false),
            //            Country = c.String(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            APIUrl = c.String(),
            //            APIKey = c.String(),
            //            SenderId = c.String(),
            //            ReceiverId = c.String(),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastModified = c.DateTime(nullable: false),
            //            CreatedBy = c.Int(nullable: false),
            //            ModifiedBy = c.Int(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            IsSSCC = c.Boolean(),
            //            ProviderId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.M_Providers", t => t.ProviderId)
            //    .Index(t => t.ProviderId);
            
            //CreateTable(
            //    "dbo.M_Providers",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50),
            //            CreatedOn = c.DateTime(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PackagingLevels",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Level = c.String(maxLength: 50),
            //            IsActive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.JobDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            JD_JobID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            JD_ProdName = c.String(),
            //            JD_ProdCode = c.String(),
            //            JD_Deckcode = c.String(nullable: false),
            //            JD_PPN = c.String(),
            //            JD_GTIN = c.String(nullable: false),
            //            JD_DeckSize = c.Int(nullable: false),
            //            JD_MRP = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            JD_Description = c.String(),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            LineCode = c.String(),
            //            SYNC = c.Boolean(nullable: false),
            //            GTINCTI = c.String(),
            //            BundleQty = c.Int(nullable: false),
            //            JD_FGCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.JOBType",
            //    c => new
            //        {
            //            TID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Job_Type = c.String(),
            //        })
            //    .PrimaryKey(t => t.TID);
            
            //CreateTable(
            //    "dbo.License",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            LicenseNo = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.LineIdelTime",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            LineCode = c.String(),
            //            JobName = c.String(),
            //            BatchNo = c.String(),
            //            IdlTime = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.LineLocation",
            //    c => new
            //        {
            //            ID = c.String(nullable: false, maxLength: 128),
            //            LocationCode = c.String(),
            //            DivisionCode = c.String(),
            //            PlantCode = c.String(),
            //            LineCode = c.String(),
            //            LineIP = c.String(),
            //            ServerName = c.String(),
            //            DBName = c.String(),
            //            SQLUsername = c.String(),
            //            SQLPassword = c.String(),
            //            IsActive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.M_Identities",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CustomerId = c.Int(nullable: false),
            //            GTIN = c.String(maxLength: 50),
            //            PPN = c.String(maxLength: 50),
            //            CreatedOn = c.DateTime(nullable: false),
            //            PackageTypeCode = c.String(),
            //            JID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            IsExtra = c.Boolean(nullable: false),
            //            IsTransfered = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.M_Customer", t => t.CustomerId, cascadeDelete: true)
            //    .Index(t => t.CustomerId);
            
            //CreateTable(
            //    "dbo.M_RequestLog",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            ServiceKey = c.String(nullable: false),
            //            JobId = c.Int(nullable: false),
            //            MfgDate = c.DateTime(nullable: false),
            //            ExpDate = c.DateTime(nullable: false),
            //            BatchNo = c.String(nullable: false),
            //            Quantity = c.Int(nullable: false),
            //            LineId = c.String(),
            //            SAPCode = c.String(),
            //            Description = c.String(),
            //            RequestDate = c.DateTime(nullable: false),
            //            IsExtraRequested = c.Boolean(nullable: false),
            //            IsSynced = c.Boolean(nullable: false),
            //            SyncedDate = c.DateTime(),
            //            IsReceived = c.Boolean(nullable: false),
            //            AcknowldgeMessage = c.String(),
            //            AcknowldgeDtTm = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.M_SOM",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CreatedOn = c.DateTime(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            BusinessId = c.String(maxLength: 30),
            //            BusinessName = c.String(maxLength: 50),
            //            Street1 = c.String(maxLength: 50),
            //            City = c.String(maxLength: 30),
            //            StateOrRegion = c.String(maxLength: 30),
            //            PostalCode = c.String(maxLength: 10),
            //            Country = c.String(maxLength: 20),
            //            FacilityId_GLN = c.String(maxLength: 30),
            //            FacilityId_SGLN = c.String(maxLength: 30),
            //            SFLI_BusinessName = c.String(maxLength: 50),
            //            SFLI_Street1 = c.String(maxLength: 50),
            //            SFLI_City = c.String(maxLength: 30),
            //            SFLI_StateOrRegion = c.String(maxLength: 30),
            //            SFLI_PostalCode = c.String(maxLength: 10),
            //            SFLI_Country = c.String(maxLength: 20),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.M_TracelinkRequest",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CustomerId = c.Int(nullable: false),
            //            Quatity = c.Int(nullable: false),
            //            GTIN = c.String(),
            //            RequestedOn = c.DateTime(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.M_Customer", t => t.CustomerId, cascadeDelete: true)
            //    .Index(t => t.CustomerId);
            
            //CreateTable(
            //    "dbo.M_UserPasswords",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PasswordOld = c.String(),
            //            PasswordOlder = c.String(),
            //            PasswordOldest = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.M_Vendor",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false),
            //            ContactPerson = c.String(nullable: false),
            //            ContactNo = c.String(nullable: false),
            //            Email = c.String(nullable: false),
            //            Address = c.String(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            ServiceKey = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.NotifyViewModel",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            JID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PackageLabelMaster",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            PAID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            JobTypeID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Code = c.String(),
            //            LabelName = c.String(),
            //            Filter = c.String(),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PackagingAsso",
            //    c => new
            //        {
            //            PAID = c.Decimal(nullable: false, precision: 18, scale: 2, identity: true),
            //            Name = c.String(nullable: false),
            //            ProductCode = c.String(nullable: false),
            //            Description = c.String(),
            //            Remarks = c.String(),
            //            CreatedDate = c.DateTime(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            LineCode = c.String(),
            //            SYNC = c.Boolean(),
            //            ScheduledDrug = c.Boolean(nullable: false),
            //            DoseUsage = c.String(),
            //            GenericName = c.String(),
            //            Composition = c.String(),
            //            ProductImage = c.String(),
            //            DAVAPortalUpload = c.Boolean(),
            //            FGCode = c.String(nullable: false),
            //            VerifyProd = c.Boolean(nullable: false),
            //            ExpDateFormat = c.String(),
            //            PlantCode = c.String(),
            //            SAPProductCode = c.String(),
            //            UseExpDay = c.Boolean(nullable: false),
            //            InternalMaterialCode = c.String(),
            //            CountryDrugCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.PAID);
            
            //CreateTable(
            //    "dbo.PackagingAssoDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            PAID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PackageTypeCode = c.String(),
            //            Size = c.Int(nullable: false),
            //            MRP = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TerCaseIndex = c.Int(nullable: false),
            //            Remarks = c.String(),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            LineCode = c.String(),
            //            SYNC = c.Boolean(nullable: false),
            //            PPN = c.String(),
            //            GTIN = c.String(),
            //            GTINCTI = c.String(),
            //            BundleQty = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PackagingDetails",
            //    c => new
            //        {
            //            PackDtlsID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Code = c.String(),
            //            PAID = c.Decimal(precision: 18, scale: 2),
            //            JobID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PackageTypeCode = c.String(),
            //            MfgPackDate = c.DateTime(nullable: false),
            //            ExpPackDate = c.DateTime(nullable: false),
            //            NextLevelCode = c.String(),
            //            IsRejected = c.Boolean(),
            //            Reason = c.String(),
            //            BadImage = c.String(),
            //            SSCC = c.String(),
            //            SSCCVarificationStatus = c.Boolean(),
            //            IsManualUpdated = c.Boolean(),
            //            ManualUpdateDesc = c.String(),
            //            CaseSeqNum = c.Decimal(precision: 18, scale: 2),
            //            OperatorId = c.Decimal(precision: 18, scale: 2),
            //            Remarks = c.String(),
            //            IsDecomission = c.Boolean(),
            //            CreatedDate = c.DateTime(nullable: false),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            LineCode = c.String(),
            //            SYNC = c.Boolean(),
            //            RCResult = c.Int(),
            //            DavaPortalUpload = c.Boolean(),
            //            IsUsed = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.PackDtlsID);
            
            //CreateTable(
            //    "dbo.Permissions",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Permission = c.String(),
            //            Remarks = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.Roles",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Roles_Name = c.String(nullable: false),
            //            Remarks = c.String(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            RoleId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.ROLESPermission",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2, identity: true),
            //            Roles_Id = c.Int(nullable: false),
            //            Permission_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Remarks = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
            //    .Index(t => t.Permission_Id);
            
            //CreateTable(
            //    "dbo.S_DateFormats",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Format = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.S_DPI",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            dpi = c.String(maxLength: 10),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.S_ZPLFonts",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            font = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.ServerSideTrails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.Int(nullable: false),
            //            Message = c.String(nullable: false),
            //            ActitvityTime = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Settings",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false),
            //            Address = c.String(nullable: false),
            //            Logo = c.String(),
            //            CompanyCode = c.String(),
            //            LineCode = c.String(),
            //            Remarks = c.String(),
            //            PlantCode = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            IAC_CIN = c.String(),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.SSCCLineHolder",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            PackageIndicator = c.Int(nullable: false),
            //            LastSSCC = c.Int(nullable: false),
            //            Remarks = c.String(),
            //            JobID = c.Int(nullable: false),
            //            FirstSSCC = c.Int(nullable: false),
            //            LineCode = c.String(),
            //            RequestId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.Users",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            UserName = c.String(nullable: false),
            //            Password = c.String(nullable: false),
            //            RoleID = c.Int(nullable: false),
            //            Active = c.Boolean(nullable: false),
            //            IsFirstLogin = c.Boolean(),
            //            Remarks = c.String(),
            //            CreatedDate = c.DateTime(nullable: false),
            //            LastUpdatedDate = c.DateTime(nullable: false),
            //            UserName1 = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //        {
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.USerTrail",
            //    c => new
            //        {
            //            ID = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Reason = c.String(nullable: false),
            //            AccessedAt = c.DateTime(nullable: false),
            //            LineCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.X_Code",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            RequestId = c.Int(nullable: false),
            //            Code = c.String(nullable: false),
            //            CodeType = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.X_Identities",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            MasterId = c.Int(nullable: false),
            //            SerialNo = c.String(maxLength: 50),
            //            CodeType = c.Boolean(nullable: false),
            //            PackTypeCode = c.String(maxLength: 3),
            //            IsTransfered = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.M_Identities", t => t.MasterId, cascadeDelete: true)
            //    .Index(t => t.MasterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.X_Identities", "MasterId", "dbo.M_Identities");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ROLESPermission", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.M_TracelinkRequest", "CustomerId", "dbo.M_Customer");
            DropForeignKey("dbo.M_Identities", "CustomerId", "dbo.M_Customer");
            DropForeignKey("dbo.Job", "ProviderId", "dbo.M_Providers");
            DropForeignKey("dbo.Job", "PackagingLvlId", "dbo.PackagingLevels");
            DropForeignKey("dbo.Job", "CustomerId", "dbo.M_Customer");
            DropForeignKey("dbo.M_Customer", "ProviderId", "dbo.M_Providers");
            DropIndex("dbo.X_Identities", new[] { "MasterId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ROLESPermission", new[] { "Permission_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.M_TracelinkRequest", new[] { "CustomerId" });
            DropIndex("dbo.M_Identities", new[] { "CustomerId" });
            DropIndex("dbo.M_Customer", new[] { "ProviderId" });
            DropIndex("dbo.Job", new[] { "ProviderId" });
            DropIndex("dbo.Job", new[] { "CustomerId" });
            DropIndex("dbo.Job", new[] { "PackagingLvlId" });
            DropTable("dbo.X_Identities");
            DropTable("dbo.X_Code");
            DropTable("dbo.USerTrail");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Users");
            DropTable("dbo.SSCCLineHolder");
            DropTable("dbo.Settings");
            DropTable("dbo.ServerSideTrails");
            DropTable("dbo.S_ZPLFonts");
            DropTable("dbo.S_DPI");
            DropTable("dbo.S_DateFormats");
            DropTable("dbo.ROLESPermission");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
            DropTable("dbo.PackagingDetails");
            DropTable("dbo.PackagingAssoDetails");
            DropTable("dbo.PackagingAsso");
            DropTable("dbo.PackageLabelMaster");
            DropTable("dbo.NotifyViewModel");
            DropTable("dbo.M_Vendor");
            DropTable("dbo.M_UserPasswords");
            DropTable("dbo.M_TracelinkRequest");
            DropTable("dbo.M_SOM");
            DropTable("dbo.M_RequestLog");
            DropTable("dbo.M_Identities");
            DropTable("dbo.LineLocation");
            DropTable("dbo.LineIdelTime");
            DropTable("dbo.License");
            DropTable("dbo.JOBType");
            DropTable("dbo.JobDetails");
            DropTable("dbo.PackagingLevels");
            DropTable("dbo.M_Providers");
            DropTable("dbo.M_Customer");
            DropTable("dbo.Job");
            DropTable("dbo.ExecutionData");
            DropTable("dbo.AppSettings");
        }
    }
}
