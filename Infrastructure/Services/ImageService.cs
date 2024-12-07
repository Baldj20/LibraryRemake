using Application.Exceptions;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public async Task<string> Upload(IFormFile file, Guid authorId)
        {
            if (Path.GetExtension(file.FileName) != ".jpg" &&
               Path.GetExtension(file.FileName) != ".jpeg" &&
               Path.GetExtension(file.FileName) != ".png") throw new NotImageException();

            var filePath = string.Empty;

            if (file != null && file.Length > 0)
            {
                filePath = Path.Combine("wwwroot\\images", authorId.ToString() + Path.GetExtension(file.FileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return filePath;
        }
    }
}
