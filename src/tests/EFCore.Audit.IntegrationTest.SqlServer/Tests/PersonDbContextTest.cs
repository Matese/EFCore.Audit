using EFCore.Audit.IntegrationTest.SqlServer.Fixture;
using EFCore.Audit.IntegrationTest.Tests;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Xunit;

namespace EFCore.Audit.IntegrationTest.SqlServer.Tests
{
    [Collection(DbServerFixture.CONTAINER)]
    public class PersonDbContextTest : PersonDbContextIntegrationTestBase
    {
        private static readonly object _lock = new();
        
        public PersonDbContextTest(DbServerFixture fixture)
          : base(new SqlConnection(fixture.Settings.ConnectionString),
                fixture.Settings.MigrationsAssembly)
        {
        }

        public override DbContextOptions<PersonDbContext> GetDbContextOptions(DbConnection connection, string migrationAssembly)
            => new DbContextOptionsBuilder<PersonDbContext>()
                .UseSqlServer(connection, b => b
                    .MigrationsAssembly(migrationAssembly)).Options;
        
        protected override void CreateDatabase()
        {
            lock (_lock)
            {
                using var context = CreateContext();
                context.Database.Migrate();
                context.SaveChanges();
            }
        }

        #region Facts

        [Fact]
        public override void Add_one_entity_with_preGenerated_properties_Generates_one_auditEntity_and_auditMetaDataEntity()
        {
            base.Add_one_entity_with_preGenerated_properties_Generates_one_auditEntity_and_auditMetaDataEntity();
        }

        [Fact]
        public override void Add_one_entity_with_onAddGenerated_properties_one_entity_with_pre_generated_properties_Generates_two_auditentity_and_two_auditMetaDataEntities()
        {
            base.Add_one_entity_with_onAddGenerated_properties_one_entity_with_pre_generated_properties_Generates_two_auditentity_and_two_auditMetaDataEntities();
        }

        [Fact]
        public override void Add_and_update_one_entity_with_preGeneratedProperties_Generates_two_audit_and_one_auditMetaDataEntity()
        {
            base.Add_and_update_one_entity_with_preGeneratedProperties_Generates_two_audit_and_one_auditMetaDataEntity();
        }

        [Fact]
        public override void Add_and_update_entities_preGenerated_and_onAddGeneratedProperties_Generates_two_audit_and_one_audit_metaDataEntity()
        {
            base.Add_and_update_entities_preGenerated_and_onAddGeneratedProperties_Generates_two_audit_and_one_audit_metaDataEntity();
        }

        #endregion
    }
}