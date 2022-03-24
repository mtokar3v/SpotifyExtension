using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TracksController : ControllerBase
    {
        private readonly ICookieService _cookieService;
        private readonly ISessionService _sessionService;
        private readonly IAuthorizeService _authorizeService;

        private readonly ISpotifyTracksRepository _tracksRepository;
        public TracksController(
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
            var access = _authorizeService.GetSpotifyAccessToken(User);
            if (string.IsNullOrEmpty(access)) return Forbid();

            var tracks = await _tracksRepository.GetUserSavedFullTracks(access);
            return Ok(tracks?.Select(t=>t.Name));
        }


        [HttpGet]
        [Route(nameof(GetPlaylistTracks))]
        public async Task<IActionResult> GetPlaylistTracks(string playlistId)
        {
            var access = _authorizeService.GetSpotifyAccessToken(User);
            if (string.IsNullOrEmpty(access)) return Forbid();

            var tracks = await _tracksRepository.GetPlaylistFullTracks(access, playlistId);
            return Ok(tracks?.Select(t=>t.Name));
        }
    }
}
