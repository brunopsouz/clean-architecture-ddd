using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using Microsoft.IdentityModel.Tokens.Experimental;
using RecipeBook.Application.UseCases.Recipe.Delete;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Recipe.Delete
{
    public class DeleteRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> acton = async () => { await useCase.Execute(recipe.Id); };

            await acton.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(recipeId: 1000); };

            var exception = await Should.ThrowAsync<NotFoundException>(action);
            exception.Message.ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        private static DeleteRecipeUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repositoryRead = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
            var repositoryWrite = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            return new DeleteRecipeUseCase(loggedUser, repositoryRead, repositoryWrite, unitOfWork);

        }
    }
}
