using MongoDB.Bson.Serialization.Attributes;

namespace ClusteringComponent.Models
{
    public class Tweet
    {
        [BsonId]
        public string id { get; set; }

        public Entity entities { get; set; }

        public string text { get; set; }

        public string lang { get; set; }

        public PublicMetric public_metrics { get; set; }
    }
}
