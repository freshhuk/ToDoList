using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;
using ToDoListWebServices.ClientSide;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

//HttpClient
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:44339") // �������� �� ����� ������ API ��� UI ����������.
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// Create DbContext with configuration
builder.Services.AddDbContext<IDataContext<ToDoTask>, TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<UserDbContext>(options =>
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

#region for logging and register
//Role


builder.Services.AddIdentity<User, IdentityRole>()
        .AddSignInManager<SignInManager<User>>()
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
var app = builder.Build();
//������� ��� ����
//app.MapHub<TasksHub>("/GeneralTasks");

app.UseCors("AllowSpecificOrigin");//���������� ��� HttpClient

app.UseAuthentication();   // ���������� middleware �������������� 
app.UseAuthorization();   // ���������� middleware ����������� 

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Account}/{action=Register}/{id?}");

app.Run();
