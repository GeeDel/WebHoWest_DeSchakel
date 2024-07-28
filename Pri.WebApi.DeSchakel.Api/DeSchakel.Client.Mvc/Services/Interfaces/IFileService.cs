namespace DeSchakel.Client.Mvc.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> Store(IFormFile file);
        bool Delete(string fileName);
        Task<string> Update(IFormFile file, string oldFileName);
        Task<string> GetPathToImages();
            }
}
