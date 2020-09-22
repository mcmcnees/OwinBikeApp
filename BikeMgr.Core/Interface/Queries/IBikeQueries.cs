using BikeMgr.Core.Models;
using System.Threading.Tasks;

namespace BikeMgr.Core.Interface.Queries
{
    public interface IBikeQueries
    {
        Task<Page<Bike>> GetBikes(string sortOrder, string search, PageParams pageParams);
        int GetBikeCount(string search);
        Bike GetBikeByID(int bikeID);

        Task<Bike> CreateBike(Bike bike);

        Task<Bike> UpdateBike(Bike bike);

        void DeleteBike(Bike bike);

        BikeType[] GetBikeTypes();

        BikeType GetBikeTypeByID(int id);
    }
}
