using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class SessionService : ISessionService
    {
        private const string accessTokenName = "AccessToken";

        public void SetAccessToken(string token, HttpContext context)
            => SetValue(accessTokenName, token, context);

        public string GetAccessToken(HttpContext context)
            => GetValue(accessTokenName, context);

        #region Private Methods
        private void SetValue(string key, string value, HttpContext context)
        {
            if (context.Session.Keys.Contains(key))
                context.Session.Remove(key);

            context.Session.SetString(key, value);
        }

        private string GetValue(string key, HttpContext context)
        {
            if (!context.Session.Keys.Contains(key))
                return string.Empty;

            return context.Session.GetString(key)!;
        }
        #endregion
    }
}
