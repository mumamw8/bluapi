﻿using System;
namespace Core.Dtos.AccountDtos;

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}