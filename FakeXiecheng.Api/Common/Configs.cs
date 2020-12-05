using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Common
{
    public abstract class Configs
    {
    }

    public class JwtConfigs : Configs
    {
        /// <summary>
        /// 发布方
        /// </summary>
        public static string Issuer { get; set; }

        /// <summary>
        /// 加密key
        /// </summary>
        public static string Key { get; set; }

        /// <summary>
        /// 生命周期，单位分
        /// </summary>
        public static int Expires { get; set; }
    }
}
