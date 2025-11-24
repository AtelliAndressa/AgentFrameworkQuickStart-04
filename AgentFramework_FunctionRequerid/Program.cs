
AIFunction weatherFunction = AIFunctionFactory.Create(GetWeather);
// Wrap the function with approval requirement
AIFunction approvalRequiredWeatherFunction = new ApprovalRequiredAIFunction(weatherFunction);

// Create the agent with the approval-required function
AIAgent agent = new AzureOpenAIClient(
    new Uri("https://<myresource>.openai.azure.com"),
    new AzureCliCredential())
     .GetChatClient("gpt-4o-mini")
     .CreateAIAgent(instructions: "You are a helpful assistant", tools: [approvalRequiredWeatherFunction]);

// Start a new thread for the conversation
AgentThread thread = agent.GetNewThread();

// Run the agent to get the weather, which will require approval
AgentRunResponse response = await agent.RunAsync("What is the weather like in Amsterdam?", thread);

// Check for function approval requests in the response
var functionApprovalRequests = response.Messages
    .SelectMany(x => x.Contents)
    .OfType<FunctionApprovalRequestContent>()
    .ToList();

// If there are approval requests, approve the first one and continue the conversation
FunctionApprovalRequestContent requestContent = functionApprovalRequests.First();
Console.WriteLine($"We require approval to execute '{requestContent.FunctionCall.Name}'");

// Approve the function call and continue the conversation
var approvalMessage = new ChatMessage(ChatRole.User, [requestContent.CreateResponse(true)]);
Console.WriteLine(await agent.RunAsync(approvalMessage, thread));