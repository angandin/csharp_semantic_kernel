using csharp_semantic_kernel.FunctionCalling;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_semantic_kernel.plugins.CreateTicketPlugin
{
    internal class CreateTicketPlugin
    {
        [SKFunction, SKName("CreateSupportTicket"), System.ComponentModel.Description("Useful when user needs to create a new support ticket. " +
            "Do not invent or provide any information over the ticket to be opened and do not ask for further support on this ticket. " +
            "You need to collect input asking for them one at a time, DO NOT CONSIDER any input already provided before the request to create a ticket, but collect new input. " +
            "Once collected all the info, recap in a bullet list all the information asking for confirmation of data before to run this function. " +
            "NEVER run this method if the user has not explictly accepted the input collected.")]
        static public string CreateNewTicket(
            
            [Description("VIN is synonimous for Vehicle Identification Number, it comes in format like 123GGHH, LK638JH or similar")] string vin,
            [Description("Mail of the user, check validity of it")] string userMail,
            [Description("Description of the issue")] string descriptionIssue,
            [Description("Date of when the issue showed up, ask for a date in dd/MM/yyyy format")] string dateIssue)
        {
            var ticket = new Ticket() { TicketId = new Guid() };
            return $"Thanks for your informations, I have created the ticket with guid: {ticket.TicketId}";
        }

        public class Ticket
        {
            public Guid TicketId { get; set; }
        }
    }
}
