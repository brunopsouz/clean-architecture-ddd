using Microsoft.Extensions.Configuration;
using RecipeBook.Domain.Enums;

namespace RecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {

        //Método que vou recuperar o banco de dados InMemoryTest.
        public static bool IsUnitTestEnvironment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
           
        }

        /// <summary>
        ///  método que vai pegar a string de conexão do banco de dados e o tipo do banco de dados.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DatabaseType DatabaseType(this IConfiguration configuration)
        {
            var databaseType = configuration.GetConnectionString("DatabaseType");

            //vai transformar o DatabaseType em databaseType 
            return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
        }

        /// <summary>
        /// método que vai pegar a string de conexão do banco de dados.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string ConnectionString(this IConfiguration configuration)
        {
            var databaseType = configuration.DatabaseType();

            if (databaseType == Domain.Enums.DatabaseType.SQLServer)
            {
                return configuration.GetConnectionString("ConnectionSQLServer")!;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
