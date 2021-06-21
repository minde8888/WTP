using System;

namespace WTP.Domain.Dtos
{
    public class AddressDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public long Phone { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
