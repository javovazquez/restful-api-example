using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Manager
{

    [TestFixture]
    public class LoyaltyDiscountManagerTests
    {

        private RestfulApiExample.Manager.LoyaltyDiscountManager loyaltyDiscountManager;

        [SetUp]
        public void SetUp()
        {
            this.loyaltyDiscountManager = new RestfulApiExample.Manager.LoyaltyDiscountManager();
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(5, 2)]
        [TestCase(6, 5)]
        [TestCase(10, 5)]
        [TestCase(11, 10)]
        [TestCase(20, 10)]
        public void CalculateDiscountByPurchases_PurchaseCounters_ReturnDiscount(int counter, decimal expectedDiscount)
        {
            var result = this.loyaltyDiscountManager.CalculateDiscountByPurchases(counter, 100);

            Assert.AreEqual(expectedDiscount, result);
        }
    }
}
