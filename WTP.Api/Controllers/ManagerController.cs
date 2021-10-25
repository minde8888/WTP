using Microsoft.AspNetCore.Authentication.JwtBearer;
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

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ManagerController : BaseController<Manager>
    {
        private readonly IManagerRepository _managerServices;

        public ManagerController(IManagerRepository managerServices, 
            IBaseRepository<Manager> manager, 
            IWebHostEnvironment hostEnvironment) : base(manager, hostEnvironment)
        {
            _managerServices = managerServices;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<ManagerDto>>> GetAll()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _managerServices.GetItemAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewManagere([FromForm] Manager manager)
        {
            try
            {
                string UserId = HttpContext.User.FindFirstValue("id");
                _managerServices.AddManager(manager, UserId);
                return CreatedAtAction("Get", new { manager.Id }, manager);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }
    }
}