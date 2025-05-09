using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories.User
{
    /// <summary>
    /// Interface para ser usada de herança em UserRepository em INFRASTRUCTER que faz transação com o Banco para apenas métodos de POST.
    /// </summary>
    public interface IUserWriteOnlyRepository
    {
        public Task Add(Entities.User user);
    }
}
