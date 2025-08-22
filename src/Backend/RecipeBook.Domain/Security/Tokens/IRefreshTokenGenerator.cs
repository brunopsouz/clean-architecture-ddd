namespace RecipeBook.Domain.Security.Tokens
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();
    }
}
