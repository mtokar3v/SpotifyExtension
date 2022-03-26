using SpotifyAPI.Web;

namespace SpotifyExtension.Interfaces.Repository
{
    public interface ISpotifyTracksRepository
    {
        Task<List<FullTrack>?> GetUserSavedFullTracks(string access);
        Task<List<FullTrack>?> GetPlaylistFullTracks(string access, string playlistId);
    }
}
