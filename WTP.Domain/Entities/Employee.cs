using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Guid? ManagerId { get; set; }
    }
}
