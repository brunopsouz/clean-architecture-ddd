using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RecipeBook.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandler
    {

        /// <summary>
        /// Função criada para converter a string _signingKey em uma SecurityKey
        /// </summary>
        /// <returns>SymmetricSecurityKey</returns>
        protected SymmetricSecurityKey SecurityKey(string signingKey)
        {
            // Converte a string de assinatura em um array de bytes usando UTF8.
            var bytes = Encoding.UTF8.GetBytes(signingKey);
            // SymetricSecurityKey recebe um array de bytes e cria uma chave de segurança simétrica.
            return new SymmetricSecurityKey(bytes);
        }
    }
}
