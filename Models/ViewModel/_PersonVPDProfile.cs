using System.Collections.Generic;

namespace R12VIS.Models.ViewModel
{
    public class _PersonVPDProfile
    {
        public Person_VPD Person { get; set; }
        public List<Vaccination_VPD> Vaccinations { get; set; }
        public string Province { get; set; }
        public string CityMunicipality { get; set; }
        public string Barangay { get; set; }
        public string Religion { get; set; }
        public string Occupation { get; set; }
        public string EducationalAttainment { get; set; }
        public string IncomeClass { get; set; }
    }
}