using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase
{
    public abstract class RecipeBookException : SystemException
    {
        //Obrigo todos que tem herança com RecipeBookException me passar uma mensagem, posteriormente passo a mensagem para o "base" que chama o construtor da classe SystemException.
        public RecipeBookException(string message) : base(message) { }

        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();

    }
}
