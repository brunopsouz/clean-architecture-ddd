using AutoMapper;

using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;
using RecipeBook.Exceptions;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Domain.Extensions;

namespace RecipeBook.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IBlobStorageService _blobStorageService;

        public GetRecipeByIdUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IRecipeReadOnlyRepository repository,
            IBlobStorageService blobStorageService)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _repository = repository;
            _blobStorageService = blobStorageService;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            var response = _mapper.Map<ResponseRecipeJson>(recipe);

            if (recipe.ImageIdentifier.NotEmpty())
            {
                var url = await _blobStorageService.GetFileUrl(loggedUser, recipe.ImageIdentifier);

                response.ImageUrl = url;
            }
            
            return response;
        }
    }
}
