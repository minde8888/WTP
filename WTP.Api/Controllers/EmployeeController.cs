//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using WTP.Domain.Entities;

//namespace WTP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        private readonly IEmployee _employeeServices;
//        private readonly IWebHostEnvironment _hostEnvironment;

//        public EmployeeController(IEmployee itemServices, IWebHostEnvironment hostEnvironment)
//        {
//            _employeeServices = itemServices;
//            _hostEnvironment = hostEnvironment;
//        }

//        [HttpGet]
//        public async Task<List<Employee>> GetManager()
//        {
//            String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
//            return await _employeeServices.GetItemAsync(ImageSrc);
//        }


//        [HttpGet("id")]
//        public IQueryable<Manager> Get(Guid id)
//        {
//            return _employeeServices.GetItemIdAsync(id);
//        }

//        [HttpPost]
//        public async Task Post([FromForm] Employee employee, Address address)
//        {
//            employee.ImageName = await SaveImage(employee.ImageFile);
//            await _employeeServices.AddItem(employee, address);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(Guid id, [FromForm] Manager manager)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            if (id != manager.Id)
//            {
//                return BadRequest();
//            }

//            if (manager.ImageFile != null)
//            {
//                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", manager.ImageName);
//                _employeeServices.DeleteImage(imagePath);
//                manager.ImageName = await SaveImage(manager.ImageFile);
//            }

//            await _employeeServices.UpdateItem(id, manager);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task Delete(Guid id)
//        {
//            await _employeeServices.DeleteItem(id);
//        }

//        [NonAction]
//        public async Task<string> SaveImage(IFormFile imageFile)
//        {
//            if (imageFile != null)
//            {
//                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
//                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
//                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
//                using (var fileStream = new FileStream(imagePath, FileMode.Create))
//                {
//                    await imageFile.CopyToAsync(fileStream);
//                }
//                return imageName;
//            }
//            return "1";
//        }
//    }
//}
