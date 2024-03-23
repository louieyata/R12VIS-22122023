using System;
using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models
{
    public class IncomeClass
    {
        public int ID { get; set; }
        [Display(Name = "Income Class ID")]
        public string Description { get; set; }
        [Display(Name = "Income Class")]
        public int? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DateLastUpdated { get; set; }
    }
}