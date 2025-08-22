using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class RefreshTokenNotFoundException : RecipeBookException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.EXPIRED_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}