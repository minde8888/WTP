using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        //[HttpGet]
        //[Authorize(Roles = "Administrator")]
        //public async Task<ActionResult<List<Post>>> GetAllPosts()
        //{
        //    try
        //    {
        //        String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
        //        return await _post.GetItemAsync(ImageSrc);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //          "Error Get data from the database");
        //    }
        //}
    }
}