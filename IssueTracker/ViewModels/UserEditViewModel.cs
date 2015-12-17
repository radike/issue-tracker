using System;
using System.ComponentModel.DataAnnotations;
using IssueTracker.Locale;

namespace IssueTracker.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "ApplicationUserEmail", ResourceType = typeof(UserStrings))]
        public string Email { get; set; }
        
        [Phone]
        [Display(Name = "ApplicationUserPhone", ResourceType = typeof(UserStrings))]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "ApplicationUserUsername", ResourceType = typeof(UserStrings))]
        public string UserName { get; set; }
    }
}