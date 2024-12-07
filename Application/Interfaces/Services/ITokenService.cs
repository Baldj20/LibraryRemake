using Application.DTO.Response;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateAccessToken(User user);
        public RefreshToken GenerateRefreshToken(User user);
        public Task<TokenResponse> RefreshJWTToken(string userLogin, string oldRefreshToken, CancellationToken token);
    }
}
