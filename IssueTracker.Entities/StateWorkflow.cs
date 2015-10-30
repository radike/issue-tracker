using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entities
{
    public class StateWorkflow : BaseEntity
    {
        // Foreign keys
        [Required]
        [Display(Name = "From")]
        public Guid FromStateId { get; set; }

        [Required]
        [Display(Name = "To")]
        public Guid ToStateId { get; set; }

        // Table definitions
        public virtual State FromState { get; set; }
        public virtual State ToState { get; set; }

    }
}