using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models
{
    public class Adverse
    {
        public int ID { get; set; }
        public string Event { get; set; }

        [Display(Name = "Averse Event Condition")]
        public string Condition { get; set; }
    }
}