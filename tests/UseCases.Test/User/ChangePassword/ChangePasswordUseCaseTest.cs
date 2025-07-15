using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();
            
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => await useCase.Execute(request);

            await action.ShouldNotThrowAsync();

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.ShouldBe(passwordEncripter.Encrypt(request.NewPassword));
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            (var user, var password) = UserBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                Password = password,
                NewPassword = string.Empty
            };

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.ShouldBe(passwordEncripter.Encrypt(password));

        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, var password) = UserBuilder.Build();
            
            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);

            var passwordEncripter = PasswordEncripterBuilder.Build();
            
            user.Password.ShouldBe(passwordEncripter.Encrypt(password));
        }

        private static ChangePasswordUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            
            
            return new ChangePasswordUseCase(loggedUser, repository, unitOfWork, passwordEncripter);
        }

    }
}
