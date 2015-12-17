using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entities
{
    public enum IssueType
    {
        [Display(Name = "IssueTypeQuestion", ResourceType = typeof(Locale.IssueStrings))]
        Question,
        [Display(Name = "IssueTypeTask", ResourceType = typeof(Locale.IssueStrings))]
        Task,
        [Display(Name = "IssueTypeBug", ResourceType = typeof(Locale.IssueStrings))]
        Bug
    }
}