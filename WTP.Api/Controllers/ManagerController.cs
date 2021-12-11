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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ManagerController : BaseController<Manager>
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;
        private readonly IMapper _mapper;
        private readonly ManagerService _managerService;

        public ManagerController(IManagerRepository managerRepository,
            IBaseRepository<Manager> manager,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService,
            IMapper mapper,
            ManagerService managerService)
            : base(manager)
        {
            _managerRepository = managerRepository;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
            _mapper = mapper;
            _managerService = managerService;
        }

        [HttpGet("id")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ManagerDto>>> Get(String id)
        {
            try
            {
                var userId = new Guid(id);
                if (userId == Guid.Empty)
                    return BadRequest();

                var manager = await _managerRepository.GetItemIdAsync(userId);
                if (manager == null)
                    return NotFound();

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var managerDto = _mapper.Map<List<ManagerDto>>(manager);
                _managerService.GetImagesAsync(managerDto, ImageSrc);

                return Ok(managerDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
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
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ManagerDto>>> UpdateAddressAsync(string id, [FromForm] RequestManagerDto updateManagerDto)
        {
            try
            {
                if (id == null)
                    return BadRequest("This user can not by updated");

                updateManagerDto.Id = new Guid(id);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (updateManagerDto.ImageFile != null && updateManagerDto.ImageName != null)
                {
                    string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", updateManagerDto.ImageName);
                    _imagesService.DeleteImage(imagePath);
                    updateManagerDto.ImageName = _imagesService.SaveImage(updateManagerDto.ImageFile, updateManagerDto.Height, updateManagerDto.Width);
                }

                await _managerRepository.UpdateManager(updateManagerDto);

                var manager = _mapper.Map<RequestManagerDto, ReturnUserDto>(updateManagerDto);

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                manager.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, manager.ImageName);

                return CreatedAtAction("Get", new { manager.Id }, manager);
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
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteManager(String id)
        {
            if (id == String.Empty)
                return BadRequest();

            await _managerRepository.RemoveManagerAsync(id);
            return Ok();
        }
    }
}