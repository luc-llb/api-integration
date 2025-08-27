using DotNetEnv;
using IntegrationApi.Infrastructure.Data;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Core.Entities;
using IntegrationApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
string connectionString = ConnectionStringProvider.GetConnectionString();

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

builder.Services.AddScoped<IRepositoryBase<Character>, RepositoryBase<Character>>();
builder.Services.AddScoped<IRepositoryBase<Animation>, RepositoryBase<Animation>>();

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

