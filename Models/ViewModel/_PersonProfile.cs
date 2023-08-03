using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace R12VIS.Models.ViewModel
{
    public class _PersonProfile
    {
        public Person Person { get; set; }
        public List<Vaccination> Vaccinations { get; set; }
    }
}