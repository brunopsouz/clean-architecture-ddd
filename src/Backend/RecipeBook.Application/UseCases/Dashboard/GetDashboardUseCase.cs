using AutoMapper;
using RecipeBook.Application.Extensions;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Application.UseCases.Dashboard
{
    public class GetDashboardUseCase : IGetDashboardUseCase
    {
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IBlobStorageService _blobStorage;

        public GetDashboardUseCase(
            IRecipeReadOnlyRepository repository,
            IMapper mapper,
            ILoggedUser loggedUser,
            IBlobStorageService blobStorageService)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
            _blobStorage = blobStorageService;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var loggedUser = await _loggedUser.User();

            var recipes = await _repository.GetForDashboard(loggedUser);

            return new ResponseRecipesJson
            {
                Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorage, _mapper)
            };

        }
    }
}
