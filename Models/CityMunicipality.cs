using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R12VIS.Models
{
    public class CityMunicipality
    {
        [Key]
        [Display(Name = "City/Municipality ID")]
        public int city_municipality_id { get; set; }

        [ForeignKey("Province")]
        public int province_id { get; set; }
        public Province Province { get; set; }

        [Display(Name = "City/Municipality Name")]
        public string CityMunicipalityName { get; set; }

        [Display(Name = "Province Code")]
        public string ProvinceCode { get; set; }
        [Display(Name = "City/Municipality Code")]
        public string CityMunicipalityCode { get; set; }

        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; }

        public string CityMunicipalityCodeExcel { get; set; }
        public string mrsia_city_municipality_code { get; set; }
    }
}