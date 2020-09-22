using BikeMgr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeMgr
{
    public static class Extensions
    {
        public static Page<T> ToPage<T>(this IEnumerable<T> items, PageParams pageParams, int count)
        {
            var totalPages = Convert.ToInt32( Math.Ceiling(count / (float)pageParams.PageSize));
            return new Page<T>(items, pageParams.PageNo, totalPages);
        }
    }
}
