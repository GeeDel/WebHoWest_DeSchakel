using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Event;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = ("MemberOfStaff"))]
    [ApiController]
    public class LocationsController : ControllerBase
    {


        private readonly ILocationsService _locationService;
        private readonly IEventService _eventService;

        public LocationsController(IEventService eventService, ILocationsService locationService)
        {
            _eventService = eventService;
            _locationService = locationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _locationService.ListAllAsync();
            if (result.Success)
            {
                var locationDtos = result.Data.Select(c => new LocationResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capacity = c.Capacity,
                }).ToList();
                return Ok(locationDtos);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _locationService.GetByIdAsync(id);
            if (result.Success)
            {
                var locationResponseDto = new LocationResponseDto
                {
                    Id = id,
                    Name = result.Data.Name,
                    Capacity = result.Data.Capacity,
                };
                return Ok(locationResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("ByName")]
        public async Task<IActionResult> GetBySearch([FromQuery] string search)
        {
            var result = await _locationService.SearchAsync(search);
            if (result.Success)
            {
                var LocationResponseDto = result.Data.Select(g =>
                     new LocationResponseDto
                     {
                         Id = g.Id,
                         Name = g.Name,
                         Capacity= g.Capacity,
                     });
                return Ok(LocationResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Add(LocationResponseDto locationRequestDto)
        {
            var location = new Location
            {
                Name = locationRequestDto.Name,
                Capacity = locationRequestDto.Capacity,
            };
            var result = await _locationService.AddAsync(location);
            if (result.Success)
            {
                var dto = new LocationResponseDto
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    Capacity = result.Data.Capacity
                };
                return CreatedAtAction(nameof(Get), new { id = location.Id }, dto);
            }
            return BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(LocationResquestDto locationRequestDto)
        {

            var result = await _locationService.GetByIdAsync(locationRequestDto.Id);
            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }
            result.Data.Id = locationRequestDto.Id;
            result.Data.Name = locationRequestDto.Name;
            result.Data.Capacity = locationRequestDto.Capacity;
            var resultUpdate = await _locationService.UpdateAsync(result.Data);
            if (resultUpdate.Success)
            {
                return Ok($"Locatie {result.Data.Id} - {result.Data.Name} is aangepast");
            }
            return BadRequest(resultUpdate.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _locationService.DoesLocationIdExistAsync(id) == false)
            {
                return NotFound($"De locatie met  id {id} is niet gevonden.");
            }
            var existingProductResult = await _locationService.GetByIdAsync(id);
            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }
            var result = await _locationService.DeleteAsync(existingProductResult.Data);
            return Ok($"Locatie verwijderd:  {existingProductResult.Data.Name}");
        }
    }
}
