using AutoMapper;
using HRM.DAL.Models;
using HRM.Web;
using HRM.Web.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.Add(typeof(Program));


var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

//redis connection
var redisEndpointUrl = (Environment.GetEnvironmentVariable("REDIS_ENDPOINT_URL") ?? "127.0.0.1:6379").Split(':');
var redisHost = redisEndpointUrl[0];
var redisPort = redisEndpointUrl[1];

string redisConnectionUrl = string.Empty;
var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
if (redisPassword != null)
{
    redisConnectionUrl = $"{redisHost}:{redisPort},password={redisPassword}";
}
else
{
    redisConnectionUrl = $"{redisHost}:{redisPort}";
}
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionUrl));

builder.Services.AddDbContext<HrmContext>();
builder.Services.AddMemoryCache();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
