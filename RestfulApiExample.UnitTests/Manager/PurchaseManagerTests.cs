using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Manager
{
    [TestFixture]
    public class PurchaseManagerTests
    {
        private readonly Stub.PurchaseRepositoryStub PurchaseRepositoryStub;
        private readonly Stub.CustomerRepositoryStub CustomerRepositoryStub;
        private readonly Mock<RestfulApiExample.Manager.Contracts.ILoyaltyDiscountManager> LoyaltyDiscountManagerMock;

        private RestfulApiExample.Manager.PurchaseManager PurchaseManager;

        public PurchaseManagerTests()
        {
            this.PurchaseRepositoryStub = new Stub.PurchaseRepositoryStub();
            this.CustomerRepositoryStub = new Stub.CustomerRepositoryStub();
            this.LoyaltyDiscountManagerMock = new Mock<RestfulApiExample.Manager.Contracts.ILoyaltyDiscountManager>();
        }

        [SetUp]
        public void SetUp()
        {
            // Always return 0
            this.LoyaltyDiscountManagerMock.Setup(
                e => e.CalculateDiscountByPurchases(
                    It.IsAny<int>(),
                    It.IsAny<decimal>()))
                .Returns(0);

            this.PurchaseManager = new RestfulApiExample.Manager.PurchaseManager(
                this.PurchaseRepositoryStub, 
                this.LoyaltyDiscountManagerMock.Object,
                this.CustomerRepositoryStub); ;
        }

        [TestCaseSource(typeof(Data.PurchaseData), nameof(Data.PurchaseData.GetCreateFailedRequests))]
        public void CreatePurchaseAsync_InvalidData_FailedRequests(Entities.DTO.CreatePurchaseRequest request)
        {
            Assert.ThrowsAsync<Common.Exceptions.ValidationException>(async () => await this.PurchaseManager.CreatePurchaseAsync(request));
        }

        [TestCase]
        public async Task CreatePurchaseAsync_Successful()
        {
            var customer = await this.CustomerRepositoryStub.AddAsync(new Entities.Customer()
            {
                Name = "John Doe"
            });

            var request = new Entities.DTO.CreatePurchaseRequest()
            {
                CustomerId = customer.Id
            };

            var result = await this.PurchaseManager.CreatePurchaseAsync(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(result.CustomerId, customer.Id);
            Assert.AreEqual(100, result.Cost);
            Assert.AreEqual(0, result.Discount);
        }

    }
}
