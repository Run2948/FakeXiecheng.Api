using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using FakeXiecheng.Api.Service;

namespace FakeXiecheng.Api.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string order, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mappingDictionary == null)
                throw new ArgumentNullException(nameof(mappingDictionary));
            if (string.IsNullOrWhiteSpace(order))
                return source;
            var orderByAfterSplit = order.Split(",");
            foreach (var orderBy in orderByAfterSplit.Reverse())
            {
                var trimmedOrderBy = orderBy.Trim();
                var orderDescending = trimmedOrderBy.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderBy.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderBy : trimmedOrderBy.Remove(indexOfFirstSpace);
                if (!mappingDictionary.ContainsKey(propertyName))
                    throw new ArgumentNullException();
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                    throw new ArgumentNullException();
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
