using System.IO;

namespace BikeMgr.Core.Models
{
    public class Bike : Model
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Wheels { get; set; }
        public string FrameMaterial { get; set; }
        public int BikeTypeID { get; set; }
        public BikeType BikeType { get; set; }
        public decimal Price { get; set; }
        public string ImageLocation { get; set; }
        public Blob Image { get; set; }
    }
}