using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "AccountEmail", ResourceType = typeof(Locale.AccountStrings))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "AccountCode", ResourceType = typeof(Locale.AccountStrings))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "AccountRememberBrowser", ResourceType = typeof(Locale.AccountStrings))]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "AccountEmail", ResourceType = typeof(Locale.AccountStrings))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "PlaceholderEmailAddress", ResourceType = typeof(Locale.AccountStrings)), DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PlaceholderPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Locale.AccountStrings))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "AccountEmail", ResourceType = typeof(Locale.AccountStrings))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "AccountPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "AccountConfirmPassword", ResourceType = typeof(Locale.AccountStrings))]
        [Compare("Password", ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordConfirmation")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "AccountEmail", ResourceType = typeof(Locale.AccountStrings))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "AccountPassword", ResourceType = typeof(Locale.AccountStrings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "AccountConfirmPassword", ResourceType = typeof(Locale.AccountStrings))]
        [Compare("Password", ErrorMessageResourceType = typeof(Locale.AccountStrings), ErrorMessageResourceName = "AccountErrorPasswordConfirmation")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "AccountEmail", ResourceType = typeof(Locale.AccountStrings))]
        public string Email { get; set; }
    }
}
