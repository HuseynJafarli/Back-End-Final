﻿using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace YouPlay.Business.DTOs.UserDTOs
{
    public record UserRegisterDto(string Fullname, string Username, string Email, string Password, string ConfirmPassword, IFormFile? ProfileImage);

    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure("ConfirmPassword", "Password and ConfirmPassword do not match");
                }
            });
        }
    }

}
