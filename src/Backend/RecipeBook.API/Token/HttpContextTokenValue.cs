using RecipeBook.Domain.Security.Tokens;

namespace RecipeBook.API.Token
{
    /// <summary>
    /// Classe criada para obter o token do contexto HTTP e passar para ITokenProvider (em Domain)
    /// uma vez que Infraestrutura não pode depender/enxergar de API. 
    /// Sendo assim ITokenProvider(Domain) passa um método "Value()" retornando um token para ser utilizado na classe LoggedUser(Infrastructure).
    /// </summary>
    public class HttpContextTokenValue : ITokenProvider
    {
        // constructor recebe IHttpContextAccessor para acessar o contexto HTTP
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Value()
        {
            var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
