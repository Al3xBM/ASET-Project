using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.DataTransferObjects
{
    public class SearchResultsDTO
    {
        public (string Name, string Abbr) Team1 { get; set; }

        public (string Name, string Abbr) Team2 { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public PlayerStats Team1MVP { get; set; }

        public PlayerStats Team2MVP { get; set; }

        public string OhterInfo { get; set; }
    }
}
