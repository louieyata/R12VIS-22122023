using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace R12VIS.Models
{
    public class Dose
    {
        public int ID { get; set; }

        [Display(Name = "Dose")]
        public string VaccineDose { get; set; }
    }
}