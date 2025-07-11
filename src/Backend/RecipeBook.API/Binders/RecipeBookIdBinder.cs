using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace RecipeBook.API.Binders
{
    public class RecipeBookIdBinder : IModelBinder
    {
        private readonly SqidsEncoder<long> _idEncoder;

        public RecipeBookIdBinder(SqidsEncoder<long> idEncoder)
        {
            _idEncoder = idEncoder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // nome da rota que está sendo chamada
            var modelName = bindingContext.ModelName;

            // valor da propriedade que esta criptografado
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            // verifica se o valor é valido.
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            // vai setar o valor neste model.
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            // pega o valor como string
            var value = valueProviderResult.FirstValue;

            // verifica se o valor é nulo ou vazio
            if (string.IsNullOrWhiteSpace(value))
                return Task.CompletedTask;

            // faz o decode do valor para pegar o id
            var id = _idEncoder.Decode(value).Single();

            // bind deu sucesso.
            bindingContext.Result = ModelBindingResult.Success(id);

            return Task.CompletedTask;

        }
    }
}
