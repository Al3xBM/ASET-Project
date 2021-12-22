using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using DataManipulationService.MOP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulationService.Services.TwitterApiService
{
    public class TwitterApiService : ITwitterApiService
    {
        private readonly ITwitterConnection _twitterConnection;
        private readonly IDatabaseService _databaseService;
        private static List<string> WhitelistIds = new List<string>() { "27667187", "50323173", "74518740", "19923144" };

        public TwitterApiService(ITwitterConnection twitterConnection, IDatabaseService databaseService)
        {
            _twitterConnection = twitterConnection;
            _databaseService = databaseService;
        }
        [TwitterApiServiceMonitor]
        public async Task<string> GetTweetsSample()
        {
            var tweetsSample = new List<Tweet>();
            int count = 20000;
            HttpClient client = _twitterConnection.GetTwitterClient();

            #region comments

            // ChangeRulesAsync();
            // string url = "2/tweets/sample/stream?tweet.fields=public_metrics,entities,lang";
            // string url = "2/tweets/search/stream/rules";

            #endregion

            string url = "2/tweets/search/stream";

            var response = await client.GetStreamAsync(url);

            using (var streamReader = new StreamReader(response))
            {
                while (count > 0)
                {
                    var result = streamReader.ReadLine();
                    JObject initialData = null;
                    try
                    {
                        if (result == "")
                        {
                            continue;
                        }
                        initialData = JObject.Parse(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    var tweet = (Tweet)JsonConvert.DeserializeObject<Tweet>(initialData["data"].ToString());
                    _databaseService.insertBasketballTweets(tweet);
                    tweetsSample.Add(tweet);
                    --count;
                }

            }

            if (count != 0)
                return "Stopped early";

            return "Sample added to database";
        }

        public async Task ChangeRulesAsync()
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            Dictionary<string, List<Dictionary<string, string>>> rules = new Dictionary<string, List<Dictionary<string, string>>>()
                {
                    {
                        "add", new List<Dictionary<string, string>>()
                        {
                            new Dictionary<string, string>()
                            {
                                {"value", "#basketball lang:en" },
                                { "tag", "basketball" }
                            },
                            new Dictionary<string, string>()
                            {
                                {"value", "#basket lang:en" },
                                { "tag", "basketball" }
                            },
                            new Dictionary<string, string>()
                            {
                                {"value", "#nba lang:en" },
                                { "tag", "basketball" }
                            }
                        }
                    }
                };

            #region comments

            /*                     
            Dictionary<string, Dictionary<string, List<string>>> rules = new Dictionary<string, Dictionary<string, List<string>>>()
                {
                    {
                        "delete",  new Dictionary<string, List<string>>
                        {
                            { "ids", new List<string>
                                {
                                    "1469645547757781003", "1469645547757781004", "1469645547757781002"
                                }
                            }
                        }
                    }
                };
            */

            #endregion

            var json = System.Text.Json.JsonSerializer.Serialize(rules);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("2/tweets/search/stream/rules", data);
            var responseString = await postResponse.Content.ReadAsStringAsync();
        }

        [TwitterApiServiceMonitor]
        public async Task<string> GetTrendingAsync(string id)
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = $"1.1/trends/place.json?id={id}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

        }
        [TwitterApiServiceMonitor]
        public async Task<string> GetAvailableTrendsAsync()
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = $"1.1/trends/available.json";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<Tweet>> SearchWhitelistedUsersTweets()
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            List<Tweet> tweets = new List<Tweet>();

            foreach (string userId in WhitelistIds)
            {
                string url = $"2/users/" + userId + $"/tweets?exclude=retweets,replies&max_results=50";
                HttpResponseMessage httpResponse = await client.GetAsync(url);

                string response = await httpResponse.Content.ReadAsStringAsync();
                JObject initialData = null;

                try
                {
                    if (response == "")
                        continue;

                    initialData = JObject.Parse(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                tweets.AddRange((List<Tweet>)JsonConvert.DeserializeObject<List<Tweet>>(initialData["data"].ToString()));
            }

            _databaseService.insertWhitelistedTweets(tweets);

            return tweets;
        }
    }
}
