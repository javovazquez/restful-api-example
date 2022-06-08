using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager.Contracts
{
    public interface ICustomerManager
    {
        Task<Entities.Customer> CreateCustomerAsync(Entities.DTO.CreateCustomerRequest request);
        Task<Entities.Customer> UpdateCustomerAsync(Guid id, Entities.DTO.UpdateCustomerRequest request);
        Task DeleteCustomerAsync(Guid id);
    }
}
