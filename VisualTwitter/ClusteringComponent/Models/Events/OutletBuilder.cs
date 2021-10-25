using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Models.Events
{
    public class OutletBuilder
    {
        public string location;
        public int totalNumberOfClothes;

        public OutletBuilder SetLocation(string location)
        {
            this.location = location;
            return this;
        }

        public OutletBuilder SetTotalNumberOfClothes(int total)
        {
            this.totalNumberOfClothes = total;
            return this;
        }

        public Outlet build()
        {
            return new Outlet(location, totalNumberOfClothes);
        }
    }
}
