using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldBe(request.Name);
            result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();

        }

        [Fact]
        public async Task Error_Email_Alreary_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            // Como é teste de UseCase, não devo executar a API diretamente, devo apenas testar a lógica do UseCase, crio uma condição dentro 
            // de CreateUseCase() que verifica se o e-mail já existe, para entrar no método "ExistActiveUserWithEmail()" e forçar o retorno de um email (true)
            // ou seja, o mesmo email passado na request, por isso aqui eu passo o e-mail da request como parametro de CreateUseCase(). 
            var useCase = CreateUseCase(request.Email);

            //Para "Execute" esperar true de uma Exception, devo armazenar ele em uma variável de função, se não o Debugtest falha.
            Func<Task> act = async () => await useCase.Execute(request);

            //Espera-se entrar na função "Execute()" e esperar um erro de e-mail já cadastrado. 
            (await Should.ThrowAsync<ErrorOnValidationException>(act))
                .ErrorMessages.Count.ShouldBe(1);

            //Espera-se entrar na função "Execute()" e esperar um erro de e-mail já cadastrado. 
            (await Should.ThrowAsync<ErrorOnValidationException>(act))
                .ErrorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

            //var exception = await Should.ThrowAsync<ErrorOnValidationException>(act);
            //exception.ErrorMessages.Count.ShouldBe(1);
            //exception.ErrorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

            //(await act.ShouldThrowAsync<ErrorOnValidationException>())
            

        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            //Para "Execute" esperar true de uma Exception, devo armazenar ele em uma variável de função, se não o Debugtest falha. 
            Func<Task> act = async () => await useCase.Execute(request);


            (await Should.ThrowAsync<ErrorOnValidationException>(act))
                .ErrorMessages.Count.ShouldBe(1);

            (await Should.ThrowAsync<ErrorOnValidationException>(act))
                .ErrorMessages.ShouldContain(ResourceMessagesException.NAME_EMPTY);

            //var exception = await Should.ThrowAsync<ErrorOnValidationException>(act);
            //exception.ErrorMessages.Count.ShouldBe(1);
            //exception.ErrorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

            //(await act.ShouldThrowAsync<ErrorOnValidationException>())

        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            // Parametros = null quer dizer que é opcional, ou seja, se não for passado nada, o teste não vai simular um e-mail já cadastrado.

            //1.Mapeia; 2.encripta senha; 3.escreve e-mail no banco; 4. Commit; 5. Verifica se existe e-mail.
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            //Se o e-mail não for nulo ou vazio vai entrar na condição, para simular um retorno de e-mail já cadastrado. 
            //uma vez que o teste de UseCase não deve chamar a API diretamente, mas sim testar a lógica do UseCase.
            if (string.IsNullOrEmpty(email) == false)
            {   
                //verifica se o e-mail existe
                readRepositoryBuilder.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(readRepositoryBuilder.Build(), userWriteOnlyRepository, unitOfWork, passwordEncripter, accessTokenGenerator, mapper);
        }

    }
}
