using Microsoft.AspNetCore.Http;

namespace Core.Servicses
{
    public interface IImageServicse
    {
        public Task<string> saveImage(IFormFile ImageFile, string folderName);
    }
}
