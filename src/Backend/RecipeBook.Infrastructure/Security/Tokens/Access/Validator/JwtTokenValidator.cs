using Microsoft.IdentityModel.Tokens;
using RecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook.Infrastructure.Security.Tokens.Access.Validator
{
    public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
    {
        public readonly string _signingKey;

        public JwtTokenValidator(string signingKey)
        {
            _signingKey = signingKey;
        }

        // Validates the JWT token and extracts the user identifier (SID) from it.
        // Crio meu próprio método de validação de token JWT, que recebe o token como parâmetro e retorna o identificador do usuário (SID) contido no token.
        public Guid ValidateAndGetUserIdentifier(string token)
        {
            var validationParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = SecurityKey(_signingKey), // Use the same signing key used to generate the token
                ClockSkew = TimeSpan.Zero, // Remove the default 5 minutes clock skew
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Valida o token e extrai o principal (claims) do token.
            var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

            var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            // Altera o valor de userIdentifier de string para Guid e o retorna.
            return Guid.Parse(userIdentifier);
        }
    }
}
