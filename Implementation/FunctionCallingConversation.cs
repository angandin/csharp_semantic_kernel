using Azure.AI.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using csharp_semantic_kernel.FunctionCalling;

namespace csharp_semantic_kernel.Implementation
{
    internal class FunctionCallingConversation
    {
        IKernel _kernel;
        KernelSettings _kernelSettings;
        static string pluginsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugins");

        public FunctionCallingConversation(IKernel kernel, KernelSettings kernelSettings)
        {
            _kernel = kernel;
            _kernelSettings = kernelSettings;
        }

        async public Task StartConversation()
        {
            Uri openAIUri = new($"{_kernelSettings.Endpoint}");
            string openAIApiKey = $"{_kernelSettings.ApiKey}";
            string model = $"{_kernelSettings.DeploymentOrModelId}";

            // Instantiate OpenAIClient for Azure Open AI.
            OpenAIClient client = new(openAIUri, new AzureKeyCredential(openAIApiKey));

            ChatCompletionsOptions chatCompletionsOptions = new();
            ChatCompletions response;
            ChatChoice responseChoice;

            bool isFunctionCalled = false;
            string tempAssistantMessage = "";
            string executedFunctionName = "";

            FunctionDefinition createTicketFuntionDefinition = CreateTicketFunction.GetFunctionDefinition();
            chatCompletionsOptions.Functions.Add(createTicketFuntionDefinition);
            FunctionDefinition runVinScanFunctionDefinition = RunVinScanFunction.GetFunctionDefinition();
            chatCompletionsOptions.Functions.Add(runVinScanFunctionDefinition);
            FunctionDefinition runVinTGS3ScanFunctionDefinition = RunVinTGS3ScanFunction.GetFunctionDefinition();
            chatCompletionsOptions.Functions.Add(runVinTGS3ScanFunctionDefinition);

            chatCompletionsOptions.Messages.Add(new(ChatRole.System, SystemPromptTemplate.GetDefaultPrompt()));
            
            var startAssistantMessage = "Hello! I am an assistant that can help you on different task!";
            Console.WriteLine(startAssistantMessage);
            chatCompletionsOptions.Messages.Add(new(ChatRole.Assistant, startAssistantMessage));

            Console.Write("User: ");
            string input = Console.ReadLine();

            while (input != "exit()" || isFunctionCalled == false)
            {
                chatCompletionsOptions.Messages.Add(new(ChatRole.User, input));
                response = await client.GetChatCompletionsAsync(model, chatCompletionsOptions);
                responseChoice = response.Choices[0];

                if (responseChoice.FinishReason == CompletionsFinishReason.FunctionCall)
                {
                    isFunctionCalled = true;

                    switch (responseChoice.Message.FunctionCall.Name)
                    {
                        case "create_ticket":

                            var ticketResultData = CreateTicketFunction.CreateNewTicket(responseChoice.Message.FunctionCall.Arguments);
                            tempAssistantMessage = ticketResultData;
                            chatCompletionsOptions.Messages.Add(new(ChatRole.Assistant, tempAssistantMessage));
                            Console.WriteLine($"Bot: {tempAssistantMessage}");
                            executedFunctionName = CreateTicketFunction.Name;
                            break;

                        case "runvinscan":
                            var vinscanResultData = RunVinScanFunction.ExecuteVinScan(responseChoice.Message.FunctionCall.Arguments);
                            tempAssistantMessage = vinscanResultData;
                            chatCompletionsOptions.Messages.Add(new(ChatRole.Assistant, tempAssistantMessage));
                            Console.WriteLine($"Bot: {tempAssistantMessage}");
                            executedFunctionName = RunVinScanFunction.Name;
                            break;

                        case "runvintgs3scan":
                            var vintgs3scanResultData = RunVinTGS3ScanFunction.ExecuteVinTGS3Scan(responseChoice.Message.FunctionCall.Arguments);
                            tempAssistantMessage = vintgs3scanResultData;
                            chatCompletionsOptions.Messages.Add(new(ChatRole.Assistant, tempAssistantMessage));
                            Console.WriteLine($"Bot: {tempAssistantMessage}");
                            executedFunctionName = RunVinScanFunction.Name;
                            break;

                        default:
                            break;
                    }

                    var functionResponseMessage = new ChatMessage(
                        ChatRole.Function,
                        JsonSerializer.Serialize(
                            tempAssistantMessage,
                            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                    functionResponseMessage.Name = executedFunctionName;
                    chatCompletionsOptions.Messages.Add(functionResponseMessage);
                }
                else
                {
                    chatCompletionsOptions.Messages.Add(new(ChatRole.Assistant, responseChoice.Message.Content));
                    Console.WriteLine($"Bot: {responseChoice.Message.Content}");
                }

                Console.Write("User: ");
                input = Console.ReadLine();
            }
        }
    }
}
