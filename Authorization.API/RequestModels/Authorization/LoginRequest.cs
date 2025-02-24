using System.ComponentModel.DataAnnotations;

namespace Authorization.API.RequestModels.Authorization;

public class LoginRequest
{
    [Required(ErrorMessage = "Login is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Login must be at least 1 character long.")]
    public required string Login { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Password must be at least 1 character long.")]
    public required string Password { get; set; }
}