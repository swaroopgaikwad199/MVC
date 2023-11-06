using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace TnT.DataLayer.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
           
            if (role == "Administrator")
            {
                return true;
            }
           else if(role == "QA")
            {
                return true;
            }
            else if (role == "Operator")
            {
                return true;
            }
            else if (role == "Supervisor")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public decimal ID { get; set; }
        public string FirstName { get; set; }        
        public int roles { get; set; }
        public List<string> permissions { get; set; }

        public string Roles_Name { get; set; }
    }
}