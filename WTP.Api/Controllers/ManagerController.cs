using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Entities;
using WTP.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace WTP.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : BaseController<Manager>
    {
        private readonly IManagerRepository _managerServices;

        public ManagerController(IManagerRepository employeeServices, IBaseRepository<Manager> manager, IWebHostEnvironment hostEnvironment) : base(manager, hostEnvironment)
        {
            _managerServices = employeeServices;
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<Manager>>> GetAllEmployees()
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
    }
}
