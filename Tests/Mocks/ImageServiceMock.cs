using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Mocks
{
    public static class ImageServiceMock
    {
        public static Mock<IImageService> Get()
        {
            var mockImageService = new Mock<IImageService>();

            mockImageService
                .Setup(service => service.Upload(It.IsAny<IFormFile>(), It.IsAny<Guid>()))
                .ReturnsAsync("wwwroot/AndreyZaitsev.jpg");

            return mockImageService;
        }
    }
}
