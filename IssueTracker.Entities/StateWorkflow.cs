using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Entities
{
    public class StateWorkflow : IIdentifiableEntity
    {
        public Guid Id { get; set; }

        // Foreign keys
        public Guid FromStateId { get; set; }

        public Guid ToStateId { get; set; }

        // Table definitions
        public virtual State FromState { get; set; }
        public virtual State ToState { get; set; }
    }
}
