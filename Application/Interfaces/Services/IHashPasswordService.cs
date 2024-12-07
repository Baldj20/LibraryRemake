namespace Application.Interfaces.Services
{
    public interface IHashPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
    }
}
