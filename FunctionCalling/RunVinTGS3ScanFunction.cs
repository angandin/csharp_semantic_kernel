using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.FunctionCalling
{
    public class RunVinTGS3ScanFunction
    {
        static public string Name = "runvintgs3scan";

        // Return the function metadata
        static public FunctionDefinition GetFunctionDefinition()
        {
            return new FunctionDefinition()
            {
                Name = Name,
                Description = "Useful when user ask to run a TGS3 scan over a specific VIN, model, engine or the combination of the three. " +
                "Be aware that TGS3 scan is a different request from running a normal scan." +
                "The format of the input variables is the following:" +
                " - VIN is synonimous for Vehicle Identification Number, it comes in format like 123GGHH, LK638JH or similar" +
                " - Model is the name of a model of a car, truck or motorbike" +
                " - engine is the engine type of the vehicle and its specific, like e10, 180tda or similar",
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

        static public string ExecuteVinTGS3Scan(string vin, string model, string engine)
        {
            return $"Executed scan on VIN: {vin}, model: {model}, engine: {engine}";
        }

        static public string ExecuteVinTGS3Scan(string payload)
        {
            ExecuteVinTGS3ScanInput vinScanInput = JsonSerializer.Deserialize<ExecuteVinTGS3ScanInput>(payload,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
            var functionResultData = RunVinTGS3ScanFunction.ExecuteVinTGS3Scan(vinScanInput.vin, vinScanInput.model, vinScanInput.engine);
            return functionResultData;
        }
    }

    // Argument for the function
    public class ExecuteVinTGS3ScanInput
    {
        public string vin { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string engine { get; set; } = string.Empty;
    }
}