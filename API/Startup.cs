using API.AOP;
using API.Filter;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Common.Helper;
using Common.LogHelper;
using Data;
using Extensions.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public string ApiName { get; set; } = "Lenceas.Blog";
        public string BasePath = AppContext.BaseDirectory;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));
            services.AddDbContext<MySqlDbContext>();

            services.AddSingleton(new LogLock(Env.ContentRootPath));

            services.AddDbSetup();
            services.AddAppConfigSetup();

            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            })
                //全局配置Json序列化处理
                .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = $"{ApiName}",
                    Description = $"{ApiName} 接口文档——.NET Core 3.1"
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(BasePath, "API.xml");
                c.IncludeXmlComments(xmlPath, true);

                var xmlModelPath = Path.Combine(BasePath, "Models.xml");
                c.IncludeXmlComments(xmlModelPath);
            });

        }

        // 注意在Program.CreateHostBuilder，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", $"{ApiName} HTTP API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
