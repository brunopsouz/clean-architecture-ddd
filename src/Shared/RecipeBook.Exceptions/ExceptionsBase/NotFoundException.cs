using RecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class NotFoundException : RecipeBookException
{
    public NotFoundException(string message) : base(message)
    {
    }

    //public override IList<string> GetErrorMessages() => [Message];

    //public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}