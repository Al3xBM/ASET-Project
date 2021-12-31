using MongoDB.Bson;
using System.Collections.Generic;

namespace ClusteringComponent.Models
{
    public class Team
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public List<string> Aliases { get; set; }

        public List<string> Tags { get; set; }
    }
}
