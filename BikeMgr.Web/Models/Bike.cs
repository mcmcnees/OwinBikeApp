using System.ComponentModel.DataAnnotations;

namespace BikeMgrWeb.Models
{
    public class Bike
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Model Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Brand { get; set; }
        [Required]
        [Display(Name = "No of Wheels")]
        public int? Wheels { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Frame Material")]
        public string FrameMaterial { get; set; }
        [Required]
        [Display(Name = "Type")]
        public virtual BikeType BikeType { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        [StringLength(500)]
        public string ImageLocation { get; set; }
    }
}