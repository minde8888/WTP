 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Helpers;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Api.Controllers
{

    public class PostController : ControllerBase
    {
        private readonly IBaseRepository<Post> _post;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostController(IBaseRepository<Post> post, IWebHostEnvironment hostEnvironment)
        {
            _post = post;
            _hostEnvironment = hostEnvironment;
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

   
     
        [HttpGet("id")]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult<List<Post>>> GetPost(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest();

                var result = await _post.GetItemIdAsync(id);
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
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Createmanager([FromForm] Post post)
        {
            post.ImageName = SaveImage(post.ImageFile);
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Put(Guid id, [FromForm] Post post)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != post.PostId)
                    return BadRequest();

                if (post.ImageFile != null)
                {
                    var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", post.ImageName);
                    _post.DeleteImage(imagePath);
                    post.ImageName = SaveImage(post.ImageFile);
                }

                await _post.UpdateItem(id, post);
                return CreatedAtAction("GetPost", new { post.PostId }, post);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error upadte data");
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
