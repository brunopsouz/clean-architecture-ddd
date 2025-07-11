using AutoMapper;

using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;
using RecipeBook.Exceptions;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IRecipeReadOnlyRepository _repository;

        public GetRecipeByIdUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IRecipeReadOnlyRepository repository)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            return _mapper.Map<ResponseRecipeJson>(recipe);
            
            
        }
    }
}
