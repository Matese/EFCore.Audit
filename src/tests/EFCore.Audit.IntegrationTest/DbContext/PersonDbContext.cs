using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace EFCore.Audit.IntegrationTest
{
    public class PersonDbContext : AuditDbContextBase<PersonDbContext>
    {
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<PersonAttributesEntity> PersonAttributes { get; set; }

        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public PersonDbContext(DbContextOptions<PersonDbContext> options, IAuditUserProvider auditUserProvider) : base(options, auditUserProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>()
               .HasMany(p => p.Addresses)
               .WithOne(a => a.Person);

            modelBuilder.Entity<PersonEntity>()
               .HasMany(p => p.Attributes)
               .WithOne(a => a.Person);

            modelBuilder.ApplyConfiguration(new PersonEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AddressEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PersonAttributesEntityConfiguration());

            HandleDateTimeOffsetOnSqlite(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        /// </summary>
        private void HandleDateTimeOffsetOnSqlite(ModelBuilder modelBuilder)
        {
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }
    }
}
