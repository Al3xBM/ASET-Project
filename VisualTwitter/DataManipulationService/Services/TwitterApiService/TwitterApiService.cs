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
            //var url = "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=public_metrics,entities,lang";

            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            
            //httpRequest.Headers["Authorization"] = "Bearer AAAAAAAAAAAAAAAAAAAAADNBVAEAAAAAbPgNZMC0c6Xi36hhzeXSHCybMrw%3DLUJeo7KwkSr547hIYi8j7Km9c9mhMBlpIJd7zhcdbghtKViulX";
            
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string buffer = "";
            using (var streamReader = new StreamReader(response))
            {
                int count = 5;
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

                    if (tweet.entities.hashtags != null && tweet.lang=="en")
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
        
    }
}
