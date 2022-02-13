namespace SpotifyExtension.DataItems.Config
{
    public class OAuthOptions
    {
        public string AuthCode { get; set; } = null!;
        public ClientOptions Client { get; set; } = null!;
    }
}
