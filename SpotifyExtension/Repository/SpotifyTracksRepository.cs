using SpotifyAPI.Web;
using SpotifyExtension.Interfaces.Repository;

namespace SpotifyExtension.Repositoty
{
    public class SpotifyTracksRepository : ISpotifyTracksRepository
    {
        public async Task<List<FullTrack>?> GetUserSavedFullTracks(string access)
        {
            var client = new SpotifyClient(access);
            var savedTracks = new List<FullTrack>();
            var offset = -1;
            var next = false;
            try
            {
                do
                {
                    var paging = await client.Library.GetTracks(new LibraryTracksRequest { Offset = ++offset });
                    savedTracks.AddRange(paging.Items?.Select(t => t.Track)!);
                    next = savedTracks.Count() < paging.Total;
                }
                while (next);

                return savedTracks;
            }
            catch(Exception ex)
            {
                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw new Exception(ex.Message);

                return null;
            }
        }

        public async Task<List<FullTrack>?> GetPlaylistFullTracks(string access, string playlistId)
        {
            var client = new SpotifyClient(access);
            var playlistTracks = new List<FullTrack>();

            var playlist = (FullPlaylist?)null;

            try
            {
                playlist = await client.Playlists.Get(playlistId);
                playlistTracks.AddRange(playlist?.Tracks?.Items?.Select(t => (FullTrack)t.Track)!);

                return playlistTracks;
            }
            catch(Exception ex)
            {
                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw new Exception(ex.Message);

                return null;
            }
        }
    }
}
