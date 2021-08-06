using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<List<ManagerDto>>> GetAll()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                //var a = await _managerServices.GetItemAsync(ImageSrc);
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