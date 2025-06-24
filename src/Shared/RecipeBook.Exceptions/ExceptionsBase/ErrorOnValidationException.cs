using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : RecipeBookException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
        {

            ErrorMessages = errorMessages;
        }
    }
}
