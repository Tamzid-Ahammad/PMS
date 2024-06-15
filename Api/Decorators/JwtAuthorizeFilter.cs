using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Api.Decorators
{
    public class JwtAuthorizeFilter : IAuthorizationFilter
    {
        private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandler;
        private readonly JwtSettings _jwtSettings;
        public JwtAuthorizeFilter(JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandler, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _jwtSettings = jwtSettings.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the [Authorize] attribute is explicitly applied to the action or controller.
            var hasAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata
                .Any(em => em is AuthorizeAttribute);

            var hasAllowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata
                .Any(em => em is AllowAnonymousAttribute);


            if (hasAuthorizeAttribute && !hasAllowAnonymousAttribute)
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        // Validate the token and extract claims
                        var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token, _jwtSettings);

                        // Extract the user ID from the token
                        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        context.HttpContext.Items["UserId"] = userId;
                    }
                    catch (Exception)
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }

    }
}
