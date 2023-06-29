using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Create DbContext with configuration
builder.Services.AddDbContext<TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TaskDbContex>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new TaskDbContex(configuration);
});

#region for logging aand register
    //Role
    builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<TaskDbContex>()
        .AddDefaultTokenProviders();
    //Config
    builder.Services.Configure<IdentityOptions>(options =>
    {
        //����������� ����� ������
        options.Password.RequiredLength = 8;
        //�������� ��������� ������� ����� ������-�����
        options.Lockout.MaxFailedAccessAttempts = 10;
        //��� �� ����� ����� ���������� �������
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

        options.Lockout.AllowedForNewUsers = true;
    });
    //coockie
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        //����� ������ ����� � ������� ����� 7 ���� �����������
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.SlidingExpiration = true;

    });
#endregion



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
