using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models
{
    public class Dose
    {
        public int ID { get; set; }

        [Display(Name = "Dose")]
        public string VaccineDose { get; set; }
    }
}