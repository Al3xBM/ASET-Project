using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Models.Events
{
    public class Tennis
    {
        public string location { get; set; }
        public int numberOfPointsA { get; set; }
        public int numberOfPointsB { get; set; }
        public string participantA { get; set; }
        public string participantB { get; set; }
        public int scoreA { get; set; }
        public int scoreB { get; set; }
    }
}
