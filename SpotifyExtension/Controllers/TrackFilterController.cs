using Microsoft.AspNetCore.Mvc;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackFilterController : ControllerBase
    {
        private readonly ICookieService _cookieService;
        private readonly ISessionService _sessionService;
        private readonly IAuthorizeService _authorizeService;

        private readonly ISpotifyTracksRepository _tracksRepository;
        public TrackFilterController(
            ICookieService cookieService,
            ISessionService sessionService,
            IAuthorizeService authorizeService,
            ISpotifyTracksRepository tracksRepository)
        {
            _cookieService = cookieService;
            _sessionService = sessionService;
            _authorizeService = authorizeService;
            _tracksRepository = tracksRepository;
        }

        [HttpGet]
        [Route(nameof(GetUserSavedTracks))]
        public async Task<IActionResult> GetUserSavedTracks()
        {
            var access = _sessionService.GetAccessToken(HttpContext);

            if (string.IsNullOrEmpty(access))
            {
                var success = await _authorizeService.TryReloadTokenAsync(HttpContext);
                if (!success) return Forbid();
                await GetUserSavedTracks();
            }

            var tracks = await _tracksRepository.GetMySavedFullTracks(access);

            return Ok(tracks.Select(t=>t.Name));
        }


        [HttpGet]
        [Route(nameof(GetUser))]
        public async Task<IActionResult> GetUser(string playlistId)
        {
            var access = _sessionService.GetAccessToken(HttpContext);

            if(string.IsNullOrEmpty(access))
            {
                var success = await _authorizeService.TryReloadTokenAsync(HttpContext);
                if (!success) return Forbid();
                await GetUser(playlistId);
            }

            var tracks = await _tracksRepository.GetPlaylistFullTracks(access, playlistId);

            return Ok(tracks.Select(t=>t.Name));
        }
    }
}
