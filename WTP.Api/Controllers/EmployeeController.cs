﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee>
    {
        private readonly IBaseRepository<Employee> _employee;
        private readonly IEmployeesRepository _employeeServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(IEmployeesRepository employeeServices,
            IBaseRepository<Employee> employee,
            IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager)
            : base(employee, hostEnvironment)
        {
            _employee = employee;
            _employeeServices = employeeServices;
            _userManager = userManager;
        }

        [HttpGet]
        //[Authorize(Policy = "Employee")]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Manager")]
        public IActionResult AddNewEmployee(Employee employee)
        {
            try
            {
                string UserId = HttpContext.User.FindFirstValue("id");
                _employeeServices.AddEmployee(UserId, employee);
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