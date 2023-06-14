using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace R12VIS.Models
{
    public class Vaccination
    {
        public int ID { get; set; }

        [ForeignKey("PriorityGroup")]
        public int PriorityGroupID { get; set; }
        public PriorityGroup PriorityGroup { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person Person { get; set; }

       
        public int ProvinceID { get; set; }
        public Province Province { get; set; } 

        
        public int CityMunicipalityID { get; set; }
        public CityMunicipality CityMunicipality { get; set; }

        
        public int BarangayID { get; set; }
        public Barangay Barangay { get; set; }

        [ForeignKey("Deferral")]
        public int? DeferralID { get; set; }
        public Deferral Deferral { get; set; }

        public DateTime VaccinationDate { get; set; }

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



    }
}