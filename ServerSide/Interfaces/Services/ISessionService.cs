namespace SpotifyExtension.Interfaces.Services
{
    public interface ISessionService
    {
        void SetAccessToken(string token, HttpContext context);
        string GetAccessToken(HttpContext context);
    }
}
