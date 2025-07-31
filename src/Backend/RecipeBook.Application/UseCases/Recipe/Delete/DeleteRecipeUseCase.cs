
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repositoryRead;
        private readonly IRecipeWriteOnlyRepository _repositoryWrite;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;

        public DeleteRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repositoryRead,
            IRecipeWriteOnlyRepository repositoryWrite,
            IUnitOfWork unitOfWork,
            IBlobStorageService blobStorageService)
        {
            _loggedUser = loggedUser;
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _unitOfWork = unitOfWork;
            _blobStorageService = blobStorageService;
        }

        public async Task Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repositoryRead.GetById(loggedUser, recipeId);

            if(recipe is null)
            {
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
            }

            if (recipe.ImageIdentifier.NotEmpty())
            {
                await _blobStorageService.Delete(loggedUser, recipe.ImageIdentifier);
            }

            await _repositoryWrite.Delete(recipeId);

            await _unitOfWork.Commit();

        }
    }
}
