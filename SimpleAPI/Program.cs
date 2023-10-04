using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Database;
using SimpleAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<TestModel>, TestModelValidator>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql("Host=127.0.0.1;Port=55298;Database=postgres;Username=postgres;Password=postgres");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() ) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello from SimpleAPI!");

app.Run();

public partial class Program {} // so you can reference it from tests