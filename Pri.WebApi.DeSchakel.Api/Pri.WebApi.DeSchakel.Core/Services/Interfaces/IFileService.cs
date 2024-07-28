using Microsoft.AspNetCore.Http;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IFileService
    {
        Task<ResultModel<string>> AddOrUpdateImageAsync(IFormFile image, string fileName = "");
    }
}
