using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models
{
    public class Vaccine
    {
        public int ID { get; set; }

        [Display(Name = "Vaccine Manufacturer")]
        public string VaccineManufacturer { get; set; }

        [Display(Name = "Vaccine Brand")]
        public string VaccineBrand { get; set; }
    }
}