using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WTP.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetItemAsync(string ImageSrc);
        Task<List<T>> GetItemIdAsync(Guid Id);
        Task AddItem(T t);
        Task DeleteItem(Guid Id);
        Task UpdateItem(Guid Id, T t);
        public void DeleteImage(string imagePath);
        Task<IEnumerable<T>> Search(string name);
    }
}
