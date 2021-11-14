using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Dtos.UpdateDto
{
    public class UpdateManagerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string Surname { get; set; }
        //public string Occupation { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Email { get; set; }
        //public string Role { get; set; }
        //public string ImageName { get; set; }
        //[NotMapped]
        //public IFormFile ImageFile { get; set; }
        //[NotMapped]
        //public string ImageSrc { get; set; }
    }
}
