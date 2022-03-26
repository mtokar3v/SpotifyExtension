namespace SpotifyExtension.Interfaces.Services
{
    public interface ICookieService
    {
        public void SetAccessToken(string token, HttpContext context);

        public string GetAccessToken(HttpContext context);

        void RemoveSEAccessToken(HttpContext context);
    }
}
