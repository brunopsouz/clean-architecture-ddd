using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories.User
{
    /// <summary>
    /// Interface para ser usada de herança em UserRepository em INFRASTRUCTER que faz transação com o Banco para apenas métodos de GET.
    /// </summary>
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistActiveUserWithEmail(string email);
        public Task<Entities.User> GetByEmailAndPassword(string email, string password);
    }
}
