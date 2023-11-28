using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.plugins.RunScan
{
    internal class RunVinScanPlugin
    {
        [SKFunction, SKName("ExecuteScan"), System.ComponentModel.Description("Useful when user ask to run a scan or get information over a specific VIN, model, engine or the combination of the three. If model or engine parameters are not provided, do not ask for them.")]
        static public string ExecuteVinScan(
            [Description("VIN is synonimous for Vehicle Identification Number, in the format like 123GGHH, LK638JH or similar. It is usually provided within the request")] string vin, 
            [Description("the name of a model of a car, truck or motorbike. If not provided, do not ask for it")] string? model = "", 
            [Description("the engine type of the vehicle and its specific, like e10, 180tda or similar. If not provided, do not ask for it")] string? engine = "")
        {
            return $"Executed scan on VIN: {vin}, model: {model}, engine: {engine}";
        }

        [SKFunction, SKName("ExecuteTGS3Scan"), System.ComponentModel.Description("Useful when user ask to run a TGS3 scan or get information over a specific VIN, model, engine or the combination of the three. If model or engine parameters are not provided, do not ask for them.")]
        static public string ExecuteTGS3VinScan(
            [Description("VIN is synonimous for Vehicle Identification Number, in the format like 123GGHH, LK638JH or similar. It is usually provided within the request")] string vin,
            [Description("the name of a model of a car, truck or motorbike. If not provided, do not ask for it")] string? model = "",
            [Description("the engine type of the vehicle and its specific, like e10, 180tda or similar. If not provided, do not ask for it")] string? engine = "")
        {
            return $"Executed TGS3 scan on VIN: {vin}, model: {model}, engine: {engine}";
        }
    }
}
