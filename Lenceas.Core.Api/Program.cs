using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lenceas.Core.Common;
using Lenceas.Core.Extensions;
using Lenceas.Core.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // 应用程序名称
    ApplicationName = typeof(Program).Assembly.FullName,
    // ContentRoot目录
    //ContentRootPath = Directory.GetCurrentDirectory(),
    // 环境变量
    //EnvironmentName = Environments.Development
    // Look for static files in webroot
    //WebRootPath = "webroot"
});

// AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));
builder.WebHost.UseUrls("http://*:8079");

#region Add services to the container.(向容器添加服务)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new AppSettings(builder.Configuration));
// 确保从认证中心返回的ClaimType不被更改，不使用Map映射
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddMemoryCacheSetup();
builder.Services.AddRedisCacheSetup();
builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = ConfigHelper.RedisConnectionString;
    options.InstanceName = "RedisDistributedCache";
});
builder.Services.AddDbSetup();
builder.Services.AddAutoMapperSetup();
builder.Services.AddCorsSetup();
builder.Services.AddMiniProfilerSetup();
builder.Services.AddSwaggerSetup();
builder.Services.AddHttpContextSetup();
builder.Services.AddAuthorizationSetup();
builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //忽略Model中为null的属性
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.(配置 HTTP 请求管道)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// 封装Swagger展示
app.UseSwaggerMildd();
// CORS跨域
app.UseCors(AppSettings.App(new string[] { "Startup", "Cors", "PolicyName" }));
// 跳转https
//app.UseHttpsRedirection();
// 使用静态文件
app.UseStaticFiles();
// 路由
app.UseRouting();
// 认证
app.UseAuthentication();
// 授权
app.UseAuthorization();
// 性能分析
app.UseMiniProfiler();
// 终结点
app.MapControllers();
// 生成种子数据
app.UseSeedDataMildd(new MySqlContext(new DbContextOptionsBuilder<MySqlContext>().Options), builder.Environment.WebRootPath);
app.Run();
#endregion