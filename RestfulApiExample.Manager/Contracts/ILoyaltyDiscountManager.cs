using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager.Contracts
{
    public interface ILoyaltyDiscountManager
    {
        decimal CalculateDiscountByPurchases(int purchaseCounter, decimal cost);
    }
}
