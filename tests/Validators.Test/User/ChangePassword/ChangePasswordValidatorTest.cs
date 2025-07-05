﻿using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new ChangePasswordValidator();
            var request = RequestChangePasswordJsonBuilder.Build();
            var result = validator.Validate(request);
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_NewPassword_Invalid(int passwordLenght)
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build(passwordLenght);
            
            var result = validator.Validate(request);
            
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_PASSWORD);
        }

        [Fact]
        public void Error_NewPassword_Empty()
        {
            var validator = new ChangePasswordValidator();
            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;
            var result = validator.Validate(request);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.PASSWORD_EMPTY);
        }
        
    }
}
