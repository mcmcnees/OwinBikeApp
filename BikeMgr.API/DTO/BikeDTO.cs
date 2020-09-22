using BikeMgr.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;

namespace BikeMgr.API.DTO
{
    public class BikeDTO: Model
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
        [Required]
        public decimal Price { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public HttpPostedFile Image { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Wheels < 0)
            {
                yield return new ValidationResult("Wheels cannot be negative.", new[] { nameof(Wheels) });
            }
            if (Price < 0)
            {
                yield return new ValidationResult("Price cannot be negative.", new[] { nameof(Wheels) });
            }
        }
    }
}