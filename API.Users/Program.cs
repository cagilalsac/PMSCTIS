using APP.Users;
using APP.Users.Domain;
using APP.Users.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();



// Add services to the container.
// Inversion of Control (IoC) for dependency injection:
var connectionString = builder.Configuration.GetConnectionString("UsersDb"); // get connection string from appsettings.json

// SQLite:
//builder.Services.AddDbContext<UsersDb>(options => options.UseSqlite(connectionString));

// SQL Server LocalDB:
builder.Services.AddDbContext<UsersDb>(options => options.UseSqlServer(connectionString)); // define the DbContext with the connection string

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UsersDbHandler).Assembly)); // for IMediator injection in controllers



// AppSettings:
// Way 1:
//var section = builder.Configuration.GetSection("AppSettings");
// Way 2:
var section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());



// Authentication:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = AppSettings.Issuer,
            ValidAudience = AppSettings.Audience,
            IssuerSigningKey = AppSettings.SigningKey,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });



// Add controllers to the service container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger:
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "API",
        Version = "v1"
    });
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1a2b3c\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme() {
                Reference = new OpenApiReference() {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

// Build the application.
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

// Authentication:
app.UseAuthentication();

// Enable authorization for the application.
app.UseAuthorization();

// Map controllers to the application.
app.MapControllers();

// Run the application.
app.Run();
