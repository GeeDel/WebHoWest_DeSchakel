using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // many Events
        public ICollection<Event> Events { get; set; }
    }
}
