﻿namespace RecipeBook.Communication.Responses
{
    public class ResponseTokensJson
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        //public DateTime Expiration { get; set; }
        //public DateTime RefreshExpiration { get; set; }
    }
}
