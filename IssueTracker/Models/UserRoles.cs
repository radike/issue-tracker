using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class UserRoles
    {
        public const String Administrators = "Administrators";
        public static UserRoles AdministratorsUserRole = new UserRoles(Administrators);
        public const String Users = "Users";
        public static UserRoles UsersUserRole = new UserRoles(Users);
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