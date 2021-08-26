﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using WTP.Api.Configuration;

namespace WTP.Domain.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public Employee Employees { get; set; }
        public Manager Manager { get; set; }
        public string? ManagerId { get; set; }
        public string Roles { get; set; }
        public bool AcceptTerms { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}