using DataManipulationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseService
    {
        public void insertTweet(Tweet tweet);
    }
}
