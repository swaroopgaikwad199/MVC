
namespace REDTR.DB.BLL
{
    public class BLLManager
    {
        private USerTrailBLL _USerTrailBLL;

        public USerTrailBLL USerTrailBLL
        {
            get { return _USerTrailBLL; }
            set { _USerTrailBLL = value; }
        }

        private DAVAExportFileTagsInfoBLL _DAVAExportFileTagsInfoBLL;

        public DAVAExportFileTagsInfoBLL DAVAExportFileTagsInfoBLL
        {
            get { return _DAVAExportFileTagsInfoBLL; }
            set { _DAVAExportFileTagsInfoBLL = value; }
        }

        private SSCCLineHolderBLL _SSCCLineHolderBLL;

        public SSCCLineHolderBLL SSCCLineHolderBLL
        {
            get { return _SSCCLineHolderBLL; }
            set { _SSCCLineHolderBLL = value; }
        }

        private DavaFileRunningSeqNoBLL _DavaFileRunningSeqNoBLL;

        public DavaFileRunningSeqNoBLL DavaFileRunningSeqNoBLL
        {
            get { return _DavaFileRunningSeqNoBLL; }
            set { _DavaFileRunningSeqNoBLL = value; }
        }

        private SettingsBLL _SettingsBLL;

        public SettingsBLL SettingsBLL
        {
            get { return _SettingsBLL; }
            set { _SettingsBLL = value; }
        }

        private PermissionsBLL _PermissionsBLL;

        public PermissionsBLL PermissionsBLL
        {
            get { return _PermissionsBLL; }
            set { _PermissionsBLL = value; }
        }

        private PackagingDetailsBLL _PackagingDetailsBLL;

        public PackagingDetailsBLL PackagingDetailsBLL
        {
            get { return _PackagingDetailsBLL; }
            set { _PackagingDetailsBLL = value; }
        }

        private LineLocationBLL _LineLocationBLL;

        public LineLocationBLL LineLocationBLL
        {
            get { return _LineLocationBLL; }
            set { _LineLocationBLL = value; }
        }
        private PackageLabelAssoBLL _PackageLabelBLL;  // For Product wise batch allocation [Sunil]

        public PackageLabelAssoBLL PackageLabelBLL
        {
            get { return _PackageLabelBLL; }
            set { _PackageLabelBLL = value; }
        }

        private PackageTypeCodeBLL _PackageTypeCodeBLL;

        public PackageTypeCodeBLL PackageTypeCodeBLL
        {
            get { return _PackageTypeCodeBLL; }
            set { _PackageTypeCodeBLL = value; }
        }

        private JOBTypeBLL _JOBTypeBLL;

        public JOBTypeBLL JOBTypeBLL
        {
            get { return _JOBTypeBLL; }
            set { _JOBTypeBLL = value; }
        }

        private MLNOBLL _MLNOBLL;

        public MLNOBLL MLNOBLL
        {
            get { return _MLNOBLL; }
            set { _MLNOBLL = value; }
        }

        private JobBLL _JobBLL;

        public JobBLL JobBLL
        {
            get { return _JobBLL; }
            set { _JobBLL = value; }
        }

        private JobDetailsBLL _JobDetailsBLL;

        public JobDetailsBLL JobDetailsBLL
        {
            get { return _JobDetailsBLL; }
            set { _JobDetailsBLL = value; }
        }

        private PackagingAssoDetailsBLL _PackagingAssoDetailsBLL;

        public PackagingAssoDetailsBLL PackagingAssoDetailsBLL
        {
            get { return _PackagingAssoDetailsBLL; }
            set { _PackagingAssoDetailsBLL = value; }
        }

        private PackagingAssoBLL _PackagingAssoBLL;

        public PackagingAssoBLL PackagingAssoBLL
        {
            get { return _PackagingAssoBLL; }
            set { _PackagingAssoBLL = value; }
        }

        private DAVAExportDetailsBLL _DAVAExportDetailsBLL;

        public DAVAExportDetailsBLL DAVAExportDetailsBLL
        {
            get { return _DAVAExportDetailsBLL; }
            set { _DAVAExportDetailsBLL = value; }
        }

        private RolesBLL _RolesBLL;

        public RolesBLL RolesBLL
        {
            get { return _RolesBLL; }
            set { _RolesBLL = value; }
        }

        private ROLESPermissionBLL _ROLESPermissionBLL;

        public ROLESPermissionBLL ROLESPermissionBLL
        {
            get { return _ROLESPermissionBLL; }
            set { _ROLESPermissionBLL = value; }
        }

        private UsersBLL _UsersBLL;

        public UsersBLL UsersBLL
        {
            get { return _UsersBLL; }
            set { _UsersBLL = value; }
        }

        private JobAssoDeckBLL _JobAssoDeckBLL;
        public JobAssoDeckBLL JobAssoDeckBLL
        {
            get { return _JobAssoDeckBLL; }
            set { _JobAssoDeckBLL = value; }
        }
        private CountryBLL _CountryBLL;

        public CountryBLL CountryBLL
        {
            get { return _CountryBLL; }
            set { _CountryBLL = value; }
        }

        private PrimaryPackDummysBLL _PrimaryPackDummysBLL;

        public PrimaryPackDummysBLL PrimaryPackDummysBLL
        {
            get { return _PrimaryPackDummysBLL; }
            set { _PrimaryPackDummysBLL = value; }
        }
        private SupplierBLL _SupplierBLL;

        public SupplierBLL SupplierBLL
        {
            get { return _SupplierBLL; }
            set { _SupplierBLL = value; }
        }
        private ChinaUIDBLL _ChinaUIDBLL;
        public ChinaUIDBLL ChinaUIDBLL
        {
            get { return _ChinaUIDBLL; }
            set { _ChinaUIDBLL = value; }
        }

        private M_CustomerBLL _M_CustomerBLL;
        public M_CustomerBLL M_CustomerBLL
        {
            get { return _M_CustomerBLL; }
            set { _M_CustomerBLL = value; }
        }

        private ProductApplicatorSettingBLL _ProductApplicatorSettingBLL;
        public ProductApplicatorSettingBLL ProductApplicatorSettingBLL
        {
            get { return _ProductApplicatorSettingBLL; }
            set { _ProductApplicatorSettingBLL = value; }
        }

        private ProductGlueSettingBLL _ProductGlueSettingBLL;

        public ProductGlueSettingBLL ProductGlueSettingBLL
        {
            get { return _ProductGlueSettingBLL; }
            set { _ProductGlueSettingBLL = value; }
        }

        public BLLManager()
        {
            USerTrailBLL = new USerTrailBLL();
            SSCCLineHolderBLL = new SSCCLineHolderBLL();
            SettingsBLL = new SettingsBLL();
            PermissionsBLL = new PermissionsBLL();
            PackagingDetailsBLL = new PackagingDetailsBLL();
            PackageTypeCodeBLL = new PackageTypeCodeBLL();
            JOBTypeBLL = new JOBTypeBLL();
            MLNOBLL = new MLNOBLL();
            JobBLL = new JobBLL();
            JobDetailsBLL = new JobDetailsBLL();
            PackagingAssoDetailsBLL = new PackagingAssoDetailsBLL();
            PackagingAssoBLL = new PackagingAssoBLL();
            RolesBLL = new RolesBLL();
            ROLESPermissionBLL = new ROLESPermissionBLL();
            UsersBLL = new UsersBLL();
            JobAssoDeckBLL = new JobAssoDeckBLL();
            SupplierBLL = new SupplierBLL();
            ChinaUIDBLL = new ChinaUIDBLL();
            CountryBLL = new CountryBLL();
            PrimaryPackDummysBLL = new PrimaryPackDummysBLL();
            DAVAExportDetailsBLL = new DAVAExportDetailsBLL();
            DAVAExportFileTagsInfoBLL = new DAVAExportFileTagsInfoBLL();
            DavaFileRunningSeqNoBLL = new DavaFileRunningSeqNoBLL();
            PackageLabelBLL = new PackageLabelAssoBLL();
            LineLocationBLL = new LineLocationBLL();
            M_CustomerBLL = new M_CustomerBLL();
            ProductApplicatorSettingBLL = new ProductApplicatorSettingBLL();
            ProductGlueSettingBLL = new ProductGlueSettingBLL();
        }

        public bool CloseDB()
        {
            return REDTR.DB.DAL.DbProviderHelper.CloseConn();
        }
    }
}
