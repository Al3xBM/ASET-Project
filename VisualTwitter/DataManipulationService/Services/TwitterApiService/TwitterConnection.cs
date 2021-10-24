using DataManipulationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataManipulationService.Services.TwitterApiService
{
    public sealed class TwitterConnection : ITwitterConnection
    {
        private static TwitterConnection instance = null;
        private static readonly object padlock = new object();
        TwitterConnection()
        {

        }
        public static TwitterConnection Instance
        {
            get
            {
                lock(padlock)
                {
                    if(instance==null)
                    {
                        instance = new TwitterConnection();
                    }
                    return instance;
                }
            }
        }
        public HttpClient GetTwitterClient()
        {
            throw new NotImplementedException();
        }
    }
}
