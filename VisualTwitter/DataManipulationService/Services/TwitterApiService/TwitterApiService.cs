using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using DataManipulationService.MOP;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
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
        public async Task<List<Tweet>> GetTweetsSample()
        {
            var tweetsSample = new List<Tweet>();
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = "2/tweets/sample/stream?tweet.fields=public_metrics,entities,lang";
            var response = await client.GetStreamAsync(url);

            using (var streamReader = new StreamReader(response))
            {
                int count = 7500;
                while(count > 0)
                {
                    var result = streamReader.ReadLine();
                    JObject initialData=null;
                    try {
                        if (result == "")
                        {
                            continue;
                        }
                         initialData = JObject.Parse(result);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    

                    var tweet = (Tweet)JsonConvert.DeserializeObject<Tweet>(initialData["data"].ToString());
                    // tweet.entities.hashtags != null && 
                    if (tweet.lang=="en")
                    {
                        _databaseService.insertTweet(tweet);
                        tweetsSample.Add(tweet);
                        File.AppendAllLines("WriteLines.txt", new List<string>() { result });
                        --count;
                    }
                    
                }

            }

            //Console.WriteLine(httpResponse.StatusCode);
            return tweetsSample;
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
