using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.Implementation
{
    internal class QueryDB
    {
        IKernel _kernel;
        static string pluginsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugins");

        public QueryDB(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async Task NL2SQL()
        {
            var queryPluginSemantic = _kernel.ImportSemanticFunctionsFromDirectory(pluginsDirectory, "QueryPlugin");
            var queryPluginNative = _kernel.ImportFunctions(new csharp_semantic_kernel.plugins.QueryPlugin.QueryPlugin(), "QueryPlugin");

            Console.WriteLine("What's your question?");
            var input = Console.ReadLine();
            input += " - The objective is to create a query able to answer the previous question";

            //var stepwisePlanner = new StepwisePlanner(_kernel);
            //var plan = stepwisePlanner.CreatePlan(input);
            var sequentialPlanner = new SequentialPlanner(_kernel);
            var plan = await sequentialPlanner.CreatePlanAsync(input);

            var result = await _kernel.RunAsync(plan);

            Console.WriteLine("Plan results:");
            Console.WriteLine(result.GetValue<string>()!.Trim());
        }
    }
}