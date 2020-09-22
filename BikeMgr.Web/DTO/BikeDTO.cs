using BikeMgrWeb.Models;

namespace BikeMgrWeb.DTO
{
    public class BikeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Wheels { get; set; }
        public string FrameMaterial { get; set; }
        public int BikeTypeID { get; set; }
        public decimal Price { get; set; }

        public BikeDTO()
        {

        }

        public BikeDTO(Bike bike)
        {
            this.BikeTypeID = bike.BikeType.ID;
            this.FrameMaterial = bike.FrameMaterial;
            this.Brand = bike.Brand;
            this.Name = bike.Name;
            this.Price = bike.Price;
            this.Wheels = bike.Wheels;
            this.ID = bike.ID;
        }
    }
}