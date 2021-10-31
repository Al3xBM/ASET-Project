using DataManipulationService.Models;
using System.Collections.Generic;

namespace DataManipulationService.Interfaces
{
    public interface ITweetsManipulationService
    {
        public List<Tweet> FilterDataForSearch(List<Tweet> tweets);
        public List<Tweet> FilterDataForTrending(List<Tweet> tweets);


    }
}
