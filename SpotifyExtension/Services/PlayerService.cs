using SpotifyAPI.Web;
using SpotifyExtension.DataItems;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IAuthorizeService _authorizeService;

        public PlayerService(
            ILogger<AuthService> logger,
            IAuthorizeService authorizeService)
        {
            _logger = logger;
            _authorizeService = authorizeService;
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
                _logger.LogInformation(LogInfo.NewLog(ex.Message, ex.StackTrace));
            }
            catch(Exception ex)
            {
                _logger.LogWarning(LogInfo.NewLog(ex.Message));

                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw;
            }

            return playingTrackInfo?.Item as FullTrack;
        }
    }
}
