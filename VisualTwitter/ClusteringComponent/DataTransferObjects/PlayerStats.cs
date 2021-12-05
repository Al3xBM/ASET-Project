using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.DataTransferObjects
{
    public class PlayerStats
    {
        public string Name { get; set; }

        public int Points { get; set; }

        public int Rebounds { get; set; }

        public int Assist { get; set; }

        public int Blocks { get; set; }
    }
}
