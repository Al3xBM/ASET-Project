using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repositories
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        public SearchResult Create(SearchResult result, int userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SearchResult> GetAllRelatedToUser()
        {
            throw new NotImplementedException();
        }

        public SearchResult GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
