using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Models
{
    public class WhitelistedUser
    {
        public ObjectId _id { get; set; }

        public string Tag { get; set; }

        public string TwitterId { get; set; }
    }
}
