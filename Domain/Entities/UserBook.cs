namespace Domain.Entities
{
    public class UserBook
    {
        public User User { get; set; }
        public string UserLogin { get; set; }
        public Book Book { get; set; }
        public string BookISBN { get; set; }

        public DateTime ReceiptDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
