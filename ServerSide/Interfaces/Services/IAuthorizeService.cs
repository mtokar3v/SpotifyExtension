using SpotifyAPI.Web;
using System.Security.Claims;

namespace SpotifyExtension.Interfaces.Services
{
    public interface IAuthorizeService
    {
        Uri CreateAuthLink(string redirectMethod, string clientId);

        Task<string?> GetAccessTokenAsync(string redirectMethod, string code);

        Task<bool> RefreshSpotifyTokensAsync(ClaimsPrincipal user);

        string? GetSpotifyAccessToken(ClaimsPrincipal User);

        string? GetSpotifyRefreshToken(ClaimsPrincipal User);
    }
}
