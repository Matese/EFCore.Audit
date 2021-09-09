using EFCore.Audit.IntegrationTest.Config;
using EFCore.Audit.TestCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.Audit.IntegrationTest.SqlServer.Migrations
{
    public class PersonDbContextDesign : IDesignTimeDbContextFactory<PersonDbContext>
    {
        public PersonDbContext CreateDbContext(string[] args)
        {
            var settings = SettingsGetter.Get();

            var options = new DbContextOptionsBuilder<PersonDbContext>()
                .UseSqlServer(settings.ConnectionString, b => b
                    .MigrationsAssembly(settings.MigrationsAssembly))
                .Options;

            return new PersonDbContext(options);
        }
    }
}