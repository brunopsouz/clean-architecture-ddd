using AutoMapper;
using CommonTestUtilities.idEncryption;
using RecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper
{
    /// <summary>
    /// Criando uma classe Mapper para testes utilitarios, 
    /// acessando o AutoMapper de "Application" através da função AddProfile() passando um new AutoMapping()
    /// </summary>
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            var idEncripter = IdEncripterBuilder.Build();

            //Retorna um novo mapeamento criado.
            var mapper = new MapperConfiguration(options =>
            {
                //função que chama a minha classe de AutoMapper em Application.
                options.AddProfile(new AutoMapping(idEncripter));
            }).CreateMapper();

            return mapper;
        }
    }
}
