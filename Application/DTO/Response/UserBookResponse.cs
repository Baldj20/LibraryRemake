namespace Application.DTO.Response
{
    public class UserBookResponse
    {
        public BookResponse Book { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
