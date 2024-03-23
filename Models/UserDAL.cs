using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace R12VIS.Models
{
    public class UserDAL
    {
        private DbContextR12 db = new DbContextR12();
        public List<User> GetUsers()
        {
            var users = db.Users.Include(u => u.Role);
            return users.ToList();
        }
        public User GetUser(int id)
        {
            var user = db.Users
                .Find(id);

            return user;
        }
        public void CreateUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            //return user;
        }

        public void UpdateUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = db.Users.Include(x => x.Role).Where(x => x.Email.ToLower() == email.ToLower() && x.Password == password).FirstOrDefault();

            return user;
        }

    }
}