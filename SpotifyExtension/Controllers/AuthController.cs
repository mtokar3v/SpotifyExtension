using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyExtension.DataItems.Config;
using SpotifyExtension.DataItems.Models;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ExtendedControllerBase
    {
        OAuthOptions _authOptions;
        IAuthorizeService _authorizeService;
        public AuthController(
            IOptions<OAuthOptions> authOptions,
            IAuthorizeService authorizeService
            )
        {
            _authOptions = authOptions.Value;
            _authorizeService = authorizeService;
        }

        [HttpGet]
        [Route(nameof(GetAuthLink))]
        public IActionResult GetAuthLink([FromQuery] AuthRequestM m)
        {
            if (!ModelState.IsValid) return BadRequest();

            var authLink = _authorizeService.CreateAuthLink(nameof(GetCallback), m.ClientId);
            return Ok(authLink);
        }

        [HttpGet]
        [Route(nameof(GetCallback))]
        public async Task<IActionResult> GetCallback(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            var accessToken = await _authorizeService.GetAccessTokenAsync(nameof(GetCallback), code);
            if (string.IsNullOrEmpty(accessToken)) return Forbid();

            SetAccessToken(accessToken);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(RefreshTokens))]
        public async Task<IActionResult> RefreshTokens()
        {
            var success = await _authorizeService.RefreshSpotifyTokensAsync(User);
            if (!success) 
            {
                Logout();
                return Forbid();
            }
            return Ok();
        }
    }
}