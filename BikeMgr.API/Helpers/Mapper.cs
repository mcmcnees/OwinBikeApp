using BikeMgr.API.DTO;
using BikeMgr.Core.Models;
using System.IO;
using System.Web;

namespace BikeMgr.API.Helpers
{
    public static class Mapper
    {
        public static Bike Map(BikeDTO bikeDTO)
        {
            return new Bike
            {
                ID = bikeDTO.ID,
                BikeType = null,
                BikeTypeID = bikeDTO.BikeTypeID,
                Brand = bikeDTO.Brand,
                FrameMaterial = bikeDTO.FrameMaterial,
                Image = Map(bikeDTO.Image),
                Name = bikeDTO.Name,
                Price = bikeDTO.Price,
                Wheels = bikeDTO.Wheels
            };
        }

        public static Blob Map(HttpPostedFile file)
        {
            if (file == null) return null;
            return new Blob()
            {
                FileStream = file.InputStream,
                Extension = Path.GetExtension(file.FileName),
                Name = file.FileName
            };
        }
    }
}