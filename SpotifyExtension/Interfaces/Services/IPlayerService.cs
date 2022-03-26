using SpotifyAPI.Web;

namespace SpotifyExtension.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<FullTrack?> GetPlayingTrack(string access);
    }
}
