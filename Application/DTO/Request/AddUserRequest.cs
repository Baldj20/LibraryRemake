namespace Application.DTO.Request
{
    public class AddUserRequest
    {
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
