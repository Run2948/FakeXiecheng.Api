using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public bool PropertyMappingExists<TSource, TDestination>(string fields)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                var propertyMapping = GetPropertyMapping<TSource, TDestination>();

                var fieldsAfterSplit = fields.Split(",");

                // foreach (var field in fieldsAfterSplit)
                // {
                //     var trimmedField = field.Trim();
                //     var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                //     var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                //     if (!propertyMapping.ContainsKey(propertyName)) return false;
                // }

                return (from field in fieldsAfterSplit select field.Trim() into trimmedField let indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal) select indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace)).All(propertyName => propertyMapping.ContainsKey(propertyName));
            }
            return true;
        }

        public bool PropertyExists<T>(string fields)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                var fieldsAfterSplit = fields.Split(",");
                return fieldsAfterSplit.Select(field => field.Trim()).Select(propertyName => typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)).All(property => property != null);
            }
            return true;
        }
    }
}
