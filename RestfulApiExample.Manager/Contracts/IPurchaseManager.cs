using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager.Contracts
{
    public interface IPurchaseManager
    {
        Task<Entities.Purchase> CreatePurchaseAsync(Entities.DTO.CreatePurchaseRequest request);
    }
}
