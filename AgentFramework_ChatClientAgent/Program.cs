using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;

// Essa ChatOptions instância permite que você escolha uma ChatResponseFormat preferida.
// Neste caso, estamos escolhendo um formato de resposta JSON Schema.
AIAgent agent = new AzureOpenAIClient(
    new Uri("https://<myresource>.openai.azure.com"),
    new AzureCliCredential())
        .GetChatClient("gpt-4o-mini")
        .CreateAIAgent(new ChatClientAgentOptions()
        {
            Name = "HelpfulAssistant",
            Instructions = "You are a helpful assistant.",
            ChatOptions = chatOptions
        });

// Use o agente para processar uma solicitação e obter uma resposta estruturada.
var response = await agent.RunAsync("Please provide information about John Smith, who is a 35-year-old software engineer.");

// Desserialize a resposta em um objeto PersonInfo.
var personInfo = response.Deserialize<PersonInfo>(JsonSerializerOptions.Web);
Console.WriteLine($"Name: {personInfo.Name}, Age: {personInfo.Age}, Occupation: {personInfo.Occupation}");

// Use o agente para processar uma solicitação e obter uma resposta estruturada em streaming.
var updates = agent.RunStreamingAsync("Please provide information about John Smith, who is a 35-year-old software engineer.");

// Desserialize a resposta em um objeto PersonInfo.
personInfo = (await updates.ToAgentRunResponseAsync()).Deserialize<PersonInfo>(JsonSerializerOptions.Web);