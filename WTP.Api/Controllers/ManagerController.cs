using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WTP.Domain.Entities;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using AutoMapper;

namespace WTP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerRepository _employeeServices;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ManagerController(IManagerRepository itemServices, IWebHostEnvironment hostEnvironment, IMapper maper)
        {
            _employeeServices = itemServices ?? throw new ArgumentException(nameof(itemServices));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentException(nameof(hostEnvironment));
        }


        [HttpGet]
        public async Task<ActionResult<List<ManagerDto>>> GetManager()
        {
            //var file = Request.Form.Files[0];
            String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
            var getAll = await _employeeServices.GetItemAsync(ImageSrc);
            if (getAll == null)
                return BadRequest(new ArgumentNullException());

            return getAll;

            //try
            //{
            //    String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
            //    var getAll = await _employeeServices.GetItemAsync(ImageSrc);
            //    if (getAll == null)
            //        return BadRequest(new ArgumentNullException());

            //    return getAll;
            //}
            //catch (ArgumentNullException){
            //    return StatusCode(StatusCodes.Status500InternalServerError,
            //     "Error Get value is null");
            //}
            //catch (NullReferenceException)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError,
            //     "Error Get not referance");
            //}
            //catch (InvalidOperationException)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError,
            //     "Error Get operation");
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError,
            //      "Error Get data from the database");
            //}
        }

        [HttpGet("id")]
        public async Task<ActionResult<List<Manager>>> GetManager(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ArgumentNullException());

                var result = await _employeeServices.GetItemIdAsync(id);
                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Get by id data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Manager>> CreateManager(Manager manager)
        {
            manager.ImageName = await SaveImage(manager.ImageFile);
            try
            {
                await _employeeServices.AddItem(manager);
                return CreatedAtAction("GetManager", new { id = manager.Id }, manager);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error post data");
            }
            //return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] Manager manager)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != manager.Id)
                return BadRequest(new ArgumentNullException());

            if (manager.ImageFile != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", manager.ImageName);
                _employeeServices.DeleteImage(imagePath);
                manager.ImageName = await SaveImage(manager.ImageFile);
            }

            await _employeeServices.UpdateItem(id, manager);
            return CreatedAtAction("GetManager", new { id = manager.Id }, manager);
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)
        {
            try
            {
                var result = await _employeeServices.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var managetToDelete = await _employeeServices.GetItemIdAsync(id);
                if (managetToDelete == null)
                {
                    return NotFound($"Manager with Id = {id} not found");
                }
               return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return imageName;
            }
            return null;
        }
    }
}
