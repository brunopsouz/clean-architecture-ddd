using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using Xunit;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // criei um UserBuilder para me retornar um usuário e uma senha já criptografada.
            (var user, var password) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldNotBeNullOrWhiteSpace(user.Name);
            result.Name.ShouldBe(user.Name);
            result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();

        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> action = async () => await useCase.Execute(request);

            var exception = await Should.ThrowAsync<InvalidLoginException>(action);
            exception.Message.ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);

        }

        private static DoLoginUseCase CreateUseCase(RecipeBook.Domain.Entities.User? user = null)
        {
            var passwordencripter = PasswordEncripterBuilder.Build(); 
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (user is not null)
                userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

            return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordencripter, accessTokenGenerator);
        }
    }
}
