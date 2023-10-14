using Animals.API.Extensions;
using Animals.BLL.Impl;
using Animals.DAL.Impl;
using Animals.DAL.Impl.Context;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add secrets configuration
builder.Configuration.AddUserSecrets<Program>();

var connectionString = builder.Configuration.GetConnectionString("AnimalsDefaultConnection");

builder.Services.AddDbContext<AnimalsContext>(
    options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.InstallRepositories();
builder.Services.InstallMappers();
builder.Services.InstallServices();

builder.Services.SeedData();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRateLimitHandler(builder.Configuration);

var app = builder.Build();

app.UseIpRateLimiting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
