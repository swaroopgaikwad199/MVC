using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using TnT.Models.Product;
using System.ComponentModel.DataAnnotations;
using TnT.DataLayer.Security;

namespace TnT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

      
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<TnT.Models.EPCIS.EpcisEventDetails> EpcisEventDetails { get; set; }
        public DbSet<TnT.Models.EPCIS.Dosage> Dosage { get; set; }
        public DbSet<TnT.Models.EPCIS.Dispositions> Dispositions { get; set; }
        public DbSet<TnT.Models.EPCIS.BizStepMaster> BizStepMaster { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.Country> Country { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.S_State> S_State { get; set; }

        public DbSet<TnT.Models.TraceLinkImporter.X_TracelinkUIDStore> X_TracelinkUIDStore { get; set; }
        public DbSet<TnT.Models.TraceLinkImporter.M_TracelinkRequest> M_TracelinkRequest { get; set; }
        public DbSet<TnT.Models.Code.M_Identities> M_Identities { get; set; }
        public DbSet<TnT.Models.Code.X_Identities> X_Identities { get; set; }
        public DbSet<TnT.Models.Home.ExecutionData> ExecutionData { get; set; }
        public DbSet<TnT.Models.Home.LineIdelTime> LineIdelTime { get; set; }
        public DbSet<TnT.Models.Account.License> License { get; set; }
        public new DbSet<TnT.Models.Account.Users> Users { get; set; }
        public DbSet<TnT.Models.Account.UserLoginPswds> UserLoginPswds { get; set; }
        public DbSet<TnT.Models.Account.M_UserPasswords> M_UserPasswords { get; set; }
        public DbSet<TnT.Models.Account.Logins> Logins { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Customer.M_Customer> M_Customer { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Vendor.M_Vendor> M_Vendor { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Code.X_Code> X_Code { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Code.SSCCLineHolder> SSCCLineHolder { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Code.M_RequestLog> M_RequestLog { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Code.PackagingDetails> PackagingDetails { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.PackagingAsso> PackagingAsso { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.PackagingLevels> PackagingLevels { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.PackagingAssoDetails> PackagingAssoDetails { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.S_DateFormats> S_DateFormats { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.LblLytDsg.S_DPI> S_DPI { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.LblLytDsg.S_ZPLFonts> S_ZPLFonts { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.JOBType> JOBTypes { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Product.S_JobFields> S_JobFields { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.X_JobTypeFields> X_JobTypeFields { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Product.PackageLabelMaster> PackageLabelMaster { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Job.Job> Job { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Job.JobDetails> JobDetails { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Job.LineLocation> LineLocation { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Vendor.ViewModels.NotifyViewModel> NotifyViewModels { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.ServerSideTrails> ServerSideTrails { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.USerTrail> USerTrail { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.AppSettings> AppSettings { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.Settings> Settings { get; set; }
       

        public new System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.Roles> Roles { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.Permissions> Permissions { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.ROLESPermission> ROLESPermission { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Providers.M_Providers> M_Providers { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.TraceLinkImporter.M_SOM> M_SOM { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Reports.RptUserViewModel> RptUserViewModel { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Reports.RptAvailableSerialNo> RptAvailableSerialNo { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Reports.RptTlinkRequest> RptTlinkRequest { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Reports.RptUnusedSrNo> RptUnusedSrNo { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.EPCIS.M_EPCISReceiver> M_EPCISReceiver { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.EPCIS.M_Transporter> M_Transporter { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.EPCIS.M_BizTransactionList> M_BizTransactionList { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Product.ProductImportViewModel> ProductImportModel { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Reports.RptUIDDetailViewModel> RptUIDDetailViewModel { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Exporter.ExporterModelView> ExporterModelViews { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.ImportXml.M_SKMaster> M_SKMaster { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.ImportXml.SK_ObjectKey> SK_ObjectKey { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.ImportXml.ModelViewXmlUIDList> ModelViewXmlUIDList { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Reports.RptDavaViewModel> RptDavaViewModels { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.Reports.S_Activity> S_Activity { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.ImportXml.X_ChinaUIDs> X_ChinaUIDs { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.RFXLImport.RFXLImportViewModel> RFXLImportViewModels { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.AS2.M_ServersAS2> M_ServersAS2 { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.AdditionBatchQty.AdditionBatchQty> AdditionBatchQty { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.ProductApplicatorSetting.ProductApplicatorSetting> ProductApplicatorSetting { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.ProductGlueSetting.ProductGlueSetting> ProductGlueSetting { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.RestoreDb> RestoreDb { get; set; }

        //public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.AlarmsViewModel> AlarmsViewModels { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.Alarms> Alarms { get; set; }
        public System.Data.Entity.DbSet<TnT.Models.SettingsNUtility.RoleAlarms> RoleAlarms { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.Job.M_SMSSync> M_SMSSync { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.TraceKey.TraceKeyViewModel> TraceKeyViewModels { get; set; }

        public System.Data.Entity.DbSet<TnT.Models.TraceKey.M_TkeySerialRequest> M_TkeySerialRequest { get; set; }

        //public System.Data.Entity.DbSet<TnT.Models.Russia.RussiaViewModel> RussiaViewModels { get; set; }
    }
}