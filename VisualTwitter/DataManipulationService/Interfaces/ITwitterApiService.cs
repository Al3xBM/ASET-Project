using DataManipulationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface ITwitterApiService
    {
        public Task<string> GetTrendingAsync(string id);

        public Task<List<Tweet>> GetTweetsSample();
    }
}
