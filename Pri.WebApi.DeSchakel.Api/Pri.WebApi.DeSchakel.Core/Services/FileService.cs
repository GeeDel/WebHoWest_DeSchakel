using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ResultModel<string>> AddOrUpdateImageAsync
           (IFormFile image, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = $"{Guid.NewGuid()}_{Path.GetExtension(image.FileName)}";
            }

            var pathOnDisk = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot",
                      "images", "events");

            if (!Directory.Exists(pathOnDisk))
            {
                Directory.CreateDirectory(pathOnDisk);
            }

            var completePathWithFilename = Path.Combine(pathOnDisk, fileName);


            using (FileStream fileStream = new(completePathWithFilename, FileMode.Create))
            {
                try
                {
                    await image.CopyToAsync(fileStream);
                    return new ResultModel<string>
                    {
                        Data = fileName
                    };
                }
                catch (FileNotFoundException exception)
                {
                    return new ResultModel<string>
                    {
                        Errors = new List<string> { exception.Message }
                    };
                }
            }
        }
    }
}

