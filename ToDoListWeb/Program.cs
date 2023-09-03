using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();



// Create DbContext with configuration
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




app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Task}/{action=GetTaskDb}/{id?}");



app.Run();
