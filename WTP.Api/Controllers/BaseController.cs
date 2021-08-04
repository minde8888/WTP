using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.Data.Helpers;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Api.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    //[ApiController]

    public class BaseController<T> : ControllerBase where T : BaseEntyti
    {
        private readonly IBaseRepository<T> _baseServices;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BaseController(IBaseRepository<T> itemServices, IWebHostEnvironment hostEnvironment)
        {
            _baseServices = itemServices;
            _hostEnvironment = hostEnvironment;
         }

        [HttpGet("id")]
        public async Task<ActionResult<List<T>>> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest();

                var result = await _baseServices.GetItemIdAsync(id);
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
        public async Task<IActionResult> CreateItem(T t)
        {
            //t.ImageName = SaveImage(t.ImageFile);
            try
            {
                //if (!String.IsNullOrEmpty(t.ImageName))
                //{
                string UserId = HttpContext.User.FindFirstValue("id");
                t.UserId = UserId;
                await _baseServices.AddItem(t);
                return CreatedAtAction("Get", new { t.Id }, t);
                //}
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error post data");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, T t) //[FromForm]
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != t.Id)
                    return BadRequest();

                if (t.ImageFile != null)
                {
                    var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", t.ImageName);
                    _baseServices.DeleteImage(imagePath);
                    t.ImageName = SaveImage(t.ImageFile);
                }

                await _baseServices.UpdateItem(id, t);
                return CreatedAtAction("GetManager", new { t.Id }, t);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error upadte data");
            }
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<ActionResult<IEnumerable<T>>> Search(string name)
        {
            try
            {
                var result = await _baseServices.Search(name);

                if (result.Any())
                    return Ok(result);

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
                var ItemToDelete = await _baseServices.GetItemIdAsync(id);
                if (ItemToDelete == null)
                {
                    return NotFound($"Manager with Id = {id} not found");
                }
                await _baseServices.DeleteItem(id);
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

                ICompressimage compress = new Compressimage();
                compress.Resize(imagePath, imageName, imageFile);

                return imageName;
            }
            return null;
        }
    }
}