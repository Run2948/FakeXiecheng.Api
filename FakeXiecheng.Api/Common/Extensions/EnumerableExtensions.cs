using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = new List<ExpandoObject>();

            var properties = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                properties.AddRange(typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance));
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");

                // foreach (var field in fieldsAfterSplit)
                // {
                //     var propertyName = field.Trim();
                //     var property = typeof(TSource).GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                //     // if(property == null) throw new KeyNotFoundException($"属性 {propertyName} 找不到 {typeof(TSource)}");
                //     if (property != null)
                //     {
                //         properties.Add(property);
                //     }
                // }

                properties.AddRange(fieldsAfterSplit.Select(field => field.Trim()).Select(propertyName => typeof(TSource).GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)).Where(property => property != null));
            }

            foreach (var item in source)
            {
                var obj = new ExpandoObject();
                foreach (var property in properties)
                {
                    obj.TryAdd(property.Name, property.GetValue(item, null));
                }
                list.Add(obj);
            }

            return list;
        }
    }
}
