using System;


namespace WTP.Domain.Entities
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public string ImageName { get; set; }
    }
}
