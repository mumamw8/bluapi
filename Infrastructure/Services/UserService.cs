using System;
using System.Text;
using Core.Dtos;
using Core.Dtos.AccountDtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly IMailService _mailService;

    public UserService(UserManager<AppUser> userManager, ITokenService tokenService, IConfiguration config, IMailService mailService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _config = config;
        _mailService = mailService;
    }

    public async Task<UserServiceResponse> RegisterUserAsync(RegisterDto model)
    {
        if (model == null)
            throw new NullReferenceException("Reigster Model is null");

        if (await _userManager.Users.AnyAsync(x => x.Email == model.Email))
            throw new NullReferenceException("Email is already taken");


        var user = new AppUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{_config["ApiUrl"]}api/account/confirmemail?userid={user.Id}&token={validEmailToken}";

            await _mailService.SendEmailAsync(user.Email, "Confirm your email", $"<h1>Welcome to Duell</h1>" +
                $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

            return new UserServiceResponse
            {
                Message = "User created successfully!",
                IsSuccess = true,
            };
        }

        return new UserServiceResponse
        {
            Message = "User did not create",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
        };
    }

    public async Task<UserServiceResponse> LoginUserAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return new UserServiceResponse
            {
                Message = "There is no user with that Email address",
                IsSuccess = false,
            };


        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
            return new UserServiceResponse
            {
                Message = "Invalid password",
                IsSuccess = false,
            };
        if (!user.EmailConfirmed)
            return new UserServiceResponse
            {
                Message = "Unconfirmed email",
                IsSuccess = false
            };

        var rcUser = CreateUserObject(user);

        return new UserServiceResponse
        {
            Message = "Login Successful",
            IsSuccess = true,
            User = rcUser
        };
    }

    public async Task<UserServiceResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserServiceResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };

        var decodedToken = WebEncoders.Base64UrlDecode(token);
        string normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);

        if (result.Succeeded)
            return new UserServiceResponse
            {
                Message = "Email confirmed successfully!",
                IsSuccess = true,
            };

        return new UserServiceResponse
        {
            IsSuccess = false,
            Message = "Email did not confirm",
            Errors = result.Errors.Select(e => e.Description)
        };
    }

    public async Task<UserServiceResponse> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new UserServiceResponse
            {
                IsSuccess = false,
                Message = "No user associated with email",
            };
        if (!user.EmailConfirmed)
            return new UserServiceResponse
            {
                Message = "Unconfirmed email",
                IsSuccess = false
            };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        string url = $"{_config["ApiUrl"]}ResetPassword?email={email}&token={validToken}";

        await _mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
            $"<p>To reset your password <a href='{url}'>Click here</a></p>");

        return new UserServiceResponse
        {
            IsSuccess = true,
            Message = "Reset password URL has been sent to the email successfully!"
        };
    }

    public async Task<UserServiceResponse> ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new UserServiceResponse
            {
                IsSuccess = false,
                Message = "No user associated with email",
            };
        if (model.NewPassword != model.ConfirmPassword)
            return new UserServiceResponse
            {
                IsSuccess = false,
                Message = "Password doesn't match its confirmation",
            };

        var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
        string normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

        if (result.Succeeded)
            return new UserServiceResponse
            {
                Message = "Password has been reset successfully!",
                IsSuccess = true,
            };

        return new UserServiceResponse
        {
            Message = "Something went wrong",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description),
        };
    }

    private UserReturnDto CreateUserObject(AppUser user)
    {
        return new UserReturnDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePictureUrl,
            Bio = user.Bio,
            Token = _tokenService.CreateToken(user),
            UserName = user.UserName
        };
    }
}

