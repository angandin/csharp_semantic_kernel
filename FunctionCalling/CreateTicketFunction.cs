using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.FunctionCalling
{
    public class CreateTicketFunction
    {
        static public string Name = "create_ticket";

        // Return the function metadata
        static public FunctionDefinition GetFunctionDefinition()
        {
            return new FunctionDefinition()
            {
                Name = Name,
                Description = "Useful when user wants to create a ticket for a current issue he/she is facing.",
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
                        UserMail = new
                        {
                            Type = "string",
                            Description = "The user email address",
                        },
                        DescriptionIssue = new
                        {
                            Type = "string",
                            Description = "A description of the issue user is facing",
                        },
                        DateIssue = new
                        {
                            Type = "string",
                            Description = "The date when user is facing the issue",
                        }
                    },
                    Required = new[] { "VIN", "UserMail", "DescriptionIssue", "DateIssue" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }

        static public string CreateNewTicket(string vin, string userMail, string descriptionIssue, string dateIssue)
        {
            var ticket = new Ticket() { TicketId = new Guid() };
            return $"Thanks for your informations, I have created the ticket with guid: {ticket.TicketId}";
        }

        static public string CreateNewTicket(string payload)
        {
            CreateNewTicketInput ticketInput = JsonSerializer.Deserialize<CreateNewTicketInput>(payload, 
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
            var functionResultData = CreateTicketFunction.CreateNewTicket(ticketInput.Vin, ticketInput.UserMail, ticketInput.DescriptionIssue, ticketInput.DateIssue);
            return functionResultData;
        }
    }

    // Argument for the function
    public class CreateNewTicketInput
    {
        public string Vin { get; set; } = string.Empty;
        public string UserMail { get; set; } = string.Empty;
        public string DescriptionIssue { get; set; } = string.Empty;
        public string DateIssue { get; set; } = string.Empty;
    }

    // Return type
    public class Ticket
    {
        public Guid TicketId { get; set; }
    }
}
