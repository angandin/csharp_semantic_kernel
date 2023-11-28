using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.plugins.QueryPlugin
{
    public class QueryPlugin
    {
        [SKFunction, Description("Retrieve list of tables available, with their definitions. These are the tables you must get before to create any query")]
        public async Task<string> GetTables()
        {
            Console.WriteLine("Retrieveing tables...");
            return "Customer (id, Name, Age, Address), Sales (id, Name, BusinessUnit), SalesOrder (customerId, salesId, productName, quantity, price)";
        }

        [SKFunction, Description("Execute a given query to retrieve results to answer user's question")]
        public async Task<string> ExecuteQuery(
            [Description("Query to be executed on SQL Database to retrieve results requested by user")] string query)
        {
            Console.WriteLine($"Created query: {query}");
            return "productName: pencil, price: 2400 euros";
        }
    }
}
