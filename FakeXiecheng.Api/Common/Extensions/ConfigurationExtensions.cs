using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FakeXiecheng.Api.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T AddSectionValue<T>(this IConfiguration configuration, string section = null) where T : Configs
        {
            var _ = configuration?.GetSection(section ?? typeof(T).Name);
            return _?.Get<T>();
        }
    }
}
