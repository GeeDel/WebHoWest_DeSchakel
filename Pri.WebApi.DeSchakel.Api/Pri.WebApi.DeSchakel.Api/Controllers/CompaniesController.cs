using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;



namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "MemberOfStaff")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        //     [Authorize(Policy = "MemberOfStaff")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var result = await _companyService.ListAllAsync();
            if (result.Success)
            {
                var companyDtos = result.Data.Select(c => new CompanyResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
                return Ok(companyDtos);
            }
            return BadRequest(result.Errors);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _companyService.GetByIdAsync(id);
            if (result.Success)
            {
                var companyResponseDto = new CompanyResponseDto
                {
                    Id = id,
                    Name = result.Data.Name,
                };
                return Ok(companyResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("Name/{name}")]
        public async Task<IActionResult> GetBySearch(string name)
        {
            var result = await _companyService.SearchAsync(name);
            if (result.Success)
            {
                var companyResponseDto = new CompanyResponseDto
                     {
                         Id = result.Data.Id,
                         Name = result.Data.Name,
                     };
                return Ok(companyResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CompanyResponseDto companyRequestDto)
        {
            var company = new Company
            {
                Name = companyRequestDto.Name,
            };
            var result = await _companyService.AddAsync(company);
            if (result.Success)
            {
                var dto = new CompanyResponseDto
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                };
                return CreatedAtAction(nameof(Get), new { id = company.Id }, dto);
            }
            return BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(LocationResquestDto companyResponseDto)
        {

            var result = await _companyService.GetByIdAsync(companyResponseDto.Id);
            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }
            result.Data.Id = companyResponseDto.Id;
            result.Data.Name = companyResponseDto.Name;
            var resultUpdate = await _companyService.UpdateAsync(result.Data);
            if (resultUpdate.Success)
            {
                return Ok($"Locatie {result.Data.Id} - {result.Data.Name} is aangepast");
            }
            return BadRequest(resultUpdate.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _companyService.DoesCompanyIdExistAsync(id) == false)
            {
                return NotFound($"De locatie met  id {id} is niet gevonden.");
            }
            var existingProductResult = await _companyService.GetByIdAsync(id);
            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }
            var result = await _companyService.DeleteAsync(existingProductResult.Data);
            return Ok($"Locatie verwijderd:  {existingProductResult.Data.Name}");
        }

    }
}
