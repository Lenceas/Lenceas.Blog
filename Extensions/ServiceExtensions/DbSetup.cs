using Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.ServiceExtensions
{
    /// <summary>
    /// Db 启动服务
    /// </summary>
    public static class DbSetup
    {
        public static void AddDbSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<DbInitializer>();
            services.AddScoped<MySqlDbContext>();
        }
    }
}
