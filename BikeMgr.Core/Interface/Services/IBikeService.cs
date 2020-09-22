using BikeMgr.Core.Models;
using System.Threading.Tasks;

namespace BikeMgr.Core.Interface.Services
{
    public interface IBikeService
    {
        Task<Page<Bike>> GetBikes(string sortOrder, string search, PageParams pageParams);

        Bike GetBikeByID(int bikeID);

        Task<Bike> CreateBike(Bike bike);

        Task<Bike> UpdateBike(int id, Bike bike);

        void DeleteBike(int bikeID);

        BikeType[] GetBikeTypes();
        BikeType GetBikeTypeByID(int id);
    }
}
