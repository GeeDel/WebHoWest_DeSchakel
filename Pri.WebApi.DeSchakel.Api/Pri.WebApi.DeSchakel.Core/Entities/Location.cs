using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        // many Events
        public ICollection<Event> Events { get; set; }
    }
}
