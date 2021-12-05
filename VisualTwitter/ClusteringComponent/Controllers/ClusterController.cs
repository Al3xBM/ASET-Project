using ClusteringComponent.Models;
using ClusteringComponent.Repositories;
using ClusteringComponent.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UserService.Models;
using System.Diagnostics;

using System.Linq;
using ClusteringComponent.Interfaces;

namespace ClusteringComponent.Controllers
{
    public class ClusterController : ControllerBase
    {

        private IClusterAlgorithm _clusterAlgorithm;

        public ClusterController(IClusterAlgorithm clusterAlgorithm)
        {
            _clusterAlgorithm = clusterAlgorithm;
        }

        [HttpPost("clusterized")]
        public IActionResult SendClusteredData(string topic)
        {
           List<Cluster> clusters = _clusterAlgorithm.PrepareTweetCluster(topic);

            Debug.WriteLine("clusters = " + clusters.Count);

            return Ok("DONE");
        }

        [HttpGet("loadCollection")]
        public IActionResult LoadCollection()
        {
            _clusterAlgorithm.LoadCollection();

            return Ok("Loaded");
        }
    }
}
