using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UseCases.Dashboard;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Dashboard
{
    public class GetDashboardUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execute();

            result.Recipes.ShouldNotBeNull();
            result.Recipes.Count.ShouldBeGreaterThan(0);
            result.Recipes.Select(r => r.Id).ShouldBeUnique();

            foreach (var recipe in result.Recipes)
            {
                recipe.ShouldSatisfyAllConditions(
                    () => recipe.Id.ShouldNotBeNullOrWhiteSpace(),
                    () => recipe.Title.ShouldNotBeNullOrWhiteSpace(),
                    () => recipe.AmountIngredients.ShouldBeGreaterThan(0)
                );
            }


        }

        private static GetDashboardUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            IList<RecipeBook.Domain.Entities.Recipe> recipes)
        {
            var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);


            return new GetDashboardUseCase(repository, mapper, loggedUser);
        }

    }
}
