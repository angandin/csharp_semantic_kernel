using csharp_semantic_kernel.FunctionCalling;
using csharp_semantic_kernel.plugins.CreateTicketPlugin;
using csharp_semantic_kernel.plugins.RunScan;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.AzureSdk;
using Microsoft.SemanticKernel.Functions.OpenAPI.Model;
using Microsoft.SemanticKernel.Functions.OpenAPI.OpenAI;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Plugins.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.Implementation
{
    internal class SKFunctionCalling
    {
        IKernel _kernel;
        static KernelSettings _kernelSettings;
        static string pluginsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugins");

        public SKFunctionCalling(IKernel kernel, KernelSettings kernelSettings)
        {
            _kernel = kernel;
            _kernelSettings = kernelSettings;
        }

        public async Task StartConversation()
        {
            IKernel kernel = await InitializeKernelAsync();
            var chatCompletion = kernel.GetService<IChatCompletion>();
            var chatHistory = chatCompletion.CreateNewChat();

            OpenAIRequestSettings requestSettings = new()
            {
                // Include all functions registered with the kernel.
                // Alternatively, you can provide your own list of OpenAIFunctions to include.
                Functions = kernel.Functions.GetFunctionViews().Select(f => f.ToOpenAIFunction()).ToList(),
                //FunctionCall = "TimePlugin_Date",
            };

            requestSettings.FunctionCall = OpenAIRequestSettings.FunctionCallAuto;
            await CompleteChatWithFunctionsAsync(chatHistory, chatCompletion, kernel, requestSettings);
        }

        private static async Task CompleteChatWithFunctionsAsync(ChatHistory chatHistory, IChatCompletion chatCompletion, IKernel kernel, OpenAIRequestSettings requestSettings)
        {
            chatHistory.AddSystemMessage(SystemPromptTemplate.GetSKPrompt());
            Console.WriteLine("Write your first question. Enter exit() to close the program");
            var ask = "";

            while (ask != "exit()")
            {
                ask = Console.ReadLine();
                Console.WriteLine($"User message: {ask}");
                chatHistory.AddUserMessage(ask);

                // Send request
                var chatResult = (await chatCompletion.GetChatCompletionsAsync(chatHistory, requestSettings))[0];

                // Check for message response
                var chatMessage = await chatResult.GetChatMessageAsync();
                if (!string.IsNullOrEmpty(chatMessage.Content))
                {
                    Console.WriteLine(chatMessage.Content);

                    // Add the response to chat history
                    chatHistory.AddAssistantMessage(chatMessage.Content);
                }

                // Check for function response
                OpenAIFunctionResponse? functionResponse = chatResult.GetOpenAIFunctionResponse();
                if (functionResponse is not null)
                {
                    // Print function response details
                    Console.WriteLine("Function name: " + functionResponse.FunctionName);
                    Console.WriteLine("Plugin name: " + functionResponse.PluginName);
                    Console.WriteLine("Arguments: ");
                    foreach (var parameter in functionResponse.Parameters)
                    {
                        Console.WriteLine($"- {parameter.Key}: {parameter.Value}");
                    }

                    // If the function returned by OpenAI is an SKFunction registered with the kernel,
                    // you can invoke it using the following code.
                    if (kernel.Functions.TryGetFunctionAndContext(functionResponse, out ISKFunction? func, out ContextVariables? context))
                    {
                        var kernelResult = await kernel.RunAsync(func, context);

                        var result = kernelResult.GetValue<object>();

                        string? resultMessage = null;
                        if (result is RestApiOperationResponse apiResponse)
                        {
                            resultMessage = apiResponse.Content?.ToString();
                        }
                        else if (result is string str)
                        {
                            resultMessage = str;
                        }

                        if (!string.IsNullOrEmpty(resultMessage))
                        {
                            Console.WriteLine(resultMessage);

                            // Add the function result to chat history
                            chatHistory.AddAssistantMessage(resultMessage);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: Function {functionResponse.PluginName}.{functionResponse.FunctionName} not found.");
                    }
                }
            }
        }

        private static async Task<IKernel> InitializeKernelAsync()
        {
            // Create kernel with chat completions service
            IKernel kernel = new KernelBuilder()
                //.WithLoggerFactory(ConsoleLogger.LoggerFactory)
                .WithAzureOpenAIChatCompletionService(_kernelSettings.DeploymentOrModelId, _kernelSettings.Endpoint, _kernelSettings.ApiKey, serviceId: "chat")
                .Build();

            // Load functions to kernel
            //kernel.ImportFunctions(new TimePlugin(), "TimePlugin");
            kernel.ImportFunctions(new RunVinScanPlugin(), "RunVinScanPlugin");
            kernel.ImportFunctions(new CreateTicketPlugin(), "CreateTicketPlugin");

            return kernel;
        }
    }
}
