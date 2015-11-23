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
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public bool Active { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        [NotMapped]
        [Display(Name = "Users")]
        public ICollection<string> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        [Display(Name = "Project owner")]
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
