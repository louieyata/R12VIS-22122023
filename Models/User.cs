using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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
}