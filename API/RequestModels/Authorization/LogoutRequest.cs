using System.ComponentModel.DataAnnotations;

namespace API.RequestModels.Authorization;

public class LogoutRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Refresh token must be at least 1 character long.")]
    public required string RefreshToken { get; set; }
}