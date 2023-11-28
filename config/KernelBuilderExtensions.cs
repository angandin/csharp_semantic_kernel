//using Microsoft.SemanticKernel;

//internal static class KernelBuilderExtensions
//{
//    /// <summary>
//    /// Adds a text completion service to the list. It can be either an OpenAI or Azure OpenAI backend service.
//    /// </summary>
//    /// <param name="kernelBuilder"></param>
//    /// <param name="kernelSettings"></param>
//    /// <exception cref="ArgumentException"></exception>
//    internal static KernelBuilder WithCompletionService(this KernelBuilder kernelBuilder, KernelSettings kernelSettings)
//    {
//        switch (kernelSettings.ServiceType.ToUpperInvariant())
//        {
//            case ServiceTypes.AzureOpenAI:
//                if (kernelSettings.EndpointType == EndpointTypes.TextCompletion)
//                {
//                    kernelBuilder.WithAzureTextCompletionService(deploymentName: kernelSettings.DeploymentOrModelId, endpoint: kernelSettings.Endpoint, apiKey: kernelSettings.ApiKey, serviceId: kernelSettings.ServiceId);
//                }
//                else if (kernelSettings.EndpointType == EndpointTypes.ChatCompletion)
//                {
//                    kernelBuilder.WithAzureOpenAIChatCompletionService(deploymentName: kernelSettings.DeploymentOrModelId, endpoint: kernelSettings.Endpoint, apiKey: kernelSettings.ApiKey, serviceId: kernelSettings.ServiceId);
//                }
//                break;

//            case ServiceTypes.OpenAI:
//                if (kernelSettings.EndpointType == EndpointTypes.TextCompletion)
//                {
//                    kernelBuilder.WithOpenAITextCompletionService(modelId: kernelSettings.DeploymentOrModelId, apiKey: kernelSettings.ApiKey, orgId: kernelSettings.OrgId, serviceId: kernelSettings.ServiceId);
//                }
//                else if (kernelSettings.EndpointType == EndpointTypes.ChatCompletion)
//                {
//                    kernelBuilder.WithOpenAIChatCompletionService(modelId: kernelSettings.DeploymentOrModelId, apiKey: kernelSettings.ApiKey, orgId: kernelSettings.OrgId, serviceId: kernelSettings.ServiceId);
//                }
//                break;

//            default:
//                throw new ArgumentException($"Invalid service type value: {kernelSettings.ServiceType}");
//        }

//        return kernelBuilder;
//    }
//}

//// Copyright (c) Microsoft. All rights reserved.

////using Microsoft.SemanticKernel;

////internal static class KernelBuilderExtensions
////{
////    /// <summary>
////    /// Adds a text completion service to the list. It can be either an OpenAI or Azure OpenAI backend service.
////    /// </summary>
////    /// <param name="kernelBuilder"></param>
////    /// <exception cref="ArgumentException"></exception>
////    internal static KernelBuilder WithCompletionService(this KernelBuilder kernelBuilder)
////    {
////        switch (Env.Var("Global:LlmService")!)
////        {
////            case "AzureOpenAI":
////                if (Env.Var("AzureOpenAI:DeploymentType")! == "text-completion")
////                {
////                    kernelBuilder.WithAzureTextCompletionService(
////                        deploymentName: Env.Var("AzureOpenAI:TextCompletionDeploymentName")!,
////                        endpoint: Env.Var("AzureOpenAI:Endpoint")!,
////                        apiKey: Env.Var("AzureOpenAI:ApiKey")!
////                    );
////                }
////                else if (Env.Var("AzureOpenAI:DeploymentType")! == "chat-completion")
////                {
////                    kernelBuilder.WithAzureChatCompletionService(
////                        deploymentName: Env.Var("AzureOpenAI:ChatCompletionDeploymentName")!,
////                        endpoint: Env.Var("AzureOpenAI:Endpoint")!,
////                        apiKey: Env.Var("AzureOpenAI:ApiKey")!
////                    );
////                }
////                break;

////            case "OpenAI":
////                if (Env.Var("OpenAI:ModelType")! == "text-completion")
////                {
////                    kernelBuilder.WithOpenAITextCompletionService(
////                        modelId: Env.Var("OpenAI:TextCompletionModelId")!,
////                        apiKey: Env.Var("OpenAI:ApiKey")!,
////                        orgId: Env.Var("OpenAI:OrgId")
////                    );
////                }
////                else if (Env.Var("OpenAI:ModelType")! == "chat-completion")
////                {
////                    kernelBuilder.WithOpenAIChatCompletionService(
////                        modelId: Env.Var("OpenAI:ChatCompletionModelId")!,
////                        apiKey: Env.Var("OpenAI:ApiKey")!,
////                        orgId: Env.Var("OpenAI:OrgId")
////                    );
////                }
////                break;

////            default:
////                throw new ArgumentException($"Invalid service type value: {Env.Var("OpenAI:ModelType")}");
////        }

////        return kernelBuilder;
////    }
////}