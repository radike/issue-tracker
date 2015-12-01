

using Entities;
using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Entities
{
    public class Issue : IIdentifiableEntity
    {
        // composite primary key
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid ProjectId { get; set; }

        
        public DateTime ProjectCreatedAt { get; set; }
        
        public Guid StateId { get; set; }

        
        public string ReporterId { get; set; }

      
        public string AssigneeId { get; set; }
        
        public bool Active { get; set; }

        // Parameters
        public string Name { get; set; }

        public State State { get; set; }
        
        public DateTime Created { get; set; }

        public string Description { get; set; }
        
        public int CodeNumber { get; set; }

        // Table definitions
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        // Custom properties
        public string Code
        {
            get { return Project.Code + "-" + CodeNumber; }
        }
        
        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
