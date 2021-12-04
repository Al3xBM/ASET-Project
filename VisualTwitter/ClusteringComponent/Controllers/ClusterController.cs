using ClusteringComponent.Models;
using ClusteringComponent.Repositories;
using ClusteringComponent.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UserService.Models;
using System.Diagnostics;

using System.Linq;
namespace ClusteringComponent.Controllers
{
    public class ClusterController : ControllerBase
    {
        /*
        private ClusterProcessing _clusterProcessing;

        public ClusterController(ClusterProcessing _clusterProcessing)
        {
            
        }
        */
        [HttpPost("clusterized")]
        public string SendClusteredData(object _tweet)
        {
            string[] lines = System.IO.File.ReadAllLines(@"E:\aset repo\ASET-Project\VisualTwitter\DataManipulationService\WriteLines.txt");
            List<Tweet> tweets = new();
            int i = 0;
            foreach (string line in lines)
            {
                JObject json = JObject.Parse(line);

                //Console.WriteLine("i = " + i++);

                var urls = json["data"]["entities"]["urls"];
                tweets.Add(new Tweet()
                {
                    Content = (string)json["data"]["text"],
                    Url = urls == null ? "" : (string)urls[0]["expanded_url"]
                });
            }

            Debug.WriteLine("lines = " + lines.Length);

            TweetCollection collection = new TweetCollection();
            collection.SetTweetList(tweets);

            TweetsProcessing processing = new TweetsProcessing(collection);
            processing.SetTweetCollection();

            List<Cluster> clusters = ClusterAlgorithm.PrepareTweetCluster(2, collection.GetTweetVectors());

            Debug.WriteLine("clusters = " + clusters.Count);

            return "DONE";
        }
    }
}
