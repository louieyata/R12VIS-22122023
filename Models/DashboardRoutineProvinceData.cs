using System;

namespace R12VIS.Models.Dashboard
{
    public class DashboardRoutineProvinceData
    {
        public string Province { get; set; }
        public int? TOTAL { get; set; }
    }

    public partial class LineChartVPDVaccinationProvinceData
    {
        public string VM { get; set; }
        public Nullable<int> NoV { get; set; }
    }

    public partial class LineChartVPDVaccinationCityMunData
    {
        public string VM { get; set; }
        public Nullable<int> NoV { get; set; }
    }

    public partial class LineChartVPDVaccinationBarangayData
    {
        public string VM { get; set; }
        public Nullable<int> NoV { get; set; }
    }

}