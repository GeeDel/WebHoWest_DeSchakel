using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Api.Dtos.Role;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IRolesService _rolesService;

        public RolesController(ApplicationDbContext applicationDbContext, IRolesService rolesService)
        {
            _applicationDbContext = applicationDbContext;
            _rolesService = rolesService;
        }

    //    [Authorize (Policy = "MemberOfManagement")]
        [HttpGet]
        public IQueryable<RoleRequestDto> Get()
        {
            return _applicationDbContext.Roles.Select( r =>
            new RoleRequestDto { Id = r.Id, Name = r.Name});
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            ResultModel<RoleResponseDto> resultModel = new ResultModel<RoleResponseDto>();
            var result = await _rolesService.GetByIdAsync(id);
            if (!result.Success)
            {
                var roleResponseDto = new RoleResponseDto
                {
                    Id = id,
                    Name = result.Data.Name,
                };
                resultModel.Data = roleResponseDto;
                return Ok(roleResponseDto);
            }
            return BadRequest(resultModel.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpGet("ByName")]
        public async Task<IActionResult> GetBySearch([FromQuery] string search)
        {
            var result = await _rolesService.SearchAsync(search);
            if (result.Success)
            {
                var roleResponseDto = result.Data.Select(g =>
                     new RoleResponseDto
                     {
                         Id = g.Id,
                         Name = g.Name,
                     });
                return Ok(roleResponseDto);
            }
            return BadRequest(result.Errors);
        }

 
        [HttpPut]
        public async Task<IActionResult> Update(RoleRequestModel roleRequestModel)
        {

            var result = await _rolesService.GetByIdAsync(roleRequestModel.Id);
            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }
            var resultUpdate = await _rolesService.UpdateAsync(roleRequestModel);
            if (resultUpdate.Success)
            {
                return Ok($"De rol {roleRequestModel.Id} - {roleRequestModel.Name} is aangepast");
            }
            return BadRequest(resultUpdate.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _rolesService.DoesLRoleIdExistAsync(id) == false)
            {
                return NotFound($"De rol met id {id} is niet gevonden.");
            }
            var existingProductResult = await _rolesService.GetByIdAsync(id);
            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }
            var newRequest = new RoleRequestModel
            {
                Id = id,
                Name = existingProductResult.Data.Name
            };
            var result = await _rolesService.DeleteAsync(newRequest);
            return Ok($"Rol verwijderd:  {existingProductResult.Data.Name}");
        }

    }
}
