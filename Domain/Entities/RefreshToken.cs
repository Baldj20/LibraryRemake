namespace Domain.Entities
{
    public class RefreshToken : EntityWithId
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string UserLogin { get; set; }
        public User User { get; set; }
    }
}
