using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GiderTakipSistemi.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // appsettings.json dosyasının yolunu belirle
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Connection string'i al
            var connectionString = configuration.GetConnectionString("GiderTakipDB");

            // SQL Server kullanacağını belirt
            optionsBuilder.UseSqlServer(connectionString);

            // DbContext'i oluştur ve döndür
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
