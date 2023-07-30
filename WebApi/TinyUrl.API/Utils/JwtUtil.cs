using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TinyUrl.API.Models.Responce;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.API.Utils
{
    public class JwtUtil : IJwtUtil
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _refreshTokenService;

        public JwtUtil(IConfiguration configuration,
            ITokenService refreshTokenService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponseModel> GenerateTokenAsync(UserDto dto)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:JwtSecret"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var nowUtc = DateTime.UtcNow;
            var exp = nowUtc.AddMinutes(double.Parse(_configuration["Token:ExpiryMinutes"]))
                .ToUniversalTime();

            var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, dto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
            new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString("D"))
        };

            var jwtToken = new JwtSecurityToken(_configuration["Token:Issuer"],
                _configuration["Token:Issuer"],
                claims,
                expires: exp,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshTokenValue = Guid.NewGuid();

            await _refreshTokenService.CreateRefreshTokenAsync(refreshTokenValue, dto.Id);

            return new TokenResponseModel()
            {
                AccessToken = accessToken,
                TokenExpiration = jwtToken.ValidTo,
                UserId = dto.Id,
                RefreshToken = refreshTokenValue
            };
        }

        public async Task RemoveRefreshTokenAsync(Guid requestRefreshToken)
        {
            await _refreshTokenService.RemoveRefreshTokenAsync(requestRefreshToken);
        }
    }
}
