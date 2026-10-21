
using System.ComponentModel.DataAnnotations;

namespace CollabWorkspace.Api.DTOs.Auth;

public sealed class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
