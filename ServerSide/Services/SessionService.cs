using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class SessionService : ISessionService
    {
        private const string accessTokenName = "AccessToken";

        public void SetAccessToken(string token, ISession session)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception($"'{accessTokenName}' can not be null or empty");

            SetValue(accessTokenName, token, session);
        }

        public string GetAccessToken(ISession session) => GetValue(accessTokenName, session);

        public void RemoveAccessToken(ISession session) => RemoveValue(accessTokenName, session);
            

        #region Private Methods
        private void SetValue(string key, string value, ISession session)
        {
            if (session.Keys.Contains(key))
                session.Remove(key);

            session.SetString(key, value);
        }

        private string GetValue(string key, ISession session)
        {
            if (!session.Keys.Contains(key))
                return string.Empty;

            return session.GetString(key)!;
        }

        public void RemoveValue(string key, ISession session)
        {
            if (session.Keys.Contains(key))
                session.Remove(key);
        }
        #endregion
    }
}
