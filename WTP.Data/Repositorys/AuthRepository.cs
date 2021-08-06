using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    class AuthRepository: IAuthRepository
    {
        private readonly AppDbContext _context;

        //public AuthRepository(AppDbContext context)
        //{
        //    _context = context;
        //}
  
        //public Task<List<IActionResult>> GetUserInfo(string role)
        //{
        //    foreach (var item in role)
        //    {
        //        switch (item)
        //        {
        //            case "Manager":
        //                return Ok(await _context.Manager.Where(u => u.UserId == id).ToListAsync());

        //            case "Employee":
        //                return Ok(await _context.Employee.Where(u => u.UserId == id).ToListAsync());
                                      
        //        }
        //    }   
        //}
    }
}
