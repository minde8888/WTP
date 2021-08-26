﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Api.Configuration
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Linked to the AspNet Identity User Id
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public string JwtId { get; set; } // Map the token with jwtId
        public bool IsUsed { get; set; } // if its used we dont want generate a new Jwt token with the same refresh token
        public bool IsRevoked { get; set; } // if it has been revoke for security reasons
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; } // Refresh token is long lived it could last for months.

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}