namespace IssueTracker.Models
{
    public class UserRoles
    {
        public const string Administrators = "Administrators";
        public static UserRoles AdministratorsUserRole = new UserRoles(Administrators);
        public const string Users = "Users";
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