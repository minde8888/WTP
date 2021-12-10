using System;
using System.Collections.Generic;
using WTP.Domain.Dtos;

namespace WTP.Services.Services
{
    public class EmployeeService
    {
        public void GetImagesAsync(List<EmployeeDto> employeeDto, string ImageSrc)
        {

            foreach (var managerImage in employeeDto)
            {
                string managerImageName = managerImage.ImageName;
                managerImage.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, managerImageName);
            }
            
        }

    }
}
