using System;

namespace R12VIS.Models.Dashboard
{
    public class RoutineVaxxDashboardData
    {
        public string Vaccine { get; set; }
        public Nullable<int> AT_BIRTH { get; set; }
        public Nullable<int> C1ST_VISIT { get; set; }
        public Nullable<int> C2ND_VISIT { get; set; }
        public Nullable<int> C3RD_VISIT { get; set; }
        public Nullable<int> C4TH_VISIT { get; set; }
        public Nullable<int> C5TH_VISIT { get; set; }
    }
}