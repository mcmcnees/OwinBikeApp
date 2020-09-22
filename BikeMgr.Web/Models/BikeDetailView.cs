using System.Web.Mvc;

namespace BikeMgrWeb.Models
{
    public class BikeDetailView : Bike
    {
        public BikeDetailView()
        {
                
        }

        public BikeDetailView(Bike bike)
        {
            this.ID = bike.ID;
            this.ImageLocation = bike.ImageLocation;
            this.Name = bike.Name;
            this.Price = bike.Price;
            this.Wheels = bike.Wheels;
            this.BikeType = bike.BikeType;
            this.Brand = bike.Brand;
            this.FrameMaterial = bike.FrameMaterial;
        }
        public SelectList BikeTypes { get; set; }
    }
}