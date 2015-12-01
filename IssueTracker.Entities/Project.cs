using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Project : IIdentifiableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Parameters
        public string Title { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        public ICollection<string> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUser Owner
        {
            get { return Users == null ? null : Users.FirstOrDefault(u => u.Id == OwnerId); }
        }

        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
