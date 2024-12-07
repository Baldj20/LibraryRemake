namespace Application.DTO.Request
{
    public class UpdateUserRequest
    {
        public string OldLogin { get; set; }
        public string NewLogin { get; set; }
        public string NewPassword { get; set; }
    }
}
