using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public class NavigationItem
    {
        public int Id { get; set; }
        public string? Area { get; set; }
        public int Position { get; set; }
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
    }
}
