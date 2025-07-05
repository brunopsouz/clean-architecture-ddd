using FluentValidation;
using FluentValidation.Validators;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.SharedValidators
{
    /// <summary>
    /// Passo o tipo T (Requisição "como responsabilidade") e a propriedade que quero validar, que é uma string (password).
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);

                return false;
            }

            if ( password.Length < 6)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.INVALID_PASSWORD);

                return false;
            }

            return true;

        }

        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

    }
}
