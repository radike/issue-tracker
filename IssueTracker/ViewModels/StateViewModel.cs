using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "StateTitle", ResourceType = typeof(Locale.StateStrings))]
        public string Title { get; set; }

        [Required]
        [Display(Name = "StateIsInitial", ResourceType = typeof(Locale.StateStrings))]
        public bool IsInitial { get; set; }

        [Display(Name = "StateColour", ResourceType = typeof(Locale.StateStrings))]
        public string Colour { get; set; }

        public int OrderIndex { get; set; }

    }
}