using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase(
            IUserReadOnlyRepository repository,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson requestLoginJson)
        {
            var encryptedPassword = _passwordEncripter.Encrypt(requestLoginJson.Password);

            var user = await _repository.GetByEmailAndPassword(requestLoginJson.Email, encryptedPassword) ?? throw new InvalidLoginException();
            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }
    }
}
