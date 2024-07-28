using Microsoft.AspNetCore.Mvc;

namespace Pri.WebApi.DeSchakel.Api.Dtos.NavigationLinks
{
    public class ActionLinkRequestDto
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public int Position { get; set; }
    }
}
