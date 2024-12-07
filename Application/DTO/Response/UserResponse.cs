using Domain.Entities;

namespace Application.DTO.Response
{
    public class UserResponse
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public ICollection<UserBookResponse> TakenBooks { get; set; }
    }
}
