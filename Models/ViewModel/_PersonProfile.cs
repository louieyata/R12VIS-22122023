using System.Collections.Generic;

namespace R12VIS.Models.ViewModel
{
    // _PersonProfile.cs
    public class _PersonProfile
    {
        public Person Person { get; set; }
        public string Categories { get; set; }
        public List<Vaccination> Vaccinations { get; set; }
    }
}