namespace Application.DTO.Request
{
    public class BookRequest
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public int Count { get; set; }
    }
}
