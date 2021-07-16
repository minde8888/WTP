using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
    public class BaseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public long MobileNumber { get; set; }
        public string Email { get; set; }
        //public string ImageName { get; set; }
        //[NotMapped]
        //public IFormFile ImageFile { get; set; }
        //[NotMapped]
        //public string ImageSrc { get; set; }
        public AddressDto Address { get; set; }
    }
}
