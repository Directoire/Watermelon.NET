using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Watermelon.NET.Configurations;

namespace Watermelon.NET.Data.Context
{
    public class WatermelonContextFactory : IDesignTimeDbContextFactory<WatermelonContext>
    {
        public WatermelonContext CreateDbContext(string[] args)
        {
            var databaseConnection = GetDatabaseConnection();
            var optionsBuilder = new DbContextOptionsBuilder()
                .UseNpgsql(databaseConnection);

            return new WatermelonContext(optionsBuilder.Options);
        }
        
        private string GetDatabaseConnection()
        {
            var configuration = new Configuration();
            
            var connectionString = new StringBuilder();
            connectionString.Append("Host=").Append(configuration.DatabaseConfiguration.Host).Append(';');

            if (configuration.DatabaseConfiguration.Port > 0)
                connectionString.Append("Port=").Append(configuration.DatabaseConfiguration.Port).Append(';');

            connectionString.Append("Username=").Append(configuration.DatabaseConfiguration.Username).Append(';')
                .Append("Password=").Append(configuration.DatabaseConfiguration.Password).Append(';')
                .Append("Database=").Append(configuration.DatabaseConfiguration.Database).Append(';');

            return connectionString.ToString();
        }
    }
}