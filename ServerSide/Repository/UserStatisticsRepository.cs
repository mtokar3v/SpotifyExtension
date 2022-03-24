using SpotifyAPI.Web;
using SpotifyExtension.DataItems;
using SpotifyExtension.DataItems.Enums;
using SpotifyExtension.DataItems.Models;
using SpotifyExtension.Interfaces.Repository;

namespace SpotifyExtension.Repository
{
    public class UserStatisticsRepository : IUserStatisticsRepository
    {
        private readonly ILogger<UserStatisticsRepository> _logger;
        public UserStatisticsRepository(ILogger<UserStatisticsRepository> logger)
        {
            _logger = logger;
        }

        public async Task<UserInfo> GetUserInfoAsync(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                var privateUser = (PrivateUser?)null;
                try
                {
                    privateUser = await new SpotifyClient(accessToken).UserProfile.Current();

                    return new UserInfo()
                    {
                        Name = privateUser.DisplayName,
                        State = UserState.Authorized
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(LogInfo.NewLog(ex.Message));
                }
            }

            return new UserInfo()
            {
                State = UserState.Unauthorized
            };
        }
    }
}
