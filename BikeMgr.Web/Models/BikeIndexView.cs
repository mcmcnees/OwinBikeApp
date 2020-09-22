using BikeMgrWeb.DTO;
using System.Collections.Generic;

namespace BikeMgrWeb.Models
{
    public class BikeIndexView<T> : Page<T>
    {
        public BikeIndexView(List<T> items, int pageNo, int totalPages) : base(items, pageNo, totalPages) { }
        public BikeIndexView(Page<T> pagList) : base(pagList.Items, pagList.PageNo, pagList.TotalPages) { }
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string BrandSort { get; set; }
        public string WheelSort { get; set; }
        public string FrameSort { get; set; }
        public string TypeSort { get; set; }
        public string PriceSort { get; set; }
        public string CurrentFilter { get; set; }
    }
}