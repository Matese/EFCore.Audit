using EFCore.Audit.IntegrationTest.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.Audit.IntegrationTest.MySql.Migrations
{
    public class PersonDbContextDesign : IDesignTimeDbContextFactory<PersonDbContext>
    {
        public PersonDbContext CreateDbContext(string[] args)
        {
            var settings = SettingsGetter.Get();

            var options = new DbContextOptionsBuilder<PersonDbContext>()
                .UseMySql(settings.ConnectionString, ServerVersion.AutoDetect(settings.ConnectionString), b => b
                    .MigrationsAssembly(settings.MigrationsAssembly))
                .Options;

            return new PersonDbContext(options);
        }
    }
}