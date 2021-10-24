namespace UserService.Models
{
    public class TweetVector
    {
        // Content represents the tweet (or any other object) to be clustered
        public string Content { get; set; }

        // represents the tf*idf of each tweet
        public float[] VectorSpace { get; set; }
    }
}