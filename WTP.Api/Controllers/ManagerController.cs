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
using WTP.Data.Helpers;


namespace WTP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManager _employeeServices;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ManagerController(IManager itemServices, IWebHostEnvironment hostEnvironment)
        {
            _employeeServices = itemServices;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Manager>>> GetManager()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _employeeServices.GetItemAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
            }
        }


        [HttpGet("id")]
        public async Task<ActionResult<List<Manager>>> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest();

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
        public async Task<IActionResult> Createmanager([FromForm] Manager manager)
        {
            manager.ImageName = SaveImage(manager.ImageFile);
            try
            {

                if (!String.IsNullOrEmpty(manager.ImageName))
                {
                    return await _employeeServices.AddItem(manager);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error post data");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] Manager manager)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != manager.Id)
                return BadRequest();

            if (manager.ImageFile != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", manager.ImageName);
                _employeeServices.DeleteImage(imagePath);
                manager.ImageName = SaveImage(manager.ImageFile);
            }

            await _employeeServices.UpdateItem(id, manager);
            return NoContent();
        }
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Manager>>> Search(string name)
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
                await _employeeServices.DeleteItem(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [NonAction]
        public string SaveImage(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

                ICompressimage compress =  new Compressimage();
                compress.Resize(imagePath, imageName, imageFile);

                return imageName;
            }
            return null;
        }
    }
}
