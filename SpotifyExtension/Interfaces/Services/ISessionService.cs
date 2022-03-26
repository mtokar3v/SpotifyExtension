namespace SpotifyExtension.Interfaces.Services
{
    public interface ISessionService
    {
        void SetAccessToken(string token, ISession session);

        string GetAccessToken(ISession session);

        void RemoveAccessToken(ISession session);
    }
}
