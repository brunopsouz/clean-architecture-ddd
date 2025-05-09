using AutoMapper;
using RecipeBook.Application.Services.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Retorna um novo mapeamento criado.
            return new MapperConfiguration(options =>
            {
                //função que chama a minha classe de AutoMapper em Application.
                options.AddProfile(new AutoMapping());
            }).CreateMapper();
        }
    }
}
