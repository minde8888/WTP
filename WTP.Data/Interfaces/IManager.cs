using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IManager
    {
        Task<List<Manager>> GetItemAsync(string ImageSrc);
        Task<List<Manager>> GetItemIdAsync(Guid Id);
        Task<IActionResult> AddItem(Manager manager);
        Task<IActionResult> DeleteItem(Guid Id);
        Task<IActionResult> UpdateItem(Guid Id, Manager manager);
        public void DeleteImage(string imagePath);
    }
}
