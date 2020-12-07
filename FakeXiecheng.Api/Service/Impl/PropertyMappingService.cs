using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;
using FakeXiecheng.Api.Models.Dtos;

namespace FakeXiecheng.Api.Service.Impl
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _touristRoutePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>{ "Id"})},
            { "Title", new PropertyMappingValue(new List<string>{ "Title"})},
            { "Rating", new PropertyMappingValue(new List<string>{ "Rating"})},
            { "OriginalPrice", new PropertyMappingValue(new List<string>{ "OriginalPrice"})}
        };





        public IList<IPropertyMapping> PropertyMappings { get; }

        public PropertyMappingService()
        {
            PropertyMappings ??= new List<IPropertyMapping>();
            PropertyMappings.Add(new PropertyMapping<TouristRouteDto, TouristRoute>(_touristRoutePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = PropertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>.");
        }
    }
}
