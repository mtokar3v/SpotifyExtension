using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyExtension.DataItems.Config;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly OAuthOptions _authOptions;
        private (string verifier, string challenge) _authCode;
        public AuthController(
            IOptions<OAuthOptions> authOptions,
            IOptions<ApplicationOptions> applicationOptions
            )
        {
            _authOptions = authOptions.Value;
            _applicationOptions = applicationOptions.Value;

            _authCode = PKCEUtil.GenerateCodes(_authOptions.AuthCode);
        }

        [HttpGet]
        [Route(nameof(GetAuthLink))]
        public IActionResult GetAuthLink()
        {
            var url = $"{_applicationOptions.Uri}/api/auth/{nameof(GetCallback)}";
            var loginRequest = new LoginRequest(
              new Uri(url),
              _authOptions.Client.Id,
              LoginRequest.ResponseType.Code
            )
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _authCode.challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            return Ok(new { loginLink = uri});
        }

        [HttpGet]
        [Route(nameof(GetCallback))]
        public async Task<IActionResult> GetCallback(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            var initialResponse = await new OAuthClient().RequestToken(
                    new PKCETokenRequest(_authOptions.Client.Id, code, new Uri(_applicationOptions.Uri), _authCode.verifier)
              );

            var spotify = new SpotifyClient(initialResponse.AccessToken);
            var albums = spotify.Albums;
            return Ok(albums);
        }
    }
}
