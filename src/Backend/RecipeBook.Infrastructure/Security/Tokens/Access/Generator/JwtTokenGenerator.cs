using Microsoft.IdentityModel.Tokens;
using RecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeBook.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : JwtTokenHandler, IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _signingKey;

        public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signingKey = signingKey;
        }

        public string Generate(Guid userIdentifier)
        {
            // Gerando uma lista de Claims, que são informações sobre o usuário que está autenticado.
            // Claims são pares chave-valor que descrevem o usuário e suas permissões. Aqui, estou adicionando apenas o identificador do usuário.
            // ClaimTypes.Sid é o identificador de segurança do usuário, geralmente é um ID/Guid gerado para o usuário.
            // "SEGURANÇA" Crio um Guid para identificar o usuário, evitando passar alguma informação sensível do usuário como algum tipo de documento, telefone, etc.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, userIdentifier.ToString())
            };

            //Para gerar um token preciso descrever esse token, então passo as informações que quero que o token tenha.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Identificador do usuário que está autenticado. (ClaimTypes.Sid é o identificador de segurança do usuário, geralmente é um ID/Guid gerado para o usuário).
                Subject = new ClaimsIdentity(claims),
                
                //Tempo que o token vai expirar. (Por exemplo, data de agora "UTC.NOW" + 30 minutos).
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),

                //Chave de assinatura do token. É a chave que vai assinar o token, garantindo que ele é válido. (SecurityAlgorithms.HmacSha256Signature algoritmo mais comum de assinatura).
                SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)

            };

            //Criando o token JWT usando o JwtSecurityTokenHandler.
            var tokenHandler = new JwtSecurityTokenHandler();
            //A variável tokenDescriptor é passada para o método CreateToken do tokenHandler, que cria o token JWT.
            //Mas como o retorno do método CreateToken é um SecurityToken, preciso converter para string usando o método WriteToken.
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            // O método WriteToken do tokenHandler converte o SecurityToken em uma string JWT.
            return tokenHandler.WriteToken(securityToken);

        }

    }
}
