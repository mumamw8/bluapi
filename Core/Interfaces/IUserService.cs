using System;
using Core.Dtos;
using Core.Dtos.AccountDtos;

namespace Core.Interfaces;

public interface IUserService
{
    Task<UserServiceResponse> RegisterUserAsync(RegisterDto model);
    Task<UserServiceResponse> LoginUserAsync(LoginDto model);
    Task<UserServiceResponse> ConfirmEmailAsync(string userId, string token);
    Task<UserServiceResponse> ForgotPasswordAsync(string email);
    Task<UserServiceResponse> ResetPasswordAsync(ResetPasswordDto model);
}

