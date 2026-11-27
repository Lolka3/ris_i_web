using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public sealed class LoginRequest
{
    [Required]
    [StringLength(100)]
    public string Login { get; init; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Password { get; init; } = string.Empty;
}
