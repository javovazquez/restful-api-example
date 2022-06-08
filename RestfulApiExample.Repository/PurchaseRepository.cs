using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Repository
{
    public class PurchaseRepository: IPurchaseRepository
    {
        private readonly ConcurrentDictionary<Guid, Entities.Purchase> data;

        public PurchaseRepository()
        {
            this.data = new ConcurrentDictionary<Guid, Entities.Purchase>();
        }

        public async Task<List<Entities.Purchase>> GetAllAsync(Guid? customerId = null, int pageSize = 50, int skip = 0)
        {
            if (pageSize <= 0)
            {
                throw Common.Errors.ArgumentOutOfRangeException(nameof(pageSize), $"Page size must be greater than 0");
            }

            if (skip < 0)
            {
                throw Common.Errors.ArgumentOutOfRangeException(nameof(skip), $"Skip must be greater than or equal to 0");
            }

            IEnumerable<Entities.Purchase> query = this.data.Values;

            if (customerId.HasValue)
            {
                query = query.Where(e => e.CustomerId == customerId);
            }

            query = query.Skip(skip)
                .Take(pageSize);

            return await Task.FromResult(query.ToList());
        }

        public async Task<Entities.Purchase> GetByIdAsync(Guid id)
        {
            var found = this.data.GetValueOrDefault(id);

            if (found == null)
            {
                throw Common.Errors.NotFoundException(nameof(Entities.Purchase), id.ToString());
            }

            return await Task.FromResult(found);
        }

        public async Task<Entities.Purchase> AddAsync(Entities.Purchase purchase)
        {
            if (purchase == null)
            {
                throw Common.Errors.ArgumentNullException(nameof(purchase));
            }

            purchase.Id = Guid.NewGuid();

            this.data.TryAdd(purchase.Id, purchase);

            return await Task.FromResult(purchase);
        }

        public async Task<Entities.Purchase> UpdateAsync(Entities.Purchase purchase)
        {
            if (purchase == null)
            {
                throw Common.Errors.ArgumentNullException(nameof(purchase));
            }
            if (purchase.Id == Guid.Empty)
            {
                throw Common.Errors.ArgumentNullException(nameof(purchase.Id));
            }

            var comparisonCustomer = await this.GetByIdAsync(purchase.Id);

            if (!this.data.TryUpdate(purchase.Id, purchase, comparisonCustomer))
            {
                throw Common.Errors.UpdateConflictException(nameof(Entities.Purchase), purchase.Id.ToString());
            }

            return purchase;
        }

        public async Task<Entities.Purchase> RemoveAsync(Guid purchaseId)
        {
            if (purchaseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(purchaseId));
            }

            Entities.Purchase purchase;

            if (!this.data.Remove(purchaseId, out purchase))
            {
                throw Common.Errors.NotFoundException(nameof(Entities.Purchase), purchaseId.ToString());
            };

            return await Task.FromResult(purchase);

        }

    }
}
