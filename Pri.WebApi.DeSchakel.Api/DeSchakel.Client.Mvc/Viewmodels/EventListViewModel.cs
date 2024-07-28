using DeSchakel.Client.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakel.Client.Mvc.Viewmodels
{
    public class EventListViewModel
    {
        public IEnumerable<EventItemViewModel> Events { get; set; }
        public string LoggedInUser { get; set; }

    }
}
