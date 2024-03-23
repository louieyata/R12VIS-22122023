using System;

namespace R12VIS.Models.CustomModels
{
    public class ModelIndividual
    {
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
        public int? EthnicGroupID { get; set; }
        public DateTime BirthDate { get; set; }
        public int ProvinceID { get; set; }
        public int CityMunicipalityID { get; set; }
        public int BarangayID { get; set; }
        public int SiblingRank { get; set; }
        public int Religion { get; set; }
        public int EducationalAttainment { get; set; }
        public int IncomeClass { get; set; }
        public int Occupation { get; set; }

    }
}