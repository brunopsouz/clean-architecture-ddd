using AutoMapper;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repository;

        public FilterRecipeUseCase(
            IMapper mapper,
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repository)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
            _repository = repository;
        }

        public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var filters = new FilterRecipesDto
            {
                RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
                CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
                Difficulties = request.Difficulties.Distinct().Select(d => (Domain.Enums.Difficulty)d).ToList(),
                DishTypes = request.DishTypes.Distinct().Select(d => (Domain.Enums.DishType)d).ToList()
            };

            var recipes = await _repository.Filter(loggedUser, filters);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
            };
        }

        private static void Validate(RequestFilterRecipeJson request)
        {
            var validator = new FilterRecipeValidator();

            var result = validator.Validate(request);

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
