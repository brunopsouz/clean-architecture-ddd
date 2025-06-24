using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserJsonBuilder
    {
        public static RequestRegisterUserJson Build(int passwordLenght = 10)
        {
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)//no RuleFor abaixo, vou passar como (f,"user") para que eu possa usar o user.Name no email, por exemplo.
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))// essa função pode ser passado firstName, lastName, provider(sempre emails @gmail.com por exemplo) e um sufixo. 
                .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLenght));
        }
    }
}
