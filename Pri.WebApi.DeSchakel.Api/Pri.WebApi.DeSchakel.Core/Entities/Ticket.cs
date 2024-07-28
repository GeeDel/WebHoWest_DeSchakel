using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public  class Ticket
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        // one visitor
        public ApplicationUser Visitor { get; set; }
        public string VisitorId { get; set; }
        // one event
        public Event Event { get; set; }
        public int EventId { get; set; }
    }
}
