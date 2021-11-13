﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WTP.Api.Configuration;
using WTP.Data.Context;
using WTP.Data.Helpers;
using WTP.Data.Interfaces;
using WTP.Domain.Entities.Auth;
using WTP.Services.Services.Dtos;

namespace WTP.Services.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailPassword _mail;
        private readonly JwtConfig _jwtConfig;

        public AuthService(IMapper mapper,
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IEmailPassword mail)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _mail = mail;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task<List<EmployeeInformationDto>> GetUserInfo(ApplicationUser user, AuthResult token)
        {
            var role = await _userManager.GetRolesAsync(user);

            foreach (var item in role)
            {
                switch (item)
                {
                    case "Manager":

                        var manager = await _context.Manager
                        .Include(employee => employee.Employees)
                        .Include(post => post.Posts)
                        .Where(u => u.UserId == new Guid(user.Id.ToString()))
                        .ToListAsync();
                        var managerDto = _mapper.Map<List<EmployeeInformationDto>>(manager);
                        managerDto.Where(t => t.Token == null).ToList().ForEach(t => t.Token = token.Token);
                        var managerActive = managerDto.Any(i => i.IsActive == true);

                        if (managerDto != null && managerActive)
                            return managerDto;

                        throw new Exception("User does not exist");

                    case "Employee":
                        var employee = await _context.Employee
                            .Include(post => post.Posts)
                            .Where(u => u.UserId == new Guid(user.Id.ToString()))
                            .ToListAsync();
                        var employeeDto = _mapper.Map<List<EmployeeInformationDto>>(employee);
                        employeeDto.Where(t => t.Token == null).ToList().ForEach(t => t.Token = token.Token);
                        var employeeActive = employeeDto.Any(i => i.IsActive == true);

                        if (employeeDto != null && employeeActive)
                            return employeeDto;

                        throw new Exception("User does not exist");

                    default:
                        throw new Exception();
                }
            }

            throw new Exception("Can not get data from DB. Role dose not exisit");
        }

         public async Task<bool> NewPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }
            else if (user.ResetToken != model.Token
                && user.ResetTokenExpires < DateTime.UtcNow) 
            {
                return false;
            }
            else
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetPassResult.Succeeded)
                {
                    user.PasswordReset = DateTime.UtcNow;
                    user.ResetToken = null;
                    user.ResetTokenExpires = null;
                    await _userManager.UpdateAsync(user);

                    return true;
                }
            }
            throw new Exception();
        }

        public async Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (token != null)
            {
                user.ResetToken = token;
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new Exception();
            }
            var link = $"{origin}/api/Auth/NewPassword?token={token}&email={user.Email}";
            bool sendEmail = _mail.SendEmailPasswordReset(model, link);
            return sendEmail;
        }

        public RefreshToken GetrefreshToken(SecurityToken token, string rand, ApplicationUser user)
        {
            RefreshToken refreshToken = new()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = rand
            };
            return refreshToken;
        }

        public async Task<AuthResult> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("guid", user.Id.ToString()),
                }.Union(roleClaims)),
                Expires = DateTime.UtcNow.AddSeconds(30), // 5-10
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            string rand = RandomString.RandString(25) + Guid.NewGuid();
            var refreshToken = GetrefreshToken(token, rand, user);

            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();

            // This validation function will make sure that the token meets the validation parameters
            // and its an actual jwt token not just a random string

            // Now we need to check if the token has a valid security algorithm
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return null;
                }
            }

            // Will get the time stamp in unix time
            var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            // we convert the expiry date from seconds to the date
            var expDate = UnixTimeStamp.UnixTimeStampToDateTime(utcExpiryDate);

            if (expDate > DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                    Success = false
                };
            }

            // Check the token we got if its saved in the db
            var storedRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "refresh token doesnt exist" },
                    Success = false
                };
            }

            // Check the date of the saved token if it has expired
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has expired, user needs to relogin" },
                    Success = false
                };
            }

            // check if the refresh token has been used
            if (storedRefreshToken.IsUsed)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has been used" },
                    Success = false
                };
            }

            // Check if the token is revoked
            if (storedRefreshToken.IsRevoked)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has been revoked" },
                    Success = false
                };
            }

            // we are getting here the jwt token id
            var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            // check the id that the recieved token has against the id saved in the db
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "the token doenst mateched the saved token" },
                    Success = false
                };
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshToken.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
            return await GenerateJwtToken(dbUser);
        }

        public string StringRandom()
        {
            string unique = RandomString.RandString(36);

            var existName = _userManager.FindByNameAsync(unique);
            if (existName.Result != null)
            {
                StringRandom();
            }
            else
            {
                return unique;
            }
            throw new Exception();
        }
    }
}