namespace SpotifyExtension.Interfaces.Services
{
    public interface ICookieService
    {
        public void SetRefreshToken(string token, HttpContext context);
        public string GetRefreshToken(HttpContext context);
    }
}
