using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ICookieService _cookieService;
        private readonly ISessionService _sessionService;
        private readonly IAuthorizeService _authorizeService;
        private readonly IPlayerService _playerService;

        private readonly ISpotifyTracksRepository _tracksRepository;
        public PlayerController(
            ICookieService cookieService,
            ISessionService sessionService,
            IAuthorizeService authorizeService,
            ISpotifyTracksRepository tracksRepository,
            IPlayerService playerService)
        {
            _cookieService = cookieService;
            _sessionService = sessionService;
            _authorizeService = authorizeService;
            _tracksRepository = tracksRepository;
            _playerService = playerService;
        }

        [HttpGet]
        [Route(nameof(Skip))]
        public async Task<IActionResult> Skip()
        {
            var access = _sessionService.GetAccessToken(HttpContext);

            if (string.IsNullOrEmpty(access))
            {
                var success = await _authorizeService.TryRefreshTokenAsync(HttpContext);
                if (!success) return Forbid();
                await Skip();
            }

            var client = new SpotifyClient(access);
            await client.Player.SkipNext();

            return Ok();
        }

        [HttpGet]
        [Route(nameof(GetTrack))]
        public async Task<IActionResult> GetTrack()
        {
            var access = _sessionService.GetAccessToken(HttpContext);

            if (string.IsNullOrEmpty(access))
            {
                var success = await _authorizeService.TryRefreshTokenAsync(HttpContext);
                if (!success) return Forbid();
                await GetTrack();
            }

            try
            {
                var track = await _playerService.GetPlayingTrack(access);
            }
            catch(APIUnauthorizedException ex)
            {
                return Forbid(ex.Message);
            }

            return Ok();
        }
    }
}
