 using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public string UserId { get; set; }
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
        [ForeignKey(nameof(UserId))]               
        public ApplicationUser ApplicationUser { get; set; }
    }

}
