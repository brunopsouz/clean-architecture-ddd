using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Recipe.Filter
{
    public class FilterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull().ShouldNotBeEmpty();
            result.Recipes.Count.ShouldBe(recipes.Count);
        }

        [Fact]
        public async Task Error_CookingTime_Invalid()
        {
            (var user, _) = UserBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((RecipeBook.Communication.Enums.CookingTime)1000);

            var useCase = CreateUseCase(user, recipes);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        private static FilterRecipeUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            IList<RecipeBook.Domain.Entities.Recipe> recipes)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();

            return new FilterRecipeUseCase(mapper, loggedUser, repository);
        }
    }
}
