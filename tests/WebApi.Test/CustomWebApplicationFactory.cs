using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Cria um servidor de teste para a aplicação. Tenho que chamar a classe "Program" pois ela que executa a aplicação.
        // Nesse momento é necessário criar um servidor de teste para a aplicação, pois o teste de integração não é unitário.
        // Teste de integração executa a API e faz de fato requisições na API testando a resposta da API.
        // Obs: Criar um construtor para Program.cs, pois o WebApplicationFactory não consegue instanciar a classe Program.cs.


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    // Aqui você pode configurar serviços específicos para os testes, como mocks ou bancos de dados em memória.
                    // services.AddDbContext<YourDbContext>(options => ...);

                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<RecipeBookDbContext>));
                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<RecipeBookDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });
                });
        }
    }
}
