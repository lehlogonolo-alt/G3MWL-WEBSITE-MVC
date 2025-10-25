namespace G3MWL.Models
{
    public class WeeklyReport
    {
        public int WeekNumber { get; set; }

        // Match MongoDB types
        public double? Weight { get; set; }   // Number in MongoDB
        public string Dosage { get; set; }    // String in MongoDB
        public List<string> SideEffects { get; set; } = new();
        public string Results { get; set; }
        public string Notes { get; set; }
    }
}









