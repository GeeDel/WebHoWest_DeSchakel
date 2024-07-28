using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Models
{
    public abstract class BaseResult
    {
        public bool Success => !Errors.Any();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
