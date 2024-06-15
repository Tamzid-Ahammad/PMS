using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public AccountController(UserManager<ApplicationUser> userManager, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public record SignUpPayload(string UserName, string Password, string PhoneNumber, string Email);
        public record SignInResponse(string Token, string RefreshToken, UserInfo User, DateTime Expiration);

        [HttpPost("signup")]
        public async Task<ActionResult<SignInResponse>> SignUp([FromBody] SignUpPayload request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
            };
            var userCreateResult = await _userManager.CreateAsync(user, request.Password);
            var normalizedUserName = _userManager.NormalizeName(user.UserName);
            var userSigninResult = await _userManager.CheckPasswordAsync(user, request.Password);
            if (userSigninResult)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
                claims.AddRange(roleClaims);

                var token = CreateToken(claims);
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtSettings.RefreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                return new SignInResponse(
                    new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken,
                    new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                    },
                    token.ValidTo
                );
            }
            return BadRequest(userCreateResult.Errors.First().Description);
        }

        public record SignInPayload(string UserName, string Password);

        [HttpPost("public-signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInPayload request)
        {
            var normalizedUserName = _userManager.NormalizeName(request.UserName);
            var user = await _userManager.Users
                .Where(it => it.NormalizedUserName == normalizedUserName)
                .SingleOrDefaultAsync();
            if(user == null)
                return BadRequest("User not found!");
            var userSigninResult = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!userSigninResult)
                return BadRequest( "Email or password incorrect.");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            claims.AddRange(roleClaims);


            var token = CreateToken(claims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            return new SignInResponse(
                    new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken,
                    new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                    },
                    token.ValidTo
                );
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
