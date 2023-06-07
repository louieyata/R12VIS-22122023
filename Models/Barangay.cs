using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace R12VIS.Models
{
    public class Barangay
    {
        [Key]
        [Display(Name = "Barangay ID")]
        public int barangay_id { get; set; }

        [ForeignKey("CityMunicipality")]
        [Display(Name = "City/Municipality")]
        public int city_municipality_id { get;set; }
        public CityMunicipality CityMunicipality { get; set; }

        [Display(Name = "Barangay Name")]
        public string barangay_name { get; set; }

        [Display(Name = "Province Code")]
        public string province_code { get; set; }

        [Display(Name = "City/Municipality Code")]
        public string city_municipality_code { get; set; }

        [Display(Name = "Barangay Code")]
        public string barangay_code { get; set; }
    }
}