using BikeMgr.Core.Interface.Queries;
using BikeMgr.Core.Models;
using BikeMgr.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeMgr.Infrastructure.Queries
{
    public class BikeQueries : IBikeQueries
    {
        private DbContext _db;

        public BikeQueries(DbContext db)
        {
            _db = db;
        }

        public async Task<Bike> CreateBike(Bike bike)
        {
            if (bike == null) throw new Exception("Invalid bike");
            BikeEntity entity = bike.Map();
            _db.Set<BikeEntity>().Add(entity);
            await _db.SaveChangesAsync();

            bike.ID = entity.ID;
            return bike;
        }

        public void DeleteBike(Bike bike)
        {
            var deleteBike = _db.Set<BikeEntity>().Find(bike.ID);
            if (deleteBike != null) _db.Set<BikeEntity>().Remove(deleteBike);
            _db.SaveChanges();
        }

        public Bike GetBikeByID(int bikeID)
        {
            if (bikeID == 0) return null;
            BikeEntity bike = _db.Set<BikeEntity>().Where(x => x.ID == bikeID).FirstOrDefault();
            var entity = bike.Map();
            entity.BikeType = bike.BikeType.Map();
            return entity;
        }

        public async Task<Page<Bike>> GetBikes(string sortOrder, string search, PageParams pageParams)
        {
            var bikeQuery = from bikes in _db.Set<BikeEntity>().Include(b => b.BikeType)
                            where (bikes.Name.Contains(search) || bikes.Brand.Contains(search) || String.IsNullOrEmpty(search))
                            select bikes;
            bikeQuery = SortBikeQuery(sortOrder, bikeQuery);

            var count = await bikeQuery.CountAsync();
            int offset = (pageParams.PageNo - 1) * pageParams.PageSize; 
            var bikeList = await bikeQuery.Skip(offset).Take(pageParams.PageSize).ToListAsync();
            var result = new List<Bike>();
            foreach(var bike in bikeList)
            {
                var value = bike.Map();
                value.BikeType = bike.BikeType.Map();
                result.Add(value);
            }
            return result.ToPage(pageParams, count);
        }

        public int GetBikeCount(string search)
        {
            var bikeQuery = from bike in _db.Set<BikeEntity>().Include(b => b.BikeType)
                            where (bike.Name.Contains(search) || bike.Brand.Contains(search) || String.IsNullOrEmpty(search))
                            select bike;
            return bikeQuery.Count();
        }

        public BikeType[] GetBikeTypes()
        {
            return _db.Set<BikeTypeEntity>().Select(Mapper.Map).ToArray();
        }

        public BikeType GetBikeTypeByID(int id)
        {
            if (id == 0) return null;
            return _db.Set<BikeTypeEntity>().Where(x => x.ID == id).Select(Mapper.Map).FirstOrDefault();
        }

        public async Task<Bike> UpdateBike(Bike bike)
        {
            var entity = _db.Set<BikeEntity>().Find(bike.ID);
            if (entity == null) throw new Exception("Invalid bike");
            entity.Name = bike.Name;
            entity.Brand = bike.Brand;
            entity.BikeTypeID = bike.BikeTypeID;
            entity.FrameMaterial = bike.FrameMaterial;
            entity.Price = bike.Price;
            entity.Wheels = bike.Wheels;
            entity.ImageLocation = bike.ImageLocation;
            await _db.SaveChangesAsync();

            var returnBike = entity.Map();
            returnBike.BikeType = bike.BikeType;
            return returnBike;
        }

        private IQueryable<BikeEntity> SortBikeQuery(string sortOrder, IQueryable<BikeEntity> bikeQuery)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    return bikeQuery.OrderByDescending(s => s.Name);
                case "brand":
                    return bikeQuery.OrderBy(s => s.Brand);
                case "brand_desc":
                    return bikeQuery.OrderByDescending(s => s.Brand);
                case "wheel":
                    return bikeQuery.OrderBy(s => s.Wheels);
                case "wheel_desc":
                    return bikeQuery.OrderByDescending(s => s.Wheels);
                case "frame":
                    return bikeQuery.OrderBy(s => s.FrameMaterial);
                case "frame_desc":
                    return bikeQuery.OrderByDescending(s => s.FrameMaterial);
                case "type":
                    return bikeQuery.OrderBy(s => s.BikeType.TypeName);
                case "type_desc":
                    return bikeQuery.OrderByDescending(s => s.BikeType.TypeName);
                case "price":
                    return bikeQuery.OrderBy(s => s.Price);
                case "price_desc":
                    return bikeQuery.OrderByDescending(s => s.Price);
                default:
                    return bikeQuery.OrderBy(s => s.Name);
            }
        }
    }
}
