 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    public class PostController : ControllerBase
    {
        private readonly IBaseRepository<Post> _post;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;

        public PostController(IBaseRepository<Post> post,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService)
        {
            _post = post;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _post.GetItemAsync(ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
            }
        }

   
     
        //[HttpGet("id")]
        //[Authorize(Roles = "Administrator, Manager")]
        //public async Task<ActionResult<List<Post>>> GetPost(Guid id)
        //{
        //    try
        //    {
        //        if (id == Guid.Empty)
        //            return BadRequest();

        //        var result = await _post.GetItemIdAsync(id);
        //        if (result == null)
        //            return NotFound();

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error Get by id data from the database");
        //    }
        //}

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Createmanager([FromForm] Post post)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", post.ImageName);
            post.ImageName = _imagesService.SaveImage(post.ImageFile);

            try
            {
                if (!String.IsNullOrEmpty(post.ImageName))
                {
                    await _post.AddItem(post);
                    return CreatedAtAction("GetManager", new { post.PostId }, post);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error post data");
            }

            return NoContent();
        }

        //[HttpPut("{id}")]
        //[Authorize(Roles = "Administrator, Manager")]
        //public async Task<IActionResult> Put(Guid id, [FromForm] Post post)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        if (id != post.PostId)
        //            return BadRequest();

        //        if (post.ImageFile != null)
        //        {
        //            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", post.ImageName);
        //            _imagesService.DeleteImage(imagePath);
        //            post.ImageName = _imagesService.SaveImage(post.ImageFile);
        //        }

        //        //await _post.UpdateItem(id, post);
        //        return CreatedAtAction("GetPost", new { post.PostId }, post);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error upadte data");
        //    }
        //}
    }
}
