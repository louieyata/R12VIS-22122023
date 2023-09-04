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
    
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            this.Vaccinations = new HashSet<Vaccination>();
        }
    
        public int ID { get; set; }
        public string UniquePersonID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string ContactNumber { get; set; }
        public string GuardianName { get; set; }
        public string Gender { get; set; }
        public bool isPWD { get; set; }
        public Nullable<int> EthnicGroupID { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public Nullable<int> CityMunicipalityID { get; set; }
        public Nullable<int> BarangayID { get; set; }
    
        public virtual Barangay Barangay { get; set; }
        public virtual CityMunicipality CityMunicipality { get; set; }
        public virtual EthnicGroup EthnicGroup { get; set; }
        public virtual Province Province { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vaccination> Vaccinations { get; set; }
    }
}
