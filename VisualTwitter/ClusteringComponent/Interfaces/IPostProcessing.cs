using ClusteringComponent.DataTransferObjects;
using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Interfaces
{
    public interface IPostProcessing
    {
        public SearchResultsDTO ProcessResults(Cluster cluster, string topic);
    }
}
