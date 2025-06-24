namespace RecipeBook.Communication.Responses
{
    /// <summary>
    /// Classe para resposta de status da chamada.
    /// </summary>
    public class ResponseRegisteredUserJson
    {
        public string Name { get; set; } = string.Empty;
        public ResponseTokensJson Tokens { get; set; } = default!;
    }
}
