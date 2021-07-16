using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class AdminController : BaseController<Manager>
    {
        private readonly IAdminRepository _adminServices;

        public AdminController(IAdminRepository adminServices, IBaseRepository<Manager> manager, IWebHostEnvironment hostEnvironment) : base(manager, hostEnvironment)
        {
            _adminServices = adminServices;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<Manager>>> GetAllManager()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _adminServices.GetManagerAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
            }
        }

        //[HttpGet]
        //[Authorize(Roles = "Administrator")]
        //public async Task<ActionResult<List<Employee>>> GetAllEmployee()
        //{
        //    try
        //    {
        //        String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
        //        return await _adminServices.GetEmployeeAsync(ImageSrc);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //          "Error Get data from the database");
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateItem(Manager manager)
        {
            manager.ImageName = SaveImage(manager.ImageFile);
            try
            {
                if (!String.IsNullOrEmpty(manager.ImageName))
                {
                    await _adminServices.AddItem(manager);
                    return CreatedAtAction("GetAllManager", new { manager.Id }, manager);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error post data");
            }

            return NoContent();
        }
    }    
}
