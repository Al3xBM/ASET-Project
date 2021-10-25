namespace ClusteringComponent.Models.Events
{
    public class Outlet
    {
        public string location { get; set; }
        public int totalNumberOfClothes { get; set; }

        public Outlet(string location, int total)
        {
            this.location = location;
            this.totalNumberOfClothes = total;
        }
    }
}
