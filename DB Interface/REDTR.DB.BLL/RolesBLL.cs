using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class RolesBLL
    {
        public enum RolesOp
        {
            GetRoles = 1,
            GetRole = 2,
            GetRoleByName
        }
        private RolesDAO _RolesDAO;

        public RolesDAO RolesDAO
        {
            get { return _RolesDAO; }
            set { _RolesDAO = value; }
        }

        public RolesBLL()
        {
            RolesDAO = new RolesDAO();
        }
        public List<Roles> GetRoles()
        {
            try
            {
                return RolesDAO.GetRoless((int)RolesOp.GetRoles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Roles GetRoles(int ID)
        {
            try
            {
                return RolesDAO.GetRoles((int)RolesOp.GetRole, ID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Roles GetRolesByName(string Value)
        {
            try
            {
                return RolesDAO.GetRoles((int)RolesOp.GetRoleByName, Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddRoles(Roles oRoles)
        {
            try
            {
                return RolesDAO.AddRoles(oRoles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddRolesForSync(Roles oRoles)
        {
            try
            {
                return RolesDAO.AddRolesForSync(oRoles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveRoles(int ID)
        {
            try
            {
                return RolesDAO.RemoveRoles(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Roles> DeserializeRoless(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<Roles>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeRoless(string Path, List<Roles> Roless)
        {
            try
            {
                GenericXmlSerializer<List<Roles>>.Serialize(Roless, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
