namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class OpenAIRequestException : RecipeBookException
    {
        public OpenAIRequestException(string message) : base(message)
        {
        }
    }
}
