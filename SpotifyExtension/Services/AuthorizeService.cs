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

        private readonly ISessionService _sessionService;
        private readonly ICookieService _cookieService;

        private readonly ILogger<AuthorizeService> _logger;
        
        private (string verifier, string challenge) _authCode;
        public AuthorizeService(
            IOptions<OAuthOptions> authOptions,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<AuthorizeService> logger,
            ISessionService sessionService,
            ICookieService cookieService
            )
        {
            _oAuthOptions = authOptions.Value;
            _applicationOptions = applicationOptions.Value;
            _authCode = PKCEUtil.GenerateCodes(_oAuthOptions.AuthCode);
            _logger = logger;

            _sessionService = sessionService;
            _cookieService = cookieService;
        }

        public Uri CreateAuthLink(string redirectMethod, string clientId)
        {
            var rediretUrl = $"{_applicationOptions.Uri}/api/auth/{redirectMethod}";

            var loginRequest = new LoginRequest(new Uri(rediretUrl), clientId, LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _authCode.challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative, Scopes.UserLibraryRead }
            };

            return loginRequest.ToUri();
        }

        public async Task<bool> TryGetAccessAsync(string redirectMethod, string code, HttpContext context)
        {
            var redirectUri = new Uri($"{_applicationOptions.Uri}/api/auth/{redirectMethod}");

            try
            {
                var responce = await new OAuthClient().RequestToken(
                        new PKCETokenRequest(_oAuthOptions.Client.Id, code, redirectUri, _authCode.verifier));

                _cookieService.SetRefreshToken(responce.RefreshToken, context);
                _sessionService.SetAccessToken(responce.AccessToken, context);

            }
            catch (Exception ex)
            {
                _logger.LogError(LogInfo.NewLog(ex.Message));

                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw;

                return false;
            }
            return true;
        }

        public async Task<bool> TryReloadTokenAsync(HttpContext context)
        {
            var refreshToken = _cookieService.GetRefreshToken(context);

            if (string.IsNullOrEmpty(refreshToken)) return false;

            try
            {
                var responce = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(_oAuthOptions.Client.Id, refreshToken));

                _cookieService.SetRefreshToken(responce.RefreshToken, context);
                _sessionService.SetAccessToken(responce.AccessToken, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogInfo.NewLog(ex.Message));

                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw;

                return false;
            }

            return true;
        }
    }
}
