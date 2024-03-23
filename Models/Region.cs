using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models
{
    public class Region
    {
        public int ID { get; set; }

        [Display(Name = "Region Name")]
        public string RegionName { get; set; }
    }
}