using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace FakeXiecheng.Api.Common.Helper
{
    public static class PaginationHelper
    {
        private static string GenerateResourceUrl(this IUrlHelper urlHelper, string routeName, PaginationRequest request, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => urlHelper.Link(routeName, request.Decr()),
                ResourceUriType.NextPage => urlHelper.Link(routeName, request.Incr()),
                _ => urlHelper.Link(routeName, request)
            };
        }

        public static KeyValuePair<string, StringValues> GeneratePaginationHeader<T>(this IUrlHelper urlHelper, PaginationList<T> list, string routeName, PaginationRequest request)
        {
            // X-Pagination Object
            var paginationMetadata = new
            {
                previousPageLink = list.HasPrevious ? urlHelper.GenerateResourceUrl(routeName, request, ResourceUriType.PreviousPage) : null,
                nextPageLink = list.HasNext ? urlHelper.GenerateResourceUrl(routeName, request, ResourceUriType.NextPage) : null,
                totalCount = list.TotalCount,
                pageSize = list.PageSize,
                currentPage = list.CurrentPage,
                totalPages = list.TotalPages
            };
            return new KeyValuePair<string, StringValues>("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
        }
    }
}
