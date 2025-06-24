using Bogus;
using CommonTestUtilities.Cryptography;
using RecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build()
        {
            // Crio uma variavel para encriptar a senha e consequentemente passo ela para o Faker do User, e como parametro uma senha gerada pelo Faker aleatoriamente.
            var passwordEncripter = PasswordEncripterBuilder.Build();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(user => user.Id, () => 1)
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(user => user.Password, (f) => passwordEncripter.Encrypt(password));

            return (user, password);
        }
    }
}
