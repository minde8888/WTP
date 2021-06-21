//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WTP.Data.Context;
//using WTP.Data.Interfaces;
//using WTP.Domain.Entities;

//namespace WTP.Data.Repositorys
//{
//    public class ManagerRepository : IManagerRepository
//    {
//        private readonly AppDbContext _context;
//        //private readonly IMapper _mapper;

//        public ManagerRepository(AppDbContext context)
//        {
//            _context = context;
//            //_mapper = mapper;
//        }
//        public async Task<IActionResult> AddItem(Manager manager)
//        {
//            await _context.AddAsync(new Manager()
//            {
//                Id = manager.Id,
//                Name = manager.Name,
//                Surname = manager.Surname,
//                Occupation = manager.Occupation,
//                Email = manager.Email,
//                ImageName = manager.ImageName,
//                Address = (new Address()
//                {
//                    Id = manager.Address.Id,
//                    IsActive = manager.Address.IsActive,
//                    Street = manager.Address.Street,
//                    City = manager.Address.City,
//                    Phone = manager.Address.Phone,
//                    Zip = manager.Address.Zip,
//                    Country = manager.Address.Country,
//                    ManagerId = manager.Address.ManagerId
//                })
//            });
//            await _context.SaveChangesAsync();
//            return new NoContentResult();
//        }

//        public async Task<IActionResult> DeleteItem(Guid Id)
//        {
//            var manager = _context.Manager.Include(x => x.Address).Single(x => x.Id == Id);
//            if (manager == null)
//                return new NotFoundResult();

//            _context.Manager.Remove(manager);
//            _context.Address.Remove(manager.Address);
//            await _context.SaveChangesAsync();

//            return new NoContentResult();
//        }

//        public async Task<List<Manager>> GetItemIdAsync(Guid Id)
//        {
//            return await _context.Manager.Include(maneger => maneger.Address).Where(x => x.Id == Id).ToListAsync(); ;
//        }

//        public async Task<List<Manager>> GetItemAsync(string ImageSrc)
//        {
//            if (_context != null)
//            {
//                return await _context.Manager.Include(maneger => maneger.Address)
//             .Select(x => new Manager()
//             {
//                 Id = x.Id,
//                 Name = x.Name,
//                 Surname = x.Surname,
//                 Occupation = x.Occupation,
//                 Email = x.Email,
//                 ImageName = x.ImageName,
//                 ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, x.ImageName),
//                 Address = x.Address
//             })
//             .ToListAsync();
//            }
//            return null;
//        }

//        public async Task<IActionResult> UpdateItem(Guid Id, Manager manager)
//        {
//            _context.Entry(manager).State = EntityState.Modified;
//            try
//            {
//                await _context.SaveChangesAsync();
//                var manegeUpdate = await GetItemIdAsync(Id);
//                if (manegeUpdate == null)
//                    return new NotFoundResult();
//            }
//            catch (Exception)
//            {
//                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//            }
//            return new NoContentResult();
//        }
//        public async Task<IEnumerable<Manager>> Search(string name)
//        {
//            IQueryable<Manager> query = _context.Manager;

//            if (!string.IsNullOrEmpty(name))
//            {
//                query = query.Where(e => e.Name.Contains(name)
//                            || e.Surname.Contains(name));
//            }
//            return await query.ToListAsync();
//        }
//        public void DeleteImage(string imagePath)
//        {
//            if (System.IO.File.Exists(imagePath))
//                System.IO.File.Delete(imagePath);
//        }

//    }
//}
