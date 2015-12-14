using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace IssueTracker.ViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "AccountNewPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "AccountConfirmNewPassword", ResourceType = typeof(Locale.AccountStrings))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorNewPasswordConfirmation")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "AccountCurrentPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "AccountNewPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "AccountConfirmNewPassword", ResourceType = typeof(Locale.AccountStrings))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorNewPasswordConfirmation")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "AccountPhoneNumber", ResourceType = typeof(Locale.AccountStrings))]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "AccountCode", ResourceType = typeof(Locale.AccountStrings))]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "AccountPhoneNumber", ResourceType = typeof(Locale.AccountStrings))]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}