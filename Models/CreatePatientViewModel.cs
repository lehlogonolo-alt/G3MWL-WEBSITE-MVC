namespace G3MWL.Models
{
    public class CreatePatientViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime TreatmentStartDate { get; set; }
        public double StartingWeight { get; set; }

        public double CurrentWeight { get; set; }
        public double Dosage { get; set; }
        public double Results { get; set; }
        public List<string> SideEffects { get; set; }
        public string Notes { get; set; }
    }

}
