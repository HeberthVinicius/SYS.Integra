using SYS.Integra.src.SYS.Integra.Application.Authentication;
using System.Security.Claims;

namespace SYS.Integra.src.SYS.Integra.Application.Helpers
{
    public static class TokenHelper
    {
        public static bool TryGetValidatedToken(HttpContext httpContext, TokenService tokenService, out ClaimsPrincipal? principal)
        {
            principal = null;

            // Obter o token do cabeçalho de autorização da solicitação
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return false; // Retorna false se o token não estiver presente
            }

            // Validar o token usando a lógica do TokenService
            principal = tokenService.ValidateTokenAsync(token);

            return principal != null; // Retorna true se o token for válido
        }
    }
}
