using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class UserRoles
    {
        public static UserRoles Administrators = new UserRoles("Administrators");
        public static UserRoles Users = new UserRoles("Users");

        private string name;

        private UserRoles(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}