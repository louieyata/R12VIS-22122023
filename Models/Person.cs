using R12VIS.Models.CustomDateValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R12VIS.Models
{
    public class Person
    {
        public int ID { get; set; }
        [Display(Name = "Unique Person ID")]
        public string UniquePersonID { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑ\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑ\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑ\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑ\- ']+$", ErrorMessage = "Invalid characters")]
        public string Suffix { get; set; }

        [RegularExpression(@"^[+]?[0-9]{10,12}$", ErrorMessage = "Invalid Contact Number")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        //[RegularExpression(@"^[a-zA-ZñÑ\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Guardian Name")]
        public string GuardianName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "PWD?")]
        public bool isPWD { get; set; }

        [ForeignKey("EthnicGroup")]
        public int? EthnicGroupID { get; set; }
        public EthnicGroup EthnicGroup { get; set; }

        [DateFormatValidation(ErrorMessage = "Invalid date")]
        [BrithDateValidation(ErrorMessage = "Invalid date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("Province")]
        public int? ProvinceID { get; set; }
        public Province Province { get; set; }

        [ForeignKey("CityMunicipality")]
        public int? CityMunicipalityID { get; set; }
        public CityMunicipality CityMunicipality { get; set; }

        [ForeignKey("Barangay")]
        public int? BarangayID { get; set; }
        public Barangay Barangay { get; set; }

    }

}