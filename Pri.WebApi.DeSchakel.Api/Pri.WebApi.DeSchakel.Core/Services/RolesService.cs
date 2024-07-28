using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RolesService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        public async Task<bool> DoesLRoleIdExistAsync(string id)
        {
            return await _applicationDbContext.Roles.AnyAsync(e => e.Id.Equals(id));
        }

        public async Task<bool> DoesRoleNameExistsAsync(RoleRequestModel entity)
        {
            return await _applicationDbContext.Roles
                       .Where(e => !e.Id.Equals(entity.Id))
                       .AnyAsync(e => e.Name.Equals(entity.Name));
        }

        public IQueryable<RoleResponseModel> GetAll()
        {
            return _applicationDbContext.Roles.Select(r => new RoleResponseModel
            {
                Id = r.Id,
                Name = r.Name,
            });
        }

        public async Task<ResultModel<IEnumerable<RoleResponseModel>>> ListAllAsync()
        {
            var resultModel = new ResultModel<IEnumerable<RoleResponseModel>>();
            var roles = await _applicationDbContext.Roles.Select(r => new RoleResponseModel
            {
                Id = r.Id,
                Name = r.Name,
            }).ToListAsync();
            if (roles.Count > 0)
            {
                resultModel.Data = roles;
            }
            else
            {
                resultModel.Errors = new List<string> { "Fout in de verbinding met de database" };
            }
            return resultModel;
        }

        public async Task<ResultModel<RoleResponseModel>> GetByIdAsync(string id)
        {

            if (!await DoesLRoleIdExistAsync(id))
            {
                return new ResultModel<RoleResponseModel> { Errors = new List<string> { $"Geen rol gevonden met id {id}" } };
            }

            var searchedRole = await _applicationDbContext.Roles
               .FirstOrDefaultAsync(r => r.Id == id);
            var result =  new RoleResponseModel
            { 
               Id = searchedRole.Id,
               Name = searchedRole.Name,
            };
            /*  role.Select(r => new RoleResponseModel
                {
                    Id = r.Id,
                    Name = r.Name,
                });  */
            return new ResultModel<RoleResponseModel> { Data = result };
        }




        public async Task<ResultModel<IEnumerable<RoleResponseModel>>> SearchAsync(string search)
            {
            search = search ?? string.Empty;
            var roles = await _applicationDbContext.Roles
                .Where(r => r.Name.Contains(search.Trim()))
                .Select(r => new RoleResponseModel { Id = r.Id, Name = r.Name})
                        .ToListAsync();
            if (roles.Count() != 0)
            {
                return new ResultModel<IEnumerable<RoleResponseModel>> { Data = roles };
            }
            return new ResultModel<IEnumerable<RoleResponseModel>> { Errors = new List<string> { $"Geen rollen gevonden met ${search}" } };
        }


        /*     public async Task<ResultModel<RoleResponseModel>> AddAsync(RoleRequestModel entity)
             {
                 var resultModel = new ResultModel<RoleResponseModel>();

                 if (await DoesRoleNameExistsAsync(entity))
                 {
                     resultModel.Errors.Add($"Een rol  met de naam {entity.Name} bestaat al");

                     return resultModel;
                 }

                 _applicationDbContext.Roles.Add((entity);
                 try
                 {
                     await _applicationDbContext.SaveChangesAsync();
                 }
                 catch (Exception ex)
                 {
                     resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                     return resultModel;

                 }

                 resultModel = new ResultModel<Location> { Data = entity };

                 return resultModel;
             }

      */      
             public async Task<ResultModel<RoleResponseModel>> UpdateAsync(RoleRequestModel entity)
             {
                 var resultModel = new ResultModel<RoleResponseModel>();

                 if (!await DoesLRoleIdExistAsync(entity.Id))
                 {
                     return new ResultModel<RoleResponseModel> { Errors = new List<string> { $"Geen rol gevonden met id {entity.Id}" } };
                 }

                 var searchedRole = await _applicationDbContext.Roles
                    .FirstOrDefaultAsync(r => r.Id == entity.Id);
                 _applicationDbContext.Roles.Update(searchedRole);
            try
            {

                await _applicationDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel.Data = null;
            return resultModel;
        }

    

        public async  Task<ResultModel<RoleResponseModel>> DeleteAsync(RoleRequestModel entity)
        {
            var resultModel = new ResultModel<RoleResponseModel>();

            if (!await DoesLRoleIdExistAsync(entity.Id))
            {
                return new ResultModel<RoleResponseModel> { Errors = new List<string> { $"Geen rol gevonden met id {entity.Id}" } };
            }

            var searchedRole = await _applicationDbContext.Roles
               .FirstOrDefaultAsync(r => r.Id == entity.Id);
            _applicationDbContext.Roles.Remove(searchedRole);
            try
            {
                await _applicationDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }


            resultModel.Data = null;

            return resultModel;
        }

    }
}
