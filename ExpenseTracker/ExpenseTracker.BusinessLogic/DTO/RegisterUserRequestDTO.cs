using System.ComponentModel.DataAnnotations;

public class RegisterUserRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }

    [Required]
    public decimal CurrentSalary { get; set; }

}
