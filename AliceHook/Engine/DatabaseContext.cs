using System;
using AliceHook.Models;
using Microsoft.EntityFrameworkCore;

namespace AliceHook.Engine
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string database = "alicehook";
            var host = Environment.GetEnvironmentVariable("POSTGRESQL_HOST");
            var user = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
            
            optionsBuilder.UseNpgsql(
                $@"Host = {host};"
                + "Port = 25060;"
                + $"Database = {database};"
                + $"Username = {user};"
                + $"Password = {password};"
                + "CommandTimeout = 20;"
                + "SslMode = Require;"
                + "TrustServerCertificate = true;"
            );
        }
    }
}