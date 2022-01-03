using ClusteringComponent.DataTransferObjects;
using ClusteringComponent.Interfaces;
using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClusteringComponent.Services
{
    public class PostProcessing : IPostProcessing
    {
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
            List<Team> teams = _databaseService.GetTeams();

            if (topicWords.Contains("vs"))
            {
                SearchForTeams(returnObj, topicWords, teams);
                Team team1 = teams.FirstOrDefault(x => x.Name == returnObj.Team1);
                Team team2 = teams.FirstOrDefault(x => x.Name == returnObj.Team2);

                List<Player> team1Players = players.Where(x =>
                        team1.Aliases.Any(y => y.Equals(x.Team.ToLower()) || y.Equals(x.TeamAbbr.ToLower())) 
                    ).ToList();
                List<Player> team2Players = players.Where(x =>
                    team2.Aliases.Any(y => y.Equals(x.Team.ToLower()) || y.Equals(x.TeamAbbr.ToLower()))
                    //  returnObj.Team2.Equals(x.Team.ToLower())
                    ).ToList();

                List<string> relevantTweets = new List<string>();

                foreach (var tweet in cluster.GroupedTweets)
                {
                    var lowerTweet = tweet.Content.ToLower();

                    /*                        lowerTweet.Contains(returnObj.Team1.Name) || lowerTweet.Contains(returnObj.Team2.Name)
                        || lowerTweet.Contains(returnObj.Team1.Abbr) || lowerTweet.Contains(returnObj.Team2.Abbr)
                        || team1Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        || team2Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        )*/
                    if (team1.Aliases.Any(x => lowerTweet.ToLower().Contains(x)) || team2.Aliases.Any(x => lowerTweet.ToLower().Contains(x))
                        || team1Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        || team2Players.Any(x => lowerTweet.Contains(x.FirstName) || lowerTweet.Contains(x.LastName))
                        )
                        relevantTweets.Add(lowerTweet);
                }

                SearchForMatchInfo(returnObj, relevantTweets, team1, team2, team1Players, team2Players);

                List<int> possibleTeam1Scores = new List<int>();
                List<int> possibleTeam2Scores = new List<int>();
            }

            return returnObj;
        }

        public void SearchForTeams(SearchResultsDTO returnObj, List<string> topicWords, List<Team> teams)
        {
            int teamsFound = 0;

            foreach (Team team in teams)
            {
                if (team.Aliases.Any(x => topicWords.Contains(x)))
                {
                    if (teamsFound == 0)
                        returnObj.Team1 = team.Name;
                    else
                        returnObj.Team2 = team.Name;
                    ++teamsFound;

                }

                if (teamsFound == 2)
                    break;
            }
        }

        public void SearchForMatchInfo(SearchResultsDTO returnObj, List<string> tweets, Team team1, Team team2, List<Player> team1Players, List<Player> team2Players)
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
                        if(CheckScoreOwner(tweetWords, scoreIndex, team1))
                        {
                            returnObj.Team1Score = scores[0];
                            returnObj.Team2Score = scores[1];
                        }
                        else if(CheckScoreOwner(tweetWords, scoreIndex, team2))
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

            int valuesCountT1 = playerStatsT1.Select(x => x.Value.Values.Count).Max();
            string mvpT1 = mvpLikelyhoodT1.Where(x => playerStatsT1[x.Key].Values.Count == valuesCountT1).Select(x => x.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(mvpT1))
            {
                PlayerStats stats = new PlayerStats();
                stats.Name = mvpT1;
                ParseStats(stats, playerStatsT1[mvpT1]);
                returnObj.Team1MVP = stats;
            }

            int valuesCountT2 = playerStatsT2.Select(x => x.Value.Values.Count).Max();
            string mvpT2 = mvpLikelyhoodT2.Where(x => playerStatsT2[x.Key].Values.Count == valuesCountT2 && x.Key != mvpT1).Select(x => x.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(mvpT2))
            {
                PlayerStats stats = new PlayerStats();
                stats.Name = mvpT2;
                ParseStats(returnObj.Team2MVP, playerStatsT2[mvpT2]);
                returnObj.Team2MVP = stats;
            }

        }

        public bool CheckScoreOwner(List<string> words, int index, Team team)
        {
            return team.Aliases.Any(x => words[index - 1].Contains(x)) 
                || (index > 1 && team.Aliases.Any(x => words[index - 2].Contains(x)));
                // || words[index - 1].Contains(name) || words[index - 1].Contains(abbr) 
                // || (index > 1 && (words[index - 2].Contains(name) || words[index - 2].Contains(abbr)));
        }

        public void ParseTweetForPlayerInfo(List<Player> players, Dictionary<string, int> mvpLikelyhood, Dictionary<string, Dictionary<string, int>> playerStats, string tweet)
        {
            string[] stats = {"pts", "reb", "blk", "ast", "points", "blocks", "rebounds", "assists" };
            tweet = tweet.Replace(",", "");
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

                /*                tweetWords.Where((x, index) =>
                                {
                                    if (stats.Contains(tweetWords[index + 1]) && int.TryParse(x, out int nr))
                                    {
                                        playerStats[player][tweetWords[index + 1]] = nr;
                                    }

                                    return true;
                                });*/

                if (tweetWords.Any(x => int.TryParse(x, out _)))
                    for (int i = 0; i < tweetWords.Count; ++i)
                        if (int.TryParse(tweetWords[i], out int nr))
                            playerStats[player][tweetWords[i + 1]] = nr;
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
                if (stat.ToLower().Contains("points") || stat.ToLower().Contains("pts"))
                    statsObj.Points = playerStats[stat];

                if (stat.ToLower().Contains("rebounds") || stat.ToLower().Contains("reb"))
                    statsObj.Rebounds = playerStats[stat];

                if (stat.ToLower().Contains("blocks") || stat.ToLower().Contains("blk"))
                    statsObj.Blocks = playerStats[stat];

                if (stat.ToLower().Contains("assists") || stat.ToLower().Contains("ast"))
                    statsObj.Assist = playerStats[stat];
            }
        }
    }
}
