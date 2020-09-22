using BikeMgr.Core.Interface.Queries;
using BikeMgr.Core.Interface.Services;
using BikeMgr.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BikeMgr.Core.Services
{
    public class BikeService : IBikeService
    {
        private IStorageService _storage;
        private IBikeQueries _queries;

        public BikeService(IBikeQueries queries, IStorageService storage)
        {
            _storage = storage;
            _queries = queries;
        }

        public async Task<Bike> CreateBike(Bike bike)
        {
            bike.Validate();

            var existingBike = _queries.GetBikeByID(bike.ID);
            if (existingBike != null) throw new Exception("Bike already exists.");

            var bikeType = _queries.GetBikeTypeByID(bike.BikeTypeID);
            if (bikeType == null) throw new NotFoundException(String.Format("No bike type found with ID {0}.", bike.BikeTypeID));
            bike.BikeType = bikeType;

            if(bike.Image != null)
                bike.ImageLocation = await SaveBikeImage(bike);
            
            var newBike = await _queries.CreateBike(bike);
            return newBike;
        }

        public void DeleteBike(int bikeID)
        {
            Bike bike = _queries.GetBikeByID(bikeID);
            _storage.DeleteFile(bike.ImageLocation);
            _queries.DeleteBike(bike);

        }

        public Bike GetBikeByID(int bikeID)
        {
            Bike bike = _queries.GetBikeByID(bikeID);
            if (bike == null) throw new NotFoundException(String.Format("No bike found with ID {0}.", bikeID));
            return bike;
        }

        public async Task<Page<Bike>> GetBikes(string sortOrder, string search, PageParams pageParams)
        {
            return await _queries.GetBikes(sortOrder, search, pageParams);
        }

        public BikeType[] GetBikeTypes()
        {
            return _queries.GetBikeTypes();
        }

        public BikeType GetBikeTypeByID(int id)
        {
            BikeType bikeType = _queries.GetBikeTypeByID(id);
            if (bikeType == null) throw new NotFoundException(String.Format("No bike type found with ID {0}.", id));
            return bikeType;
        }

        public async Task<Bike> UpdateBike(int id, Bike bike)
        {
            if (id != bike.ID) throw new Exception("Invalid bike ID for update.");
            bike.Validate();

            Bike oldBike = _queries.GetBikeByID(bike.ID);
            if (oldBike == null) throw new NotFoundException(String.Format("No bike found with ID {0}.", bike.ID));

            var bikeType = _queries.GetBikeTypeByID(bike.BikeTypeID);
            if (bikeType == null) throw new Exception("Invalid Bike Type ID.");
            bike.BikeType = bikeType;

            string imageLocation = oldBike.ImageLocation;
            bool hasImage = false;
            if (bike.Image != null)
            {
                hasImage = true;
                bike.ImageLocation = await SaveBikeImage(bike);
            }
            else
                bike.ImageLocation = imageLocation;

            Bike updatedBike = await _queries.UpdateBike(bike);
            if (hasImage)
                _storage.DeleteFile(imageLocation);
            return updatedBike;
        }

        private async Task<string> SaveBikeImage(Bike bike)
        {
            string imageLocation = $"Images\\Bikes\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + System.Guid.NewGuid().ToString() + bike.Image.Extension;
            return await _storage.SaveFile(bike.Image.FileStream, imageLocation); 
        }
    }
}
