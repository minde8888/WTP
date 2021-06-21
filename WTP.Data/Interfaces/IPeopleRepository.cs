using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IPeopleRepository<T>
    {
        Task<List<T>> GetItemAsync(string ImageSrc);
        Task<List<T>> GetItemIdAsync(Guid Id);
        Task<IActionResult> AddItem(T t);
        Task<IActionResult> DeleteItem(Guid Id);
        Task<IActionResult> UpdateItem(Guid Id, T t);
        void DeleteImage(string imagePath);
        Task<IEnumerable<T>> Search(string name);
    }
}
