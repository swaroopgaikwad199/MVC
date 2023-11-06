using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        public LoginViewModel()
        {
            Attempts = 0;
            
        }
       
        [Required(ErrorMessageResourceName = "UserLoginuseridfieldReq", ErrorMessageResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Display(Name = "UsersUserId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string username { get; set; }


        [Required(ErrorMessageResourceName = "UserLoginPasswordfieldReq", ErrorMessageResourceType = typeof(TnT.LangResource.GlobalRes))]
        [DataType(DataType.Password)]
        [Display(Name = "UsersPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "UsersRembMe", ErrorMessageResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Display(Name = "UsersRembMe", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool RememberMe { get; set; }
        
  
        public int? Attempts { get; set; }
        public string prevUser { get; set; }

    }
}