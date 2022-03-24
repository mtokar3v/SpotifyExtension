using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ExtendedControllerBase
    {
        IUserStatisticsRepository _userStatistics;
        IAuthorizeService _authorizeService;
        public UsersController(
            IAuthorizeService authorizeService,
            IUserStatisticsRepository userStatistics)
        {
            _userStatistics = userStatistics;
            _authorizeService = authorizeService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetUserInfo))]
        public async Task<IActionResult> GetUserInfo()
        {
            var accessToken = _authorizeService.GetSpotifyAccessToken(User);
            var userInfo = await _userStatistics.GetUserInfoAsync(accessToken);
            return Ok(userInfo);
        }
    }
}
