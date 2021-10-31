using System.Net.Http;

namespace DataManipulationService.Interfaces
{
    public interface ITwitterConnection
    {
       HttpClient GetTwitterClient();
        
    }
}
