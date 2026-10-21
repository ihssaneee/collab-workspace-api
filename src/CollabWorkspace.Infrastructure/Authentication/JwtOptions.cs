using System.ComponentModel.DataAnnotations;

namespace CollabWorkspace.Infrastructure.Authentication;

public sealed class JwtOptions
{
    [Required]
    [MinLength(32)]
    public string Secret { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ExpirationMinutes { get; set; }
}