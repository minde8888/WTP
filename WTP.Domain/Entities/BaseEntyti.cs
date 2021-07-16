 using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Entities
{
    public class BaseEntyti
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public long MobileNumber { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }
        public Address Address { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //public Guid? UserCreated { get; set; } 
        //public Guid? UserUpdated { get; set; }
    }
}
