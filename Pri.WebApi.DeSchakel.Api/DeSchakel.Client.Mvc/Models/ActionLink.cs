using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Models
{
    public class ActionLink
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        [HiddenInput]
        public int Position { get; set; }
    }
}
