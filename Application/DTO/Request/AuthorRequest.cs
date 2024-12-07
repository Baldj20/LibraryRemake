using Microsoft.AspNetCore.Http;

namespace Application.DTO.Request
{
    public class AuthorRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public string BooksJson { get; set; }
        public IFormFile Image { get; set; }
    }
}
