using csharp_semantic_kernel.Implementation;
using Microsoft.SemanticKernel;

namespace csharp_semantic_kernel
{
    public class Program
    {
        static KernelSettings? _kernelSettings;

        static async Task Main(string[] args)
        {
            _kernelSettings = KernelSettings.LoadSettings();
            IKernel kernel = new KernelBuilder()
                .WithAzureOpenAIChatCompletionService("","","", serviceId:"chat")
                //.WithLogger(logger)
                .Build();

            Console.WriteLine(@"Choose:
            1. Query DB - Natural Language
            2. FunctionCall conversation
            3. SKFunctionCalling");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    var queryDb = new QueryDB(kernel);
                    await queryDb.NL2SQL();
                    break;
                case "2":
                    var functionCallingConversation = new FunctionCallingConversation(kernel, _kernelSettings);
                    await functionCallingConversation.StartConversation();
                    break;
                case "3":
                    var sKFunctionCalling = new SKFunctionCalling(kernel, _kernelSettings);
                    await sKFunctionCalling.StartConversation();
                    break;
                default:
                    break;
            }
        }
    }
}