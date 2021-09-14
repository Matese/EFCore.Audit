using EFCore.Audit.IntegrationTest;
using EFCore.Audit.IntegrationTest.Sqlite.Fixture;
using EFCore.Audit.IntegrationTest.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;

namespace EFCore.Audit.UnitTest
{
    [Collection(DbServerFixture.CONTAINER)]
    public class PersonDbContextTestAsync : PersonDbContextIntegrationTestBaseAsync
    {
        private static readonly object _lock = new();

        public PersonDbContextTestAsync(DbServerFixture fixture)
            : base(new SqliteConnection(fixture.Settings.ConnectionString), null)
        {
        }

        public override DbContextOptions<PersonDbContext> GetDbContextOptions(DbConnection Connection, string migrationAssembly)
            => new DbContextOptionsBuilder<PersonDbContext>().UseSqlite(this.Connection).Options;

        protected override void CreateDatabase()
        {
            lock (_lock)
            {
                using var context = CreateContext();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }

        #region Facts

        [Fact]
        public override Task Add_and_update_entities_preGenerated_and_onAddGeneratedProperties_Generates_two_audit_and_one_audit_metaDataEntity()
        {
            return base.Add_and_update_entities_preGenerated_and_onAddGeneratedProperties_Generates_two_audit_and_one_audit_metaDataEntity();
        }

        [Fact]
        public override Task Add_and_update_one_entity_with_preGeneratedProperties_Generates_two_audit_and_one_auditMetaDataEntity()
        {
            return base.Add_and_update_one_entity_with_preGeneratedProperties_Generates_two_audit_and_one_auditMetaDataEntity();
        }

        [Fact]
        public override Task Add_one_entity_with_onAddGenerated_properties_one_entity_with_pre_generated_properties_Generates_two_auditentity_and_two_auditMetaDataEntities()
        {
            return base.Add_one_entity_with_onAddGenerated_properties_one_entity_with_pre_generated_properties_Generates_two_auditentity_and_two_auditMetaDataEntities();
        }

        [Fact]
        public override Task Add_one_entity_with_preGenerated_properties_Generates_one_auditEntity_and_auditMetaDataEntity()
        {
            return base.Add_one_entity_with_preGenerated_properties_Generates_one_auditEntity_and_auditMetaDataEntity();
        }

        #endregion
    }
}
