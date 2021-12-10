using System;
using System.Collections.Generic;
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

                foreach (var employeeImage in managerImage.Employees)
                {
                    string employeeImageName = employeeImage.ImageName;
                    employeeImage.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, employeeImageName);
                }
            }
        }
    }
}