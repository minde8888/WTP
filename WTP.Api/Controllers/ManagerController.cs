using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ManagerController : BaseController<Manager>
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IWebHostEnvironment _hostEnvironment;  
        private readonly ImagesService _imagesService;

        public ManagerController(IManagerRepository managerRepository,
            IBaseRepository<Manager> manager,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService)
            : base(manager, hostEnvironment, imagesService)
        {
            _managerRepository = managerRepository;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<ManagerDto>>> GetAll()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _managerRepository.GetItemAsync(ImageSrc);
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
                _managerRepository.AddManager(manager, UserId);
                return CreatedAtAction("Get", new { manager.Id }, manager);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAddressAsync(string id, [FromForm] UpdateManagerDto updateManagerDto)
        {
            

            //var address = _context.Address.FirstOrDefault(a => a.ManagerId == id);

            //address.Street = updateEmployeeAddressDto.Address;

            //_context.Update(address);



            try
            {
                if (id == null)
                    return BadRequest("This user can not by updated");

                updateManagerDto.Id = new Guid(id);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //if (updateManagerDto.ImageFile != null && updateManagerDto.ImageName != null)
                //{
                //    string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", updateManagerDto.ImageName);
                //    _imagesService.DeleteImage(imagePath);
                //    updateManagerDto.ImageName = _imagesService.SaveImage(updateManagerDto.ImageFile, imagePath);
                //}

                 await _managerRepository.UpdateManager(updateManagerDto);

                //return CreatedAtAction("Get", new { updateManagerDto.Id }, updateManagerDto);
                return Ok();
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
    }
}