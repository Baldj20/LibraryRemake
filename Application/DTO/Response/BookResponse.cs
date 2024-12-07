namespace Application.DTO.Response
{
    public class BookResponse
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public AuthorResponse Author { get; set; }
        public int Count { get; set; }
    }
}
