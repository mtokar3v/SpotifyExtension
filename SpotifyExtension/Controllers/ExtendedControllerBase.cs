using Microsoft.AspNetCore.Mvc;
using SpotifyExtension.DataItems.Options;

namespace SpotifyExtension.Controllers
{
    public abstract class ExtendedControllerBase : ControllerBase
    {
        private string accessTokenName = JwtAuthOptions.AccessTokenCookieName;

        internal void Logout()
        {
            RemoveAccessToken();
        }

        #region Cookie
        internal string GetAccessToken()
        {
            if (!HttpContext.Request.Cookies.ContainsKey(accessTokenName))
                return string.Empty;

            return HttpContext.Request.Cookies[accessTokenName]!;
        }

        internal void SetAccessToken(string value)
        {
            if (HttpContext.Request.Cookies.ContainsKey(accessTokenName))
                HttpContext.Response.Cookies.Delete(accessTokenName);

            HttpContext.Response.Cookies.Append(accessTokenName, value, new CookieOptions
            {
                HttpOnly = true,
            });
        }

        internal void RemoveAccessToken()
        {
            if (HttpContext.Request.Cookies.ContainsKey(accessTokenName))
                HttpContext.Response.Cookies.Delete(accessTokenName);
        }
        #endregion
    }
}
