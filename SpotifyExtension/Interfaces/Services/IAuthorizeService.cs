using SpotifyAPI.Web;

namespace SpotifyExtension.Interfaces.Services
{
    public interface IAuthorizeService
    {
        Uri CreateAuthLink(string redirectMethod, string clientId);
        Task<PKCETokenResponse> GetPkceToken(string redirectMethod, string code);
    }
}
