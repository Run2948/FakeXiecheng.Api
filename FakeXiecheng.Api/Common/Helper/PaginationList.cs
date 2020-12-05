using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.Api.Common.Helper
{
    public class PaginationList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginationList(int currentPage, int pageSize, IEnumerable<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
        }

        public static async Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> result)
        {
            var skip = (currentPage - 1) * pageSize;
            var items = await result.Skip(skip).Take(pageSize).ToListAsync();
            return new PaginationList<T>(currentPage, pageSize, items);
        }
    }
}
