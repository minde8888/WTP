using System;

namespace WTP.Domain.Entities
{
    public class Rent
    {
        public Guid RentId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Project Project { get; set; }
    }
}
