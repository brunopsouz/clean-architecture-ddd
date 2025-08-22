using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.User.Update;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => await useCase.Execute(request);

            await Should.NotThrowAsync(action);

            user.Name.ShouldBe(request.Name);
            user.Email.ShouldBe(request.Email);

        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NAME_EMPTY);

            user.Name.ShouldNotBe(request.Name);
            user.Email.ShouldNotBe(request.Email);

        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

            user.Name.ShouldNotBe(request.Name);
            user.Email.ShouldNotBe(request.Email);

        }

        private static UpdateUserUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, string? email = null)
        {
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
            if (string.IsNullOrEmpty(email).IsFalse())
                userReadOnlyRepository.ExistActiveUserWithEmail(email!);

            var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);


            return new UpdateUserUseCase(userReadOnlyRepository.Build(), userUpdateOnlyRepository, unitOfWork, loggedUser);
        }

    }
}
