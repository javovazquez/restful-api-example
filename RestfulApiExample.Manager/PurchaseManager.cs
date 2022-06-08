using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager
{
    public class PurchaseManager: IPurchaseManager
    {
        private readonly IPurchaseRepository PurchaseRepository;
        private readonly ICustomerRepository CustomerRepository;
        private readonly ILoyaltyDiscountManager LoyaltyDiscountManager;
        public PurchaseManager(IPurchaseRepository purchaseRepository, ILoyaltyDiscountManager loyaltyDiscountManager, ICustomerRepository customerRepository)
        {
            this.PurchaseRepository = purchaseRepository;
            this.LoyaltyDiscountManager = loyaltyDiscountManager;
            this.CustomerRepository = customerRepository;
        }

        public async Task<Entities.Purchase> CreatePurchaseAsync(Entities.DTO.CreatePurchaseRequest request)
        {
            if (request == null)
            {
                throw Common.Errors.ValidationException("Request body is required");
            }
            if (request.CustomerId == Guid.Empty)
            {
                throw Common.Errors.ValidationException("Customer Id is required");
            }

            var customer = await this.CustomerRepository.GetByIdAsync(request.CustomerId);

            decimal cost = 100M;

            var purchaseCounter = this.PurchaseRepository.CountByCustomerId(customer.Id);

            var discount = this.LoyaltyDiscountManager.CalculateDiscountByPurchases(purchaseCounter, cost);

            var purchase = new Entities.Purchase()
            {
                CustomerId = request.CustomerId,
                Cost = cost - discount,
                Discount = discount
            };

            return await this.PurchaseRepository.AddAsync(purchase);
        }
    }
}
