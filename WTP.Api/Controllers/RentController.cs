using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Services.Services;
using WTP.Data.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using WTP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : Controller
    {
        private readonly IRentRepository _rentRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;
        private readonly IMapper _mapper;
        private readonly RentService _rentService;

        public RentController(IWebHostEnvironment hostEnvironment,
             ImagesService imagesService, 
             IRentRepository rentRepository,
             IMapper mapper,
             RentService rentService)
        {
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
            _rentRepository = rentRepository;
            _mapper = mapper;
            _rentService = rentService;
        }
        [HttpGet("id")]
        [Authorize(Roles = "Manager, Admin, Employee")]
        public async Task<ActionResult<List<RentDto>>> Get(String id)
        {
            try
            {
                var rentId = new Guid(id);
                if (rentId == Guid.Empty)
                    return BadRequest();

                var rent = await _rentRepository.GetRentIdAsync(rentId);
                if (rent == null)
                    return NotFound();

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var rentDto = _mapper.Map<List<RentDto>>(rent);
                _rentService.GetImagesAsync(rentDto, ImageSrc);

                return Ok(rentDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find rented tolls");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin, Employee")]
        public async Task<ActionResult<List<RentDto>>> GetAllRentTolls()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _rentRepository.GetAllAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Manager, Admin, Employee")]
        public async Task<IActionResult> AddNewRent([FromForm] RentDto rent)
        {
            try
            {
                if (!String.IsNullOrEmpty(rent.ImageName))
                {
                    string path = _hostEnvironment.ContentRootPath;
                    var imageName = _imagesService.SaveImage(rent.ImageFile, rent.Height, rent.Width);
                }
                await _rentRepository.AddRentToolAsync(rent);

                return CreatedAtAction("Get", new { rent.RentId }, rent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ManagerDto>>> UpdateAddressAsync(string id, [FromForm] RentDto rent)
        {
            try
            {
                if (id == null)
                    return BadRequest("This user can not by updated");

                rent.RentId = new Guid(id);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (rent.ImageFile != null && rent.ImageName != null)
                {
                    string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", rent.ImageName);
                    _imagesService.DeleteImage(imagePath);
                    rent.ImageName = _imagesService.SaveImage(rent.ImageFile, rent.Height, rent.Width);
                }

                var updatedRent = await _rentRepository.UpdateRent(rent);

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                rent.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, rent.ImageName);


                return Ok(rent);
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
        [Authorize(Roles = "Manager, Admin, Employee")]
        public async Task<ActionResult> DeleteManager(String id)
        {
            if (id == String.Empty)
                return BadRequest();

            await _rentRepository.RemoveRetedToolAsync(id);
            return Ok();
        }
    }
}
