using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Navigation
{
    public class NavigationResponseApiModel
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public int Position { get; set; }
    }
}
