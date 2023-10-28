using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;
using ToDoListWebServices.Authorization;
using ToListWebUI.HttpServisec;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();


// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict; // или SameSiteMode.Lax
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always; // Устанавливает Secure для HTTPS
});

// Create DbContext with configuration
#region DbSettings
builder.Services.AddDbContext<IDataContext<ToDoTask>, TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<IDataContext<User>, UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));


builder.Services.AddDbContext<GeneralTasksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GeneralTaskConnection")));

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
#endregion


//Создаем службу нашего Http сервиса авторизации

builder.Services.AddHttpClient();//Это просто настройка http
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthorizationHttpServisec>();
builder.Services.AddHttpClient<AuthorizationHttpServisec>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7212/"); // Установите правильный базовый адрес вашего API
                                                              // Другие настройки HttpClient, если необходимо
});

//Создаем службу нашего Http сервиса апишки
builder.Services.AddScoped<APIToDoListHttpServices>();
builder.Services.AddHttpClient<APIToDoListHttpServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:56478/"); // Установите правильный базовый адрес вашего API
                                                              // Другие настройки HttpClient, если необходимо
});

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

//Маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=StartPage}/{id?}");

app.MapControllerRoute(
    name: "httpresult",
    pattern: "httpresult/{*article}",
    defaults: new { controller = "MyHttpResults", action = "ResultAddTaskDb" });

app.Run();
