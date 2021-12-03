using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    public class BaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;

        public BaseController(IBaseRepository<T> itemServices,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService)
        {
            _baseRepository = itemServices;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
        }

        //[HttpGet("id")]
        //public async Task<ActionResult<List<T>>> Get(String id)
        //{
        //    try
        //    {
        //        var userId = new Guid(id);
        //        if (userId == Guid.Empty)
        //            return BadRequest();

        //        var result = await _baseRepository.GetItemIdAsync(userId);
        //        if (result == null)
        //            return NotFound();

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Could not find web user account");
        //    }
        //}

        [HttpGet("Search")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<ActionResult<IEnumerable<T>>> Search(string name)
        {
            try
            {
                var result = await _baseRepository.Search(name);

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

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        var ItemToDelete = await _baseRepository.GetItemIdAsync(id);
        //        if (ItemToDelete == null)
        //        {
        //            return NotFound($"Manager with Id = {id} not found");
        //        }
        //        await _baseRepository.DeleteItem(id);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error deleting data");
        //    }
        //}
    }
}