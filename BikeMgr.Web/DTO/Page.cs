using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BikeMgrWeb.DTO
{
    [JsonObjectAttribute]
    public class Page<T> : List<T>
    {
        public int PageNo { get; private set; }
        public int TotalPages { get; private set; }        
        public List<T> Items { get; private set; }

        public Page(List<T> items, int pageNo, int totalPages)
        {
            PageNo = pageNo;
            TotalPages = totalPages == 0 ? 1 : totalPages;
            Items = new List<T>(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNo > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNo < TotalPages);
            }
        }

        public static Page<T> Create(IEnumerable<T> source, int pageNo, int totalPages)
        {
            var items = source.ToList();
            return new Page<T>(items, pageNo, totalPages);
        }
    }
}
