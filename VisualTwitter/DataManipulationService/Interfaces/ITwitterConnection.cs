using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface ITwitterConnection
    {
       HttpClient GetTwitterClient();
        
    }
}
