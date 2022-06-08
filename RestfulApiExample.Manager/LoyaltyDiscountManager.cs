using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager
{
    public class LoyaltyDiscountManager : ILoyaltyDiscountManager
    {
        public LoyaltyDiscountManager()
        {
        }

        public decimal CalculateDiscountByPurchases(int purchaseCounter, decimal cost)
        {
            var discount = this.GetDiscountByPurchases(purchaseCounter);

            return cost * discount;
        }

        private decimal GetDiscountByPurchases(int purchaseCounter)
        {
            if (purchaseCounter <= 0)
                return 0;
            if (purchaseCounter <= 2)
                return 0.01M;
            if (purchaseCounter <= 5)
                return 0.02M;
            if (purchaseCounter <= 10)
                return 0.05M;
            return 0.1M;
        }
    }
}
