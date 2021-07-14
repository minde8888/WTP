using System;


namespace WTP.Domain.Entities
{
    public class Address : BaseEntyti
    {
        public string Street { get; set; }
        public string City { get; set; }
        public long Phone { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public Guid? ManagerId { get; set; }
        public Manager Manager { get; set; }
        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
