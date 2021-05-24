using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.UnitTest
{
    public class MockManagerRepository : IManagerRepository
    {
        public Task AddItem(Manager manager)
        {
            return null;
        }

        public void DeleteImage(string imagePath)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteItem(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ManagerDto>> GetItemAsync(string ImageSrc)
        {
            throw new NotImplementedException();
        }

        public Task<List<Manager>> GetItemIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Manager>> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateItem(Guid Id, Manager manager)
        {
            throw new NotImplementedException();
        }
    }
}
