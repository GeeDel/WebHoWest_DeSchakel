using Microsoft.AspNetCore.Mvc;

namespace Pri.WebApi.DeSchakel.Api.Dtos.NavigationLinks
{
    public class ActionLinkResponseDto
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        [HiddenInput]
        public int Position { get; set; }
    }
}
