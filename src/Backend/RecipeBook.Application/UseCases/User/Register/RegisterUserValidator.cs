using FluentValidation;
using RecipeBook.Application.SharedValidators;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
            When(user => user.Email.NotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
