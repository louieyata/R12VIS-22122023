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
        [Display(Name = "Uniqu Person ID")]
        public string UniquePersonID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Suffix { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Guardian Name")]
        public string GuardianName { get; set; }

        [Display(Name = "Gender")]
        public bool isMale{ get; set; }

        [Display(Name = "Is PWD")]
        public bool isPWD { get; set; }

        [ForeignKey("EthnicGroup")]
        public int? EthnicGroupID { get; set; }
        public EthnicGroup EthnicGroup { get; set; }

        public DateTime? BirthDate { get; set; }


        public int ProvinceID { get; set; }
        public Province Province { get; set; }


        public int CityMunicipalityID { get; set; }
        public CityMunicipality CityMunicipality { get; set; }


        public int BarangayID { get; set; }
        public Barangay Barangay { get; set; }


    }
}