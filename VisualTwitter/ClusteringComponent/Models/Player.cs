using MongoDB.Bson;

namespace ClusteringComponent.Models
{
    public class Player
    {
        public ObjectId _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Team { get; set; }

        public string TeamAbbr { get; set; }

        public string Position { get; set; }

        public int Number { get; set; }

        public int Height { get; set; }
    }
}
