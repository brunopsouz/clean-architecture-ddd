using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Infrastructure.Security.Criptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            return new Sha512Encripter("abc1234");
        }
    }
}
