using System.Collections.Generic;

namespace UserService.Models
{
    public class TweetVector
    {
        // Content represents the tweet (or any other object) to be clustered
        public string Content { get; set; }

        // represents the tf*idf of each tweet word
        public Dictionary<string, double> VectorSpace { get; set; }
    }
}