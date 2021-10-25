namespace ClusteringComponent.Models.Events
{
    public class FootballBuilder
    {
        public int numberOfParticipants;
        public string location;
        public int numberOfRedCards;
        public int numberOfInjuredPersons;
        public string teamA;
        public string teamB;
        public int scoreA;
        public int scoreB;

        public FootballBuilder SetNumberOfParticipants(int numberOfParticipants)
        {
            this.numberOfParticipants = numberOfParticipants;
            return this;
        }

        public FootballBuilder SetLocation(string location)
        {
            this.location = location;
            return this;
        }

        public FootballBuilder SetNumberOfRedCards(int numberOfRedCards)
        {
            this.numberOfRedCards = numberOfRedCards;
            return this;
        }

        public FootballBuilder SetNumberOfInjuredPersons(int numberOfInjuredPersons)
        {
            this.numberOfInjuredPersons = numberOfInjuredPersons;
            return this;
        }

        public FootballBuilder SetNumberOfInjuredPersons(string teamA)
        {
            this.teamA = teamA;
            return this;
        }

        public FootballBuilder SetTeamA(string teamA)
        {
            this.teamA = teamA;
            return this;
        }

        public FootballBuilder SetTeamB(string teamB)
        {
            this.teamB = teamB;
            return this;
        }

        public FootballBuilder SetScoreA(int scoreA)
        {
            this.scoreA = scoreA;
            return this;
        }

        public FootballBuilder SetScoreB(int scoreB)
        {
            this.scoreB = scoreB;
            return this;
        }

        public Football build()
        {
            return new Football(numberOfParticipants, location, numberOfRedCards, numberOfInjuredPersons,
            teamA, teamB, scoreA, scoreB);
        }
    }
}
