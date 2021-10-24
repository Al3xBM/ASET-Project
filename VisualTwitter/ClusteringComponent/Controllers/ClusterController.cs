using ClusteringComponent.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ClusteringComponent.Controllers
{
    public class ClusterController : ControllerBase
    {
        private ClusterProcessing _clusterProcessing;

        public ClusterController(ClusterProcessing _clusterProcessing)
        {
            
        }

        [HttpPost("clusterized")]
        public string SendClusteredData(object Tweet)
        {
            // 
            throw new NotImplementedException();
        }
    }
}
