using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
    [Serializable()]
    public class M_Customer
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _ContactPerson;

        public string ContactPerson
        {
            get { return _ContactPerson; }
            set { _ContactPerson = value; }
        }

        private string _ContactNo;

        public string ContactNo
        {
            get { return _ContactNo; }
            set { _ContactNo = value; }
        }
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private Nullable<int> _Country;

        public Nullable<int> Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private Nullable<bool> _IsActive;

        public Nullable<bool> IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        private string _APIKey;

        public string APIKey
        {
            get { return _APIKey; }
            set { _APIKey = value; }
        }
        private string _SenderId;

        public string SenderId
        {
            get { return _SenderId; }
            set { _SenderId = value; }
        }
        private string _ReceiverId;

        public string ReceiverId
        {
            get { return _ReceiverId; }
            set { _ReceiverId = value; }
        }

        private Nullable<DateTime> _CreatedOn;

        public Nullable<DateTime> CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        private Nullable<DateTime> _LastModified;

        public Nullable<DateTime> LastModified
        {
            get { return _LastModified; }
            set { _LastModified = value; }
        }

        private Nullable<int> _CreatedBy;

        public Nullable<int> CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private Nullable<int> _ModifiedBy;

        public Nullable<int> ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        private Nullable<bool> _IsDeleted;

        public Nullable<bool> IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        private string _APIUrl;

        public string APIUrl
        {
            get { return _APIUrl; }
            set { _APIUrl = value; }
        }
        private Nullable<int> _ProviderId;

        public Nullable<int> ProviderId
        {
            get { return _ProviderId; }
            set { _ProviderId = value; }
        }
        private Nullable<bool> _IsSSCC;

        public Nullable<bool> IsSSCC
        {
            get { return _IsSSCC; }
            set { _IsSSCC = value; }
        }

        private string _CompanyCode;

        public string CompanyCode
        {
            get { return _CompanyCode; }
            set { _CompanyCode = value; }
        }
        private string _BizLocGLN;

        public string BizLocGLN
        {
            get { return _BizLocGLN; }
            set { _BizLocGLN = value; }
        }
        private string _BizLocGLN_Ext;

        public string BizLocGLN_Ext
        {
            get { return _BizLocGLN_Ext; }
            set { _BizLocGLN_Ext = value; }
        }

        private Nullable<int> _stateOrRegion;

        public Nullable<int> stateOrRegion
        {
            get { return _stateOrRegion; }
            set { _stateOrRegion = value; }
        }

        private string _city;

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _postalCode;

        public string postalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        private string _License;

        public string License
        {
            get { return _License; }
            set { _License = value; }
        }

        private string _LicenseState;

        public string LicenseState
        {
            get { return _LicenseState; }
            set { _LicenseState = value; }
        }

        private string _LicenseAgency;

        public string LicenseAgency
        {
            get { return _LicenseAgency; }
            set { _LicenseAgency = value; }
        }

        private string _street1;

        public string street1
        {
            get { return _street1; }
            set { _street1 = value; }
        }

        private string _street2;

        public string street2
        {
            get { return _street2; }
            set { _street2 = value; }
        }

        private string _Host;

        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        }

        private string _HostPswd;

        public string HostPswd
        {
            get { return _HostPswd; }
            set { _HostPswd = value; }
        }

        private Nullable<int> _HostPort;

        public Nullable<int> HostPort
        {
            get { return _HostPort; }
            set { _HostPort = value; }
        }

        private string _HostUser;

        public string HostUser
        {
            get { return _HostUser; }
            set { _HostUser = value; }
        }

        private string _Loosext;

        public string Loosext
        {
            get { return _Loosext; }
            set { _Loosext = value; }
        }


        public void M_CustomerBLL(int Id, string CompanyName, string ContactPerson, string ContactNo, string Email, string Address, Nullable<int> Country, Nullable<bool> IsActive, string APIKey, string SenderId, string ReceiverId, Nullable<DateTime> CreatedOn, Nullable<DateTime> LastModified, Nullable<int> CreatedBy, Nullable<int> ModifiedBy, Nullable<bool> IsDeleted, string APIUrl, Nullable<int> ProviderId, Nullable<bool> IsSSCC, string CompanyCode, string BizLocGLN, string BizLocGLN_Ext, Nullable<int> stateOrRegion, string city, string postalCode, string License, string LicenseState, string LicenseAgency, string street1, string street2, string Host, string HostPswd, Nullable<int> HostPort, string HostUser,string Loosext)
        {
            this.Id = Id;
            this.CompanyName = CompanyName;
            this.ContactPerson = ContactPerson;
            this.ContactNo = ContactNo;
            this.Email = Email;
            this.Address = Address;
            this.Country = Country;
            this.IsActive = IsActive;
            this.APIKey = APIKey;
            this.SenderId = SenderId;
            this.ReceiverId = ReceiverId;
            this.CreatedOn = CreatedOn;
            this.LastModified = LastModified;
            this.CreatedBy = CreatedBy;
            this.ModifiedBy = ModifiedBy;
            this.APIUrl = APIUrl;
            this.IsDeleted = IsDeleted;
            this.ProviderId = ProviderId;
            this.IsSSCC = IsSSCC;
            this.CompanyCode = CompanyCode;
            this.BizLocGLN = BizLocGLN;
            this.BizLocGLN_Ext = BizLocGLN_Ext;
            this.stateOrRegion = stateOrRegion;
            this.postalCode = postalCode;
            this.city = city;
            this.License = License;
            this.LicenseAgency = LicenseAgency;
            this.LicenseState = LicenseState;
            this.street1 = street1;
            this.street2 = street2;
            this.Host = Host;
            this.HostPort = HostPort;
            this.HostPswd = HostPswd;
            this.HostUser = HostUser;
            this.Loosext = Loosext;
        }

      
    }

}
