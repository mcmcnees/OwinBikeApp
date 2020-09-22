using BikeMgr.Infrastructure.Entities;
using System.Data.Entity;

namespace BikeMgr.Infrastructure
{
    public class DataContext: DbContext
    {
        public DbSet<BikeEntity> Bikes { get; set; }
        public DbSet<BikeTypeEntity> BikeTypes { get; set; }

        public DataContext(): base("DataContext")
        {

        }
    }
}
