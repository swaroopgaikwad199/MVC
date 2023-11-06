using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class PermissionsBLL
    {
        public enum PermissionOp
        {
            GetAllPermissions = 1,//Param[@Flag]
            GetPermission = 2,//Param[@Flag,@ID as PermissionId]
            GetPermissionByName = 3,

            AddPermission = 1,
            UpdatePermission = 2,
        }
        private PermissionsDAO _PermissionsDAO;

        public PermissionsDAO PermissionsDAO
        {
            get { return _PermissionsDAO; }
            set { _PermissionsDAO = value; }
        }
        public PermissionsBLL()
        {
            PermissionsDAO = new PermissionsDAO();
        }
        public List<Permissions> GetPermissionss()
        {
            try
            {
                return PermissionsDAO.GetPermissionss((int)PermissionOp.GetAllPermissions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Permissions GetPermissionss(int ID)
        {
            try
            {
                return PermissionsDAO.GetPermission((int)PermissionOp.GetPermission, ID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Permissions GetPermissionByName(string Permission)
        {
            try
            {
                return PermissionsDAO.GetPermission((int)PermissionOp.GetPermissionByName, Permission.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdatePermissions(PermissionOp Op, Permissions oPermissions)
        {
            try
            {
                return PermissionsDAO.InsertOrUpdatePermissions((int)Op, oPermissions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemovePermissions(Decimal ID)
        {
            try
            {
                return PermissionsDAO.RemovePermissions(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Permissions> DeserializePermissionss(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<Permissions>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializePermissionss(string Path, List<Permissions> Permissionss)
        {
            try
            {
                GenericXmlSerializer<List<Permissions>>.Serialize(Permissionss, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
