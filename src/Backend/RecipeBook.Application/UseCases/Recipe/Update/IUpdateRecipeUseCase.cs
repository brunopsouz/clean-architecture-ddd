using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public interface IUpdateRecipeUseCase
{
    Task Execute(long recipeId, RequestRecipeJson request);
}