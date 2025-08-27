using DotNetEnv;
using IntegrationApi.Infrastructure.Data;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");

// Para SQL Server, a string de conexão é diferente
string connectionString;
if (dbType == "sqlserver")
{
    connectionString = $"Server={host},{port};Database={dbName};User Id={user};Password={password};TrustServerCertificate=True;";
}
else
{
    connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={dbName}";
}

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (dbType == "sqlserver")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    throw new Exception("Database type not supported");
}

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!string.IsNullOrEmpty(app.Urls.FirstOrDefault(url => url.StartsWith("https://"))))
{
    app.UseHttpsRedirection();
}

app.Run();

