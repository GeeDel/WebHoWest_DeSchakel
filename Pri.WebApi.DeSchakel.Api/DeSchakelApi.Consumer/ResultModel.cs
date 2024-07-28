using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer
{
    public class ResultModel<T>
    {
        public bool Success => !Errors.Any();
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
    }
}
