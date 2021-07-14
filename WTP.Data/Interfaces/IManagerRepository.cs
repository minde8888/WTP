using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IManagerRepository
    {
        Task<List<Manager>> GetItemAsync(string ImageSrc);
        Task<List<Manager>> GetItemIdAsync(Guid Id);
        Task AddItem(Manager manager); 
        Task DeleteItem(Guid Id);
        Task UpdateItem(Guid Id, Manager manager);
        public void DeleteImage(string imagePath);
        Task<IEnumerable<Manager>> Search(string name);
    }
}
