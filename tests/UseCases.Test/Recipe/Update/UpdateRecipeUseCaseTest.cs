using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.Recipe.Update;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Recipe.Update
{
    public class UpdateRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user, recipe);

            Func<Task> action = async () => await useCase.Execute(recipe.Id, request);

            // Sem retorno, pois o método é void
            await action.ShouldNotThrowAsync();

        }

        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => await useCase.Execute(recipeId: 1000, request);

            var exception = await Should.ThrowAsync<NotFoundException>(action);
            exception.Message.ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);

        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            (var user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty; 

            var useCase = CreateUseCase(user, recipe);

            Func<Task> action = async () => await useCase.Execute(recipe.Id, request);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }


        private static UpdateRecipeUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();

            return new UpdateRecipeUseCase(loggedUser, unitOfWork, mapper, repository);
        }
    }
}
