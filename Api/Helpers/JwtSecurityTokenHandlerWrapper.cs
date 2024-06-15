using Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Helpers
{
    public class JwtSecurityTokenHandlerWrapper
    {
        // Create an instance of JwtSecurityTokenHandler
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtSecurityTokenHandlerWrapper()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        // Generate a JWT token based on user ID and role
        public string GenerateJwtToken(ApplicationUser user, IList<string> roles, JwtSettings jwtSettings)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings.ExpirationInDays));
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate a JWT token
        public ClaimsPrincipal ValidateJwtToken(string token, JwtSettings jwtSettings)
        {
            // Retrieve the JWT secret from environment variables and encode it
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

            try
            {
                // Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Issuer,
                    RequireExpirationTime = true,
                    IssuerSigningKey = key,
                }, out SecurityToken validatedToken);

                // Return the claims principal
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Handle token expiration
                throw new ApplicationException("Token has expired.");
            }
        }
    }
}
