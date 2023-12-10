using Core.Servicses;
using Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Infrastructure.Services
{
    public class ImageServicse : IImageServicse
    {
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ImageServicse(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<string> saveImage(IFormFile ImageFile, string folderName)
        {


            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"{FileSettings.ImagesPath}/{folderName}");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            var imagePath = $"{FileSettings.ImagesPath}/{folderName}/{uniqueFileName}";

            return imagePath;
        }
    }
}
