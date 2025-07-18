﻿using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;

namespace RecipeBook.Application.UseCases.Dashboard
{
    public class GetDashboardUseCase : IGetDashboardUseCase
    {
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetDashboardUseCase(
            IRecipeReadOnlyRepository repository,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var loggedUser = await _loggedUser.User();

            var recipes = await _repository.GetForDashboard(loggedUser);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
            };

        }
    }
}
