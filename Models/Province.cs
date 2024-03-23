using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R12VIS.Models
{
    public class Province
    {
        [Key]
        [Display(Name = "Province ID")]
        public int province_id { get; set; }

        [Display(Name = "Province Name")]
        public string province_name { get; set; }

        [ForeignKey("Region")]
        public int? RegionID { get; set; }
        public Region Region { get; set; }

        [Display(Name = "Province Code")]
        public string province_code { get; set; }
        public string province_code_excel { get; set; }
        public string mrsia_province_code { get; set; }
    }
}