using SpotifyAPI.Web;
using System.Security.Claims;

namespace SpotifyExtension.Interfaces.Services
{
    public interface IAuthorizeService
    {
        Uri CreateAuthLink(string redirectMethod, string clientId);

        Task<string?> GetAccessTokenAsync(string redirectMethod, string code, HttpContext context);

        Task<bool> TryRefreshTokenAsync(HttpContext context);

        string GetAccessToken(ClaimsPrincipal User);

        string GetRefreshToken(ClaimsPrincipal User);

    }
}
