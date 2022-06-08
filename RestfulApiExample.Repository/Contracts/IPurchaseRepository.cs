using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Repository.Contracts
{
    public interface IPurchaseRepository
    {
        Task<List<Entities.Purchase>> GetAllAsync(Guid? customerId = null, int pageSize = 50, int skip = 0);
        Task<Entities.Purchase> GetByIdAsync(Guid id);
        Task<Entities.Purchase> AddAsync(Entities.Purchase purchase);
        Task<Entities.Purchase> UpdateAsync(Entities.Purchase purchase);
        Task<Entities.Purchase> RemoveAsync(Guid purchaseId);

        int CountByCustomerId(Guid customerId);
    }
}
