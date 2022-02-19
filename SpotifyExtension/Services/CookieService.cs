using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class CookieService : ICookieService
    {
        private const string refreshTokenCookieName = "RefreshToken"; 

        public void SetRefreshToken(string token, HttpContext context)
            => SetCookie(refreshTokenCookieName, token, context);

        public string GetRefreshToken(HttpContext context)
            => GetCookie(refreshTokenCookieName, context);

        #region Private Methods
        private void SetCookie(string key, string value, HttpContext context, bool httpOnly = true)
        {
            if(context.Request.Cookies.ContainsKey(key))
                context.Response.Cookies.Delete(key);

            context.Response.Cookies.Append(key, value, new CookieOptions
            {
                HttpOnly = httpOnly,
            });
        }

        private string GetCookie(string key, HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey(key))
                return string.Empty;

            return context.Request.Cookies[key]!;
        }
        #endregion
    }
}
