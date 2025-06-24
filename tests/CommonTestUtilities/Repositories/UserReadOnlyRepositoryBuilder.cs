using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistActiveUserWithEmail(string email)
        {
            // criada condição para teste de UseCase para simular um retorno true de um email já cadastrado no banco de dados
            _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public void GetByEmailAndPassword(User user)
        {
            _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
        }

        public IUserReadOnlyRepository Build()
        {
            return _repository.Object;
        }

    }
}
