using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(login => login.Email, (f) => f.Internet.Email())
                .RuleFor(login => login.Password, (f) => f.Internet.Password());
        }
    }
}
