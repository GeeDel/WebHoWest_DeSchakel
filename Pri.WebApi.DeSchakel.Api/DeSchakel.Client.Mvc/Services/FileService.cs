using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakelApi.Consumer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace DeSchakel.Client.Mvc.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostEnvironment _hostEnvironment;


        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;
        private readonly IHttpContextFactory _httpContextFactory;

        public FileService(IWebHostEnvironment webHostEnvironment, IHostEnvironment hostEnvironment,
            IHttpContextFactory httpContextFactory, IHttpClientFactory httpClientFactory)
        {
            _webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri("https://localhost:44326/api\\");
        }

        /*
         * 
         *             _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Events);



        */


        public async Task<string> Store(IFormFile file)
        {
            if (file == null)
            {
                return "default.jpg";
            }
            // 1.create unique filename
            var filename = $"{Guid.NewGuid}_{file.FileName}";
            // 2. create path to filename
            // uit api  var pathOnDisk = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "img", "food");

            //    var pathToImagesApi = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "events");
/* working version with CreateEvent form StaffController
            var apiPathToImage = Path.Combine(_hostEnvironment.ContentRootPath =
               "C:\\Users\\Gebruiker\\source\\repos\\2024\\WorkingPI\\st-ee-h-s4-pri-GeertDeloof-w\\Pri.WebApi.DeSchakel.Api\\Pri.WebApi.DeSchakel.Api\\wwwroot\\images\\events\\");
*/
            /*   
        // vroegere code : Path.Combine(_webHostEnvironment.WebRootPath, "images/events");
            */
            var apiPathToImage = Path.Combine(_webHostEnvironment.WebRootPath, "images/events");
            if (!Directory.Exists(apiPathToImage))
            {
                Directory.CreateDirectory(apiPathToImage);
            }
            var fullPathToFile = Path.Combine(apiPathToImage, filename);
            // - 3.copyimage to disk
            using (FileStream fileStream = new FileStream(fullPathToFile, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            //-4.return imagename (path +name) to the controller and there store in the database
            return filename;
        }

        public bool Delete(string fileName)
        {
            string pathToImage = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
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

        public async Task<string> Update(IFormFile file, string oldFileName)
        {
            if (Delete(oldFileName))
            {
                return await Store(file);   // retourneert een string met url
            };
            return "Error";
        }

        public async Task<string>  GetPathToImages()
        {
           string path =  Path.Combine(_webHostEnvironment.WebRootPath, "images/events");
   
            return path;
        }

    }
}
