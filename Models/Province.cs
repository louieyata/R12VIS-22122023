using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace R12VIS.Models
{
    public class Province
    {
        [Key]
        [Display(Name = "Province ID")]
        public int province_id { get; set; }

        [Display(Name = "Province Name")]
        public string province_name { get; set;}

        [Display(Name = "Province Code")]
        public string province_code { get; set; }
    }
}