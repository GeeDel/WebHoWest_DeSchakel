using DeSchakelApi.Consumer.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Locations
{
    public class LocationResponseApiModel : BaseResponseApiModel
    {
        public int Capacity { get; set; }
    }
}
