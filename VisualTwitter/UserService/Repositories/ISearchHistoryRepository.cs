using System.Collections.Generic;
using UserService.Models;

namespace UserService.Repositories
{
    public interface ISearchHistoryRepository
    {
        IEnumerable<SearchResult> GetAllRelatedToUser();
        SearchResult GetById(int id);
        SearchResult Create(SearchResult result, int userId);
        void Delete(int id);
    }
}
