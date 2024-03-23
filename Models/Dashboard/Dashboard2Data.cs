namespace R12VIS.Models.Dashboard
{
    public class Dashboard2Data
    {
        public int categoryid { get; set; }
        public string categoryname { get; set; }
        public int firstdose { get; set; }
        public int seconddose { get; set; }
        public int firstbooster { get; set; }
        public int secondbooster { get; set; }
        public int thirdbooster { get; set; }
        public int total { get; set; }
        // add grand total & total per age group on return
    }
}