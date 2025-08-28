using DotNetEnv;
using IntegrationApi.Infrastructure.Data;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Core.Entities;
using IntegrationApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using IntegrationApi.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
string connectionString = ConnectionStringProvider.GetConnectionString();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddScoped<IRepositoryBase<Character>, RepositoryBase<Character>>();
builder.Services.AddScoped<IRepositoryBase<Animation>, RepositoryBase<Animation>>();
builder.Services.AddScoped<IAnimationService, AnimationService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddHttpClient<GraphQLService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!string.IsNullOrEmpty(app.Urls.FirstOrDefault(url => url.StartsWith("https://"))))
{
    app.UseHttpsRedirection();
}

app.UseGlobalExceptionHandler();

app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();

