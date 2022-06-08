using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Manager
{
    [TestFixture]
    public class CustomerManagerTests
    {
        private readonly Stub.CustomerRepositoryStub CustomerRepositoryStub;

        private RestfulApiExample.Manager.CustomerManager CustomerManager;

        public CustomerManagerTests()
        {
            this.CustomerRepositoryStub = new Stub.CustomerRepositoryStub();
        }


        [SetUp]
        public void SetUp()
        {
            this.CustomerManager = new RestfulApiExample.Manager.CustomerManager(this.CustomerRepositoryStub);
        }

        [TestCaseSource(typeof(Data.CustomerData), nameof(Data.CustomerData.GetCreateFailedRequests))]
        public void CreateCustomerAsync_InvalidData_ThrowsValidationException(Entities.DTO.CreateCustomerRequest request)
        {
            Assert.ThrowsAsync<Common.Exceptions.ValidationException>(async () => await this.CustomerManager.CreateCustomerAsync(request));
        }

        [TestCaseSource(typeof(Data.CustomerData), nameof(Data.CustomerData.GetCreateSuccessfulRequests))]
        public async Task CreateCustomerAsync_Added_Successful(Entities.DTO.CreateCustomerRequest request)
        {
            var result = await this.CustomerManager.CreateCustomerAsync(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(result.Name, request.Name);
        }

        [TestCaseSource(typeof(Data.CustomerData), nameof(Data.CustomerData.GetUpdateFailedRequests))]
        public void UpdateCustomerAsync_InvalidData_ThrowsValidationException(Guid id, Entities.DTO.UpdateCustomerRequest request)
        {
            Assert.ThrowsAsync<Common.Exceptions.ValidationException>(async () => await this.CustomerManager.UpdateCustomerAsync(id, request));
        }

        [TestCase]
        public async Task UpdateCustomerAsync_Successful()
        {
            var currentCustomer = new Entities.Customer()
            {
                Name = "Current Name"
            };

            currentCustomer = await this.CustomerRepositoryStub.AddAsync(currentCustomer);

            var updateCustomerRequest = new Entities.DTO.UpdateCustomerRequest()
            {
                Id = currentCustomer.Id,
                Name = "New Name"
            };

            var result = await this.CustomerManager.UpdateCustomerAsync(currentCustomer.Id, updateCustomerRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(currentCustomer.Id, result.Id);
            Assert.AreEqual("New Name", result.Name);
        }

        [TestCase]
        public void DeleteCustomerAsync_InvalidData_ThrowsValidationException()
        {
            Assert.ThrowsAsync<Common.Exceptions.ValidationException>(async () => await this.CustomerManager.DeleteCustomerAsync(Guid.Empty));
        }

        [TestCase]
        public async Task DeleteCustomerAsync_Successful()
        {
            var currentCustomer = new Entities.Customer()
            {
                Name = "Name"
            };

            currentCustomer = await this.CustomerRepositoryStub.AddAsync(currentCustomer);

            await this.CustomerManager.DeleteCustomerAsync(currentCustomer.Id);

            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await this.CustomerRepositoryStub.GetByIdAsync(currentCustomer.Id));
        }

    }
}
