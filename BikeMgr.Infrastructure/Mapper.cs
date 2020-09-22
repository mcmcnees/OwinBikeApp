using BikeMgr.Core.Models;
using BikeMgr.Infrastructure.Entities;

namespace BikeMgr.Infrastructure
{
    public static class Mapper
    {
        public static Bike Map(this BikeEntity bike)
        {
            if (bike == null) return null;
            Bike bikeModel = new Bike
            {
                ID = bike.ID,
                Brand = bike.Brand,
                FrameMaterial = bike.FrameMaterial,
                Name = bike.Name,
                Price = bike.Price,
                Wheels = bike.Wheels,
                ImageLocation = bike.ImageLocation,
                BikeTypeID = bike.BikeTypeID
            };
            return bikeModel;
        }

        public static BikeEntity Map(this Bike bikeModel)
        {
            if (bikeModel == null) return null;
            BikeEntity bike = new BikeEntity
            {
                ID = bikeModel.ID,
                Brand = bikeModel.Brand,
                FrameMaterial = bikeModel.FrameMaterial,
                Name = bikeModel.Name,
                Price = bikeModel.Price,
                Wheels = bikeModel.Wheels,
                ImageLocation = bikeModel.ImageLocation,
                BikeTypeID = bikeModel.BikeType.ID,
            };
            return bike;
        }

        public static BikeType Map(this BikeTypeEntity bikeType)
        {
            if (bikeType == null) return null;
            BikeType bikeTypeModel = new BikeType
            {
                ID = bikeType.ID,
                TypeName = bikeType.TypeName
            };
            return bikeTypeModel;
        }

        public static BikeTypeEntity Map(this BikeType bikeTypeModel)
        {
            if (bikeTypeModel == null) return null;
            BikeTypeEntity bikeType = new BikeTypeEntity
            {
                ID = bikeTypeModel.ID,
                TypeName = bikeTypeModel.TypeName
            };
            return bikeType;
        }
    }
}
