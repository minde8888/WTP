using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Entities
{
    public class Person : BaseEntyti
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public int MobileNumber { get; set; }
        public string Email { get; set; }
    }
}
