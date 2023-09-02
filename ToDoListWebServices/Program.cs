using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;
using ToDoListWebServices.ClientSide;

var builder = WebApplication.CreateBuilder(args);

// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();



// Create DbContext with configuration
builder.Services.AddDbContext<IDataContext<ToDoTask>, TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<IDataContext<User>, UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));



builder.Services.AddScoped<TaskDbContex>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new TaskDbContex(configuration);
});

builder.Services.AddScoped<UserDbContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new UserDbContext(configuration);
});

#region for logging aand register
//Role
builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
//Config
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;             // Требовать цифры
    options.Password.RequireLowercase = false;          // Требовать строчные буквы
    options.Password.RequireUppercase = false;          // Требовать прописные буквы
    options.Password.RequireNonAlphanumeric = false;   // Требовать специальные символы
                                                       //минимальноя длина пороля
    options.Password.RequiredLength = 8;
    //максимум повторных попыток ввода пороля-имени
    options.Lockout.MaxFailedAccessAttempts = 10;
    //бан на время после исчерпания попыток
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

    options.Lockout.AllowedForNewUsers = true;
});
//coockie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    //нужно заново войти в аккаунт после 7 дней бездействия
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;

});
#endregion
var app = builder.Build();
//Маршрут для хаба
//app.MapHub<TasksHub>("/GeneralTasks");


app.MapGet("/", () => "Hello World!");

app.Run();
