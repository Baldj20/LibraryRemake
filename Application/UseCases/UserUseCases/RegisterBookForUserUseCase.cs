namespace Application.UseCases.UserUseCases
{
    public class RegisterBookForUserUseCase
    {
        public string UserLogin { get; set; }
        public string BookISBN { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
