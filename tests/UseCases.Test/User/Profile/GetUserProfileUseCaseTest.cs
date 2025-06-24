using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using RecipeBook.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // 1. Crio um usuário fictício para testes. Como não preciso de um password, posso ignorar com um "var _".
            // 2. Crio uma instância do UseCase passando o usuário fictício.
            // 3. Executo o UseCase e verifico se o resultado não é nulo e se os campos Name e Email estão preenchidos corretamente.
            (var user, var _) = UserBuilder.Build();
            var useCase = CreateUseCase(user);
            var result = await useCase.Execute();
            result.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Email.ShouldBe(user.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }


    }
}
