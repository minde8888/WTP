using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee>
    {
        private readonly IBaseRepository<Employee> _employee;
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;
        private readonly IMapper _mapper;
        private readonly EmployeeService _employeeService;

        public EmployeeController(IEmployeesRepository employeeRepository,
            IBaseRepository<Employee> employee,
            IWebHostEnvironment hostEnvironment,
             ImagesService imagesService,
             IMapper mapper,
             EmployeeService employeeService)
            : base(employee)
        {
            _employee = employee;
            _employeeRepository = employeeRepository;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet("id")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<EmployeeDto>>> Get(String id)
        {
            try
            {
                var userId = new Guid(id);
                if (userId == Guid.Empty)
                    return BadRequest();

                var result = await _employeeRepository.GetItemIdAsync(userId);
                if (result == null)
                    return NotFound();

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var employeeDto = _mapper.Map<List<EmployeeDto>>(result);
                 _employeeService.GetImagesAsync(employeeDto, ImageSrc);

                return Ok(employeeDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
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
        public IActionResult AddNewEmployee([FromForm] RequestEmployeeDto employee)
        {
            try
            {
                if (!String.IsNullOrEmpty(employee.ImageName))
                {
                    string path = _hostEnvironment.ContentRootPath;
                    var imageName = _imagesService.SaveImage(employee.ImageFile, employee.Height,  employee.Width);
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

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<List<ManagerDto>>> UpdateAddressAsync(string id, [FromForm] RequestEmployeeDto updateEmployeeDto)
        {
            try
            {
                if (id == null)
                    return BadRequest("This user can not by updated");

                updateEmployeeDto.Id = new Guid(id);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (updateEmployeeDto.ImageFile != null && updateEmployeeDto.ImageName != null)
                {
                    string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", updateEmployeeDto.ImageName);
                    _imagesService.DeleteImage(imagePath);
                    updateEmployeeDto.ImageName = _imagesService.SaveImage(updateEmployeeDto.ImageFile, updateEmployeeDto.Height, updateEmployeeDto.Width);
                }

                await _employeeRepository.UpdateEmployee(updateEmployeeDto);

                var employee = _mapper.Map<RequestEmployeeDto, ReturnUserDto>(updateEmployeeDto);

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                employee.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, employee.ImageName);

                return CreatedAtAction("Get", new { employee.Id }, employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error save DB");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex);
            }
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteEmployee(String id)
        {
            if (id == String.Empty)
                return BadRequest();

            await _employeeRepository.RemoveEmployeeAsync(id);
            return Ok();
        }
    }
}