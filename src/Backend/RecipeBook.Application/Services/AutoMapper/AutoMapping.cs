using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        /// <summary>
        /// AutoMapper eu irei mapear SEMPRE a classe de REQUEST com a classe da ENTIDADE. 
        /// </summary>
        public AutoMapping()
        {
            RequestToDomain();
        }

        /// <summary>        
        /// Por boas praticas devo usar a classe de Request para passar os dados.
        /// Se as propriedades forem iguais eu não necessito mexer.
        /// Posso ignorar o map de Password para fazer a validação de encriptação.
        /// </summary>
        private void RequestToDomain()
        {
            // Classe de Request , Entidade
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        private void DomainToResponse()
        {

        }

    }
}
