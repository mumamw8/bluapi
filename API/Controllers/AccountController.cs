using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Dtos.AccountDtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;

    public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IUserService userService, IMailService mailService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userService = userService;
        _mailService = mailService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserServiceResponse>> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Some properties are not valid");

        var result = await _userService.LoginUserAsync(model);

        if (result.IsSuccess)
        {
            //await _mailService.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpPost("Register")]
    public async Task<ActionResult<UserServiceResponse>> Register(RegisterDto model)
    {
        // if (ModelState.IsValid)
        //     return BadRequest("Some properties are not valid"); // Status code: 400

        var result = await _userService.RegisterUserAsync(model);

        if (result.IsSuccess)
            return Ok(result); // Status Code: 200 

        return BadRequest(result);
    }

    // /api/account/confirmemail?userid&token
    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return NotFound();

        var result = await _userService.ConfirmEmailAsync(userId, token);

        if (result.IsSuccess)
            return Ok(result); // route to email confirmed page

        return BadRequest(result);
    }

    // /api/account
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserReturnDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        return CreateUserObject(user);
    }

    // forgot password api/account/forgotpassword?email=example@email.com
    [HttpPost("ForgotPassword")]
    public async Task<ActionResult<UserServiceResponse>> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
            return NotFound();

        var result = await _userService.ForgotPasswordAsync(email);

        if (result.IsSuccess)
            return Ok(result); // 200

        return BadRequest(result); // 400
    }

    // reset password
    [HttpPost("ResetPassword")]
    public async Task<ActionResult<UserServiceResponse>> ResetPassword([FromForm] ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Some properties are not valid");

        var result = await _userService.ResetPasswordAsync(model);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
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