﻿using Microsoft.AspNetCore.Identity;
using System;

namespace WTP.Domain.Entities.Roles
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
