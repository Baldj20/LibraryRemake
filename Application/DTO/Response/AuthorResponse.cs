using Domain.Entities;

namespace Application.DTO.Response
{
    public class AuthorResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public ICollection<BookResponse> Books { get; set; }
    }
}
