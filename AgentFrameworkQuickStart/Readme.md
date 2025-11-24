dotnet new console -o AgentFrameworkQuickStart
cd AgentFrameworkQuickStart

dotnet add package Azure.AI.OpenAI --prerelease
dotnet add package Azure.Identity
dotnet add package Microsoft.Agents.AI --prerelease
dotnet add package Microsoft.Agents.AI.OpenAI --prerelease

Certifique-se de substituir https://your-resource.openai.azure.com/ pelo ponto de extremidade do seu recurso do Azure OpenAI.

dotnet nuget add source --username GITHUBUSERNAME --password GITHUBPERSONALACCESSTOKEN --store-password-in-clear-text --name GitHubMicrosoft "https://nuget.pkg.github.com/microsoft/index.json"
