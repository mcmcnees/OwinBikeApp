using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeMgrWeb.Models
{
    [Table(@"BikeType")]
    public class BikeType
    {
        [Key]
        public int ID { get; set; }
        [StringLength(200)]
        public string TypeName { get; set; }
    }
}