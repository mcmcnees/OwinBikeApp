using System.Collections.Generic;
using System.Linq;

namespace BikeMgr.Core.Models
{
    public class Page<T>
    {
        public int PageNo { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; }

        public Page(IEnumerable<T> items, int pageNo, int totalPages)
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
    }
}
