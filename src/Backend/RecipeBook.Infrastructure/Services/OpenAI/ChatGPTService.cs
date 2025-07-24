using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Services.OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace RecipeBook.Infrastructure.Services.OpenAI;
public class ChatGptService : IGenerateRecipeAI
{
    private readonly ChatClient _chatClient;

    public ChatGptService(ChatClient chatClient) => _chatClient = chatClient;

    public async Task<GenerateRecipeDto> Generate(IList<string> ingredients)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE),
            //pega o texto e transforma em uma string onde cada elemento é separado por ";".
            new UserChatMessage(string.Join(";", ingredients))
        };

        var completion = await _chatClient.CompleteChatAsync(messages);

        if (completion.Value.Content.Count == 0)
            throw new Exception("Resposta da OpenAI veio vazia.");

        var responseList = completion.Value.Content[0].Text
            .Split("\n")
            //.Where(response => response.Trim().Equals(string.Empty).IsFalse())
            .Where(item => string.IsNullOrWhiteSpace(item).IsFalse())
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GenerateRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()
        };
    }
}