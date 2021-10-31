using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface ITwitterApiService
    {
        public Task<string> GetTrendingAsync(string id);
    }
}
