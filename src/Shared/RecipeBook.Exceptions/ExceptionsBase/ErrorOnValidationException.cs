using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : RecipeBookException
    {
        private readonly IList<string> _errorMessages;

        public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
        {

            _errorMessages = errorMessages;
        }

        public override IList<string> GetErrorMessages() => _errorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
