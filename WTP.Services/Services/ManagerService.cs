using System;
using System.Collections.Generic;
using System.Linq;
using WTP.Domain.Dtos;

namespace WTP.Services.Services
{
    public class ManagerService
    {
        public void GetImagesAsync(List<ManagerDto> managerDto, string ImageSrc)
        {
            
            foreach (var managerImage in managerDto)
            {
                string managerImageName = managerImage.ImageName;
                managerImage.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, managerImageName);

                var employees = managerImage.Employees.Where(i => i.IsDeleted == false);
                foreach (var employeeImage in employees)
                {
                    string employeeImageName = employeeImage.ImageName;
                    employeeImage.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, employeeImageName);
                }
                var hidenEmployees = managerImage.Employees.Where(i => i.IsDeleted == true);
                foreach (var hide in hidenEmployees)
                {
                    hide.Email = null;
                    hide.ImageName = null;
                    hide.Address = null;
                    hide.ImageSrc = null;
                    hide.ManagerId = null;
                    hide.Name = null;
                    hide.Occupation = null;
                    hide.PhoneNumber = null;
                    hide.Posts = null;
                    hide.Role = null;
                    hide.Surname = null;
                    hide.IsActive = false;
                    hide.Id = Guid.Empty;
                }
            }
        }
    }
}