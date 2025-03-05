using APP.Projects.Domain;
using APP.Projects.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();



// Add services to the container.
// Inversion of Control (IoC) for dependency injection:
var connectionString = builder.Configuration.GetConnectionString("ProjectsDb"); // get connection string from appsettings.json

// SQLite:
//builder.Services.AddDbContext<ProjectsDb>(options => options.UseSqlite(connectionString));

// SQL Server LocalDB:
builder.Services.AddDbContext<ProjectsDb>(options => options.UseSqlServer(connectionString)); // define the DbContext with the connection string

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ProjectsDbHandler).Assembly)); // for IMediator injection in controllers



// Add controllers to the service container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Map default endpoints for the application.
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection for the application.
app.UseHttpsRedirection();

// Enable authorization for the application.
app.UseAuthorization();

// Map controllers to the application.
app.MapControllers();

// Run the application.
app.Run();
