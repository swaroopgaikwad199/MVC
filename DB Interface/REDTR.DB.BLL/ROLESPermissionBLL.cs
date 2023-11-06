using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class ROLESPermissionBLL
    {
        public enum ROLESPermissionOp
        {
            GetAllROLESPermissions = 1,//[@Flag as Numeric]
            GetROLESPermission = 2,//[@Flag,@ID as RolePermissionId]
            GetROLESPermissionsOfRoles = 3,//[@Flag,@ID as RoleId]
            GetROLESPermissionsOfPermissions = 4,
            AddRolePermission = 1,
            UpdateRolePermission = 2,
            RemoveRolesPermission=1,
            RemoveRolePermissionByRoleID=1,
            RemoveRolePermissionByPermissionID=2
        }
        private ROLESPermissionDAO _ROLESPermissionDAO;

        public ROLESPermissionDAO ROLESPermissionDAO
        {
            get { return _ROLESPermissionDAO; }
            set { _ROLESPermissionDAO = value; }
        }

        public ROLESPermissionBLL()
        {
            ROLESPermissionDAO = new ROLESPermissionDAO();
        }

        public List<ROLESPermission> GetROLESPermissions(ROLESPermissionOp Op, Nullable<Decimal> ID)
        {
            try
            {
                return ROLESPermissionDAO.GetROLESPermissions((int)Op, ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ROLESPermission GetROLESPermission(ROLESPermissionOp Op, Decimal ID)
        {
            try
            {
                return ROLESPermissionDAO.GetROLESPermission((int)Op, ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddROLESPermission(ROLESPermission oROLESPermission)
        {
            try
            {
                return ROLESPermissionDAO.AddROLESPermission(oROLESPermission);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddROLESPermission(ROLESPermissionOp Op, ROLESPermission oROLESPermission)//Added by Arvind on 04.05.2015
        {
            try
            {
                return ROLESPermissionDAO.AddROLESPermission((int)Op, oROLESPermission);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveROLESPermission(ROLESPermissionOp Op,Decimal RoleID ,int PermissionID )
        {
            try
            {
                return ROLESPermissionDAO.RemoveROLESPermission((int)Op,RoleID,PermissionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ROLESPermission> DeserializeROLESPermissions(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<ROLESPermission>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeROLESPermissions(string Path, List<ROLESPermission> ROLESPermissions)
        {
            try
            {
                GenericXmlSerializer<List<ROLESPermission>>.Serialize(ROLESPermissions, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
