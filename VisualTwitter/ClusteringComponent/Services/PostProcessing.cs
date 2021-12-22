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

        private readonly IDatabaseService _databaseService;

        public PostProcessing(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public SearchResultsDTO ProcessResults(Cluster cluster, string topic)
        {
            SearchResultsDTO returnObj = new SearchResultsDTO();
            returnObj.Team1MVP = new PlayerStats();
            returnObj.Team2MVP = new PlayerStats();

            List<string> topicWords = topic.ToLower().Split(" ").ToList();
            List<Player> players = _databaseService.GetPlayers();
            List<(string Name, string Abbr)> teamsList = players.Select(x =>
            (
                x.Team.ToLower(),
                x.TeamAbbr.ToLower()
            )).Distinct().ToList();
            // teamNames.ToLower().Split(", ").ToList();

            if (topicWords.Contains("vs"))
            {
                SearchForTeams(returnObj, topicWords, teamsList);

                List<Player> team1Players = players.Where(x =>
                         returnObj.Team1.Name.Equals(x.Team.ToLower())
                    ).ToList();
                List<Player> team2Players = players.Where(x =>
                        returnObj.Team2.Name.Equals(x.Team.ToLower())
                    ).ToList();

                List<string> relevantTweets = new List<string>();

                foreach (var tweet in cluster.GroupedTweets)
                {
                    var lowerTweet = tweet.Content.ToLower();

                    if (lowerTweet.Contains(returnObj.Team1.Name) || lowerTweet.Contains(returnObj.Team2.Name)
                        || lowerTweet.Contains(returnObj.Team1.Abbr) || lowerTweet.Contains(returnObj.Team2.Abbr)
                        || team1Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        || team2Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        )
                        relevantTweets.Add(lowerTweet);
                }

                SearchForMVP(returnObj, relevantTweets, team1Players, team2Players);

                List<int> possibleTeam1Scores = new List<int>();
                List<int> possibleTeam2Scores = new List<int>();
            }

            return returnObj;
        }

        public void SearchForTeams(SearchResultsDTO returnObj, List<string> topicWords, List<(string Name, string Abbr)> teams)
        {
            int teamsFound = 0;

            foreach ((string Name, string Abbr) team in teams)
            {
                if(topicWords.Contains(team.Name) || topicWords.Contains(team.Abbr))
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
        }

        public void SearchForMVP(SearchResultsDTO returnObj, List<string> tweets, List<Player> team1Players, List<Player> team2Players)
        {
            Dictionary<string, int> mvpLikelyhoodT1 = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, int>> playerStatsT1 = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> mvpLikelyhoodT2 = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, int>> playerStatsT2 = new Dictionary<string, Dictionary<string, int>>();
            Regex scoreRegex = new Regex("[0-9]+[-][0-9]+");

            foreach (string tweet in tweets)
            {
                if (team1Players.Any(x => tweet.Contains(x.FirstName.ToLower()) || tweet.Contains(x.LastName.ToLower())))
                    ParseTweetForPlayerInfo(team1Players, mvpLikelyhoodT1, playerStatsT1, tweet);

                if (team2Players.Any(x => tweet.Contains(x.FirstName.ToLower()) || tweet.Contains(x.LastName.ToLower())))
                    ParseTweetForPlayerInfo(team2Players, mvpLikelyhoodT2, playerStatsT2, tweet);

                Match scoreMatch = scoreRegex.Match(tweet);
                if(scoreMatch.Success)
                {
                    List<int> scores = scoreMatch.Value.Split("-").Select(x => int.Parse(x)).OrderBy(x => x).ToList();
                    
                    List<string> tweetWords = tweet.Split(" ").ToList();
                    int scoreIndex = tweetWords.IndexOf(scoreMatch.Value);

                    if (scoreIndex > 0)
                    {
                        if(CheckScoreOwner(tweetWords, scoreIndex, returnObj.Team1.Name, returnObj.Team1.Abbr))
                        {
                            returnObj.Team1Score = scores[0];
                            returnObj.Team2Score = scores[1];
                        }
                        else if(CheckScoreOwner(tweetWords, scoreIndex, returnObj.Team2.Name, returnObj.Team2.Abbr))
                        {
                            returnObj.Team1Score = scores[1];
                            returnObj.Team2Score = scores[0];
                        }
                    }
                    else
                    {
                        returnObj.Team1Score = scores[0];
                        returnObj.Team2Score = scores[1];
                    }
                }
            }

            string mvpT1 = mvpLikelyhoodT1.Where(x => x.Value == mvpLikelyhoodT1.Values.Max()).Select(x => x.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(mvpT1))
            {
                returnObj.Team1MVP.Name = mvpT1;
                ParseStats(returnObj.Team1MVP, playerStatsT1[mvpT1]);
            }

            string mvpT2 = mvpLikelyhoodT2.Where(x => x.Value == mvpLikelyhoodT2.Values.Max() && x.Key != mvpT1).Select(x => x.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(mvpT2))
            {
                returnObj.Team2MVP.Name = mvpT2;
                ParseStats(returnObj.Team2MVP, playerStatsT2[mvpT2]);
            }

        }

        public bool CheckScoreOwner(List<string> words, int index, string name, string abbr)
        {
            return words[index - 1].Contains(name) || words[index - 1].Contains(abbr) 
                || (index > 1 && (words[index - 2].Contains(name) || words[index - 2].Contains(abbr)));
        }

        public void ParseTweetForPlayerInfo(List<Player> players, Dictionary<string, int> mvpLikelyhood, Dictionary<string, Dictionary<string, int>> playerStats, string tweet)
        {
            string[] stats = {"PTS", "REB", "BLK", "AST", "points", "blocks", "rebounds", "assists" };

            string player = players.Where(x =>
                                        x.FirstName != null && x.LastName != null &&
                                        (tweet.Contains(x.FirstName.ToLower()) || tweet.Contains(x.LastName.ToLower()))
                                    ).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();

            if (!string.IsNullOrEmpty(player))
            {
                if (!mvpLikelyhood.ContainsKey(player))
                    mvpLikelyhood.Add(player, 0);

                mvpLikelyhood[player] += 1;
                
                if (!playerStats.ContainsKey(player))
                    playerStats[player] = new Dictionary<string, int>();

                List<String> tweetWords = tweet.Split(" ").ToList();

                tweetWords.Where((x, index) =>
                {
                    if (stats.Contains(tweetWords[index + 1]) && int.TryParse(x, out int nr))
                    {
                        playerStats[player][tweetWords[index + 1]] = nr;
                    }

                    return true;
                });
/*
                if (tweetWords.Any(x => int.TryParse(x, out _)))
                    for (int i = 0; i < tweetWords.Count; ++i)
                        if (int.TryParse(tweetWords[i], out int nr))
                            playerStats[player][tweetWords[i+1]] = nr;*/
            }

            #region comments

/*            Regex playerRegex = new Regex(@"\w+\s\w+\:\s\d+");
            if (tweet.Contains("PTS") && tweet.Contains("REB"))
            {
                var match = playerRegex.Match(tweet);
                returnObj.Team1MVP.Name = match.Value.Split(":")[0];
                returnObj.Team1MVP.Points = int.Parse(match.Value.Split(":")[1]);
            }*/

            #endregion
        }

        public void ParseStats(PlayerStats statsObj, Dictionary<string, int> playerStats)
        {
            foreach(string stat in playerStats.Keys)
            {
                if (stat.ToLower() == "points" || stat.ToLower() == "pts")
                    statsObj.Points = playerStats[stat];

                if (stat.ToLower() == "rebounds" || stat.ToLower() == "reb")
                    statsObj.Points = playerStats[stat];

                if (stat.ToLower() == "blocks" || stat.ToLower() == "blk")
                    statsObj.Points = playerStats[stat];

                if (stat.ToLower() == "assists" || stat.ToLower() == "ast")
                    statsObj.Points = playerStats[stat];
            }
        }
    }
}
