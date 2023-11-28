using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.FunctionCalling
{
    public class RunVinScanFunction
    {
        static public string Name = "runvinscan";

        // Return the function metadata
        static public FunctionDefinition GetFunctionDefinition()
        {
            return new FunctionDefinition()
            {
                Name = Name,
                Description = "Useful when user ask to run a scan or get information over a specific VIN, model, engine or the combination of the three.",
                Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        VIN = new
                        {
                            Type = "string",
                            Description = "The VIN of the vehicle",
                        },
                        Model = new
                        {
                            Type = "string",
                            Description = "The model of the vehicle. Could be not provided",
                        },
                        Engine = new
                        {
                            Type = "string",
                            Description = "The engine name of the vehicle. Could be not provided",
                        }
                    },
                    Required = new[] { "VIN" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }

        static public string ExecuteVinScan(string vin, string model, string engine)
        {
            return $"Executed scan on VIN: {vin}, model: {model}, engine: {engine}";
        }

        static public string ExecuteVinScan(string payload)
        {
            ExecuteVinScanInput vinScanInput = JsonSerializer.Deserialize<ExecuteVinScanInput>(payload,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
            var functionResultData = RunVinScanFunction.ExecuteVinScan(vinScanInput.vin, vinScanInput.model, vinScanInput.engine);
            return functionResultData;
        }
    }

    // Argument for the function
    public class ExecuteVinScanInput
    {
        public string vin { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string engine { get; set; } = string.Empty;
    }
}