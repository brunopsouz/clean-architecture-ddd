﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Application.UseCases.Dashboard;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Application.UseCases.Recipe.Delete;
using RecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Application.UseCases.Recipe.Generate;
using RecipeBook.Application.UseCases.Recipe.GetById;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Application.UseCases.Recipe.Update;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Application.UseCases.User.Profile;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.User.Update;
using Sqids;

namespace RecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddIdEncoder(services, configuration);
            AddUseCases(services);
            
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
            {
                var sqids = option.GetService<SqidsEncoder<long>>()!;

                autoMapperOptions.AddProfile(new AutoMapping(sqids));
            }).CreateMapper());
        }

        private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("settings:IdCryptographyAlphabet")!
            });

            services.AddSingleton(sqids);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
            services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
            services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
            services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
            services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
            services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
            services.AddScoped<IGenerateRecipeUseCase, GenerateRecipeUseCase>();
        }

    }
}
