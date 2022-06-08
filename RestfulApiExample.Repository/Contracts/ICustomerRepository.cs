using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<List<Entities.Customer>> GetAllAsync(string? search = null, int pageSize = 50, int skip = 0);
        Task<Entities.Customer> GetByIdAsync(Guid id);
        Task<Entities.Customer> AddAsync(Entities.Customer customer);
        Task<Entities.Customer> UpdateAsync(Entities.Customer customer);
        Task<Entities.Customer> RemoveAsync(Guid customerId);
    }
}
