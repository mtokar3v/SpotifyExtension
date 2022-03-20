using SpotifyAPI.Web;
using SpotifyExtension.DataItems;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ILogger<AuthService> _logger;

        public PlayerService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        public async Task<FullTrack?> GetPlayingTrack(string access)
        {
            var client = new SpotifyClient(access);
            var currentlyPlayingRequest = new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
            var playingTrackInfo = (CurrentlyPlaying?)null;

            try
            {
                playingTrackInfo = await client.Player.GetCurrentlyPlaying(currentlyPlayingRequest);
            }
            catch(APIUnauthorizedException ex)
            {
                _logger.LogInformation(LogInfo.NewLog(ex.Message));
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(LogInfo.NewLog(ex.Message));
                throw;
            }

            return playingTrackInfo?.Item as FullTrack;
        }
    }
}
