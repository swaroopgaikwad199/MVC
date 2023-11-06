using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class ResetPswViewModel
    {

      //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{7,20}$", ErrorMessage = @"Error. Password must have one special character and one numerical character. It can not start with a special character or a digit. Must be minimum 7 characters.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "SetPasswordNewPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Password { get; set; }


     
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Password dont match.")]
        [Display(Name = "SetPasswordConfirmNewPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ConfirmPassword { get; set; }

        public int Uid { get; set; }
    }
}