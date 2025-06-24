using Moq;
using RecipeBook.Domain.Services.LoggedUser;

namespace UseCases.Test.User.Profile
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(RecipeBook.Domain.Entities.User user)
        {
            var mock = new Mock<ILoggedUser>();
            mock.Setup(x => x.User()).ReturnsAsync(user);
            return mock.Object;
        }
    }
}
