using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.AutoMapper;
using Microsoft.Extensions.Configuration;
using RecipeBook.Application.Services.Criptography;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Application.UseCases.User.Profile;

namespace RecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncrypter(services, configuration);
            AddAutoMapper(services);
            AddUseCases(services);
            
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }
        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            ///Microsoft.Extensions.Configuration.Binder
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped(options => new PasswordEncripter(additionalKey!));
        }
    }
}
