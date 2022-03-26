using System.ComponentModel.DataAnnotations;

namespace SpotifyExtension.DataItems.Models
{
    public class AuthRequestM
    {
        [Required]
        public string ClientId { get; set; } = null!;
    }
}
