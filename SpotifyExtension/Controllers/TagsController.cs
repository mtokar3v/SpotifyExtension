using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ICookieService _cookieService;
        private readonly ISessionService _sessionService;
        private readonly IAuthorizeService _authorizeService;
        public TagsController(
            ICookieService cookieService,
            ISessionService sessionService,
            IAuthorizeService authorizeService)
        {
            _cookieService = cookieService;
            _sessionService = sessionService;
            _authorizeService = authorizeService;
        }

        [HttpGet]
        [Route(nameof(GetUser))]
        public async Task<IActionResult> GetUser()
        {
            var access = _sessionService.GetAccessToken(HttpContext);
            if(string.IsNullOrEmpty(access))
            {
                var success = await _authorizeService.TryReloadTokenAsync(HttpContext);
                if (!success) return Forbid();
                await GetUser();
            }

            var user = await new SpotifyClient(access).UserProfile.Current();

            return Ok(user);
        }
    }
}
