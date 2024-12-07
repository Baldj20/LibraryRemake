using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IImageService
    {
        public Task<string> Upload(IFormFile file, Guid authorId);
    }
}
