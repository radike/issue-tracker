using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Is initial?")]
        public bool IsInitial { get; set; }

        public string Colour { get; set; }
    }
}