using RecipeBook.Domain.Extensions;
using System.Globalization;

namespace RecipeBook.API.Middleware
{
    /// <summary>
    /// Classe para configurar a cultura/linguagem da aplicação
    /// </summary>
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Obtenho em uma variavel as linguagens suportadas. 
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

            //Recupero a linguagem na chamada.
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            //Cria um default em English
            var cultureInfo = new CultureInfo("en");

            ////Verifica se não é nulo ou tem espaço em branco & compara se a cultura existe na lista de culturas suportadas que obtive em supportedLanguages.
            //if (string.IsNullOrWhiteSpace(requestedCulture).IsFalse()
            //    && supportedLanguages.Exists(c => c.Name.Equals(requestedCulture)))
            //{
            //    cultureInfo = new CultureInfo(requestedCulture!);
            //}
            if (requestedCulture.NotEmpty()
                && supportedLanguages.Exists(c => c.Name.Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);


        }

    }
}
