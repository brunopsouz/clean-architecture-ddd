﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Metodo POST. 
        /// 
        /// Uso uma Interface de Caso de Uso ((UseCases) em Application) onde ficam todas as validações e condições dos métodos.
        ///
        /// Aqui preciso configurar o [HttpPost] para dizer que é um Insert. 
        /// 
        /// O ProducesResponseType passo uma Class que criei para Response (ResponseRegisteredUserJson) com a propriedade string Name. Também configuro por exemplo o StatusCode 201.
        /// 
        /// </summary>
        /// <param name="useCase">Parametro da INTERFACE que fica SEMPRE na camada APPLICATION. Uso uma INTERFACE de CASO DE USO/USE CASE onde ficam todas as validações e condições dos métodos</param>                     
        /// <param name="request">Classe criada para REQUEST/REQUISITAR, onde tenho as propriedades da Entidade, MAPEADAS VIA AutoMapper, que chegarão na requisição.
        /// Essa classe crio SEMPRE na camada COMMUNICATION.</param>
        ///                       
        /// <returns>Retorna a criação de um novo usuário.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            IRegisterUserUseCase useCase,
            RequestRegisterUserJson request)
        {
            // Vai executar o metodo da Interface RegisterUserUseCase passando a Request como parametro.
            // Onde em RegisterUserUseCase só recebe Request como parametro no método Execute.
            var result = await useCase.Execute(request); 

            return Created(string.Empty, result);
        }
    }
}
