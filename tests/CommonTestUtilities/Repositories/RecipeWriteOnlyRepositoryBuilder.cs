using Moq;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories
{
    public class RecipeWriteOnlyRepositoryBuilder
    {
        public static IRecipeWriteOnlyRepository Build()
        {
            // Aqui você deve criar uma instância do IRecipeWriteOnlyRepository com as dependências necessárias.
            // Por exemplo, você pode usar mocks ou stubs para o repositório de escrita de receitas.
            // Exemplo de implementação fictícia:
            var mock = new Mock<IRecipeWriteOnlyRepository>();
            return mock.Object;
        }
    }
}
