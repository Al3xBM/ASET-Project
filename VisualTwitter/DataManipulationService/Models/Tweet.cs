namespace DataManipulationService.Models
{
    public class Tweet
    {
        public string id { get; set; }
        public Entity entities { get; set; }
        public string text { get; set; }
        public string lang { get; set; }
        //public PublicMetric PublicMetrics { get; set; }
    }
}
