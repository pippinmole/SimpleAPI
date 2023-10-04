﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleAPI.Database;
using SimpleAPI.Tests.Impl;

namespace SimpleAPI.Tests; 

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<DatabaseContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using an in-memory database for testing.
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });
            
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", _ => {});

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DatabaseContext>();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            // Seed the database with test data.
            // SeedData.Initialize(scopedServices);
        });
    }
}