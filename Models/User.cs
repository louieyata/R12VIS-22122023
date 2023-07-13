using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using R12VIS.Models;
using System.Data.Entity.Migrations;



namespace R12VIS.Models
{
    public class User
    {

        public int ID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Status")]
        public bool isActive { get; set; } = true;

        [ForeignKey("Role")]
        [Display(Name = "Role")]
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }

    public class UserDAL
    {
        private DbContextR12 db;

        public UserDAL()
        {
            db = new DbContextR12();
        }
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
            User user = db.Users.Where(x => x.Email.ToLower() == email.ToLower() && x.Password == password).FirstOrDefault();
    

            return user;
            
        }
       
    }
}