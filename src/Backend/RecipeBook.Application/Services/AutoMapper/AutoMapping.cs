using AutoMapper;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain;
using RecipeBook.Domain.Entities;
using DishType = RecipeBook.Communication.Enums.DishType;

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
            DomainToResponse();
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

            CreateMap<RequestRecipeJson, Recipe>()
                .ForMember(dest => dest.Instructions, opt => opt.Ignore())
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
                .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.Ingredients.Distinct()));

            CreateMap<string, Ingredient>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

            CreateMap<DishType, Domain.Entities.DishType>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

            CreateMap<RequestInstructionJson, Instruction>();

        }

        private void DomainToResponse()
        {
            CreateMap<User, ResponseUserProfileJson>();

        }

    }
}
