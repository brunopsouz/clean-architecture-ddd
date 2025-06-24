using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Extensions;

namespace RecipeBook.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
        {
            if (databaseType == DatabaseType.SQLServer)
                EnsureDatabaseCreated(connectionString);


            MigrationDatabase(serviceProvider);
        }

        private static void EnsureDatabaseCreated(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var dataBaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.Remove("Database");

            using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("name", dataBaseName);

            var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name=@name", parameters);

            if (records.Any().IsFalse())
                dbConnection.Execute($"CREATE DATABASE {dataBaseName}");

        }
        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();

            runner.MigrateUp();

        }
    }
}
