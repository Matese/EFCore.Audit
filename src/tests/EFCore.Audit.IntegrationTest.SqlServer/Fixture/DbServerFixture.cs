using EFCore.Audit.IntegrationTest.Fixture;
using Xunit;

namespace EFCore.Audit.IntegrationTest.SqlServer.Fixture
{
    public class DbServerFixture : DbServerFixturer
    {
        public const string CONTAINER = "DbServer";

        [CollectionDefinition(CONTAINER)]
        public class DbServerCollection : ICollectionFixture<DbServerFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }
    }
}
