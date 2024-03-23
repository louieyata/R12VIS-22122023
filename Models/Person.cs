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

        [RegularExpression(@"^[a-zA-ZÒ—\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-ZÒ—\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^[a-zA-ZÒ—\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-ZÒ—\- ']+$", ErrorMessage = "Invalid characters")]
        public string Suffix { get; set; }

        [RegularExpression(@"^[+]?[0-9]{10,12}$", ErrorMessage = "Invalid Contact Number")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        //[RegularExpression(@"^[a-zA-ZÒ—\- ']+$", ErrorMessage = "Invalid characters")]
        [Display(Name = "Guardian Name")]
        public string GuardianName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "PWD?")]
        public bool isPWD { get; set; }

        [ForeignKey("EthnicGroup")]
        [Display(Name = "Ethnic Group")]
        public int? EthnicGroupID { get; set; }
        public EthnicGroup EthnicGroup { get; set; }

        [DateFormatValidation(ErrorMessage = "Invalid date")]
        [BrithDateValidation(ErrorMessage = "Invalid date")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("Province")]
        public int? ProvinceID { get; set; }
        public Province Province { get; set; }

        [ForeignKey("CityMunicipality")]
        [Display(Name = "City/Municipality?")]
        public int? CityMunicipalityID { get; set; }
        public CityMunicipality CityMunicipality { get; set; }

        [ForeignKey("Barangay")]
        public int? BarangayID { get; set; }
        public Barangay Barangay { get; set; }

        [ForeignKey("Religion")]
        public int? ReligionID { get; set; }
        public Religion Religion { get; set; }

        [ForeignKey("Occupation")]
        public int? OccupationID { get; set; }
        public Occupation Occupation { get; set; }

        [ForeignKey("EducationalAttainment")]
        public int? EducationalAttainmentID { get; set; }
        public EducationalAttainment EducationalAttainment { get; set; }

        [ForeignKey("IncomeClass")]
        public int? IncomeClassID { get; set; }
        public IncomeClass IncomeClass { get; set; }

        [Display(Name = "Sibling Rank")]
        public int SiblingRank { get; set; }
    }
}