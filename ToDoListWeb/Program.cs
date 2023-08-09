using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWeb.Interfaces;
using Microsoft.Extensions.Configuration;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;
using ToDoListWeb.ClientSide;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Create DbContext with configuration
builder.Services.AddDbContext<IDataContext, TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));


builder.Services.AddScoped<TaskDbContex>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new TaskDbContex(configuration);
});
builder.Services.AddScoped<GeneralTasksDbContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new GeneralTasksDbContext(configuration);
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
        options.Password.RequireDigit = false;             // ��������� �����
        options.Password.RequireLowercase = false;          // ��������� �������� �����
        options.Password.RequireUppercase = false;          // ��������� ��������� �����
        options.Password.RequireNonAlphanumeric = false;   // ��������� ����������� �������
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

builder.Services.Configure<MvcViewOptions>(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

builder.Services.AddSignalR();

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

//������� ��� ����
app.MapHub<TasksHub>("/GeneralTasks");


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=StartPage}/{id?}");
    

app.Run();
