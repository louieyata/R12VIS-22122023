//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace R12VIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vaccination_VPD
    {
        public int vaccination_id { get; set; }
        public int vaccination_person_id { get; set; }
        public int vaccination_region_id { get; set; }
        public int vaccination_province_id { get; set; }
        public int vaccination_city_municipality_id { get; set; }
        public int vaccination_barangay_id { get; set; }
        public int vaccination_vaccine_id { get; set; }
        public System.DateTime vaccination_date { get; set; }
        public string vaccination_vaccinator { get; set; }
        public Nullable<int> vaccination_type_id { get; set; }
    
        public virtual Person_VPD Person_VPD { get; set; }
        public virtual Vaccine_VPD Vaccine_VPD { get; set; }
    }
}