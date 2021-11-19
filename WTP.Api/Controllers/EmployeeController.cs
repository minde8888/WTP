using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    [Route("v1/api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee>
    {
        private readonly IBaseRepository<Employee> _employee;
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;

        public EmployeeController(IEmployeesRepository employeeRepository,
            IBaseRepository<Employee> employee,
            IWebHostEnvironment hostEnvironment,
             ImagesService imagesService)
            : base(employee, hostEnvironment, imagesService)
        {
            _employee = employee;
            _employeeRepository = employeeRepository;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployee()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _employee.GetItemAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database -> NewItem");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult AddNewEmployee([FromForm] EmployeeDto employee)
        {
            try
            {
                if (!String.IsNullOrEmpty(employee.ImageName))
                {
                    string path = _hostEnvironment.ContentRootPath;
                    //var imageName = _imagesService.SaveImage(employee.ImageFile);
                }
                string UserId = HttpContext.User.FindFirstValue("id");
                _employeeRepository.AddEmployee(UserId, employee);

                return CreatedAtAction("Get", new { employee.Id }, employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }
    }
}