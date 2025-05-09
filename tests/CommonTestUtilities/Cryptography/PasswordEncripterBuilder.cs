using RecipeBook.Application.Services.Criptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static PasswordEncripter Build()
        {
            return new PasswordEncripter("abc1234");
        }
    }
}
