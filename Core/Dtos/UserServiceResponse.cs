using System;
namespace Core.Dtos;

public class UserServiceResponse
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public UserReturnDto? User { get; set; }
}

