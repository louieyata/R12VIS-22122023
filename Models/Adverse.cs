using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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