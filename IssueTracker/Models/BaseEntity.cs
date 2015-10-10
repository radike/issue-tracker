using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}