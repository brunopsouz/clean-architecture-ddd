using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _writeRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterRecipeUseCase(
            IRecipeWriteOnlyRepository writeRepository,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _writeRepository = writeRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.Id;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for(var index = 0; index < instructions.Count; index++)
                instructions.ElementAt(index).Step = index + 1;

            recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);

            await _writeRepository.Add(recipe);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);

        }

        private async Task Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if(result.IsValid.IsFalse())
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
            
        }
    }
}
