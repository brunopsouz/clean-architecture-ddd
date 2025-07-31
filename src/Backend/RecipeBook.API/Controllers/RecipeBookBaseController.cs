using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Domain.Extensions;
using System.Security.Cryptography;

namespace RecipeBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeBookBaseController : ControllerBase
    {
        protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
        {
            return authenticate.Succeeded.IsFalse()
                || authenticate.Principal is null
                || authenticate.Principal.Identities.Any(id => id.IsAuthenticated).IsFalse();
        }
    }
}
