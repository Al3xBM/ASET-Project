using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Models.Events
{
    public class Football
    {
        public int numberOfParticipants { get; set; }
        public string location { get; set; }
        public int numberOfRedCards { get; set; }
        public int numberOfInjuredPersons { get; set; }
        public string teamA { get; set; }
        public string teamB { get; set; }
        public int scoreA { get; set; }
        public int scoreB { get; set; }

        public Football(int numberOfParticipants, string location, int numberOfRedCards, int numberOfInjuredPersons,
            string teamA, string teamB, int scoreA, int scoreB)
        {
            this.numberOfParticipants = numberOfParticipants;
            this.location = location;
            this.numberOfRedCards = numberOfRedCards;
            this.numberOfInjuredPersons = numberOfInjuredPersons;
            this.teamA = teamA;
            this.teamB = teamB;
            this.scoreA = scoreA;
            this.scoreB = scoreB;
        }
    }
}
