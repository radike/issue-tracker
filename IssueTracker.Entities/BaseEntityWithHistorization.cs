using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entities
{
    public class BaseEntityWithHistorization
    {
        [Key]
        public Guid Id { get; set; }

        // properties for historization of data
        public DateTime? DeletedAt { get; set; }

        public int CodeNumber { get; set; }

        public BaseEntityWithHistorization()
        {
            DeletedAt = null;
        }
    }
}