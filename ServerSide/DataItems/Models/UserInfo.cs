using SpotifyExtension.DataItems.Enums;

namespace SpotifyExtension.DataItems.Models
{
    public class UserInfo
    {
        public string? Name { get; set; }
        public UserState State { get; set; }
        public AvalableModes Modes { get; set; }
    }
}
