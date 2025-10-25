namespace G3MWL.Models
{
    public class Patient
    {
        public string _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime TreatmentStartDate { get; set; }
        public double StartingWeight { get; set; }
        public List<WeeklyReport> WeeklyReports { get; set; }
    }

}
