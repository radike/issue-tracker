using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}