using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace EFCore.Audit.IntegrationTest.Tests
{
    //https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/#unit-testing
    //https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sharing-databases
    public abstract class PersonDbContextTestBase : IDisposable
    {
        private bool _disposed;

        public DbConnection Connection { get; private set; }
        public string MigrationAssembly { get; private set; }
        public List<PersonEntity> PersonTestData { get; private set; }
        public List<AddressEntity> AddressTestData { get; private set; }
        public List<PersonAttributesEntity> PersonAttributeTestData { get; private set; }

        public PersonDbContextTestBase(DbConnection connection, string migrationAssembly)
        {
            this.Connection = connection;
            this.Connection.Open();
            this.MigrationAssembly = migrationAssembly;
            this.ReloadTestData();
            this.CreateDatabase();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Connection.Close();
                    Connection.Dispose();
                }

                _disposed = true;
            }
        }

        public PersonDbContext CreateContext(DbTransaction transaction = null)
        {
            var options = GetDbContextOptions(this.Connection, this.MigrationAssembly);

            var context = new PersonDbContext(options, new UserProvider());

            if (transaction != null)
                context.Database.UseTransaction(transaction);

            return context;
        }

        public abstract DbContextOptions<PersonDbContext> GetDbContextOptions(DbConnection Connection, string migrationAssembly);

        protected abstract void CreateDatabase();

        private void ReloadTestData()
        {
            if (this.PersonTestData == default)
                this.PersonTestData = new List<PersonEntity>();

            if (this.AddressTestData == default)
                this.AddressTestData = new List<AddressEntity>();

            if (this.PersonAttributeTestData == default)
                this.PersonAttributeTestData = new List<PersonAttributesEntity>();

            this.PersonTestData.Clear();
            this.AddressTestData.Clear();
            this.PersonAttributeTestData.Clear();

            this.PersonTestData
                .AddRange(GetList<PersonEntity>("Seed\\person-test-data.json"));
            
            this.AddressTestData
                .AddRange(GetList<AddressEntity>("Seed\\address-test-data.json"));

            this.PersonAttributeTestData
                .AddRange(GetList<PersonAttributesEntity>("Seed\\person-attribute-test-data.json"));
        }

        private List<T> GetList<T>(string path)
        {
            using var file = File.OpenText(path);
            
            var collection = new Newtonsoft.Json.JsonSerializer()
                .Deserialize(file, typeof(List<T>)) as List<T>;

            return collection;
        }
    }
}
