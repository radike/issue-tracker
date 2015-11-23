using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StateWorkflow : IIdentifiableEntity
    {
        public Guid Id { get; set; }

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

        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
