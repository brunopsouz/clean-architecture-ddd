using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        public Task Execute(RequestUpdateUserJson request);
    }
}
