using Microsoft.Extensions.AI;

ChatOptions chatOptions = new()
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema(
        schema: schema,
        schemaName: "PersonInfo",
        schemaDescription: "Information about a person including their name, age, and occupation")
};