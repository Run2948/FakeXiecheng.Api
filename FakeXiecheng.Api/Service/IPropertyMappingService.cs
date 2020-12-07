using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Service
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool PropertyMappingExists<TSource, TDestination>(string fields);

        bool PropertyExists<T>(string fields);
    }
}
