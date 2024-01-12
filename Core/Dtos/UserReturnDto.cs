using System;
namespace Core.Dtos;

public class UserReturnDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string? UserName { get; set; }
    public string? Token { get; set; }
}