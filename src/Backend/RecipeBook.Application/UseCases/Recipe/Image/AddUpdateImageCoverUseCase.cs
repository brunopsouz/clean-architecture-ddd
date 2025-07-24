using Microsoft.AspNetCore.Http;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Image
{
    public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeUpdateOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddUpdateImageCoverUseCase(
            ILoggedUser loggedUser,
            IRecipeUpdateOnlyRepository repository,
            IUnitOfWork commit)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _unitOfWork = commit;
        }

        public async Task Execute(long recipeId, IFormFile file)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
            {
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
            }

        }

    }
}
