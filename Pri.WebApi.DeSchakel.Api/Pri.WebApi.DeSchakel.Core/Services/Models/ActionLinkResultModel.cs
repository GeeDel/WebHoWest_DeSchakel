using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Models
{
    public class ActionLinkResultModel
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public int Position { get; set; }
    }
}
