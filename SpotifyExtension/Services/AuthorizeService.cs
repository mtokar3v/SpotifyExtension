using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyExtension.DataItems;
using SpotifyExtension.DataItems.Config;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly OAuthOptions _oAuthOptions;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AuthorizeService> _logger;
        private (string verifier, string challenge) _authCode;
        public AuthorizeService(
            IOptions<OAuthOptions> authOptions,
            IOptions<ApplicationOptions> applicationOptions,
            IWebHostEnvironment environment,
            ILogger<AuthorizeService> logger
            )
        {
            _oAuthOptions = authOptions.Value;
            _applicationOptions = applicationOptions.Value;
            _environment = environment;
            _authCode = PKCEUtil.GenerateCodes(_oAuthOptions.AuthCode);
            _logger = logger;
        }

        public Uri CreateAuthLink(string redirectMethod, string clientId)
        {
            var rediretUrl = $"{_applicationOptions.Uri}/api/auth/{redirectMethod}";

            var loginRequest = new LoginRequest(new Uri(rediretUrl), clientId, LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _authCode.challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };

            return loginRequest.ToUri();
        }

        public async Task<PKCETokenResponse> GetPkceToken(string redirectMethod, string code)
        {
            var redirectUri = new Uri($"{_applicationOptions.Uri}/api/auth/{redirectMethod}");

            try
            {
                return await new OAuthClient().RequestToken(
                        new PKCETokenRequest(_oAuthOptions.Client.Id, code, redirectUri, _authCode.verifier));
            }
            catch (Exception ex)
            {
                if (_environment.EnvironmentName == "Development")
                    throw;

                _logger.LogError(LogInfo.NewLog(ex.Message));
            }
            return null!;
        }

    }
}
