using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.AccountDtos;

public class RegisterDto
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Password requires a number, lowercase letter, uppercase letter, and must be at least 6 characters long")]
    public string? Password { get; set; }
}

