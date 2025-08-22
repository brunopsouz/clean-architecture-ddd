using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class RefreshTokenExpiredException : RecipeBookException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}