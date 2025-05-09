using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.User.Register
{
    /// <summary>
    /// Interface de RegisterUserUseCase da principal classe de caso de uso de registro do User.
    /// 
    /// Aqui serão implementados os métodos que serao adicionados na classe principal RegisterUserUseCase.
    /// 
    /// Não necessário usar Async.
    /// </summary>
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
