using System;
using System.Data.Common;
using ApplicationCore.Interfaces.DataAccess;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DataAccess;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShareDataBase;

namespace IntegrationTests
{
	public class UnitOfworkFixture
	{
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;
        public IPublisher Publisher { get; private set; }
        private readonly string dbName = "IntegrationTestsDatabase.db";

        public DbConnection Connection { get; }

        public UnitOfworkFixture()
		{
            Connection = new SqliteConnection($"Filename={dbName}");
            Publisher = new Mock<IPublisher>().Object;
            Seed();
            Connection.Open();
        }
        public HomeNursingContext CreateContext()
        {
            var context = new HomeNursingContext(new DbContextOptionsBuilder<HomeNursingContext>().UseSqlite(Connection).Options);
            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        ShareDatabaseMock.SeedData(context);

                    }

                    _databaseInitialized = true;
                }
            }
        }
    }
}

