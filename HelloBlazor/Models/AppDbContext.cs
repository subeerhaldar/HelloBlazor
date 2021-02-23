using HelloBlazor.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace HelloBlazor.Models
{
    public class AppDbContext : IdentityDbContext //DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecast { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>()//.HasNoKey()
                    .HasData(
                    new WeatherForecast { Id = 1, Date = DateTime.Now, TemperatureC = 10, Summary = "Freezing" },
                    new WeatherForecast { Id = 2, Date = DateTime.Now, TemperatureC = 30, Summary = "Cool" },
                    new WeatherForecast { Id = 3, Date = DateTime.Now, TemperatureC = 35, Summary = "Mild" },
                    new WeatherForecast { Id = 4, Date = DateTime.Now, TemperatureC = 40, Summary = "Warm" },
                    new WeatherForecast { Id = 5, Date = DateTime.Now, TemperatureC = 50, Summary = "Hot" }
                    );
        }
    }
}
