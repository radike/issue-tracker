using IssueTracker_persistence.entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker_persistence.managers
{
    public class UserManager
    {
        private IssueTrackerDbContext db;

        public UserManager(IssueTrackerDbContext db)
        {
            this.db = db;
        }

        public void CreateUser(User user)
        {
            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            this.db.Users.Remove(user);
            this.db.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();
        }

        public IQueryable<User> GetAllUsers()
        {
            return this.db.Users;
        }
    }
}
