using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.idEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeBook.API.BackgroundServices;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Enums;
using RecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Cria um servidor de teste para a aplicação. Tenho que chamar a classe "Program" pois ela que executa a aplicação.
        // Nesse momento é necessário criar um servidor de teste para a aplicação, pois o teste de integração não é unitário.
        // Teste de integração executa a API e faz de fato requisições na API testando a resposta da API.
        // Obs: Criar um construtor para Program.cs, pois o WebApplicationFactory não consegue instanciar a classe Program.cs.

        private RecipeBook.Domain.Entities.Recipe _recipe = default!;
        private RecipeBook.Domain.Entities.User _user = default!;
        private RecipeBook.Domain.Entities.RefreshToken _refreshToken = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    // Aqui você pode configurar serviços específicos para os testes, como mocks ou bancos de dados em memória.
                    // services.AddDbContext<YourDbContext>(options => ...);

                    //Vou buscar na minha injeção de dependência o contexto do banco de dados RecipeBookDbContext.
                    //se existir eu vou remover ele e substituir o banco de dados por um banco de dados em memória.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<RecipeBookDbContext>));
                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }

                    //Aqui estou adicionando o banco de dados em memória, substituto do banco de dados existente.
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    var blobStorage = new BlobStorageServiceBuilder().Build();
                    services.AddScoped(option => blobStorage);

                    services.AddDbContext<RecipeBookDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    // Remove background service não necessário nos testes
                    var deleteUserServiceDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IHostedService) &&
                             d.ImplementationType == typeof(DeleteUserService));

                    if (deleteUserServiceDescriptor is not null)
                    {
                        services.Remove(deleteUserServiceDescriptor);
                    }

                    // p/ criar teste de integração de Login.
                    // Remove o banco de dados existente para garantir que os testes comecem com um estado limpo.
                    // Isso é útil para evitar conflitos entre testes que podem ter criado dados no banco de dados.
                    //
                    // Crio um escopo para acessar o contexto do banco de dados, busco um serviço do tipo RecipeBookDbContext e chamo EnsureDeleted para remover o banco de dados.
                    using var scope = services.BuildServiceProvider().CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();

                    dbContext.Database.EnsureDeleted();

                    StartDatabase(dbContext);

                });
        }

        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetName() => _user.Name;
        public string GetRefreshToken() => _refreshToken.Value;
        public Guid GetUserIdentifier() => _user.UserIdentifier;


        public string GetRecipeId() => IdEncripterBuilder.Build().Encode(_recipe.Id);
        public string GetRecipeTitle() => _recipe.Title;
        public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
        public CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
        public IList<RecipeBook.Communication.Enums.DishType> GetDishTypes() => (IList<RecipeBook.Communication.Enums.DishType>)_recipe.DishTypes.Select(c => c.Type).ToList();

        private void StartDatabase(RecipeBookDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();

            _recipe = RecipeBuilder.Build(_user);

            _refreshToken = RefreshTokenBuilder.Build(_user);

            dbContext.Users.Add(_user);

            dbContext.Recipes.Add(_recipe);

            dbContext.RefreshTokens.Add(_refreshToken);

            dbContext.SaveChanges();

        }
    }
}
