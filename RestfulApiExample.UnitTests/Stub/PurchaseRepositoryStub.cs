using RestfulApiExample.Entities;
using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Stub
{
    public class PurchaseRepositoryStub : IPurchaseRepository
    {
        private readonly ConcurrentDictionary<Guid, Entities.Purchase> purchases = new ConcurrentDictionary<Guid, Purchase>();
        
        public Task<Purchase> AddAsync(Purchase purchase)
        {
            purchase.Id = Guid.NewGuid();
            this.purchases[purchase.Id] = purchase;

            return Task.FromResult(purchase);
        }

        public int CountByCustomerId(Guid customerId)
        {
            return this.purchases.Where(e => e.Value.CustomerId == customerId).Count();
        }

        public Task<List<Purchase>> GetAllAsync(Guid? customerId = null, int pageSize = 50, int skip = 0)
        {
            return Task.FromResult(this.purchases.Values.ToList());
        }

        public Task<Purchase> UpdateAsync(Purchase purchase)
        {
            var comparisonPurchase = this.purchases[purchase.Id];
            this.purchases.TryUpdate(purchase.Id, purchase, comparisonPurchase);
            return Task.FromResult(purchase);
        }

        public Task<Purchase> GetByIdAsync(Guid id)
        {
            Entities.Purchase purchase;
            if (!this.purchases.TryGetValue(id, out purchase))
            {
                throw Common.Errors.NotFoundException(nameof(Purchase), id.ToString());
            }
            return Task.FromResult(purchase);
        }

        public Task<Purchase> RemoveAsync(Guid purchaseId)
        {
            Entities.Purchase purchase;
            this.purchases.Remove(purchaseId, out purchase);
            return Task.FromResult(purchase);
        }
    }
}
