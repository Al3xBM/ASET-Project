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
using ClusteringComponent.DataTransferObjects;

namespace ClusteringComponent.Controllers
{
    public class ClusterController : ControllerBase
    {

        private IClusterAlgorithm _clusterAlgorithm;
        private IPostProcessing _postProcessing;

        public ClusterController(IClusterAlgorithm clusterAlgorithm, IPostProcessing postProcessing)
        {
            _clusterAlgorithm = clusterAlgorithm;
            _postProcessing = postProcessing;
        }

        [HttpPost("clusterized")]
        public IActionResult SendClusteredData([FromBody] ClusterizedDTO input)
        {
            var topic = input.topic;
            List<Cluster> clusters = _clusterAlgorithm.PrepareTweetCluster(topic);
            SearchResultsDTO dto = _postProcessing.ProcessResults(clusters[0], topic);

            return Ok(dto);
        }

        [HttpGet("loadCollection")]
        public IActionResult LoadCollection()
        {
            _clusterAlgorithm.LoadCollection();

            return Ok("Loaded");
        }
    }
}
