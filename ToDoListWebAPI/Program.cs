using Microsoft.EntityFrameworkCore;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure app configuration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

//HttpClient Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5157/") //адрес  UI приложения.
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Create DbContext with configuration
builder.Services.AddDbContext<IDataContext<ToDoTask>, TaskDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<TaskDbContex>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new TaskDbContex(configuration);
});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseRouting();

app.UseCors("AllowSpecificOrigin");//Используем для HttpClient

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Task}/{action=AddTaskDb}/{id?}");

app.Run();
