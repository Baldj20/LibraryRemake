using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Mocks
{
    public static class ImageFileMock
    {
        public static Mock<IFormFile> Get()
        {
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.Setup(f => f.FileName).Returns("AndreyZaitsev.jpg");
            imageFileMock.Setup(f => f.Length).Returns(12345);
            imageFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[12345]));

            return imageFileMock;
        }
    }
}
