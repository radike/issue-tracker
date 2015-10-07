using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker_persistence.entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Nickname { get; set; }

        [Required]
        public String Password { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }
    }
}
