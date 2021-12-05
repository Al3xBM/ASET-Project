using ClusteringComponent.DataTransferObjects;
using ClusteringComponent.Interfaces;
using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClusteringComponent.Services
{
    public class PostProcessing : IPostProcessing
    {
        private static string teamNames = "Boston Celtics, Brooklyn Nets, New York Knicks, Philadelphia 76ers, Toronto Raptors, Central Chicago, Cleveland Cavaliers, Detroit Pistons, Indiana Pacers, Milwaukee Bucks, Atlanta Hawks, Charlotte Hornets, Miami Heat, Orlando Magic, Washington Wizards, Denver Nuggets, Minnesota Timberwolves, Oklahoma City Thunder, Portland Trail Blazers, Utah Jazz, Pacific Golden State Warriors, Los Angeles Clippers, Los Angeles Lakers, Phoenix Suns, Sacramento Kings, Dallas Mavericks, Houston Rockets, Memphis Grizzlies, New Orleans Pelicans, San Antonio Spurs"; 

        public SearchResultsDTO ProcessResults(Cluster cluster, string topic)
        {
            SearchResultsDTO returnObj = new SearchResultsDTO();
            returnObj.Team1MVP = new PlayerStats();
            returnObj.Team2MVP = new PlayerStats();

            List<string> topicWords = topic.ToLower().Split(" ").ToList();
            List<string> teamsList = teamNames.ToLower().Split(", ").ToList();
            int teamsFound = 0;

            foreach(string team in teamsList)
            {
                foreach(string word in topicWords)
                {
                    if(team.Contains(word))
                    {
                        if (teamsFound == 0)
                            returnObj.Team1 = team;
                        else
                            returnObj.Team2 = team;
                        ++teamsFound;
                    }

                    if (teamsFound == 2)
                        break;
                }

                if (teamsFound == 2)
                    break;
            }

            List<string> relevantTweets = new List<string>();
            string team1Short = returnObj.Team1.Split(" ").Last();
            string team2Short = returnObj.Team2.Split(" ").Last();
            foreach (var tweet in cluster.GroupedTweets)
            {
                if (tweet.Content.Contains(returnObj.Team1) || tweet.Content.Contains(returnObj.Team2)
                    || tweet.Content.Contains(team1Short) || tweet.Content.Contains(team2Short)
                    )
                    relevantTweets.Add(tweet.Content);
            }

            List<int> possibleTeam1Scores = new List<int>();
            List<int> possibleTeam2Scores = new List<int>();
            Regex playerRegex = new Regex(@"\w+\s\w+\:\s\d+");
            foreach (string tweet in relevantTweets)
            {

                if (tweet.Contains(returnObj.Team1) || tweet.Contains(team1Short))
                {

                    if (tweet.Contains("PTS") && tweet.Contains("REB"))
                    {
                        var match = playerRegex.Match(tweet);
                        returnObj.Team1MVP.Name = match.Value.Split(":")[0];
                        returnObj.Team1MVP.Points = int.Parse(match.Value.Split(":")[1]);
                    }
                }

                if (tweet.Contains(returnObj.Team2) || tweet.Contains(team2Short))
                {
                    if (tweet.Contains("PTS") && tweet.Contains("REB"))
                    {
                        var match = playerRegex.Match(tweet);
                        if(match.Value.Contains(":"))
                        {
                            returnObj.Team2MVP.Name = match.Value.Split(":")[0];
                            returnObj.Team2MVP.Points = int.Parse(match.Value.Split(":")[1]);
                        }
                    }
                }


            }

            return returnObj;
        }

    }
}
