using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class Users
    {
        [Key]
        public decimal ID { get; set; }

        [Required]
        [Display(Name = "UsersUserId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public  string UserName { get; set; }

        //[RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,8})")]
        //[RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,20}", ErrorMessage = @"Error. Password must have one capital, one special character and one numerical character. It can not start with a special character or a digit. Must be minimum 8 characters.")]
       // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,16}$", ErrorMessage = @"Error. Password must have one special character and one numerical character. It can not start with a special character or a digit. Must be minimum 8 characters.")]
        //[StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "UsersPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Password { get; set; }


        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "SettingChangePwdOldPassword",ResourceType =typeof(TnT.LangResource.GlobalRes))]
        public string OldPassword { get; set; }


        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Password dont match.")]
        [Display(Name = "UsersConfirmPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "UsersIndexRole", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int RoleID { get; set; }

        [Display(Name = "UsersIndexIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool Active { get; set; }

        public bool? IsFirstLogin { get; set; }

        [Display(Name = "UserRemark", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Remarks { get; set; }

        [Display(Name = "UsersIndexCreatedDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime CreatedDate { get; set; }

        
        public DateTime LastUpdatedDate { get; set; }

       
        public int? UserType { get; set; }

        [Required]
        [Display(Name = "UsersUserName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string UserName1 { get; set; }

        [Required]
        [Display(Name = "UsersEmailID", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string EmailId { get; set; }
        //[DataType(DataType.Password)]    
        //public string Password1 { get; set; }

        //[DataType(DataType.Password)]
        //public string Password2 { get; set; }

        //public bool? IsFirstlogin { get; set; }

    }

    public class Roles
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "UsersCreateSelectRole", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Roles_Name { get; set; }

        public string Remarks { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public decimal ID { get; set; }
        public string FirstName { get; set; }     
        public int Role { get; set; }
        public List<string> Permissions { get; set; }

        public string Roles_Name { get; set; }
    }

    public class CustomPermission
    {
        public List<string> Permissions { get; set; }
    }
}