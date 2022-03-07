using SpotifyAPI.Web;

namespace SpotifyExtension.Interfaces.Repository
{
    public interface ISpotifyTracksRepository
    {
        Task<List<FullTrack>> GetMySavedFullTracks(string access);
        Task<List<FullTrack>> GetPlaylistFullTracks(string access, string playlistId);
    }
}
