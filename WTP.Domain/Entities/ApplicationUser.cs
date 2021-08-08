using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Employee Employees { get; set; }
        public Manager Manager { get; set; }
        public string? ManagerId { get; set; }
        public string Roles { get; set; }
    }
}
