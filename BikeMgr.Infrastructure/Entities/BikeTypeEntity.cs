using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeMgr.Infrastructure.Entities
{
    [Table("BikeType")]
    public class BikeTypeEntity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeName { get; set; }
    }
}
