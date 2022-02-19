namespace SpotifyExtension.Interfaces.Services
{
    public interface IAuthorizeService
    {
        Uri CreateAuthLink(string redirectMethod, string clientId);

        Task<bool> TryGetAccessAsync(string redirectMethod, string code, HttpContext context);

        Task<bool> TryReloadTokenAsync(HttpContext context);
    }
}
