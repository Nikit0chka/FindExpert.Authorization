using System.ComponentModel.DataAnnotations;

namespace API.RequestModels.Authorization;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Email must be at least 1 character long.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Password must be at least 1 character long.")]
    public required string Password { get; set; }
}