using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Data.Interfaces
{
    public interface IEmployeesRepository 
    {
        public Guid getManagerId(string UserId);
    }
}
