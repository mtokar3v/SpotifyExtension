using SpotifyExtension.DataItems.Models;

namespace SpotifyExtension.Interfaces.Repository
{
    public interface IUserStatisticsRepository
    {
        Task<UserInfo> GetUserInfoAsync(string accessToken);
    }
}
