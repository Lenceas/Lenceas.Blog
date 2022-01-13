var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.(向容器添加服务)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.(配置 HTTP 请求管道)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion