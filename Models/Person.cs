using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace R12VIS.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string UniquePersonID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public string GuardianName { get; set; }
        public bool isMale{ get; set; }

        public bool isPWD { get; set; }

        [ForeignKey("EthnicGroup")]
        public int? EthnicGroupID { get; set; }
        public EthnicGroup EthnicGroup { get; set; }

        public DateTime BirthDate { get; set; }

    }
}