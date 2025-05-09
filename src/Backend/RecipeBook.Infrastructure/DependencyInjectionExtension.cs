﻿using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
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
            var databaseType = configuration.DatabaseType();

            if (databaseType == DatabaseType.SQLServer)
            {
                AddDbContextSqlServer(services, configuration);
                AddFluentMigrator_SqlServer(services, configuration);
            }
            
            AddRepositories(services);
            
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
    }
}
