﻿using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Services.OpenAI;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Generate
{
    public class GenerateRecipeUseCase : IGenerateRecipeUseCase
    {
        private readonly IGenerateRecipeAI _generator;

        public GenerateRecipeUseCase(IGenerateRecipeAI generator)
        {
            _generator = generator;
        }

        public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
        {
            Validate(request);

            var response = await _generator.Generate(request.Ingredients);

            return new ResponseGeneratedRecipeJson
            {
                Title = response.Title,
                Ingredients = response.Ingredients,
                CookingTime = (Communication.Enums.CookingTime)response.CookingTime,
                Instructions = response.Instructions.Select(c => new ResponseGeneratedInstructionJson
                {
                    Step = c.Step,
                    Text = c.Text,
                }).ToList(),
                Difficulty = Communication.Enums.Difficulty.Low
            };
        }

        private static void Validate(RequestGenerateRecipeJson request)
        {
            var result = new GenerateRecipeValidator().Validate(request);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
