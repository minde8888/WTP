using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    public class BaseController<T> : ControllerBase where T : BaseEntiy
    {
        private readonly IBaseRepository<T> _baseServices;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;

        public BaseController(IBaseRepository<T> itemServices,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService)
        {
            _baseServices = itemServices;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
        }

        [HttpGet("id")]
        public async Task<ActionResult<List<T>>> Get(String id)
        {
            try
            {
                var userId = new Guid(id);
                if (userId == Guid.Empty)
                    return BadRequest();

                var result = await _baseServices.GetItemIdAsync(userId);
                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<List<T>>> Update(IFormCollection formCollection)
        {
            foreach (var key in formCollection)
            {
                var value = formCollection[key.ToString()]; // etc.
            }

            foreach (var key in formCollection.Keys)
            {
                var value = formCollection[key.ToString()]; // etc.
            }
            return Ok();
            //try
            //{
            //    var userId = new Guid(data["id"]);

            //    if (!ModelState.IsValid)
            //        return BadRequest(ModelState);

            //    if (t.ImageFile != null)
            //    {
            //        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", t.ImageName);
            //        _baseServices.DeleteImage(imagePath);
            //        t.ImageName = _imagesService.SaveImage(t.ImageFile, imagePath);
            //    }

            //    await _baseServices.UpdateItem(new Guid(data["id"]), data);
            //    return CreatedAtAction("GetManager", new { t.Id }, t);
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError,
            //        "Error upadte data");
            //}
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
    }
}