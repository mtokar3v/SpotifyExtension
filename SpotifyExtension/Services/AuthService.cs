using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Web;
using SpotifyExtension.DataItems;
using SpotifyExtension.DataItems.Config;
using SpotifyExtension.DataItems.Models;
using SpotifyExtension.DataItems.Options;
using SpotifyExtension.DataItems.Other;
using SpotifyExtension.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpotifyExtension.Services
{
    public class AuthService : IAuthorizeService
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly OAuthOptions _oAuthOptions;

        private readonly ISessionService _sessionService;
        private readonly ICookieService _cookieService;

        private readonly ILogger<AuthService> _logger;
        
        private (string verifier, string challenge) _authCode;
        public AuthService(
            IOptions<OAuthOptions> authOptions,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<AuthService> logger,
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

        public string? GetSpotifyAccessToken(ClaimsPrincipal User) => User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.AccessToken)?.Value;

        public string? GetSpotifyRefreshToken(ClaimsPrincipal User) => User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RefreshToken)?.Value;

        public Uri CreateAuthLink(string redirectMethod, string clientId)
        {
            var rediretUrl = $"{_applicationOptions.Uri}/api/auth/{redirectMethod}";

            var loginRequest = new LoginRequest(new Uri(rediretUrl), clientId, LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _authCode.challenge,
                Scope = new[] 
                { 
                    Scopes.PlaylistReadPrivate,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.UserLibraryRead,
                    Scopes.AppRemoteControl,
                    Scopes.Streaming,
                    Scopes.UserReadCurrentlyPlaying
                }
            };

            return loginRequest.ToUri();
        }

        public async Task<string?> GetAccessTokenAsync(string redirectMethod, string code)
        {
            var redirectUri = new Uri($"{_applicationOptions.Uri}/api/auth/{redirectMethod}");

            try
            {
                var responce = await new OAuthClient().RequestToken(new PKCETokenRequest(_oAuthOptions.Client.Id, code, redirectUri, _authCode.verifier));

                var identity = CreateIdentity(new IdentityM 
                { 
                    RefreshToken = responce.RefreshToken,
                    AccessToken = responce.AccessToken
                });

                return CreateEncodedJwt(identity);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogInfo.NewLog(ex.Message));
                return null;
            }
        }

        public async Task<bool> RefreshSpotifyTokensAsync(ClaimsPrincipal user)
        {
            try
            {
                var refreshToken = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RefreshToken)?.Value;

                if (string.IsNullOrEmpty(refreshToken)) throw new NullReferenceException("Refresh Token is null or empty");

                var responce = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(_oAuthOptions.Client.Id, refreshToken!));
                var identity = user.Identities.FirstOrDefault(i => i.Claims.Any(c => c.Issuer == JwtAuthOptions.Issuer));

                if (identity == null) throw new NullReferenceException($"User {user.Identity?.Name} doesn't has a identity");
                
                RemoveClaims(new List<string> { CustomClaimTypes.RefreshToken, CustomClaimTypes.AccessToken }, identity, user);
                identity.AddClaims(new List<Claim>
                {
                    new Claim(CustomClaimTypes.RefreshToken, responce.RefreshToken),
                    new Claim(CustomClaimTypes.AccessToken, responce.AccessToken)
                });

                return true;
            }
            catch (APIException ex)
            {
                _logger.LogError(LogInfo.NewLog(ex.Message));

                var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (stage == "Development")
                    throw;
                return false;

            }
        }

        #region Private methods
        private ClaimsIdentity CreateIdentity(IdentityM m)
        {
            if (string.IsNullOrEmpty(m.RefreshToken)) throw new Exception($"Refresh token is null or empty");
            if (string.IsNullOrEmpty(m.AccessToken)) throw new Exception($"Access token is null or empty");

            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.RefreshToken, m.RefreshToken!),
                new Claim(CustomClaimTypes.AccessToken, m.AccessToken!),
            };

            return new ClaimsIdentity(claims, JwtAuthOptions.AuthenticationType, "123", "321");
        }

        private string CreateEncodedJwt(ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.CreateJwtSecurityToken(
                        JwtAuthOptions.Issuer,
                        JwtAuthOptions.Audience,
                        identity,
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddMinutes(JwtAuthOptions.Lifetime),
                        DateTime.UtcNow,
                        new SigningCredentials(JwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return handler.WriteToken(jwt);
        }

        public void RemoveClaims(IEnumerable<string> types, ClaimsIdentity identity, ClaimsPrincipal user)
        {
            foreach (var type in types)
            {
                var claim = user.Claims.First(c => c.Type == type);
                identity.RemoveClaim(claim);
            }
        }

        #endregion
    }
}
