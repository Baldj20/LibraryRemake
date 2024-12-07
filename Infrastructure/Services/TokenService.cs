
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;

        public TokenService(IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
        }
        public string GenerateAccessToken(User user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecretKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                audience: jwtSettings.GetSection("Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("Expires").Value)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                User = user,
                UserLogin = user.Login,
            };

            return refreshToken;
        }

        public async Task<TokenResponse> RefreshJWTToken(string userLogin, string oldRefreshToken, CancellationToken token)
        {
            var storedToken = (await unitOfWork.RefreshTokens.GetAll(token))
                .Where(token => token.Token.Equals(oldRefreshToken)).FirstOrDefault();

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
                throw new SecurityTokenException("Invalid or expired refresh token");           

            var user = await unitOfWork.Users.GetByLogin(userLogin);

            if (user == null) throw new NotFoundException("Owner of token not found");

            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken(user);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
            };
        }
    }
}
