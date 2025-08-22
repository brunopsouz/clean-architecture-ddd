using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UseCases.Recipe.GetById;
using RecipeBook.Domain.Entities;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Recipe.GetById
{
    public class GetRecipeByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            var response = await useCase.Execute(recipe.Id);

            response.ShouldNotBeNull();
            response.Id.ShouldNotBeNullOrWhiteSpace();
            response.Title.ShouldBe(recipe.Title);

        }

        [Fact]
        public async Task Error_Recipe_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(recipeId: 1000); };

            var exception = await Should.ThrowAsync<NotFoundException>(action);
            exception.Message.ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        private static GetRecipeByIdUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
            var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

            return new GetRecipeByIdUseCase(loggedUser, mapper, repository, blobStorage);
        }

    }
}
