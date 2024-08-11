using Microsoft.AspNetCore.Hosting;
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
           (IFormFile image, string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                filename = $"{Guid.NewGuid()}_{Path.GetExtension(image.FileName)}";
            }

            var pathOnDisk = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot",
                      "media", "performances");

            if (!Directory.Exists(pathOnDisk))
            {
                Directory.CreateDirectory(pathOnDisk);
            }
            string serverFilename = $"{Guid.NewGuid}_{filename}";
            var completePathWithFilename = Path.Combine(pathOnDisk, serverFilename);


            using (FileStream fileStream = new(completePathWithFilename, FileMode.Create))
            {
                try
                {
                    await image.CopyToAsync(fileStream);
                    return new ResultModel<string>
                    {
                        Data = serverFilename,
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

        public bool Delete(string fileName)
        {
            string pathToImage = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot",
                      "media", "performances", fileName);
            try
            {
                System.IO.File.Delete(pathToImage);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine(fileNotFoundException.Message);
                return false;
            }
            return true;
        }
    }
}

