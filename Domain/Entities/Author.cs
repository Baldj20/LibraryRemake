namespace Domain.Entities
{
    public class Author : EntityWithId
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public string ImagePath { get; set; }
    }
}
