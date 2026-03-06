using System.ComponentModel.DataAnnotations;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    public string DeviceId { get; set; }

    public string DeviceName { get; set; }
}