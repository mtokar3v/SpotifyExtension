using Microsoft.AspNetCore.Authentication.JwtBearer;
using SpotifyExtension.DataItems.Options;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class CookieService : ICookieService
    {
        public void SetAccessToken(string token, HttpContext context)
            => SetCookie(JwtAuthOptions.AccessTokenCookieName, token, context);

        public string GetAccessToken(HttpContext context)
            => GetCookie(JwtAuthOptions.AccessTokenCookieName, context);

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
