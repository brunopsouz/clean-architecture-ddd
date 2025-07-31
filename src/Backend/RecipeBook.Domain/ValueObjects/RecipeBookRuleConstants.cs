namespace RecipeBook.Domain.ValueObjects
{
    public abstract class RecipeBookRuleConstants
    {
        public const int MAXIMUM_INGREDIENTS_GENERATE_RECIPE = 5;

        public const string CHAT_MODEL = "gpt-4o";

        public const int MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES = 10;
    }
}
