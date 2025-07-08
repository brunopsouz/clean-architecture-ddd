using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Security.Criptography;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using RecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using RecipeBook.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace RecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// Configuraçoes da INJEÇÃO DE DEPENDENCIAS para o projeto de INFRASTRUCTURE.
        /// passando "THIS IServiceCollection" como parametro. Onde posteriormente esse método será adicionado 
        /// na classe PROGRAM de RecipeBook.API.
        /// 
        /// Isso para evitar de muitas linhas serem adicionadas em PROGRAM. Boas praticas separar em uma classe.
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncrypter(services, configuration);
            AddRepositories(services);
            AddLoggerUser(services);
            AddTokens(services, configuration);
            

            // método que vai verificar se o ambiente é de teste integração (banco de dados inMemory).
            if (configuration.IsUnitTestEnvironment())
                return;

            // método que vai pegar a string de conexão do banco de dados e o tipo do banco de dados.
            var databaseType = configuration.DatabaseType();

            if (databaseType == DatabaseType.SQLServer)
            {
                AddDbContextSqlServer(services, configuration);
                AddFluentMigrator_SqlServer(services, configuration);
            }
        } 
        /// <summary>
        /// Configuração do DbContext com SQL Server onde referencio a classe de DbContext em AddDbContext.
        /// </summary>
        /// <param name="services"></param>
        private static void AddDbContextSqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<RecipeBookDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }
        /// <summary>
        /// Configuração de SERVIÇO das Interfaces, onde cada uma delas deve referenciar a UserRepository do proj INFRASTRUCTURE.
        /// 
        /// Ambas Interfaces herdam UserRepository.
        /// 
        /// IUserWriteOnlyRepository: Configurada para POST/ESCREVER os dados de User no banco.
        /// 
        /// IUserReadOnlyRepository: Configurada para GET/LER os dados de User no banco.
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        }

        private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All();
            });
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
        }

        private static void AddLoggerUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            ///Microsoft.Extensions.Configuration.Binder
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped<IPasswordEncripter>(options => new Sha512Encripter(additionalKey!));
        }

    }
}
