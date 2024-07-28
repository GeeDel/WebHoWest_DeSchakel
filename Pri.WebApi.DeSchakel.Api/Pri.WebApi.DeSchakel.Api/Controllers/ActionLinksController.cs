using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Api.Dtos.NavigationLinks;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionLinksController : ControllerBase
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IActionLinkService _actionLinkService;

        public ActionLinksController(ApplicationDbContext applicationDbContext, IActionLinkService actionLinkService)
        {
            _applicationDbContext = applicationDbContext;
            _actionLinkService = actionLinkService;
        }

        [HttpGet("{area}")]
        public async Task<IActionResult> Get(string area)
        {
            var result = await _actionLinkService.GetNavigationLinks(area);
            if (result.Success)
            {
                var linkDtos = result.Data.Select(a => new ActionLinkResponseDto
                {
                    Name = a.Name,
                    Controller = a.Controller,
                    Action = a.Action,
                    Position = a.Position,

                }).ToList();
                return Ok(linkDtos);
            }
            return BadRequest(result.Errors);
        }
    }
}
