using System;
using System.Collections.Generic;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
namespace REDTR.DB.BLL
{
    public partial class JobBLL
    {
        public enum ReportOp
        {
            /// <summary>
            /// @Value1 -primary deck
            /// @Value2- tertiary deck
            /// @AppID- application ID
            /// @JobFromDate-Job from Date
            /// @JobToDate-Job To date
            /// @IsDecomissioned
            /// </summary>
            /// 
            ForALLRpt = 1,
           
            /// <summary>
            ///@Flag only, Parameters not required ....
            /// </summary>
            ForProdDtls = 2,

            /// <summary>
            /// @Value1 null
            /// @AppId App ID
            /// jobstatus-JobStatus
            /// </summary>
            ForJobDtls = 3,

            /// <summary>
            /// Value1 -  Primary deck code
            /// Value2 -null
            /// AppID - app ID
            /// JobFromDate -null
            /// JobTodate-null
            /// IsDecomissioned-@IsDecomissioned
            /// jobstatus-JobStatus
            /// </summary>
            ForOperatorDtls = 4,

            /// <summary>
            /// Value1 - JobID
            /// Value2 - deck code          
            /// @IsDecomissioned
            /// </summary>
            ReconciliationCount=5,
            /// <summary>
            /// @Value1 -JobID
            /// @IsDecomissioned
            /// </summary>
            Forprintingproductionsummary = 6,

            /// <summary>
            /// @Value1 -JobID
            /// @Value2 -Deck code (optional)
            /// @AppID -AppID
            /// @IsDecomissioned-IsDecomissioned
            /// </summary>
            ForJobdeckProcess = 7


            
        }    

        public DataSet GetReportDataSet(ReportOp mJOp, string Value1, string Value2, Nullable<int> AppID, Nullable<DateTime> JobFromDate, Nullable<DateTime> JobToDate, bool isDecomission, int jobstatus)
        {
            try
            {
                return JobDAO.GetReportDataSet((int)mJOp, Value1, Value2, AppID, JobFromDate, JobToDate, isDecomission ,jobstatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public partial class UsersBLL
    {   
    }
    public partial class RolesBLL
    {
        public void SaveDefaultRoles()
        {
            try
            {
                //To add Administrator
                Roles mrole = new Roles();
                mrole.Roles_Name = "Administrator";
                RolesDAO.AddRoles(mrole);

                //To add Operator
                mrole = new Roles();
                mrole.Roles_Name = "Operator";
                RolesDAO.AddRoles(mrole);

                //To add Supervisor
                mrole = new Roles();
                mrole.Roles_Name = "Supervisor";
                RolesDAO.AddRoles(mrole);

                //To add QA
                mrole = new Roles();
                mrole.Roles_Name = "QA";
                RolesDAO.AddRoles(mrole);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
    }
    public partial class PermissionsBLL
    {
        public void savePermssion()
        {
            try
            {

                Permissions Prm = new Permissions();
                Prm.Permission = "Add Project";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "By Pass Jobs";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Delete Jobs";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Edit Project";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Print Report";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "View Report";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Access Adv Settings";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Delete User";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);

                Prm = new Permissions();
                Prm.Permission = "Set Camera";
                PermissionsDAO.InsertOrUpdatePermissions((int)PermissionOp.AddPermission, Prm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
    }
    public partial class RolesPermissionsBLL
    {
    }
    public partial class PackagingAssoBLL
    {
        //public DataSet GetProductDetails()
        //{
        //    try
        //    {
        //        return PackagingAssoDAO.GetProductDetails();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
    public partial class SSCCLineHolderBLL
    {
    }
    public partial class UserTrailBLL
    {

    }
    public partial class PackagingDetailsBLL
    {
        public bool GEtVerifiedUIDStatus(string JobID, string Code)
        {
            try
            {
                return PackagingDetailsDAO.GEtUIDStatus((int)OP.GET_VerifiedUIDStatus, JobID, Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdatePcMAp(string UID, string NextLevelCode)
        {
            try
            {
                PackagingDetails Pck = new PackagingDetails();
                Pck.Code = UID;
                Pck.NextLevelCode = NextLevelCode;
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails((int)OP.UpdatePCMAp, Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateTertiary(string UID, string SSCC, decimal CaseSeqNo)
        {
            try
            {
                PackagingDetails Pck = new PackagingDetails();
                Pck.Code = UID;
                Pck.SSCC = SSCC;
                Pck.CaseSeqNum = CaseSeqNo;
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails((int)OP.UpdateTertiary, Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateResultStatus(bool IsFailure, string failureReason, string UID)
        {
            try
            {
                PackagingDetails Pck = new PackagingDetails();
                Pck.IsRejected = IsFailure;
                Pck.Reason = failureReason;
                Pck.Code = UID;
                Pck.RCResult = 0;// Convert.ToInt16(!IsFailure);
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails((int)OP.UpdateResultStatusnReason, Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateRelationCamResult(bool IsFailure, string UID)
        {
            try
            {
                PackagingDetails Pck = new PackagingDetails();
                Pck.RCResult = Convert.ToInt32(IsFailure);
                //Pck.Reason = failureReason;
                Pck.Code = UID;
                return PackagingDetailsDAO.InsertOrUpdatePackagingDetails((int)OP.UpdateRelationResult, Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetIncompleteDeckUid(decimal JobId, string childDeck,string ParentDeck, int parentDeckPcMapSize, out int QtyAdded,string defaultUidNotInclude)
        {
            try
            {
                return PackagingDetailsDAO.GetIncompleteDeckUid((int)PackagingDetailsBLL.OP.GetIncompleteParents, JobId, childDeck,ParentDeck, parentDeckPcMapSize, out QtyAdded, defaultUidNotInclude);
            }
            catch (Exception)
            {
                throw;
            }
        }
         
    }
}

