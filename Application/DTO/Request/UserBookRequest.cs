using Domain.Entities;

namespace Application.DTO.Request
{
    public class UserBookRequest
    {
        public string UserLogin { get; set; }
        public string BookISBN { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
