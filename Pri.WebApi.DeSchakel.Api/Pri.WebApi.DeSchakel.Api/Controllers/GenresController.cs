using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Event;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IGenreService _genreService;

        public GenresController(IEventService eventService, IGenreService genreService)
        {
            _eventService = eventService;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _genreService.ListAllAsync();
            if (result.Success) 
            { 
                var genreDtos = result.Data.Select(c => new GenreResponseDto 
                { 
                    Id = c.Id,
                    Name = c.Name 
                }).ToList();
                return Ok(genreDtos);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _genreService.GetByIdAsync(id);
            if (result.Success)
            {
                var genreResponseDto = new GenreResponseDto
                {
                    Id = id,
                    Name = result.Data.Name
                };
                return Ok(genreResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("ByName")]
        public async Task<IActionResult> GetBySearch([FromQuery] string search)
        {
            var result = await _genreService.SearchAsync(search);
            if (result.Success)
            {
                var genreResponseDto = result.Data.Select(g =>
                     new GenreResponseDto
                     {
                         Id = g.Id,
                         Name = g.Name
                     });
                return Ok(genreResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpPost]
        public async Task<IActionResult> Add(GenreResponseDto genreRequestDto)
        {
            var genre = new Genre
            {
                Name = genreRequestDto.Name,
            };
            var result = await _genreService.AddAsync(genre);
            if (result.Success)
            {
                var dto = new GenreResponseDto
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name,

                };
                return CreatedAtAction(nameof(Get), new { id = genre.Id }, dto);
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpPut]
        public async Task<IActionResult> Update(GenreRequestDto genreRequestDto)
        {

            var result = await _genreService.GetByIdAsync(genreRequestDto.Id);
            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }
            result.Data.Id = genreRequestDto.Id;
            result.Data.Name = genreRequestDto.Name;
            var resultUpdate = await _genreService.UpdateAsync(result.Data);
            if (resultUpdate.Success)
            {
                return Ok($"Genre {result.Data.Id} - {result.Data.Name} is aangepast");
            }
            return BadRequest(resultUpdate.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _genreService.DoesGenreIdExistAsync(id) == false)
            {
                return NotFound($"Het genre met  id {id} is niet gevonden.");
            }
            var existingProductResult = await _genreService.GetByIdAsync(id);
            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }
            var result = await _genreService.DeleteAsync(existingProductResult.Data);
            return Ok($"Genre verwijderd:  {existingProductResult.Data.Name}");
        }

    }
}
