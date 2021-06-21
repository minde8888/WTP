//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WTP.Data.Context;

//namespace WTP.Data.Repositorys
//{
//    public class BaseRepository
//    {
//        private readonly AppDbContext _context;

//        public BaseRepository(AppDbContext context)
//        {           
//            _context = context;
//        }
//        public async Task<IActionResult> SaveToDb(Guid Id)
//        {
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException e)
//            {
//                 var sqlException = e.GetBaseException() as SqlException;
//                //2601 is error number of unique index violation
//                if (sqlException != null && sqlException.Number == 2601)
//                {
//                    throw; 
//                    //Unique index was violated. Show corresponding error message to user.
//                }
//            }

//            return new NoContentResult();
//        }
//    }
//}
