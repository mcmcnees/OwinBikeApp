using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeMgr.Infrastructure.Entities
{
    [Table("Bike")]
    public class BikeEntity
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Brand { get; set; }
        [Required]
        public int? Wheels { get; set; }
        [Required]
        [StringLength(200)]
        public string FrameMaterial { get; set; }
        [Required]
        public int BikeTypeID { get; set; }
        public virtual BikeTypeEntity BikeType { get; set; }
        [Required]
        public decimal Price { get; set; }
        [StringLength(500)]
        public string ImageLocation { get; set; }
    }
}
