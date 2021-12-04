using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Models
{
    public class PublicMetric
    {
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int like_count { get; set; }
        public int quote_count { get; set; }
    }
}
