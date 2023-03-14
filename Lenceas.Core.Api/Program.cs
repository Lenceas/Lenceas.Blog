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
    // Ӧ�ó�������
    ApplicationName = typeof(Program).Assembly.FullName,
    // ContentRootĿ¼
    //ContentRootPath = Directory.GetCurrentDirectory(),
    // ��������
    //EnvironmentName = Environments.Development
    // Look for static files in webroot
    //WebRootPath = "webroot"
});

// AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));
builder.WebHost.UseUrls("http://*:8079");

#region Add services to the container.(��������ӷ���)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new AppSettings(builder.Configuration));
// ȷ������֤���ķ��ص�ClaimType�������ģ���ʹ��Mapӳ��
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
                    //����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //��ʹ���շ���ʽ��key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //����Model��Ϊnull������
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.(���� HTTP ����ܵ�)
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
// ��װSwaggerչʾ
app.UseSwaggerMildd();
// CORS����
app.UseCors(AppSettings.App(new string[] { "Startup", "Cors", "PolicyName" }));
// ��תhttps
//app.UseHttpsRedirection();
// ʹ�þ�̬�ļ�
app.UseStaticFiles();
// ·��
app.UseRouting();
// ��֤
app.UseAuthentication();
// ��Ȩ
app.UseAuthorization();
// ���ܷ���
app.UseMiniProfiler();
// �ս��
app.MapControllers();
// ������������
app.UseSeedDataMildd(new MySqlContext(new DbContextOptionsBuilder<MySqlContext>().Options), builder.Environment.WebRootPath);
app.Run();
#endregion