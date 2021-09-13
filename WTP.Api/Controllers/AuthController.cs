﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Api.Configuration.Requests;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Dtos.Responses;
using WTP.Domain.Entities.Auth;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationsParams,
            IUserRepository userRepository,
            AuthService authService)

        {
            _userManager = userManager;
            _tokenValidationParams = tokenValidationsParams;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Success = false
                    });
                }

                var newUser = new ApplicationUser()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    ManagerId = user.ManagerId,
                    Roles = user.Roles
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    var id = newUser.Id;
                    await _userManager.AddToRoleAsync(newUser, user.Roles.ToString());
                    await _userRepository.AddUser(user, id);

                    return Ok(await _authService.GenerateJwtToken(newUser));
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>()
                    {
                    "Invalig payloade"
                    },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }
                try
                {
                    var result = await _authService.GetUserInfo(existingUser);
                    return Ok(result);
                }
                catch (Exception)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Error login !!!"
                            },
                        Success = false
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string rawUserId = HttpContext.User.FindFirstValue("id");

                if (!Guid.TryParse(rawUserId, out Guid userId))
                {
                    return Unauthorized();
                }

                bool result = await _userRepository.removeRefreshToken(rawUserId);
                if (result)
                    return NoContent();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Can not to remove RefreshToken !!!" });
            }
            return BadRequest(new { message = "Problems with Logout !!!" });
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                bool emailHelper = await _authService.SendEmailPasswordReset(model, Request.Headers["origin"], token);
                if (emailHelper)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error PasswordReset !!!" });
            }
            return BadRequest(new { message = "Problems with ForgotPasswordToken !!!" });
        }

        [AllowAnonymous]// nebaigtas reikia fronto
        [HttpGet("NewPassword")]
        public async Task<ActionResult> NewPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                bool result = await _authService.NewPassword(model);
                if (result)
                {
                    return Ok(new { message = "Password reset successful, you can now login" });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error NewPassword !!!" });
            }
            return BadRequest(new { message = "Error ResetPassword !!!" });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequests tokenRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
                    var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                    var res = await _authService.VerifyToken(tokenRequest, principal, validatedToken);
                    if (res == null)
                    {
                        return BadRequest(new RegistrationResponse()
                        {
                            Errors = new List<string>() {
                    "Invalid tokens"
                    },
                            Success = false
                        });
                    }

                    return Ok(res);
                }
                catch (Exception)
                {
                    return BadRequest(new { message = "ValidateToken or VerifyToken Error !!!" });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                "Invalid payload"
            },
                Success = false
            });
        }
    }
}