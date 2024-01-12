using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.AccountDtos;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Password requires a number, lowercase letter, uppercase letter, and must be at least 6 characters long")]
    public string NewPassword { get; set; }
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Password requires a number, lowercase letter, uppercase letter, and must be at least 6 characters long")]
    public string ConfirmPassword { get; set; }
}