using DataManipulationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface ITweetsManipulationService
    {
        public List<Tweet> FilterDataForSearch(List<Tweet> tweets);
        public List<Tweet> FilterDataForTrending(List<Tweet> tweets);


    }
}
