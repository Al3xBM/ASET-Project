using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Services
{
    public class TweetsProcessing
    {
        //Calculates TF-IDF weight for each term t in tweet d
        // tf - idf(t,d) = tf(d,t) * idf(t)
        private static float FindTFIDF(string tweet, string term)
        {
            throw new NotImplementedException();
        }

        private static float FindTermFrequency(string tweet, string term)
        {
            throw new NotImplementedException();
        }

        private static float FindInverseDocumentFrequency(string term)
        {
            throw new NotImplementedException();
        }

        // Finding Similarity Score
        public static float FindCosineSimilarity(float[] vecA, float[] vecB)
        {
            throw new NotImplementedException();
        }
    }
}
