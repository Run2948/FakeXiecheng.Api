using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.Api.Common.Helper
{
    public class PaginationList<T> : List<T>
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginationList(int totalCount, int currentPage, int pageSize, IEnumerable<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public static async Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> result)
        {
            var total = await result.CountAsync();
            var skip = (currentPage - 1) * pageSize;
            var items = await result.Skip(skip).Take(pageSize).ToListAsync();
            return new PaginationList<T>(total, currentPage, pageSize, items);
        }
    }
}
