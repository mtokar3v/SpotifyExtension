using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class SessionService : ISessionService
    {
        private const string accessTokenName = "AccessToken";

        public void SetAccessToken(string token, HttpContext context)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception($"'{accessTokenName}' can not be null or empty");

            SetValue(accessTokenName, token, context);
        }

        public string GetAccessToken(HttpContext context) => GetValue(accessTokenName, context);

        public void RemoveAccessToken(HttpContext context) => RemoveValue(accessTokenName, context);
            

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

        public void RemoveValue(string key, HttpContext context)
        {
            if (context.Session.Keys.Contains(key))
                context.Session.Remove(key);
        }
        #endregion
    }
}
