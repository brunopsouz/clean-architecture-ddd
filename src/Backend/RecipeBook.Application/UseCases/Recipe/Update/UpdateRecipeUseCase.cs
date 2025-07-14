using AutoMapper;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRecipeUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRecipeUpdateOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Execute(long recipeId, RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();

        _mapper.Map(request, recipe);

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Instruction>>(instructions);

        _repository.Update(recipe);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}