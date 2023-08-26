using Microsoft.AspNetCore.Identity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebServices.ClientSide;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

//������� ��� ����
app.MapHub<TasksHub>("/GeneralTasks");


app.MapGet("/", () => "Hello World!");

app.Run();
