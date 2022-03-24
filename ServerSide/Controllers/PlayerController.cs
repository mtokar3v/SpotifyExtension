using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var accessToken = _authorizeService.GetSpotifyAccessToken(User);
            if (string.IsNullOrEmpty(accessToken)) return Forbid();

            var client = new SpotifyClient(accessToken);
            await client.Player.SkipNext();

            return Ok();
        }

        [HttpGet]
        [Route(nameof(GetTrack))]
        public async Task<IActionResult> GetTrack()
        {
            var accessToken = _authorizeService.GetSpotifyAccessToken(User);
            if (string.IsNullOrEmpty(accessToken)) return Forbid();

  
            var track = await _playerService.GetPlayingTrack(accessToken);


            return Ok(track);
        }
    }
}
