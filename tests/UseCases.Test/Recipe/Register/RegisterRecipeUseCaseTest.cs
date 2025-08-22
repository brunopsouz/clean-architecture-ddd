using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Domain.Entities;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.User.Profile;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            
            var request = RequestRegisterRecipeFormDataBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBeNullOrWhiteSpace();
            result.Title.ShouldBe(request.Title);

        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => { await useCase.Execute(request); };

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(action);
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }

        private static RegisterRecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            // Aqui você deve criar uma instância do RegisterRecipeUseCase com as dependências necessárias.
            // Por exemplo, você pode usar mocks ou stubs para IRecipeWriteOnlyRepository, ILoggedUser, IUnitOfWork e IMapper.

            // IRecipeWriteOnlyRepository writeRepository = ...; // Mock ou instância real
            // ILoggedUser loggedUser = ...; // Mock ou instância real
            // IUnitOfWork unitOfWork = ...; // Mock ou instância real
            // IMapper mapper = ...; // Mock ou instância real

            var writeRepository = RecipeWriteOnlyRepositoryBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var blobStorage = new BlobStorageServiceBuilder().Build();

            return new RegisterRecipeUseCase(writeRepository, loggedUser, unitOfWork, mapper, blobStorage);
        }

    }
}
