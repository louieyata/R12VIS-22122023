using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace R12VIS.Models.ViewModel
{
    public class _VaccinedPersons
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Dose Dose { get; set; }

        public Vaccine Vaccine { get; set; }
        public string PriorityGroup { get; set; }
    }
}