using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.Security
{
    public class RolePermissions
    {
        private bool[] AssignUserPermission(Users CurrentUser)
        {
            try
            {
                DbHelper objDBHelper = new DbHelper();
                bool a = false;
                bool[] array = new bool[30];
                List<REDTR.DB.BusinessObjects.ROLESPermission> RP2 = objDBHelper.DBManager.ROLESPermissionBLL.GetROLESPermissions(ROLESPermissionBLL.ROLESPermissionOp.GetROLESPermissionsOfRoles, CurrentUser.RoleID);
                List<REDTR.DB.BusinessObjects.Permissions> p = objDBHelper.DBManager.PermissionsBLL.GetPermissionss();
                //CheckBox chkbox = new CheckBox();
                int i = 0;
                array = new bool[p.Count];
                foreach (Permissions u1 in p)
                {
                    string Role = objDBHelper.GetPermission((int)u1.ID);
                    foreach (ROLESPermission u2 in RP2)
                    {
                        if (u2.Permission_Id == u1.ID)
                        {
                            a = true;
                            break;
                        }
                        else
                            a = false;
                    }
                    array[i] = a;
                    i++;
                }

                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }

  




}