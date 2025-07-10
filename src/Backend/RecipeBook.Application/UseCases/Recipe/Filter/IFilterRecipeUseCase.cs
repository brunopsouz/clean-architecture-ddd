using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;
public interface IFilterRecipeUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request);
}