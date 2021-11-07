using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public TwitterApiService(ITwitterConnection twitterConnection)
        {
            _twitterConnection = twitterConnection;
        }

        public async Task<List<Tweet>> GetTweetsSample()
        {
            /*            HttpClient client = _twitterConnection.GetTwitterClient();
                        string url = "1.1/statuses/lookup.json";
                        var response = await client.GetAsync(url);*/
            var url = "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=public_metrics,entities";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "Bearer AAAAAAAAAAAAAAAAAAAAADNBVAEAAAAAbPgNZMC0c6Xi36hhzeXSHCybMrw%3DLUJeo7KwkSr547hIYi8j7Km9c9mhMBlpIJd7zhcdbghtKViulX";
            
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string buffer = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                int count = 10000;
                while(count > 0)
                {
                    var result = streamReader.ReadLine();
                    File.AppendAllLines("WriteLines.txt", new List<string>() { result });
                    --count;
                }

            }

            Console.WriteLine(httpResponse.StatusCode);
            return null;
        }

        public async Task<string> GetTrendingAsync(string id)
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = $"1.1/trends/place.json?id={id}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
            
        }

        
    }
}
