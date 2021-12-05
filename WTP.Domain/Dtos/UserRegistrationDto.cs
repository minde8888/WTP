﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WTP.Domain.Dtos.Requests
{
    public class UserRegistrationDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public AddressDto Address { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}