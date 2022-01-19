using Lenceas.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// Redis缓存 启动服务
    /// </summary>
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisBaseRepository, RedisBaseRepository>();
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton(sp =>
            {
                var configuration = ConfigurationOptions.Parse(ConfigHelper.RedisConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });

        }
    }
}
