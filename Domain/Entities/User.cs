namespace Domain.Entities
{
    public class User
    {
        public string Login { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public ICollection<UserBook> TakenBooks { get; set; } = new List<UserBook>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
