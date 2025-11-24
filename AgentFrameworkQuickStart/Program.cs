using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

AIAgent agent = new AzureOpenAIClient(
  new Uri("https://your-resource.openai.azure.com/"),
  new AzureCliCredential())
    .GetChatClient("gpt-4o-mini")
    .CreateAIAgent(instructions: "You are good at telling jokes.");



// Non-streaming example
Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate."));


// Streaming example
await foreach (var update in agent.RunStreamingAsync("Tell me a joke about a pirate."))
{
    Console.WriteLine(update);
}


// Example with image input
ChatMessage message = new(ChatRole.User, [
    new TextContent("Tell me a joke about this image?"),
    new UriContent("https://upload.wikimedia.org/wikipedia/commons/1/11/Joseph_Grimaldi.jpg", "image/jpeg")
]);

Console.WriteLine(await agent.RunAsync(message));


// Example with system and user messages
ChatMessage systemMessage = new(
    ChatRole.System,
    """
    If the user asks you to tell a joke, refuse to do so, explaining that you are not a clown.
    Offer the user an interesting fact instead.
    """);
ChatMessage userMessage = new(ChatRole.User, "Tell me a joke about a pirate.");

Console.WriteLine(await agent.RunAsync([systemMessage, userMessage]));



/*
Passando imagens para o agente:
*/

// Example with multiple agents
AIAgent agents = new AzureOpenAIClient(
    new Uri("https://<myresource>.openai.azure.com"),
    new AzureCliCredential())
    .GetChatClient("gpt-4o")
    .CreateAIAgent(
        name: "VisionAgent",
        instructions: "You are a helpful agent that can analyze images");

// Example with image analysis
ChatMessage messages = new(ChatRole.User, [
    new TextContent("What do you see in this image?"),
    new UriContent("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg", "image/jpeg")
]);

Console.WriteLine(await agents.RunAsync(messages));




/*Executando o agente com uma conversa de vários turnos:

*** Os agentes são sem estado e não mantêm nenhum estado internamente entre chamadas. 
Para ter uma conversa de vários turnos com um agente, você precisa criar um objeto 
para manter o estado da conversa e passar esse objeto para o agente ao executá-lo. ***
*/

// Método para criar o objeto AgentThread
AgentThread thread = agent.GetNewThread();

Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate.", thread));
Console.WriteLine(await agent.RunAsync("Now add some emojis to the joke and tell it in the voice of a pirate's parrot.", thread));

/*Um único agente com múltiplas conversas*/

AgentThread thread1 = agent.GetNewThread();
AgentThread thread2 = agent.GetNewThread();
Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate.", thread1));
Console.WriteLine(await agent.RunAsync("Tell me a joke about a robot.", thread2));
Console.WriteLine(await agent.RunAsync("Now add some emojis to the joke and tell it in the voice of a pirate's parrot.", thread1));
Console.WriteLine(await agent.RunAsync("Now add some emojis to the joke and tell it in the voice of a robot.", thread2));


