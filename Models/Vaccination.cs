using R12VIS.Models.CustomDateValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R12VIS.Models
{
    public class Vaccination
    {
        public int ID { get; set; }

        [ForeignKey("PriorityGroup")]
        public int PriorityGroupID { get; set; }
        public PriorityGroup PriorityGroup { get; set; }

        public string Comorbidity { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person Person { get; set; }

        [ForeignKey("Deferral")]
        public int? DeferralID { get; set; }
        public Deferral Deferral { get; set; }

        [DateFormatValidation(ErrorMessage = "Invalid Vaciination Date")]
        [DataType(DataType.Date)]
        public DateTime? VaccinationDate { get; set; }

        [ForeignKey("Vaccine")]
        public int VaccineID { get; set; }
        public Vaccine Vaccine { get; set; }

        [Display(Name = "Batch Number")]
        public string BatchNumber { get; set; }

        [Display(Name = "Lot Number")]
        public string LotNumber { get; set; }

        [Display(Name = "Bakuna Center CBCR ID")]
        public string BakunaCenterCBCRID { get; set; }

        [Display(Name = "Vaccinator Name")]
        public string VaccinatorName { get; set; }

        public int DoseID { get; set; }
        public Dose Dose { get; set; }

        [Display(Name = "Adverse Event")]
        public int? AdverseID { get; set; }
        public Adverse Adverse { get; set; }

        [DateFormatValidation(ErrorMessage = "Invalid date")]
        [DataType(DataType.Date)]
        public DateTime DateCreate { get; set; } = DateTime.Now.Date;

        public string CreatedBy { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }
    }
}