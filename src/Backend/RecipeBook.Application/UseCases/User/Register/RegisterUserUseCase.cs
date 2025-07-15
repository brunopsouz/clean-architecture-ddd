using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register
{
    /// <summary>
    /// internal/public 
    /// </summary>
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserReadOnlyRepository _iuserReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _iuserWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IPasswordEncripter _passwordEncripter;

        /// <summary>
        ///     Construtor da Classe, onde serão instanciados todas as classes que ajudam no código.
        /// </summary>
        /// <param name="iuserReadOnlyRepository">Interface fica em INFRASTRUCTURE, responsavel por fazer a transação com o banco para GET/LER da chamada</param>
        /// <param name="iuserWriteOnlyRepository">Interface fica em INFRASTRUCTURE, responsavel por fazer a transação com o banco para PUT/ADD da chamada</param>
        /// <param name="passwordEncripter">Classe responsavel por encriptar o Password. Essa classe SEMPRE FICA em APPLICATION.</param>
        /// <param name="mapper">Mapper responsavel por mapear as classes.</param>
        public RegisterUserUseCase(
            IUserReadOnlyRepository iuserReadOnlyRepository, 
            IUserWriteOnlyRepository iuserWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator,
            IMapper mapper)
        {
            _iuserReadOnlyRepository = iuserReadOnlyRepository;
            _iuserWriteOnlyRepository = iuserWriteOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        /// <summary>
        /// Metodo POST
        /// Aqui ficam todas as validações do metodo da chamada que vem da Controller.
        /// Dentro do método estou documentando uma ordem de passos/tarefas a serem feitas.
        /// </summary>
        /// <param name="request"></param>
        /// <returns> ResponseRegisteredUserJson retornando NAME </returns>
        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            // 1. Validar a Request
            await Validate(request);

            // 2. Mapear a Request em uma entidade.
            var user = _mapper.Map<Domain.Entities.User>(request);

            // 3. Criptografar a senha
            user.Password = _passwordEncripter.Encrypt(request.Password);

            user.UserIdentifier = Guid.NewGuid();

            // 4. Salvar no banco de dados
            await _iuserWriteOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }

        /// <summary>
        /// Metodo para validar as propriedades de User
        /// Se Nome não esta vazio, se Email nao está vazio ou é valido, se Password nao é vazio e tem quantidade correta.
        /// Obs: Crio uma nova classe de RegisterUserValidator usando "FluentValidation". 
        /// Então eu instancio com new essa classe para usar as regras configuradas em RegisterUserValidator.
        /// </summary>
        /// <param name="request">Parametro de Request (Json) vindo da chamada.</param>
        /// <exception cref="ErrorOnValidationException"></exception>
        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = await validator.ValidateAsync(request);

            var emailExist = await _iuserReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

            if (result.IsValid.IsFalse()) 
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }

        }


    }
}
