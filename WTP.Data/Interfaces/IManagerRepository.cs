using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IManagerRepository
    {
        Task<List<ManagerDto>> GetItemAsync(string ImageSrc);
        public Task AddManager(Manager manager, string  UserId);
    }
}
