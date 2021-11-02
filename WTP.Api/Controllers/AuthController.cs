using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
                var exist = _userManager.Users.Any(u => u.PhoneNumber == user.PhoneNumber || u.Email == user.Email);             

                if (exist)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email or phone number is already in use !!!"
                        },
                        Success = false
                    });
                }

                var newUser = new ApplicationUser()
                {
                    Roles = user.Roles,
                    Email = user.Email,
                    UserName = _authService.StringRandom(),
                    PhoneNumber = user.PhoneNumber
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    try
                    {
                        await _userManager.AddToRoleAsync(newUser, user.Roles.ToString());
                        user.UserId = newUser.Id;
                        await _userRepository.AddUser(user);

                        //return Ok(await _authService.GenerateJwtToken(newUser));
                        return Ok("Success");
                    }
                    catch (Exception)
                    {
                        return BadRequest(new RegistrationResponse()
                        {
                            Errors = new List<string>() {
                                "Error to add user in the DB !!!"
                            },
                            Success = false
                        });
                    }
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
                    var token = await _authService.GenerateJwtToken(existingUser);
                    var result = await _authService.GetUserInfo(existingUser, token);
                    var json = JsonSerializer.Serialize(result);
                    return Ok(json);
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

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
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
                return BadRequest(new { message = "The link you followed has expired !!!" });
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